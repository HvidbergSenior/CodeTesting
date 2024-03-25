using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.Pieceworks.Domain.projectCompensation;
using Marten;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectCompensationListRepository : MartenDocumentRepository<ProjectCompensationList>, IProjectCompensationListRepository
    {
        public ProjectCompensationListRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
        }

        public Task<ProjectCompensationList> GetByProjectId(Guid projectId)
        {
            var result = Query().FirstOrDefault(x => x.ProjectId.Value == projectId);
            if (result == null)
            {
                throw new ProjectCompensationListNotFoundException(projectId);
            }

            return Task.FromResult(result);
        }
    }
}
