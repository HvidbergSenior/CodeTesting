using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveWorkItem;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using FluentAssertions;
using Xunit;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Application.RemoveWorkItem
{
    public class RemoveWorkItemCommandTest
    {
        private readonly FakeUnitOfWork _uow;
        private readonly FakeExecutionContext _executionContext;
        private readonly ProjectFolderRootInMemoryRepository _projectFolderRootRepository;
        private readonly ProjectFolderWorkInMemoryRepository _folderWorkRepository;

        public RemoveWorkItemCommandTest()
        {
            _uow = new FakeUnitOfWork();
            _executionContext = new FakeExecutionContext();
            _projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            _folderWorkRepository = new ProjectFolderWorkInMemoryRepository();
        }

        [Fact]
        public async Task RemoveMultipleWorkItems_ShouldNotReturnWorkItems()
        {
            var project = Any.Project();
            var projectFolderRoot = ProjectFolderRoot.Create(project.ProjectId, ProjectName.Empty(), Any.ProjectFolderRootId(), GetDefaultFolderRateAndSupplement());

            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);
            await _projectFolderRootRepository.Add(projectFolderRoot);

            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), project.ProjectId, folder.ProjectFolderId);

            var workItem = Any.WorkItem();
            var workItem2 = Any.WorkItem();
            folderWork.AddWorkItem(workItem, GetDefaultBaseRateAndSupplementProxy());
            folderWork.AddWorkItem(workItem2, GetDefaultBaseRateAndSupplementProxy());

            await _folderWorkRepository.Add(folderWork);
            
            var handler = new RemoveWorkItemCommandHandler(_folderWorkRepository, _uow, _executionContext);
            var cmd = RemoveWorkItemCommand.Create(project.ProjectId.Value, folderWork.ProjectFolderId.Value, new List<Guid>{ workItem.WorkItemId.Value, workItem2.WorkItemId.Value });
            await handler.Handle(cmd, CancellationToken.None);
            
            Assert.Single(_folderWorkRepository.Entities);
            Assert.True(_folderWorkRepository.SaveChangesCalled);
            Assert.True(_uow.IsCommitted);

            var getFolderWork = await _folderWorkRepository.GetByProjectAndFolderId(project.ProjectId.Value, folder.ProjectFolderId.Value, CancellationToken.None);

            getFolderWork.WorkItems.Should().BeEmpty();
        }

        [Fact]
        public async Task RemoveUnknownWorkItem_ShouldThrow()
        {
            var project = Any.Project();
            var projectFolderRoot = ProjectFolderRoot.Create(project.ProjectId, ProjectName.Empty(), Any.ProjectFolderRootId(), GetDefaultFolderRateAndSupplement());

            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);
            await _projectFolderRootRepository.Add(projectFolderRoot);

            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), project.ProjectId, folder.ProjectFolderId);
            
            await _folderWorkRepository.Add(folderWork);
            
            var handler = new RemoveWorkItemCommandHandler(_folderWorkRepository, _uow, _executionContext);
            var cmd = RemoveWorkItemCommand.Create(project.ProjectId.Value, folderWork.ProjectFolderId.Value, new List<Guid>{Any.WorkItemId().Value});

            Func<Task> act = async () => await handler.Handle(cmd, CancellationToken.None);
            await act.Should().ThrowAsync<WorkItemNotFoundException>();
        }
    }
}
