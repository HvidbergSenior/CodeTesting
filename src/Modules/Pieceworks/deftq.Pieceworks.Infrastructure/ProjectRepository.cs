using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.Pieceworks.Domain.project;
using Marten;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectRepository : MartenDocumentRepository<Project>, IProjectRepository
    {
        public ProjectRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
            
        }
    }
}
