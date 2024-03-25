using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Application.CreateProject;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Application.GetProject
{
    public sealed class GetProjectQuery : IQuery<GetProjectResponse>
    {
        public ProjectId ProjectId { get; }

        private GetProjectQuery(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static GetProjectQuery Create(ProjectId projectId)
        {
            return new GetProjectQuery(projectId);
        }
    }

    internal class GetProjectQueryHandler : IQueryHandler<GetProjectQuery, GetProjectResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetProjectQueryHandler(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<GetProjectResponse> Handle(GetProjectQuery query, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellationToken);

            var participants = project.ProjectParticipants.Select(p => new ProjectParticipant(p.Id, p.Name.Value)).ToList();
            ProjectRole userRole = MapFromDomainRole(project, _executionContext.UserId);

            DateOnly? startDate = project.ProjectStartDate == ProjectStartDate.Empty() ? null : project.ProjectStartDate.Value;
            DateOnly? endDate = project.ProjectEndDate == ProjectEndDate.Empty() ? null : project.ProjectEndDate.Value;

            decimal? lumpSumPaymentDkr = project.ProjectLumpSumPaymentDkr.IsEmpty() ? null : project.ProjectLumpSumPaymentDkr.Value;

            var companyName = project.ProjectCompany.ProjectCompanyName.Value;
            var companyAddress = project.ProjectCompany.ProjectAddress.Value;
            var companyCvrNo = project.ProjectCompany.ProjectCompanyCvrNo.Value;
            var companyPNo = project.ProjectCompany.ProjectCompanyPNo.Value;

            return new GetProjectResponse(project.ProjectId.Value, project.ProjectName.Value, project.ProjectNumber.Value, project.ProjectPieceWorkNumber.Value,
                project.ProjectOrderNumber.Value, project.ProjectDescription.Value, project.ProjectPieceworkType.FromDomain(), lumpSumPaymentDkr, participants, userRole, startDate,
                endDate, project.ProjectCreatedTime.Value, companyName, companyAddress, companyCvrNo, companyPNo);
        }

        private static ProjectRole MapFromDomainRole(Project project, Guid userId)
        {
            if (project.IsOwner(userId))
            {
                return ProjectRole.ProjectOwner;
            }
            
            if (project.IsParticipant(userId))
            {
                return ProjectRole.ProjectParticipant;
            }
            
            if (project.IsProjectManager(userId))
            {
                return ProjectRole.ProjectManager;
            }
            
            throw new UserNotAffiliatedWithProjectException();
        }
    }

    internal class UserNotAffiliatedWithProjectException : NotFoundException
    {
        public UserNotAffiliatedWithProjectException() : base() { }

        public UserNotAffiliatedWithProjectException(Guid entityId) : base(entityId) { }

        public UserNotAffiliatedWithProjectException(ProjectId projectProjectId, Guid executionContextUserId) : base(
            $"UserId {executionContextUserId} not found in project {projectProjectId}.")
        {
        }

        public UserNotAffiliatedWithProjectException(string message) : base(message) { }

        public UserNotAffiliatedWithProjectException(string message, Exception inner) : base(message, inner) { }
    }

    internal class GetProjectQueryValidator : AbstractCommandValidator<GetProjectQuery>
    {
        public GetProjectQueryValidator()
        {
        }
    }

    internal class GetProjectQueryAuthorizer : IAuthorizer<GetProjectQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetProjectQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetProjectQuery query, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellation);
            if (project.IsOwner(_executionContext.UserId) || project.IsParticipant(_executionContext.UserId) || project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
