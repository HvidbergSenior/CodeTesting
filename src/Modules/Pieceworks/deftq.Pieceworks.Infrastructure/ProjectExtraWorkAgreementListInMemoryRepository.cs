using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectExtraWorkAgreementListInMemoryRepository : InMemoryRepository<ProjectExtraWorkAgreementList>, IProjectExtraWorkAgreementListRepository
    {
        public Task<ProjectExtraWorkAgreementList> GetByProjectId(Guid id, CancellationToken cancellationToken)
        {
            var result = Entities.Values.Select(Deserialize).FirstOrDefault(x => x.ProjectId.Value == id);
            if (result == null)
            {
                throw new NotFoundException($"Unknown project Id {id}");
            }
            return Task.FromResult(result);
        }
    }
}
