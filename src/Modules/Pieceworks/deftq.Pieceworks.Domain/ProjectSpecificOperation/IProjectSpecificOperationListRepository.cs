using deftq.BuildingBlocks.DataAccess;

namespace deftq.Pieceworks.Domain.projectSpecificOperation
{
    public interface IProjectSpecificOperationListRepository : IRepository<ProjectSpecificOperationList>
    {
        Task<ProjectSpecificOperationList> GetByProjectId(Guid id, CancellationToken cancellationToken);
    }
}
