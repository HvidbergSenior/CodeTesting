using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Application.UpdateProjectLumpSum
{
    public sealed class UpdateProjectLumpSumCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public ProjectLumpSumPayment ProjectLumpSum { get; private set; }

        private UpdateProjectLumpSumCommand()
        {
            ProjectId = ProjectId.Empty();
            ProjectLumpSum = ProjectLumpSumPayment.Empty();
        }

        private UpdateProjectLumpSumCommand(ProjectId projectId, ProjectLumpSumPayment projectLumpSum)
        {
            ProjectId = projectId;
            ProjectLumpSum = projectLumpSum;
        }

        public static UpdateProjectLumpSumCommand Create(Guid projectId, decimal projectLumpSum)
        {
            return new UpdateProjectLumpSumCommand(ProjectId.Create(projectId), ProjectLumpSumPayment.Create(projectLumpSum));
        }
    }

    internal class UpdateProjectLumpSumCommandHandler : ICommandHandler<UpdateProjectLumpSumCommand, ICommandResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProjectLumpSumCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(UpdateProjectLumpSumCommand command, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellationToken);
            project.UpdateLumpSumPayment(ProjectLumpSumPayment.Create(command.ProjectLumpSum.Value));
            
            await _projectRepository.Update(project, cancellationToken);
            await _projectRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            
            return EmptyCommandResponse.Default;
        }
    }

    internal class UpdateProjectLumpSumCommandAuthorizer : IAuthorizer<UpdateProjectLumpSumCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public UpdateProjectLumpSumCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }
        
        public async Task<AuthorizationResult> Authorize(UpdateProjectLumpSumCommand command, CancellationToken cancellation)
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
