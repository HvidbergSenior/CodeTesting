using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using FluentAssertions;
using Xunit;
using ProjectCreatedEventHandler = deftq.Pieceworks.Domain.ProjectCatalogFavorite.ProjectCreatedEventHandler;

namespace deftq.Pieceworks.Test.Domain.ProjectCatalogFavorite
{
    public class ProjectCreatedEventHandlerTest
    {
        [Fact]
        public async Task WhenProjectIsCreated_CatalogFavoriteListIsCreate()
        {
            // When
            var repo = new ProjectCatalogFavoriteListInMemoryRepository();
            var projectCreatedEventHandler = new ProjectCreatedEventHandler(repo);
            await projectCreatedEventHandler.Handle(ProjectCreatedDomainEvent.Create(Any.ProjectId(), Any.ProjectName(), Any.ProjectDescription(),
                Any.ProjectOwner(), ProjectPieceworkType.TwelveOneA), CancellationToken.None);
            
            // Then
            repo.Entities.Should().ContainSingle();
        }
    }
}
