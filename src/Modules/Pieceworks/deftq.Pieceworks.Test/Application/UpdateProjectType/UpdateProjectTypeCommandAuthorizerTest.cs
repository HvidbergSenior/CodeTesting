using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectType;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateProjectType
{
    public class UpdateProjectTypeCommandAuthorizerTest
    {
        [Fact]
        public async Task UpdateProjectTypeAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectTypeCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    UpdateProjectTypeCommand.Create(project.ProjectId.Value, PieceworkType.TwelveOneA, ProjectStartDate.Empty(),
                        ProjectEndDate.Empty(), ProjectLumpSumPayment.Empty()), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task UpdateProjectTypeAsProjectManager_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectTypeCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    UpdateProjectTypeCommand.Create(project.ProjectId.Value, PieceworkType.TwelveOneA, ProjectStartDate.Empty(),
                        ProjectEndDate.Empty(), ProjectLumpSumPayment.Empty()), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task UpdateProjectTypeAsNonProjectManagerAndNonOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectTypeCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    UpdateProjectTypeCommand.Create(project.ProjectId.Value, PieceworkType.TwelveOneA, ProjectStartDate.Empty(),
                        ProjectEndDate.Empty(), ProjectLumpSumPayment.Empty()), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
