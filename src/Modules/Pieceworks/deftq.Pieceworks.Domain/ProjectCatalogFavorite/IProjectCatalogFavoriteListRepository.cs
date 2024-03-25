using deftq.BuildingBlocks.DataAccess;

namespace deftq.Pieceworks.Domain.ProjectCatalogFavorite
{
    public interface IProjectCatalogFavoriteListRepository : IRepository<ProjectCatalogFavoriteList>
    {
        Task<ProjectCatalogFavoriteList> GetByProjectId(Guid projectId);
    }
}
