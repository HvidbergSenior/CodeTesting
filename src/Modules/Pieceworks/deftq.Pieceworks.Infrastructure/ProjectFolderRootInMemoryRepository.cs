using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectFolderRootInMemoryRepository : InMemoryRepository<ProjectFolderRoot>, IProjectFolderRootRepository
    {
        public Task<ProjectFolderRoot> GetByProjectId(Guid id, CancellationToken cancellationToken = default)
        {
            var result = Entities.Values.Select(Deserialize).FirstOrDefault(x => x.ProjectId.Value == id);
            if (result == null)
            {
                throw new NotFoundException($"Unknown project id {id}");
            }
            return Task.FromResult(result);
        }
    }
}
