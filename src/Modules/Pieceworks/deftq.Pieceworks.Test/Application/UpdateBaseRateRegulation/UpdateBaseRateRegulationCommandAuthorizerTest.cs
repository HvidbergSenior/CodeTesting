using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateBaseRateRegulation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateBaseRateRegulation
{
    public class UpdateBaseRateRegulationCommandAuthorizerTest
    {
        [Fact]
        public async Task UpdateBaseRateRegulationAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateBaseRateRegulationCommandAuthorizer(projectRepository, executionContext);
            var command = UpdateBaseRateRegulationCommand.Create(project.ProjectId.Value, Any.Guid(), 100,
                UpdateBaseRateRegulationCommand.UpdateBaseRateRegulationStatusEnum.Overwrite);
            var authorizationResult = await authorizer.Authorize(command, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task UpdateBaseRateRegulationAsNonOwner_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new UpdateBaseRateRegulationCommandAuthorizer(projectRepository, executionContext);
            var command = UpdateBaseRateRegulationCommand.Create(project.ProjectId.Value, Any.Guid(), 100,
                UpdateBaseRateRegulationCommand.UpdateBaseRateRegulationStatusEnum.Overwrite);
            var authorizationResult = await authorizer.Authorize(command, CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
