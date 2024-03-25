using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;
using Marten;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectCatalogFavoriteListRepository : MartenDocumentRepository<ProjectCatalogFavoriteList>, IProjectCatalogFavoriteListRepository
    {
        public ProjectCatalogFavoriteListRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
        }

        public Task<ProjectCatalogFavoriteList> GetByProjectId(Guid projectId)
        {
            var result = Query().FirstOrDefault(x => x.ProjectId.Value == projectId);
            if (result == null)
            {
                throw new ProjectCatalogFavoriteListNotFoundException(projectId);
            }

            return Task.FromResult(result);
        }
    }
}
