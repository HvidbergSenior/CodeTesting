using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Application.GetProjectUsers
{
    public sealed class GetProjectUsersQuery : IQuery<GetProjectUsersQueryResponse>
    {
        public ProjectId ProjectId { get; }

        private GetProjectUsersQuery(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static GetProjectUsersQuery Create(ProjectId projectId)
        {
            return new GetProjectUsersQuery(projectId);
        }
    }

    internal class GetProjectUsersQueryHandler : IQueryHandler<GetProjectUsersQuery, GetProjectUsersQueryResponse>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectUsersQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<GetProjectUsersQueryResponse> Handle(GetProjectUsersQuery query, CancellationToken cancellationToken)
        {
            var projectId = query.ProjectId.Value;

            var project = await _projectRepository.GetById(projectId, cancellationToken);
            var list = new List<ProjectUserResponse>();
            var owner = project.ProjectOwner;
            list.Add(new ProjectUserResponse(owner.Id, owner.Name, ProjectUserRole.Owner, "", null, null));
            list.AddRange(project.ProjectManagers.Select(m => new ProjectUserResponse(m.Id, m.Name, ProjectUserRole.Manager, m.Email.Value, m.Phone, m.Address)));
            list.AddRange(project.ProjectParticipants.Select(p => new ProjectUserResponse(p.Id, p.Name.Value, ProjectUserRole.Participant, p.Email.Value, p.PhoneNumber?.Value, p.Address?.Value)));
            return new GetProjectUsersQueryResponse(list);
        }
    }

    internal class GetProjectUsersQueryAuthorizer : IAuthorizer<GetProjectUsersQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetProjectUsersQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetProjectUsersQuery query, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellation);
            if (project.IsOwner(_executionContext.UserId) ||
                project.IsParticipant(_executionContext.UserId) ||
                project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
