using deftq.BuildingBlocks.Application.Generators;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using ProjectCreatedEventHandler = deftq.Pieceworks.Domain.projectFolderRoot.ProjectCreatedEventHandler;

namespace deftq.Pieceworks.Test.Domain.projectFolderRoot
{
    public class ProjectCreatedEventHandlerTest
    {
        [Fact]
        public async Task Should_Create_ProjectFolderRoot_When_Project_Is_Created()
        {
            var projectId = Any.ProjectId();
            var repository = new ProjectFolderRootInMemoryRepository();
            var baseRateAndSupplementRepository = new BaseRateAndSupplementRepository();
            var evt = ProjectCreatedDomainEvent.Create(projectId, Any.ProjectName(), Any.ProjectDescription(), Any.ProjectOwner(), ProjectPieceworkType.TwelveTwo);
            
            var handler = new ProjectCreatedEventHandler(repository, baseRateAndSupplementRepository, new GuidIdGenerator());
            await handler.Handle(evt, CancellationToken.None);

            var projectFolderRootFromRepository = await repository.GetByProjectId(projectId.Value);
            
            Assert.Equal(projectId, projectFolderRootFromRepository.ProjectId);
        }
    }
}
