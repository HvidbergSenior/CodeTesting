using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.Pieceworks.Domain.projectDocument;
using Marten;

namespace deftq.Pieceworks.Domain.projectFolder
{
    public class ProjectDocumentRepository : MartenDocumentRepository<ProjectDocument>, IProjectDocumentRepository
    {
         public ProjectDocumentRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
            
        }
    }
}