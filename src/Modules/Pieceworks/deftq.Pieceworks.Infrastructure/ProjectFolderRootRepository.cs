using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.BuildingBlocks.Exceptions;
using Marten;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectFolderRootRepository : MartenDocumentRepository<ProjectFolderRoot>, IProjectFolderRootRepository
    {
        public ProjectFolderRootRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession,
            aggregateEventsPublisher)
        {
        }

        public Task<ProjectFolderRoot> GetByProjectId(Guid id, CancellationToken cancellationToken = default)
        {
            var result = Query().FirstOrDefault(x => x.ProjectId.Value == id);
            if (result == null)
            {
                throw new NotFoundException($"Unknown project id {id}");
            }

            return Task.FromResult(result);
        }
    }
}
