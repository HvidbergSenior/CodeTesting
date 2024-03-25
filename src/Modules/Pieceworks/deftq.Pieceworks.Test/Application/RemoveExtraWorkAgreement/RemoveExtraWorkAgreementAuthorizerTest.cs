using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveExtraWorkAgreement;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveExtraWorkAgreement
{
    public class RemoveExtraWorkAgreementAuthorizerTest
    {
        private readonly FakeUnitOfWork _uow;
        private readonly ProjectInMemoryRepository _projectRepository;
        private readonly FakeExecutionContext _executionContext;

        public RemoveExtraWorkAgreementAuthorizerTest()
        {
            _uow = new FakeUnitOfWork();
            _projectRepository = new ProjectInMemoryRepository();
            _executionContext = new FakeExecutionContext();
        }

        [Fact]
        public async Task RemoveExtraWorkAgreementAsProjectOwner_ShouldBeAuthorized()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectRepository.Add(project);

            var authorizer = new RemoveExtraWorkAgreementCommandAuthorizer(_projectRepository, _uow, _executionContext);
            var authorizationResult = await authorizer.Authorize(RemoveExtraWorkAgreementCommand.Create(project.ProjectId.Value, new List<Guid>()),
                CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task RemoveExtraWorkAgreementAsProjectManager_ShouldBeAuthorized()
        {
            var project = Any.Project().WithProjectManager(_executionContext.UserId);
            await _projectRepository.Add(project);

            var authorizer = new RemoveExtraWorkAgreementCommandAuthorizer(_projectRepository, _uow, _executionContext);
            var authorizationResult = await authorizer.Authorize(RemoveExtraWorkAgreementCommand.Create(project.ProjectId.Value, new List<Guid>()),
                CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task RemoveExtraWorkAgreementAsNonOwnerAndNonProjectManager_ShouldNotBeAuthorized()
        {
            var project = Any.Project();
            await _projectRepository.Add(project);

            var authorizer = new RemoveExtraWorkAgreementCommandAuthorizer(_projectRepository, _uow, _executionContext);
            var authorizationResult = await authorizer.Authorize(RemoveExtraWorkAgreementCommand.Create(project.ProjectId.Value, new List<Guid>()),
                CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
