using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectCatalogFavoriteListInMemoryRepository : InMemoryRepository<ProjectCatalogFavoriteList>, IProjectCatalogFavoriteListRepository
    {
        public Task<ProjectCatalogFavoriteList> GetByProjectId(Guid projectId)
        {
            var result = Entities.Values.Select(Deserialize).FirstOrDefault(x => x.ProjectId.Value == projectId);
            if (result == null)
            {
                throw new ProjectCatalogFavoriteListNotFoundException(projectId);
            }
            return Task.FromResult(result);
        }
    }
}
