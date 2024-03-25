using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.projectUser;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectUserInMemoryRepository : InMemoryRepository<ProjectUser>, IProjectUserRepository
    {
    }
}
