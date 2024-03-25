using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectInMemoryRepository : InMemoryRepository<Project>, IProjectRepository
    {
        
    }
}
