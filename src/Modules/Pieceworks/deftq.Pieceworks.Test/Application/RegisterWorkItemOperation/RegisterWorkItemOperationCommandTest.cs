using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterWorkItemOperation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using Supplement = deftq.Pieceworks.Application.RegisterWorkItemOperation.Supplement;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Application.RegisterWorkItemOperation
{
    public class RegisterWorkItemOperationCommandTest
    {
        private readonly FakeUnitOfWork _uow;
        private readonly FakeExecutionContext _executionContext;
        private readonly BaseRateAndSupplementRepository _baseRateRepository;
        private readonly ProjectFolderRootInMemoryRepository _projectFolderRootRepository;
        private readonly ProjectFolderWorkInMemoryRepository _folderWorkRepository;

        public RegisterWorkItemOperationCommandTest()
        {
            _uow = new FakeUnitOfWork();
            _executionContext = new FakeExecutionContext();
            _baseRateRepository = new BaseRateAndSupplementRepository();
            _projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            _folderWorkRepository = new ProjectFolderWorkInMemoryRepository();
        }

        [Fact]
        public async Task Should_Register_WorkItem_Successfully()
        {
            var projectId = ProjectId.Create(Guid.NewGuid());
            var projectFolderRoot = ProjectFolderRoot.Create(projectId, ProjectName.Empty(), Any.ProjectFolderRootId(), GetDefaultFolderRateAndSupplement());

            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);
            await _projectFolderRootRepository.Add(projectFolderRoot);

            var initialFolderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectId, folder.ProjectFolderId);
            await _folderWorkRepository.Add(initialFolderWork);

            var handler = new RegisterWorkItemOperationCommandHandler(_projectFolderRootRepository, _baseRateRepository, _folderWorkRepository, _uow,
                _executionContext);
            var cmd = RegisterWorkItemOperationCommand.Create(projectId.Value, folder.Id, Guid.NewGuid(), "", Guid.NewGuid(), "testText",
                DateOnly.MaxValue, 60000m, 2, new List<Supplement>());
            await handler.Handle(cmd, CancellationToken.None);

            var folderWork = await _folderWorkRepository.GetByProjectAndFolderId(projectId.Value, folder.ProjectFolderId.Value);

            Assert.Single(folderWork.WorkItems);
            var workItem = folderWork.WorkItems[0];
            Assert.True(workItem.IsOperation());
            Assert.False(workItem.IsMaterial());
            Assert.Equal(workItem.WorkItemId, cmd.WorkItemId);
            Assert.Equal(cmd.WorkItemId, workItem.WorkItemId);
            Assert.Single(_folderWorkRepository.Entities);
            Assert.True(_folderWorkRepository.SaveChangesCalled);
            Assert.True(_uow.IsCommitted);
        }

        [Fact]
        public async Task When_WorkItemAddedToUnknownProject_ExceptionIsThrown()
        {
            var handler = new RegisterWorkItemOperationCommandHandler(_projectFolderRootRepository, _baseRateRepository, _folderWorkRepository, _uow,
                _executionContext);
            var cmd = RegisterWorkItemOperationCommand.Create(Any.Instance<Guid>(), Any.Instance<Guid>(), Guid.NewGuid(),
                "", Guid.NewGuid(), "testText", DateOnly.MaxValue, 60000m, 2, new List<Supplement>());
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }

        [Fact]
        public async Task When_WorkItemAddedToUnknownFolder_ExceptionIsThrown()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            await _projectFolderRootRepository.Add(projectFolderRoot);

            var handler = new RegisterWorkItemOperationCommandHandler(_projectFolderRootRepository, _baseRateRepository, _folderWorkRepository, _uow,
                _executionContext);
            var cmd = RegisterWorkItemOperationCommand.Create(projectFolderRoot.ProjectId.Value, Any.Instance<Guid>(), Guid.NewGuid(),
                "", Guid.NewGuid(), "testText", DateOnly.MaxValue, 60000m, 2, new List<Supplement>());

            await Assert.ThrowsAsync<ProjectFolderNotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }
    }
}
