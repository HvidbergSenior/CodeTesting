using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.FolderWork
{
    public class ProjectFolderMovedEventHandlerTest
    {
        private readonly ProjectFolderRootInMemoryRepository _projectFolderRootInMemoryRepository;
        private readonly ProjectFolderWorkInMemoryRepository _projectFolderWorkInMemoryRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly FakeExecutionContext _fakeExecutionContext;

        public ProjectFolderMovedEventHandlerTest()
        {
            _projectFolderRootInMemoryRepository = new ProjectFolderRootInMemoryRepository();
            _projectFolderWorkInMemoryRepository = new ProjectFolderWorkInMemoryRepository();
            _baseRateAndSupplementRepository = new BaseRateAndSupplementInMemoryRepository();
            _fakeExecutionContext = new FakeExecutionContext();
        }

        [Fact]
        public async Task GivenProjectFolder_WhenMovingFolder_WorkItemsAreRecalculatedWithCorrectRateAndSupplements()
        {
            var eventHandler = new ProjectFolderMovedEventHandler(_projectFolderRootInMemoryRepository, _projectFolderWorkInMemoryRepository,
                _baseRateAndSupplementRepository, _fakeExecutionContext);

            var baseRateAndSupplement = await _baseRateAndSupplementRepository.Get();
            // Create folders
            // Root 
            //    |- folderA (work item with 0 supplements)
            //       |- folderA1 (work item with 0 supplements)
            //    |- folderB (default supplements)
            var folderRoot = Any.ProjectFolderRoot();
            var folderA = Any.ProjectFolder();
            var folderA1 = Any.ProjectFolder();
            var folderB = Any.ProjectFolder();

            folderRoot.AddFolder(folderB);
            folderRoot.AddFolder(folderA, folderB);
            folderRoot.AddFolder(folderA1, folderA);

            var folderAWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folderA.ProjectFolderId);
            var folderA1Work = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folderA1.ProjectFolderId);
            var folderBWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folderB.ProjectFolderId);

            // Add work items with no base supplements
            var folderIndirectTimeSupplement = FolderIndirectTimeSupplement.Create(0, FolderValueInheritStatus.Overwrite());
            var folderSiteSpecificTimeSupplement = FolderSiteSpecificTimeSupplement.Create(0, FolderValueInheritStatus.Overwrite());
            var folderBaseRateRegulation = FolderBaseRateRegulation.Create(0, FolderValueInheritStatus.Overwrite());
            var folderRateAndSupplement = FolderRateAndSupplement.Create(folderIndirectTimeSupplement, folderSiteSpecificTimeSupplement, folderBaseRateRegulation);
            var folder = Any.ProjectFolder(folderRateAndSupplement);
            BaseRateAndSupplementProxy baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(baseRateAndSupplement, folder);

            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(),
                Any.WorkItemText(), Any.WorkItemEanNumber(), WorkItemMountingCode.FromCode(3), WorkItemDuration.Create(3700),
                WorkItemAmount.Create(2), Any.WorkItemUnit(), new List<SupplementOperation>(), new List<Supplement>());
            folderAWork.AddWorkItem(workItem, baseRateAndSupplementProxy);
            folderA1Work.AddWorkItem(workItem, baseRateAndSupplementProxy);

            await _projectFolderWorkInMemoryRepository.Add(folderAWork);
            await _projectFolderWorkInMemoryRepository.Add(folderA1Work);
            await _projectFolderWorkInMemoryRepository.Add(folderBWork);
            
            await _projectFolderRootInMemoryRepository.Add(folderRoot);

            Assert.Equal(0.47m, folderAWork.WorkItems[0].TotalPayment.Value, 2);
            Assert.Equal(0.47m, folderA1Work.WorkItems[0].TotalPayment.Value, 2);

            // Handle folder moved event
            await eventHandler.Handle(
                ProjectFolderMovedDomainEvent.Create(folderRoot.ProjectId, folderA.ProjectFolderId, ProjectFolderRoot.RootFolderId,
                    folderB.ProjectFolderId), CancellationToken.None);

            // Assert work items are recalculated
            var folderAWorkFromRepo =
                await _projectFolderWorkInMemoryRepository.GetByProjectAndFolderId(folderRoot.ProjectId.Value, folderA.ProjectFolderId.Value);
            var folderA1WorkFromRepo =
                await _projectFolderWorkInMemoryRepository.GetByProjectAndFolderId(folderRoot.ProjectId.Value, folderA1.ProjectFolderId.Value);
            Assert.Equal(0.78m, folderAWorkFromRepo.WorkItems[0].TotalPayment.Value, 2);
            Assert.Equal(0.78m, folderA1WorkFromRepo.WorkItems[0].TotalPayment.Value, 2);
        }
    }
}
