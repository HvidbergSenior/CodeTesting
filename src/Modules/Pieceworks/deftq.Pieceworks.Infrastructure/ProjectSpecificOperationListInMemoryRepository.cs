using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.projectSpecificOperation;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectSpecificOperationListInMemoryRepository : InMemoryRepository<ProjectSpecificOperationList>, IProjectSpecificOperationListRepository
    {
        public Task<ProjectSpecificOperationList> GetByProjectId(Guid projectId, CancellationToken cancellationToken)
        {
            var result = Entities.Values.Select(Deserialize).FirstOrDefault(x => x.ProjectId.Value == projectId);

            if (result == null)
            {
                throw new NotFoundException($"Unknown project Id {projectId}");
            }

            return Task.FromResult(result);
        }
    }
}
