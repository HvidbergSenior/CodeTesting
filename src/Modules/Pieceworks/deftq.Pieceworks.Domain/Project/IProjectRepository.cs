using deftq.BuildingBlocks.DataAccess;

namespace deftq.Pieceworks.Domain.project
{
    public interface IProjectRepository : IRepository<Project>, IReadonlyRepository<Project>
    {
        
    }
}
