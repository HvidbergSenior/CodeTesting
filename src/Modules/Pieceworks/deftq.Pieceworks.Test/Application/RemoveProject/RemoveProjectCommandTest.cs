using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveProject;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveProject
{
    public class RemoveProjectCommandTest
    {
        [Fact]
        public async Task RemoveProjectCommand_ShouldNotReturnProject()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectInMemoryRepository();

            var project = Any.Project();

            await repository.Add(project);

            var handler = new RemoveProjectCommandHandler(repository, uow);
            var cmd = RemoveProjectCommand.Create(project.ProjectId.Value);
            await handler.Handle(cmd, CancellationToken.None);

            Assert.Empty(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
        }
    }
}
