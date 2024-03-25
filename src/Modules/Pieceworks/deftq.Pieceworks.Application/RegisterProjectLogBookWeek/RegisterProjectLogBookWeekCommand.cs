using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;

namespace deftq.Pieceworks.Application.RegisterProjectLogBookWeek
{
    public sealed class RegisterProjectLogBookWeekCommand : ICommand<ICommandResponse>
    {
        public ProjectId ProjectId { get; }
        public ProjectLogBookUser LogBookUser { get;}
        public LogBookWeek LogBookWeek { get; }
        public LogBookYear LogBookYear { get; }
        public LogBookNote LogBookNote { get; }
        public IList<ProjectLogBookDay> LogBookDays { get; }
        
        private RegisterProjectLogBookWeekCommand(ProjectId projectId, ProjectLogBookUser logBookUser, LogBookWeek logBookWeek, LogBookYear logBookYear, LogBookNote logBookNote, IList<ProjectLogBookDay> logBookDays)
        {
            ProjectId = projectId;
            LogBookUser = logBookUser;
            LogBookWeek = logBookWeek;
            LogBookYear = logBookYear;
            LogBookNote = logBookNote;
            LogBookDays = logBookDays;
        }

        public static RegisterProjectLogBookWeekCommand Create(Guid projectId, Guid userId, int year, int week, string note, IList<ProjectLogBookDay> days)
        {
            var user = ProjectLogBookUser.Create(userId);
            return new RegisterProjectLogBookWeekCommand(ProjectId.Create(projectId), user, LogBookWeek.Create(week), LogBookYear.Create(year), LogBookNote.Create(note), days);
        }
    }

    internal class RegisterProjectLogBookCommandHandler : ICommandHandler<RegisterProjectLogBookWeekCommand, ICommandResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectLogBookRepository _projectLogBookRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public RegisterProjectLogBookCommandHandler(IProjectRepository projectRepository, IProjectLogBookRepository projectLogBookRepository,
            IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _projectLogBookRepository = projectLogBookRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<ICommandResponse> Handle(RegisterProjectLogBookWeekCommand request,
            CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(request.ProjectId.Value, cancellationToken);
            var projectLogBook = await _projectLogBookRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);
            var userIdToRegisterWeekFor = request.LogBookUser.UserId;

            if (project.IsOwner(userIdToRegisterWeekFor))
            {
                var owner = ProjectLogBookUser.Create(LogBookName.Create(project.ProjectOwner.Name), project.ProjectOwner.Id);
                projectLogBook.RegisterWeek(owner, request.LogBookYear, request.LogBookWeek, request.LogBookNote, request.LogBookDays);
            }
            else
            {
                var participant = project.GetParticipant(userIdToRegisterWeekFor);
                var user = ProjectLogBookUser.Create(LogBookName.Create(participant.Name.Value), participant.Id);
                projectLogBook.RegisterWeek(user, request.LogBookYear, request.LogBookWeek, request.LogBookNote, request.LogBookDays);
            }
            
            await _projectLogBookRepository.Update(projectLogBook, cancellationToken);
            await _projectLogBookRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }
    
    internal class RegisterProjectLogBookWeekCommandAuthorizer : IAuthorizer<RegisterProjectLogBookWeekCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public RegisterProjectLogBookWeekCommandAuthorizer(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(RegisterProjectLogBookWeekCommand command, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellation);
            if (project.IsOwner(_executionContext.UserId) || (project.IsParticipant(_executionContext.UserId) && command.LogBookUser.UserId == _executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}
