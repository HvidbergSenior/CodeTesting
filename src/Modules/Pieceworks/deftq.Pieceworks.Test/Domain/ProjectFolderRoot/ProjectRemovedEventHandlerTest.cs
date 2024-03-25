using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.projectFolderRoot
{
    public class ProjectRemovedEventHandlerTest
    {
        [Fact]
        public async Task ShouldNotReturnFolderRootWhenProjectIsDeleted()
        {
            var uow = new FakeUnitOfWork();
            var folderRootRepository = new ProjectFolderRootInMemoryRepository();

            var folderRoot = Any.ProjectFolderRoot();

            await folderRootRepository.Add(folderRoot);

            var handler = new ProjectRemovedEventHandler(folderRootRepository, uow);
            var evt = ProjectRemovedDomainEvent.Create(folderRoot.ProjectId);

            await handler.Handle(evt, CancellationToken.None);

            Assert.Empty(folderRootRepository.Entities);
            Assert.True(folderRootRepository.SaveChangesCalled);
        }
    }
}
