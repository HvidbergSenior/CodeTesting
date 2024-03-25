using deftq.BuildingBlocks.Application.Generators;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using ProjectCreatedEventHandler = deftq.Pieceworks.Domain.projectLogBook.ProjectCreatedEventHandler;

namespace deftq.Pieceworks.Test.Domain.projectLogBook
{
    public class ProjectCreatedEventHandlerTest
    {
        [Fact]
        public async Task Should_Create_ProjectLogBook_When_Project_Is_Created()
        {
            var projectId = Any.ProjectId();
            var projectName = Any.ProjectName();
            var projectOwner = Any.ProjectOwner();
            var projectDescription = Any.ProjectDescription();

            var repository = new ProjectLogBookInMemoryRepository();
            var evt = ProjectCreatedDomainEvent.Create(projectId, projectName, projectDescription, projectOwner, ProjectPieceworkType.TwelveTwo);
            
            var handler = new ProjectCreatedEventHandler(repository, new GuidIdGenerator());

            await handler.Handle(evt, CancellationToken.None);
            var projectLogBookFromRepository = await repository.GetByProjectId(projectId.Value);
            
            Assert.Equal(projectId, projectLogBookFromRepository.ProjectId);
        }
    }
}
