using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Application.RemoveProject
{
    public sealed class RemoveProjectCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }

        private RemoveProjectCommand(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static RemoveProjectCommand Create(Guid projectId)
        {
            return new RemoveProjectCommand(ProjectId.Create(projectId));
        }
    }

    internal class RemoveProjectCommandHandler : ICommandHandler<RemoveProjectCommand, ICommandResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(RemoveProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(request.ProjectId.Value, cancellationToken);
            project.RemoveProject();
            
            await _projectRepository.Delete(project, cancellationToken);
            await _projectRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            
            return EmptyCommandResponse.Default;
        }
    }

    internal class RemoveProjectCommandAuthorizer : IAuthorizer<RemoveProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public RemoveProjectCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(RemoveProjectCommand command, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellationToken);
            
            if (project.IsOwner(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}
