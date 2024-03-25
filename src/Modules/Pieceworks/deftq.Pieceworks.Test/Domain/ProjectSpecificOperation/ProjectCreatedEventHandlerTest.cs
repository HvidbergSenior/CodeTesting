
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using ProjectCreatedEventHandler = deftq.Pieceworks.Domain.projectSpecificOperation.ProjectCreatedEventHandler;

namespace deftq.Pieceworks.Test.Domain.ProjectSpecificOperation
{
    public class ProjectCreatedEventHandlerTest
    {
        [Fact]
        public async Task Should_Create_ProjectSpecificOperationsList_When_Project_Is_Created()
        {
            var projectId = Any.ProjectId();
            var projectSpecificOperationListRepository = new ProjectSpecificOperationListInMemoryRepository();
            var evt = ProjectCreatedDomainEvent.Create(projectId, Any.ProjectName(), Any.ProjectDescription(), Any.ProjectOwner(),
                ProjectPieceworkType.TwelveTwo);

            var handler = new ProjectCreatedEventHandler(projectSpecificOperationListRepository);
            await handler.Handle(evt, CancellationToken.None);

            var projectSpecificOperationListFromRepository = await projectSpecificOperationListRepository.GetByProjectId(projectId.Value, CancellationToken.None);

            Assert.Equal(projectId, projectSpecificOperationListFromRepository.ProjectId);
        }
    }
}
