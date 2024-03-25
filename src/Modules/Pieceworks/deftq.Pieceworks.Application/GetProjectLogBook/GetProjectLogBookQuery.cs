using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;


namespace deftq.Pieceworks.Application.GetProjectLogBook
{
    public sealed class GetProjectLogBookQuery : IQuery<GetProjectLogBookQueryResponse>
    {
        public ProjectId ProjectId { get; }

        private GetProjectLogBookQuery(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static GetProjectLogBookQuery Create(ProjectId projectId)
        {
            return new GetProjectLogBookQuery(projectId);
        }
    }

    public sealed class GetProjectLogBookQueryResponse
    {
        public Guid ProjectId { get; private set; }

        public IList<LogBookUserResponse> Users { get; private set; }


        public GetProjectLogBookQueryResponse(Guid projectId, IList<LogBookUserResponse> users)
        {
            ProjectId = projectId;
            Users = users;
        }
    }

    public class LogBookUserResponse
    {
        public string Name { get; private set; }
        public Guid UserId { get; private set; }

        public LogBookUserResponse(string name, Guid userId)
        {
            Name = name;
            UserId = userId;
        }
    }
    
    internal class GetProjectLogBookQueryHandler : IQueryHandler<GetProjectLogBookQuery, GetProjectLogBookQueryResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectLogBookRepository _projectLogBookRepository;

        public GetProjectLogBookQueryHandler(IProjectRepository projectRepository,
            IProjectLogBookRepository projectLogBookRepository)
        {
            _projectRepository = projectRepository;
            _projectLogBookRepository = projectLogBookRepository;
        }

        public async Task<GetProjectLogBookQueryResponse> Handle(GetProjectLogBookQuery query,
            CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellationToken);
            var logBook = await _projectLogBookRepository.GetByProjectId(query.ProjectId.Value, cancellationToken);

            var owners = new List<ProjectOwner>(){project.ProjectOwner}.Select(o => ProjectLogBookUser.Create(LogBookName.Create(o.Name), o.Id));

            var participants = project.ProjectParticipants.Select(p => ProjectLogBookUser.Create(LogBookName.Create(p.Name.Value), p.Id));
            var logBookUsers = logBook.ProjectLogBookUsers;
            
            var projectLogBook = await _projectLogBookRepository.GetByProjectId(query.ProjectId.Value, cancellationToken);

            var logbookUsersAndParticipants = participants.UnionBy(logBookUsers, p => p.UserId)
                .UnionBy(owners, o => o.UserId)
                    .Select(l => new LogBookUserResponse(l.Name.Value, l.UserId )).ToList();

            return new GetProjectLogBookQueryResponse(projectLogBook.ProjectId.Value, logbookUsersAndParticipants);
        }
    }
    
    public class GetProjectLogBookQueryAuthorizer : IAuthorizer<GetProjectLogBookQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetProjectLogBookQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetProjectLogBookQuery query, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellationToken);
            if (project.IsOwner(_executionContext.UserId) || project.IsParticipant(_executionContext.UserId) || project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
