using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using ProjectCreatedEventHandler = deftq.Pieceworks.Domain.projectCompensation.ProjectCreatedEventHandler;

namespace deftq.Pieceworks.Test.Domain.ProjectCompensation
{
    public class ProjectCreatedEventHandlerTest
    {
        [Fact]
        public async Task WhenProjectIsCreated_CompensationListIsCreated()
        {
            var repository = new ProjectCompensationListInMemoryRepository();
            var projectCreatedEventHandler = new ProjectCreatedEventHandler(repository);
            await projectCreatedEventHandler.Handle(ProjectCreatedDomainEvent.Create(Any.ProjectId(),
                Any.ProjectName(), Any.ProjectDescription(),
                Any.ProjectOwner(), ProjectPieceworkType.TwelveOneA), CancellationToken.None);

            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
        }
    }
}
