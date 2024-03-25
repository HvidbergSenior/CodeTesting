using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.FolderWork
{
    public class ProjectFolderRateAndSupplementUpdatedEventHandlerTest
    {
        [Fact]
        public async Task GivenFolderWork_WhenFolderRateAndSupplementsUpdated_WorkItemsAreRecalculated()
        {
            var folderWorkRepository = new ProjectFolderWorkInMemoryRepository();
            var projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            var baseRateAndSupplementRepository = new BaseRateAndSupplementInMemoryRepository();
            var baseRateAndSupplement = await baseRateAndSupplementRepository.Get();
            
            var projectId = Any.ProjectId();
            var projectFolderId = Any.ProjectFolderId();
            
            //Create folder root
            var projectFolderRoot = ProjectFolderRoot.Create(projectId, Any.ProjectName(), Any.ProjectFolderRootId(),
                FolderRateAndSupplement.Create(baseRateAndSupplement));
            var folderRateAndSupplement = FolderRateAndSupplement.Create(baseRateAndSupplement);
            projectFolderRoot.AddFolder(ProjectFolder.Create(projectFolderId, Any.ProjectFolderName(), Any.ProjectFolderDescription(),
                Any.ProjectFolderCreatedBy(), folderRateAndSupplement, ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Normal()));
            await projectFolderRootRepository.Add(projectFolderRoot);

            //Create folder work and add work item
            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectId, projectFolderId);
            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(), WorkItemMountingCode.FromCode(3), WorkItemDuration.Create(3700), WorkItemAmount.Create(7),
                WorkItemUnit.Piece, new List<SupplementOperation>(), new List<Supplement>());
            folderWork.AddWorkItem(workItem, new BaseRateAndSupplementProxy(baseRateAndSupplement, Any.ProjectFolder(folderRateAndSupplement)));
            await folderWorkRepository.Add(folderWork);
            
            //Assert payment and work time is calculated correctly
            Assert.Equal(2.72m, folderWork.WorkItems[0].TotalPayment.Value, 2);
            Assert.Equal(45542.56m, folderWork.WorkItems[0].TotalWorkTime.Value, 5);
            
            //New rates and supplements 
            var newIndirectTime = FolderIndirectTimeSupplement.Create(66, FolderValueInheritStatus.Overwrite());
            var newSiteSpecificTime = FolderSiteSpecificTimeSupplement.Create(4, FolderValueInheritStatus.Overwrite());
            var newBaseRateRegulation = FolderBaseRateRegulation.Create(100, FolderValueInheritStatus.Overwrite());
            var projectFolder = projectFolderRoot.GetFolder(projectFolderId);
            projectFolder.UpdateFolderBaseSupplement(projectId, newIndirectTime, newSiteSpecificTime, folderRateAndSupplement);
            projectFolder.UpdateFolderBaseRateRegulation(projectId, newBaseRateRegulation, folderRateAndSupplement);
            
            //Update folder rate and supplements
            await projectFolderRootRepository.Update(projectFolderRoot);
            var eventHandler = new ProjectFolderRateAndSupplementUpdatedEventHandler(folderWorkRepository, projectFolderRootRepository,
                    baseRateAndSupplementRepository);
            await eventHandler.Handle(ProjectFolderRateAndSupplementUpdatedDomainEvent.Create(folderWork.ProjectId, folderWork.ProjectFolderId), CancellationToken.None);

            //Assert folder rates and supplements are updated
            var folderWorkFromRepo = await folderWorkRepository.GetByProjectAndFolderId(folderWork.ProjectId.Value, folderWork.ProjectFolderId.Value);
            Assert.Equal(5.56m, folderWorkFromRepo.WorkItems[0].TotalPayment.Value, 2);
            Assert.Equal(46609.64m, folderWorkFromRepo.WorkItems[0].TotalWorkTime.Value, 5);
        }
    }
}
