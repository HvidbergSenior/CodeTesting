using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Domain.InvitationFlow;
using deftq.Pieceworks.Domain.project;
using Marten;

namespace deftq.Pieceworks.Infrastructure
{
    public class InvitationRepository : MartenDocumentRepository<Invitation>, IInvitationRepository
    {
        private readonly IDocumentSession _documentSession;
        public InvitationRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
            _documentSession = documentSession;
        }

        public Task DeleteAllInvitationsByProjectId(Guid projectId, CancellationToken cancellationToken)
        {
            _documentSession.DeleteWhere<Invitation>(x => x.ProjectId.Value == projectId);
            return Task.CompletedTask;
        }

        public async Task<Invitation> GetInvitationByInvitationIdAndRandomValue(Guid invitationId, string randomValue, CancellationToken cancellationToken)
        {
            var result = await Query().FirstOrDefaultAsync(x => x.InvitationId.Value == invitationId && x.RandomValue.Value.Equals(randomValue, StringComparison.OrdinalIgnoreCase), cancellationToken);
            if (result is null)
            {
                throw new NotFoundException($"Could not find invitation for invitation id {invitationId} and random value {randomValue}");
            }
            return result;
        }

        public async Task<Invitation> GetInvitationByMailAndProject(Guid projectId, string email, CancellationToken cancellationToken = default)
        {
            // to do: implement
            var result = await Query().FirstOrDefaultAsync(x => x.ProjectId.Value == projectId && x.Email.Value == email, cancellationToken);
            if (result is null)
            {
                throw new NotFoundException($"Could not find invitation for project id {projectId} and email {email}");
            }
            return result;
        }

        async Task IInvitationRepository.DeleteAllInvitationsByProjectId(Guid projectId, CancellationToken cancellationToken)
        {
            await DeleteAllInvitationsByProjectId(projectId, cancellationToken);
        }

        public async Task<IEnumerable<Invitation>> GetInvitationsByProjectId(Guid projectId)
        {
            return await Query().Where(x => x.ProjectId.Value == projectId).ToListAsync();
        }
    }
}
