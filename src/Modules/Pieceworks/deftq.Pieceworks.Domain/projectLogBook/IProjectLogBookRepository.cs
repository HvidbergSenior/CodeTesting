using deftq.BuildingBlocks.DataAccess;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public interface IProjectLogBookRepository : IRepository<ProjectLogBook>
    {
        Task<ProjectLogBook> GetByProjectId(Guid id, CancellationToken cancellationToken = default);
    }
}
