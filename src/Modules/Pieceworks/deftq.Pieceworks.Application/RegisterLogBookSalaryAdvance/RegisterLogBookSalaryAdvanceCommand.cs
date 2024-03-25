using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;

namespace deftq.Pieceworks.Application.RegisterLogBookSalaryAdvance
{
    public sealed class RegisterLogBookSalaryAdvanceCommand : ICommand<ICommandResponse>
    {
        public ProjectId ProjectId { get; }
        public ProjectLogBookUser LogBookUser { get;}
        public LogBookWeek LogBookWeek { get; }
        public LogBookYear LogBookYear { get; }
        public LogBookSalaryAdvanceRole Role { get; }
        public LogBookSalaryAdvanceAmount Amount { get; }

        private RegisterLogBookSalaryAdvanceCommand(ProjectId projectId, ProjectLogBookUser logBookUser, LogBookWeek logBookWeek, LogBookYear logBookYear, LogBookSalaryAdvanceRole role, LogBookSalaryAdvanceAmount amount)
        {
            ProjectId = projectId;
            LogBookUser = logBookUser;
            LogBookWeek = logBookWeek;
            LogBookYear = logBookYear;
            Role = role;
            Amount = amount;
        }
        
        public static RegisterLogBookSalaryAdvanceCommand Create(ProjectId projectId, Guid userId, int year, int week, LogBookSalaryAdvanceRole role, decimal amount)
        {
            var user = ProjectLogBookUser.Create(userId);
            return new RegisterLogBookSalaryAdvanceCommand(projectId, user, LogBookWeek.Create(week), LogBookYear.Create(year), role, LogBookSalaryAdvanceAmount.Create(amount));
        }
    }
    
    internal class RegisterLogBookSalaryAdvanceCommandHandler : ICommandHandler<RegisterLogBookSalaryAdvanceCommand, ICommandResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectLogBookRepository _projectLogBookRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public RegisterLogBookSalaryAdvanceCommandHandler(IProjectRepository projectRepository, IProjectLogBookRepository projectLogBookRepository,
            IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _projectLogBookRepository = projectLogBookRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<ICommandResponse> Handle(RegisterLogBookSalaryAdvanceCommand request,
            CancellationToken cancellationToken)
        {
            var projectLogBook = await _projectLogBookRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);

            projectLogBook.UpdateSalaryAdvance(request.LogBookUser, request.LogBookYear, request.LogBookWeek, request.Role, request.Amount);

            await _projectLogBookRepository.Update(projectLogBook, cancellationToken);
            await _projectLogBookRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }

        internal class RegisterLogBookSalaryAdvanceCommandAuthorizer : IAuthorizer<RegisterLogBookSalaryAdvanceCommand>
        {
            private readonly IProjectRepository _projectRepository;
            private readonly IExecutionContext _executionContext;

            public RegisterLogBookSalaryAdvanceCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
            {
                _projectRepository = projectRepository;
                _executionContext = executionContext;
            }

            public async Task<AuthorizationResult> Authorize(RegisterLogBookSalaryAdvanceCommand command, CancellationToken cancellation)
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
