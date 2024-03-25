using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateWorkItem;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Application.UpdateWorkItem
{
    public class UpdateWorkItemCommandTest
    {
        private readonly FakeUnitOfWork _uow;
        private readonly ProjectFolderRootInMemoryRepository _projectFolderRootRepository;
        private readonly ProjectFolderWorkInMemoryRepository _folderWorkRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;

        public UpdateWorkItemCommandTest()
        {
            _baseRateAndSupplementRepository = new BaseRateAndSupplementInMemoryRepository();
            _uow = new FakeUnitOfWork();
            _projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            _folderWorkRepository = new ProjectFolderWorkInMemoryRepository();
        }

        [Fact]
        public async Task UpdateWorkItemAmount_ShouldUpdate()
        {
            var project = Any.Project();
            var projectFolderRoot = ProjectFolderRoot.Create(project.ProjectId, ProjectName.Empty(), Any.ProjectFolderRootId(),
                GetDefaultFolderRateAndSupplement());

            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);
            await _projectFolderRootRepository.Add(projectFolderRoot);

            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), project.ProjectId, folder.ProjectFolderId);
            var workItem = Any.WorkItem();
            folderWork.AddWorkItem(workItem, GetDefaultBaseRateAndSupplementProxy());

            await _folderWorkRepository.Add(folderWork);
            var getFolderWork =
                await _folderWorkRepository.GetByProjectAndFolderId(project.ProjectId.Value, folder.ProjectFolderId.Value, CancellationToken.None);
            Assert.Equal(0.00m, getFolderWork.WorkItems[0].TotalPayment.Value, 2);
            Assert.Equal(0.00m, getFolderWork.WorkItems[0].TotalWorkTime.Value, 2);
            
            var newAmount = 20;

            var handler = new UpdateWorkItemCommandHandler(_projectFolderRootRepository, _folderWorkRepository, _baseRateAndSupplementRepository,
                _uow);
            var cmd = UpdateWorkItemCommand.Create(project.ProjectId.Value, folder.ProjectFolderId.Value, workItem.WorkItemId.Value, newAmount);
            await handler.Handle(cmd, CancellationToken.None);

            Assert.Single(_folderWorkRepository.Entities);
            Assert.True(_folderWorkRepository.SaveChangesCalled);
            Assert.True(_uow.IsCommitted);

            getFolderWork =
                await _folderWorkRepository.GetByProjectAndFolderId(project.ProjectId.Value, folder.ProjectFolderId.Value, CancellationToken.None);

            Assert.Equal(20, getFolderWork.WorkItems[0].Amount.Value);
            Assert.Equal(6.83m, getFolderWork.WorkItems[0].TotalPayment.Value, 2);
            Assert.Equal(114436.67m, getFolderWork.WorkItems[0].TotalWorkTime.Value, 2);
        }
    }
}
