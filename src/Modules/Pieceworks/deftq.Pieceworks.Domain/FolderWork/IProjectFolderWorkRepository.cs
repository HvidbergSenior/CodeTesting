using deftq.BuildingBlocks.DataAccess;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public interface IProjectFolderWorkRepository : IRepository<ProjectFolderWork>
    {
        Task<ProjectFolderWork> GetByProjectAndFolderId(Guid projectId, Guid folderId, CancellationToken cancellationToken = default);
        Task<IList<ProjectFolderWork>> GetByProjectAndFolderIds(Guid projectGuid, IList<Guid> folderGuids, CancellationToken cancellationToken = default);
        Task<IList<ProjectFolderWork>> GetByProjectId(Guid projectIdValue, CancellationToken cancellationToken = default);
    }
}
