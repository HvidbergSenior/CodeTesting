using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.Pieceworks.Domain.projectSpecificOperation;
using Marten;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectSpecificOperationListRepository : MartenDocumentRepository<ProjectSpecificOperationList>, IProjectSpecificOperationListRepository
    {
        public ProjectSpecificOperationListRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
        }

        public Task<ProjectSpecificOperationList> GetByProjectId(Guid id, CancellationToken cancellationToken)
        {
            var result = Query().FirstOrDefault(x => x.ProjectId.Value == id);
            if (result == null)
            {
                throw new ProjectSpecificOperationListNotFoundException($"Unknown project specific operation list id {id}");
            }

            return Task.FromResult(result);
        }
    }
}
