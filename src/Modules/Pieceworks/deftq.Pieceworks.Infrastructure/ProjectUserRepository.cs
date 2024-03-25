using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.Pieceworks.Domain.projectUser;
using Marten;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectUserRepository : MartenDocumentRepository<ProjectUser>, IProjectUserRepository
    {
        public ProjectUserRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
            
        }
    }
}
