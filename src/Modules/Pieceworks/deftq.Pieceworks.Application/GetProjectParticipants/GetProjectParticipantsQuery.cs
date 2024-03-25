using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain.InvitationFlow;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Application.GetProjectParticipants
{
    public sealed class GetProjectParticipantsQuery : IQuery<GetProjectParticipantsResponse>
    {
        public ProjectId ProjectId { get; }

        private GetProjectParticipantsQuery(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static GetProjectParticipantsQuery Create(ProjectId projectId)
        {
            return new GetProjectParticipantsQuery(projectId);
        }
    }

    internal class GetProjectParticipantsQueryHandler : IQueryHandler<GetProjectParticipantsQuery, GetProjectParticipantsResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IExecutionContext _executionContext;

        public GetProjectParticipantsQueryHandler(IProjectRepository projectRepository, IExecutionContext executionContext, IInvitationRepository invitationRepository)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
            _invitationRepository = invitationRepository;
        }

        public async Task<GetProjectParticipantsResponse> Handle(GetProjectParticipantsQuery query, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellationToken);
            var invitations = await _invitationRepository.GetInvitationsByProjectId(query.ProjectId.Value);

            var participants = project.ProjectParticipants.Select(p => new GetProjectParticipantResponse(p.Name.Value, ProjectParticipantRole.ProjectParticipant, "")).ToList();

            participants.Add(new GetProjectParticipantResponse(project.ProjectOwner.Name, ProjectParticipantRole.ProjectOwner, ""));

            var managers = project.ProjectManagers.Select(m => new GetProjectParticipantResponse(m.Name, ProjectParticipantRole.ProjectManager, ""));
            participants.AddRange(managers);
            FillInvitations(participants, invitations.ToList());
            return new GetProjectParticipantsResponse(participants);
        }

        private void FillInvitations(List<GetProjectParticipantResponse> participants, List<Invitation>? invitations)
        {
            if (invitations is null || invitations.Count < 1)
            {
                return;
            }

            for (var i = 0; i != participants.Count; i++)
            {
                var current = participants[i];
                // must be changed to compare participantId, when that is done instead of email.
                // So for not, I am doing a simple email comparison
                var targetInvitation = invitations.SingleOrDefault(x => string.Equals(x.Email.Value, current.Email, StringComparison.OrdinalIgnoreCase));
                current.SetParticipantInvitation(targetInvitation);
            }
        }
    }

    internal class GetProjectParticipantsQueryAuthorizer : IAuthorizer<GetProjectParticipantsQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetProjectParticipantsQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetProjectParticipantsQuery query, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellation);
            if (project.IsOwner(_executionContext.UserId) || project.IsParticipant(_executionContext.UserId) ||
                project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
