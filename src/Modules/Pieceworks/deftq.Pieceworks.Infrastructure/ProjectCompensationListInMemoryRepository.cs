using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.projectCompensation;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectCompensationListInMemoryRepository : InMemoryRepository<ProjectCompensationList>, IProjectCompensationListRepository
    {
        public Task<ProjectCompensationList> GetByProjectId(Guid projectId)
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
