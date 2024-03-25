using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterExtraWorkAgreement;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RegisterExtraWorkAgreement
{
    public class RegisterExtraWorkAgreementCommandAuthorizerTest
    {
        [Fact]
        public async Task RegisterExtraWorkAgreementAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RegisterExtraWorkAgreementCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RegisterExtraWorkAgreementCommand.Create(project.ProjectId.Value, Any.Guid(),
                        Any.String(10), Any.String(10),
                        Any.String(10), ExtraWorkAgreementType.Other, Decimal.One, 20,
                        20), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RegisterExtraWorkAgreementAsProjectManager_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RegisterExtraWorkAgreementCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RegisterExtraWorkAgreementCommand.Create(project.ProjectId.Value, Any.Guid(),
                        Any.String(10), Any.String(10),
                        Any.String(10), ExtraWorkAgreementType.Other, Decimal.One,
                        20, 20), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RegisterExtraWorkAgreementAsNonOwnerAndNonProjectManager_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new RegisterExtraWorkAgreementCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RegisterExtraWorkAgreementCommand.Create(project.ProjectId.Value, Any.Guid(),
                        Any.String(10), Any.String(10),
                        Any.String(10), ExtraWorkAgreementType.Other, Decimal.One,
                        20, 20), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
