using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Domain.FolderWork;
using Marten;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectFolderWorkRepository : MartenDocumentRepository<ProjectFolderWork>, IProjectFolderWorkRepository
    {
         public ProjectFolderWorkRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
            
        }

         public async Task<ProjectFolderWork> GetByProjectAndFolderId(Guid projectId, Guid folderId, CancellationToken cancellationToken = default)
         {
             var result = await Query().FirstOrDefaultAsync(x => x.ProjectId.Value == projectId && x.ProjectFolderId.Value == folderId, cancellationToken);
             if (result == null)
             {
                 throw new NotFoundException($"Could not find work items for project id {projectId} and folder id  {folderId}");
             }
             return result;
         }

         public Task<IList<ProjectFolderWork>> GetByProjectAndFolderIds(Guid projectGuid, IList<Guid> folderGuids, CancellationToken cancellationToken = default)
         {
             var projectFolderWorks = Query()
                     .Where(folderWork => folderWork.ProjectId.Value == projectGuid && folderGuids.Contains(folderWork.ProjectFolderId.Value))
                     .ToList();
                 
             if (folderGuids.Count != projectFolderWorks.Count)
             {
                 throw new NotFoundException($"Could not find all work items for project id {projectGuid} and folder ids {folderGuids}");
             }

             return Task.FromResult<IList<ProjectFolderWork>>(projectFolderWorks);
         }

         public async Task<IList<ProjectFolderWork>> GetByProjectId(Guid projectGuid, CancellationToken cancellationToken = default)
         {
             var projectFolderWorks = await Query()
                 .Where(folderWork => folderWork.ProjectId.Value == projectGuid)
                 .ToListAsync(cancellationToken);

             return projectFolderWorks.ToList();
         }
    }
}
