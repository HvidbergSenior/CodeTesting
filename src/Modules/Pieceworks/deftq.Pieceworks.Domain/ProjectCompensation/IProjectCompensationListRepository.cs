using deftq.BuildingBlocks.DataAccess;

namespace deftq.Pieceworks.Domain.projectCompensation
{
    public interface IProjectCompensationListRepository : IRepository<ProjectCompensationList>
    {
        Task<ProjectCompensationList> GetByProjectId(Guid projectId);
    }
}
