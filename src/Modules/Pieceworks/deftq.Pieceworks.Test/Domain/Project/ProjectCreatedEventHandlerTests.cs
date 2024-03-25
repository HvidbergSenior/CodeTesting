using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.project
{
    public class ProjectCreatedEventHandlerTests
    {
        [Fact]
        public async Task When_ProjectUser_Is_Updated_With_Ownership()
        {
            var projectUser = Any.ProjectUser();

            var repository = new ProjectUserInMemoryRepository();
            await repository.Add(projectUser);
            var projectId = Any.ProjectId();
            var name = Any.ProjectName();
            var projectDescription = Any.ProjectDescription();

            var projectOwner = Any.ProjectOwner(projectUser.ProjectUserId.Value);
            var evt = ProjectCreatedDomainEvent.Create(projectId, name, projectDescription, projectOwner, ProjectPieceworkType.TwelveOneC);

            var handler = new ProjectCreatedEventHandler(repository);

            await handler.Handle(evt, CancellationToken.None);

            var projectUserFromDatabase = await repository.GetById(projectOwner.Id);

            Assert.Contains(projectId, projectUserFromDatabase.OwnedProjects);
        }
    }
}
