using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateExtraWorkAgreement;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateExtraWorkAgreement
{
    public class UpdateExtraWorkAgreementCommandAuthorizerTest
    {
        [Fact]
        public async Task UpdateExtraWorkAgreementAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var unitOfWork = new FakeUnitOfWork();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateExtraWorkAgreementCommandAuthorizer(projectRepository, unitOfWork, executionContext);
            var authorizationResult = await authorizer.Authorize(
                UpdateExtraWorkAgreementCommand.Create(project.ProjectId.Value, Any.Guid(),
                    Any.String(10), Any.String(10),
                    Any.String(10), UpdateExtraWorkAgreementType.Other, Decimal.One, 20,
                    20), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task UpdateExtraWorkAgreementAsProjectManager_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var unitOfWork = new FakeUnitOfWork();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateExtraWorkAgreementCommandAuthorizer(projectRepository, unitOfWork, executionContext);
            var authorizationResult = await authorizer.Authorize(
                UpdateExtraWorkAgreementCommand.Create(project.ProjectId.Value, Any.Guid(),
                    Any.String(10), Any.String(10),
                    Any.String(10), UpdateExtraWorkAgreementType.Other, Decimal.One, 20,
                    20), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task UpdateExtraWorkAgreementAsNonOwnerAndNonProjectManager_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var unitOfWork = new FakeUnitOfWork();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new UpdateExtraWorkAgreementCommandAuthorizer(projectRepository, unitOfWork, executionContext);
            var authorizationResult = await authorizer.Authorize(
                UpdateExtraWorkAgreementCommand.Create(project.ProjectId.Value, Any.Guid(),
                    Any.String(10), Any.String(10),
                    Any.String(10), UpdateExtraWorkAgreementType.Other, Decimal.One, 20,
                    20), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
