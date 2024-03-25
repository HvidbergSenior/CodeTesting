using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.InvitationFlow;

namespace deftq.Pieceworks.Infrastructure.ProjectInvitation
{
    public class InMemoryInvitationRepository: InMemoryRepository<Invitation>, IInvitationRepository
    {
        public Task<Invitation> GetInvitationByMailAndProject(Guid projectId, string email, CancellationToken cancellationToken = default)
        {
            var result = Query().FirstOrDefault(x => x.ProjectId.Value == projectId && x.Email.Value == email);
            if (result == null)
            {
                throw new NotFoundException($"Invitation with email:{email} and projectId:{projectId} was not found");
            }
            return Task.FromResult(result);
        }

        async Task IInvitationRepository.DeleteAllInvitationsByProjectId(Guid projectId, CancellationToken cancellationToken)
        {
            await DeleteAllInvitationsByProjectId(projectId, cancellationToken);
        }

        public Task<Invitation> GetInvitationByInvitationIdAndRandomValue(Guid invitationId, string randomValue, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public Task<Invitation> DeleteAllInvitationsByProjectId(Guid projectId, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public Task<IEnumerable<Invitation>> GetInvitationsByProjectId(Guid ProjectId)
        {
            // placeholder
            return Task.FromResult<IEnumerable<Invitation>>(new List<Invitation>());
        }
    }
}
