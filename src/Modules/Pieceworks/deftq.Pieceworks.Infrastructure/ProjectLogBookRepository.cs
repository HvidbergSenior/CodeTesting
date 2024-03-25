using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Domain.projectLogBook;
using Marten;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectLogBookRepository: MartenDocumentRepository<ProjectLogBook>, IProjectLogBookRepository
    {
        public ProjectLogBookRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
            
        }
        
        public Task<ProjectLogBook> GetByProjectId(Guid id, CancellationToken cancellationToken = default)
        {
            var result = Query().FirstOrDefault(x => x.ProjectId.Value == id);
            if (result == null)
            {
                throw new NotFoundException($"ProjectLogBook With id {id} not found");
            }
            return Task.FromResult(result);
        }
    }
}
