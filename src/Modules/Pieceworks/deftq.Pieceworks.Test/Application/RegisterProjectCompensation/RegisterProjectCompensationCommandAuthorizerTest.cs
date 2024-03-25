using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterProjectCompensation;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RegisterProjectCompensation
{
    public class RegisterProjectCompensationCommandAuthorizerTest
    {
        [Fact]
        public async Task RegisterCompensationAsOwner_ShouldBeAuthorized()
        {
            var ctx = CancellationToken.None;
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project, ctx);

            var authorizer = new RegisterProjectCompensationCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RegisterProjectCompensationCommand.Create(project.ProjectId.Value, Any.Guid(), new List<Guid>(), Decimal.One,
                        ProjectCompensationDate.Create(DateOnly.MinValue), ProjectCompensationDate.Create(DateOnly.MaxValue)), ctx);
            Assert.True(authorizationResult.IsAuthorized);
        }
        [Fact]
        public async Task RegisterCompensationAsNonOwner_ShouldNotBeAuthorized()
        {
            var ctx = CancellationToken.None;
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project, ctx);

            var authorizer = new RegisterProjectCompensationCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RegisterProjectCompensationCommand.Create(project.ProjectId.Value, Any.Guid(), new List<Guid>(), Decimal.One,
                        ProjectCompensationDate.Create(DateOnly.MinValue), ProjectCompensationDate.Create(DateOnly.MaxValue)), ctx);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
