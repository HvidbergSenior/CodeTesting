using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Application.CreateProjectFolder;
using deftq.Pieceworks.Domain.InvitationFlow;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Application.CreateProjectInvitation
{
    public sealed class CreateProjectInvitationCommand: ICommand<ICommandResponse>
    {
        public InvitationId InvitationId { get; }
        internal ProjectId ProjectId { get;}
        public InvitationEmail Email { get;}

        public CreateProjectInvitationCommand(InvitationId invitationId, ProjectId projectId, InvitationEmail invitationEmail)
        {
            InvitationId = invitationId;
            ProjectId= projectId;
            Email= invitationEmail;
        }
        public static CreateProjectInvitationCommand Create(Guid invitationId, Guid projectId, string email)
        {
            return new CreateProjectInvitationCommand(InvitationId.Create(invitationId), ProjectId.Create(projectId), InvitationEmail.Create(email));
        }
    }

    internal class CreateProjectInvitationCommandHandler : ICommandHandler<CreateProjectInvitationCommand, ICommandResponse>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unit;

        public CreateProjectInvitationCommandHandler(IInvitationRepository invitationRepository, IProjectRepository projectRepository, IUnitOfWork unit)
        {
            _invitationRepository = invitationRepository;
            _projectRepository = projectRepository;
            _unit = unit;
        }
        public async Task<ICommandResponse> Handle(CreateProjectInvitationCommand request, CancellationToken cancellationToken)
        {
            var invitation = Invitation.Create(
                request.InvitationId,
                request.ProjectId,
                request.Email,
                InvitationRandomValue.NewValue(), 
                InvitationExpiration.Default(),
                InvitationRetries.Empty()
            );
            await _invitationRepository.Add(invitation, cancellationToken);
            await _invitationRepository.SaveChanges(cancellationToken);
            await _unit.Commit(cancellationToken);
            return EmptyCommandResponse.Default;
        }
    }
    
    internal class CreateProjectInvitationCommandAuthorizer : IAuthorizer<CreateProjectInvitationCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public CreateProjectInvitationCommandAuthorizer(IExecutionContext executionContext, IUnitOfWork unitOfWork, IProjectRepository projectRepository)
        {
            _executionContext = executionContext;
            _unitOfWork = unitOfWork;
            _projectRepository = projectRepository;
        }

        public async Task<AuthorizationResult> Authorize(CreateProjectInvitationCommand command, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellation);
            if (project.IsOwner(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
    
    
}
