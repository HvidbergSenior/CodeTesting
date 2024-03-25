using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateExtraWorkAgreementRates;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateExtraWorkAgreementRates
{
    public class UpdateExtraWorkAgreementRatesCommandAuthorizerTest
    {
        [Fact]
        public async Task UpdateBaseRateRegulationAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateExtraWorkAgreementRatesCommandAuthorizer(projectRepository, executionContext);
            var command = UpdateExtraWorkAgreementRatesCommand.Create(project.ProjectId.Value, 100, 100);
            var authorizationResult = await authorizer.Authorize(command, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task UpdateBaseRateRegulationAsProjectManager_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateExtraWorkAgreementRatesCommandAuthorizer(projectRepository, executionContext);
            var command = UpdateExtraWorkAgreementRatesCommand.Create(project.ProjectId.Value, 100, 100);
            var authorizationResult = await authorizer.Authorize(command, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task UpdateBaseRateRegulationAsNonOwnerOrNonProjectManager_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new UpdateExtraWorkAgreementRatesCommandAuthorizer(projectRepository, executionContext);
            var command = UpdateExtraWorkAgreementRatesCommand.Create(project.ProjectId.Value, 100, 100);
            var authorizationResult = await authorizer.Authorize(command, CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        } 
    }
}
