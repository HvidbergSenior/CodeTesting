using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;

namespace deftq.Pieceworks.Application.OpenProjectLogBookWeek
{
    public sealed class OpenProjectLogBookWeekCommand : ICommand<ICommandResponse>
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectLogBookUser LogBookUser { get; private set; }
        public LogBookYear LogBookYear { get; private set; }
        public LogBookWeek LogBookWeek { get; private set; }

        private OpenProjectLogBookWeekCommand(ProjectId projectId, ProjectLogBookUser logBookUser,
            LogBookYear logBookYear, LogBookWeek logBookWeek)
        {
            ProjectId = projectId;
            LogBookUser = logBookUser;
            LogBookWeek = logBookWeek;
            LogBookYear = logBookYear;
        }

        public static OpenProjectLogBookWeekCommand Create(Guid projectId, Guid userId, int year, int week)
        {
            var user = ProjectLogBookUser.Create(userId);
            return new OpenProjectLogBookWeekCommand(ProjectId.Create(projectId), user, LogBookYear.Create(year), LogBookWeek.Create(week));
        }
    }

    internal class OpenProjectLogBookWeekCommandHandler : ICommandHandler<OpenProjectLogBookWeekCommand, ICommandResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectLogBookRepository _projectLogBookRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public OpenProjectLogBookWeekCommandHandler(IProjectRepository projectRepository, IProjectLogBookRepository projectLogBookRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _projectLogBookRepository = projectLogBookRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<ICommandResponse> Handle(OpenProjectLogBookWeekCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(request.ProjectId.Value, cancellationToken);
            var projectLogBook = await _projectLogBookRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);
            var userId = request.LogBookUser.UserId;
            
            if (project.IsOwner(userId))
            {
                var owner = ProjectLogBookUser.Create(LogBookName.Create(project.ProjectOwner.Name), project.ProjectOwner.Id);
                projectLogBook.OpenWeek(owner, request.LogBookYear, request.LogBookWeek);
            }
            else
            {
                var participant = project.ProjectParticipants.First(p => p.Id == userId);
                var participantLogBookUser = ProjectLogBookUser.Create(LogBookName.Create(participant.Name.Value), participant.Id);
                projectLogBook.OpenWeek(participantLogBookUser, request.LogBookYear, request.LogBookWeek);
            }

            await _projectLogBookRepository.Update(projectLogBook, cancellationToken);
            await _projectLogBookRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }

        internal class OpenProjectLogBookWeekCommandAuthorizer : IAuthorizer<OpenProjectLogBookWeekCommand>
        {
            private readonly IProjectRepository _projectRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IExecutionContext _executionContext;

            public OpenProjectLogBookWeekCommandAuthorizer(IProjectRepository projectRepository,
                IUnitOfWork unitOfWork, IExecutionContext executionContext)
            {
                _projectRepository = projectRepository;
                _unitOfWork = unitOfWork;
                _executionContext = executionContext;
            }

            public async Task<AuthorizationResult> Authorize(OpenProjectLogBookWeekCommand command, CancellationToken cancellation)
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
}
