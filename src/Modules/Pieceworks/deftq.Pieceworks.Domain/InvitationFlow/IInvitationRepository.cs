using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.DataAccess.Marten;

namespace deftq.Pieceworks.Domain.InvitationFlow
{
    public interface IInvitationRepository : IRepository<Invitation>
    {
        public Task<IEnumerable<Invitation>> GetInvitationsByProjectId(Guid ProjectId);
        public Task<Invitation> GetInvitationByMailAndProject(Guid projectId, string email, CancellationToken cancellationToken = default);
        public Task DeleteAllInvitationsByProjectId(Guid projectId, CancellationToken cancellationToken);
        public Task<Invitation> GetInvitationByInvitationIdAndRandomValue(Guid invitationId, string randomValue, CancellationToken cancellationToken);
    }
}
