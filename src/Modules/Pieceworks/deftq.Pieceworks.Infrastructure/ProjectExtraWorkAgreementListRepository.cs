using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using Marten;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectExtraWorkAgreementListRepository : MartenDocumentRepository<ProjectExtraWorkAgreementList>, IProjectExtraWorkAgreementListRepository
    {
        public ProjectExtraWorkAgreementListRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
        }

        public Task<ProjectExtraWorkAgreementList> GetByProjectId(Guid id, CancellationToken cancellationToken)
        {
            var result = Query().FirstOrDefault(x => x.ProjectId.Value == id);
            if (result == null)
            {
                throw new ProjectExtraWorkAgreementListNotFoundException($"Unknown extra work agreement list id {id}");
            }

            return Task.FromResult(result);
        }
    }
}
