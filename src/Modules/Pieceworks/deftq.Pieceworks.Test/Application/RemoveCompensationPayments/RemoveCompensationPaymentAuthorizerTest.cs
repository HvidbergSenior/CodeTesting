using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveCompensationPayments;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveCompensationPayments
{
    public class RemoveCompensationPaymentAuthorizerTest
    {
        private readonly FakeUnitOfWork _uow;
        private readonly ProjectInMemoryRepository _projectRepository;
        private readonly FakeExecutionContext _executionContext;

        public RemoveCompensationPaymentAuthorizerTest()
        {
            _uow = new FakeUnitOfWork();
            _projectRepository = new ProjectInMemoryRepository();
            _executionContext = new FakeExecutionContext();
        }

        [Fact]
        public async Task RemoveCompensationPaymentAsProjectOwner_ShouldBeAuthorized()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectRepository.Add(project);

            var authorizer = new RemoveCompensationPaymentsCommandAuthorizer(_projectRepository, _uow, _executionContext);
            var authorizationResult = await authorizer.Authorize(RemoveCompensationPaymentsCommand.Create(project.ProjectId.Value, new List<Guid>()),
                CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RemoveCompensationPaymentAsProjectManager_ShouldBeNotAuthorized()
        {
            var project = Any.Project().WithProjectManager(_executionContext.UserId);
            await _projectRepository.Add(project);

            var authorizer = new RemoveCompensationPaymentsCommandAuthorizer(_projectRepository, _uow, _executionContext);
            var authorizationResult = await authorizer.Authorize(RemoveCompensationPaymentsCommand.Create(project.ProjectId.Value, new List<Guid>()),
                CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RemoveCompensationPaymentAsParticipant_ShouldNotBeAuthorized()
        {
            var project = Any.Project();
            await _projectRepository.Add(project);

            var authorizer = new RemoveCompensationPaymentsCommandAuthorizer(_projectRepository, _uow, _executionContext);
            var authorizationResult = await authorizer.Authorize(RemoveCompensationPaymentsCommand.Create(project.ProjectId.Value, new List<Guid>()),
                CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
