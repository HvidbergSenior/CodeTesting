using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.FolderWork
{
    public class ProjectRemovedEventHandlerTest
    {
        [Fact]
        public async Task GivenProject_WhenRemovingIt_FolderWorksAreDeleted()
        {
            var projectId = Any.ProjectId();
            var repo = new ProjectFolderWorkInMemoryRepository();
            await repo.Add(ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectId, Any.ProjectFolderId()));
            await repo.Add(ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectId, Any.ProjectFolderId()));

            var handler = new ProjectRemovedEventHandler(repo, new FakeUnitOfWork());
            var projectRemovedDomainEvent = ProjectRemovedDomainEvent.Create(projectId);
            await handler.Handle(projectRemovedDomainEvent, CancellationToken.None);
            
            Assert.Empty(repo.Entities);
        }
    }
}
