using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetExtraWorkAgreements;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetExtraWorkAgreements
{
    public class GetExtraWorkAgreementsQueryAuthorizerTest
    {
        [Fact]
        public async Task GetExtraWorkAgreementRatesAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetExtraWorkAgreementsQueryAuthorizer(projectRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(GetExtraWorkAgreementsQuery.Create(project.ProjectId), CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetExtraWorkAgreementRatesAsParticipant_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetExtraWorkAgreementsQueryAuthorizer(projectRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(GetExtraWorkAgreementsQuery.Create(project.ProjectId), CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetExtraWorkAgreementRatesAsProjectManager_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetExtraWorkAgreementsQueryAuthorizer(projectRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(GetExtraWorkAgreementsQuery.Create(project.ProjectId), CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetExtraWorkAgreementRatesAsNonProjectAffiliate_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new GetExtraWorkAgreementsQueryAuthorizer(projectRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(GetExtraWorkAgreementsQuery.Create(project.ProjectId), CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
