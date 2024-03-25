using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetCompensationPaymentParticipantsInPeriod;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetCompensationPaymentParticipantsInPeriod
{
    public class GetCompensationPaymentParticipantsInPeriodQueryAuthorizerTest
    {
        [Fact]
        public async Task GetCompensationPaymentAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetCompensationPaymentParticipantsInPeriodQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetCompensationPaymentParticipantsInPeriodQuery.Create(project.ProjectId, new DateOnly(2023, 1 ,1), new DateOnly(2023, 1 ,2), 20),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetCompensationPaymentAsParticipant_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetCompensationPaymentParticipantsInPeriodQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetCompensationPaymentParticipantsInPeriodQuery.Create(project.ProjectId, new DateOnly(2023, 1 ,1), new DateOnly(2023, 1 ,2), 20),
                CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetCompensationPaymentAsManager_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var otherParticipant = Any.Guid();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetCompensationPaymentParticipantsInPeriodQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetCompensationPaymentParticipantsInPeriodQuery.Create(project.ProjectId, new DateOnly(2023, 1 ,1), new DateOnly(2023, 1 ,2), 20),
                CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
