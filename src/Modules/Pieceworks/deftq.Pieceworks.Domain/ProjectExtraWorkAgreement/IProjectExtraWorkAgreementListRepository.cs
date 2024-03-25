using deftq.BuildingBlocks.DataAccess;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public interface IProjectExtraWorkAgreementListRepository : IRepository<ProjectExtraWorkAgreementList>
    {
        Task<ProjectExtraWorkAgreementList> GetByProjectId(Guid id, CancellationToken cancellationToken);
    }
}
