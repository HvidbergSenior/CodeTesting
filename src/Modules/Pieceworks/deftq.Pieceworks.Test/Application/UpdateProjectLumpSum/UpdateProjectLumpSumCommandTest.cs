using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectLumpSum;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateProjectLumpSum
{
    public class UpdateProjectLumpSumCommandTest
    {
        [Fact]
        public async Task GivenProject_WhenUpdatingLumpSum_ProjectLumpSumIsUpdated()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            
            var project = Any.Project();
            await projectRepository.Add(project);
            var command = UpdateProjectLumpSumCommand.Create(project.ProjectId.Value, 123.45m);
            
            var handler = new UpdateProjectLumpSumCommandHandler(projectRepository, uow);
            await handler.Handle(command, CancellationToken.None);
            
            var projectFromRepo = await projectRepository.GetById(project.ProjectId.Value);
            
            Assert.Equal(123.45m, projectFromRepo.ProjectLumpSumPaymentDkr.Value);
            Assert.True(projectRepository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
        }
    }
}
