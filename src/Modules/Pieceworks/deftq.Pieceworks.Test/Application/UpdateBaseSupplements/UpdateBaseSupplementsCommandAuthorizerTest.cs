using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateBaseSupplements;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateBaseSupplements
{
    public class UpdateBaseSupplementsCommandAuthorizerTest
    {
        [Fact]
        public async Task UpdateBaseRateRegulationAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateBaseSupplementsCommandAuthorizer(projectRepository, executionContext);
            var command = UpdateBaseSupplementsCommand.Create(project.ProjectId.Value, Any.Guid(), 100,
                UpdateBaseSupplementsCommand.UpdateBaseSupplementStatusEnum.Overwrite, 100,
                UpdateBaseSupplementsCommand.UpdateBaseSupplementStatusEnum.Overwrite);
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

            var authorizer = new UpdateBaseSupplementsCommandAuthorizer(projectRepository, executionContext);
            var command = UpdateBaseSupplementsCommand.Create(project.ProjectId.Value, Any.Guid(), 100,
                UpdateBaseSupplementsCommand.UpdateBaseSupplementStatusEnum.Overwrite, 100,
                UpdateBaseSupplementsCommand.UpdateBaseSupplementStatusEnum.Overwrite);
            var authorizationResult = await authorizer.Authorize(command, CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
