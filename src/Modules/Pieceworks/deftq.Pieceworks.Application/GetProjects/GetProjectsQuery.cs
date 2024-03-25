using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Application.CreateProject;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Application.GetProjects
{
    public sealed class GetProjectsQuery : IQuery<GetProjectsResponse>
    {
        private GetProjectsQuery()
        {
        }

        public static GetProjectsQuery Create()
        {
            return new GetProjectsQuery();
        }
    }

    internal class GetProjectsQueryHandler : IQueryHandler<GetProjectsQuery, GetProjectsResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetProjectsQueryHandler(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<GetProjectsResponse> Handle(GetProjectsQuery query, CancellationToken cancellationToken)
        {
            var userId = _executionContext.UserId;

            var participantProjects = await _projectRepository.Query()
                .Where(p => p.ProjectParticipants.Any(pp => pp.Id == userId))
                .ToListAsync(cancellationToken);
            
            var ownedProjects = await _projectRepository.Query()
                .Where(p => p.ProjectOwner.Id == userId)
                .ToListAsync(cancellationToken);

            var managedProjects = await _projectRepository.Query()
                .Where(p => p.ProjectManagers.Any(pp => pp.Id == userId))
                .ToListAsync(cancellationToken);
            
            return MapResponse(participantProjects.Concat(ownedProjects).Concat(managedProjects).ToList());
        }

        private static GetProjectsResponse MapResponse(IReadOnlyList<Project> projects)
        {
            return new GetProjectsResponse(projects);
        }
    }

    public class GetProjectsResponse
    {
        public IList<ProjectResponse> Projects { get; }

        public GetProjectsResponse(IReadOnlyList<Project> projects)
        {
            Projects = projects.Select(p =>
                    new ProjectResponse(p.ProjectId.Value, p.ProjectName.Value, p.ProjectDescription.Value, p.ProjectPieceworkType.FromDomain()))
                .ToList();
        }
    }

    public class ProjectResponse
    {
        public Guid ProjectId { get; }
        public string ProjectName { get; }
        public string Description { get; }
        public PieceworkType PieceworkType { get; }

        public ProjectResponse(Guid projectId, string projectName, string description, PieceworkType pieceworkType)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            Description = description;
            PieceworkType = pieceworkType;
        }
    }

    internal class GetProjectsQueryValidator : AbstractCommandValidator<GetProjectsQuery>
    {
        public GetProjectsQueryValidator()
        {
        }
    }

    public class GetProjectsQueryAuthorizer : IAuthorizer<GetProjectsQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetProjectsQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public Task<AuthorizationResult> Authorize(GetProjectsQuery query, CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}
