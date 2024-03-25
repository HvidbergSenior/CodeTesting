using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectFolderWorkInMemoryRepository : InMemoryRepository<ProjectFolderWork>, IProjectFolderWorkRepository
    {
        public Task<ProjectFolderWork> GetByProjectAndFolderId(Guid projectId, Guid folderId, CancellationToken cancellationToken = default)
        {
            var result = Entities.Values.Select(Deserialize)
                .FirstOrDefault(x => x.ProjectId.Value == projectId && x.ProjectFolderId.Value == folderId);
            if (result == null)
            {
                throw new ProjectFolderNotFoundException(folderId);
            }
            return Task.FromResult(result);
        }

        public Task<IList<ProjectFolderWork>> GetByProjectAndFolderIds(Guid projectId, IList<Guid> folderIds,
            CancellationToken cancellationToken = default)
        {
            IList<ProjectFolderWork> folderWorks = Entities.Values.Select(Deserialize)
                .Where(x => x.ProjectId.Value == projectId && folderIds.Contains(x.ProjectFolderId.Value)).ToList();

            if (folderIds.Count != folderWorks.Count)
            {
                throw new NotFoundException($"Could not find all work items for project id {projectId} and folder ids {folderIds}");
            }

            return Task.FromResult(folderWorks);
        }

        public Task<IList<ProjectFolderWork>> GetByProjectId(Guid projectId, CancellationToken cancellationToken = default)
        {
            IList<ProjectFolderWork> folderWorks = Entities.Values.Select(Deserialize)
                .Where(x => x.ProjectId.Value == projectId).ToList();
            
            return Task.FromResult(folderWorks);
        }
    }
}
