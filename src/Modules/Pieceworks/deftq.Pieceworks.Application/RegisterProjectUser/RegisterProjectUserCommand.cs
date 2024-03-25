using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Application.RegisterProjectUser
{
    public sealed class RegisterProjectUserCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public ProjectParticipant? Participant { get; private set; }
        public ProjectManager? Manager { get; private set; }

        public RegisterProjectUserCommand()
        {
            ProjectId = ProjectId.Empty();
        }

        private RegisterProjectUserCommand(ProjectId projectId, ProjectParticipant? participant, ProjectManager? manager)
        {
            ProjectId = projectId;
            Participant = participant;
            Manager = manager;
        }

        public static RegisterProjectUserCommand Create(Guid projectId, ProjectParticipant participant)
        {
            return new RegisterProjectUserCommand(ProjectId.Create(projectId), participant, null);
        }

        public static RegisterProjectUserCommand Create(Guid projectId, ProjectManager manager)
        {
            return new RegisterProjectUserCommand(ProjectId.Create(projectId), null, manager);
        }
    }

    internal class RegisterProjectUserCommandHandler : ICommandHandler<RegisterProjectUserCommand, ICommandResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterProjectUserCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(RegisterProjectUserCommand command, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellationToken);

            if (command.Participant is not null)
            {
                project.AddProjectParticipant(command.Participant);
            }
            else if (command.Manager is not null)
            {
                project.AddProjectManager(command.Manager);
            }

            await _projectRepository.Update(project, cancellationToken);
            await _projectRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class RegisterProjectUserCommandAuthorizer : IAuthorizer<RegisterProjectUserCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public RegisterProjectUserCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(RegisterProjectUserCommand command, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellationToken);
            if (project.IsOwner(_executionContext.UserId) || project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
