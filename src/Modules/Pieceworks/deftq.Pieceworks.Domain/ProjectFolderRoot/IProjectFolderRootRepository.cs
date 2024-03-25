using deftq.BuildingBlocks.DataAccess;
namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public interface IProjectFolderRootRepository : IRepository<ProjectFolderRoot>
    {
        Task<ProjectFolderRoot> GetByProjectId(Guid id, CancellationToken cancellationToken = default);
    }
}