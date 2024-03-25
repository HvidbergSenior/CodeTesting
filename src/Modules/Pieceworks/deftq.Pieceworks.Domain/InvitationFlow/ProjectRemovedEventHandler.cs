using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.InvitationFlow
{
    public class ProjectRemovedEventHandler : IDomainEventListener<ProjectRemovedDomainEvent>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectRemovedEventHandler(IInvitationRepository invitationRepository, IUnitOfWork unitOfWork)
        {
            _invitationRepository = invitationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProjectRemovedDomainEvent notification, CancellationToken cancellationToken)
        {
            await _invitationRepository.DeleteAllInvitationsByProjectId(notification.ProjectId.Value, cancellationToken);
            await _invitationRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
        }
    }
}
