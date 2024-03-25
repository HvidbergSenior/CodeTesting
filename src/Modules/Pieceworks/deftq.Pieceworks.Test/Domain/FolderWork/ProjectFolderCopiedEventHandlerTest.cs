using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Time;
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
    public class ProjectFolderCopiedEventHandlerTest
    {
        private readonly ProjectFolderRootInMemoryRepository _projectFolderRootInMemoryRepository;
        private readonly ProjectFolderWorkInMemoryRepository _projectFolderWorkInMemoryRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly FakeExecutionContext _fakeExecutionContext;

        public ProjectFolderCopiedEventHandlerTest()
        {
            _projectFolderRootInMemoryRepository = new ProjectFolderRootInMemoryRepository();
            _projectFolderWorkInMemoryRepository = new ProjectFolderWorkInMemoryRepository();
            _baseRateAndSupplementRepository = new BaseRateAndSupplementInMemoryRepository();
            _fakeExecutionContext = new FakeExecutionContext();
        }
        
        [Fact]
        #pragma warning disable MA0051
        public async Task GivenProjectFolder_WhenCopyingFolder_WorkItemsAreRecalculatedWithCorrectRateAndSupplements()
        {
            var eventHandler = new ProjectFolderCopiedEventHandler(_projectFolderRootInMemoryRepository, _projectFolderWorkInMemoryRepository,
                _baseRateAndSupplementRepository, _fakeExecutionContext);

            var baseRateAndSupplement = await _baseRateAndSupplementRepository.Get();
            // Create folders
            // Root 
            //    |- folderA (work item with 0 in all supplements)
            //       |- folderA1 (work item with 0 in all supplements)
            //    |- folderB (default supplements)
            var folderRoot = Any.ProjectFolderRoot();
            var projectId = folderRoot.ProjectId.Value;
            var folderA = Any.ProjectFolder();
            var folderA1 = Any.ProjectFolder();
            var folderB = Any.ProjectFolder();

            folderRoot.AddFolder(folderB);
            folderRoot.AddFolder(folderA);
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

            // Copy in folder root, and collect copy events
            folderRoot.CopyFolder(folderA.ProjectFolderId, folderB.ProjectFolderId, _fakeExecutionContext, new SystemTime());
            var copyEvents = folderRoot.DomainEvents.Where(e => e is ProjectFolderCopiedDomainEvent).ToList();

            await _projectFolderRootInMemoryRepository.Add(folderRoot);

            Assert.Equal(0.47m, folderAWork.WorkItems[0].TotalPayment.Value, 2);
            Assert.Equal(0.47m, folderA1Work.WorkItems[0].TotalPayment.Value, 2);
            Assert.Equal(2, copyEvents.Count);

            // Handle folder copied event and assert work items are recalculated
            foreach (var evt in copyEvents)
            {
                var copiedEvent = (ProjectFolderCopiedDomainEvent)evt;
                await eventHandler.Handle(copiedEvent, CancellationToken.None);
                var copiedFolderId = copiedEvent.Copy.Value;
                var copiedFolderWork = await _projectFolderWorkInMemoryRepository.GetByProjectAndFolderId(projectId, copiedFolderId);
                Assert.Equal(0.78m, copiedFolderWork.WorkItems[0].TotalPayment.Value, 2);
                Assert.Equal(0.78m, copiedFolderWork.WorkItems[0].TotalPayment.Value, 2);
            }
        }
    }
}
