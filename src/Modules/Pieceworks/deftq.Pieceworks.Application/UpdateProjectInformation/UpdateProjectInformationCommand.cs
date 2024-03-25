using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Application.UpdateProjectInformation
{
    public sealed class UpdateProjectInformationCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        internal ProjectName ProjectName { get; }
        public ProjectDescription ProjectDescription { get; }
        public ProjectPieceWorkNumber ProjectPieceWorkNumber { get; }
        public ProjectOrderNumber ProjectOrderNumber { get; }

        private UpdateProjectInformationCommand(ProjectId projectId, ProjectName projectName, ProjectDescription projectDescription,
            ProjectPieceWorkNumber projectPieceWorkNumber, ProjectOrderNumber projectOrderNumber)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            ProjectDescription = projectDescription;
            ProjectPieceWorkNumber = projectPieceWorkNumber;
            ProjectOrderNumber = projectOrderNumber;
        }

        public static UpdateProjectInformationCommand Create(Guid id, string nameValue, string description, string pieceWorkNumber, string orderNumber)
        {
            var projectId = ProjectId.Create(id);
            var projectName = ProjectName.Create(nameValue);
            var projectDescription = ProjectDescription.Create(description);
            var projectPieceWorkNumber = ProjectPieceWorkNumber.Create(pieceWorkNumber);
            var projectOrderNumber = ProjectOrderNumber.Create(orderNumber);
            var command = new UpdateProjectInformationCommand(projectId, projectName, projectDescription, projectPieceWorkNumber, projectOrderNumber);
            return command;
        }
    }

    internal class UpdateProjectInformationCommandHandler : ICommandHandler<UpdateProjectInformationCommand, ICommandResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProjectInformationCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(UpdateProjectInformationCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(request.ProjectId.Value, cancellationToken);

            project.ChangeName(request.ProjectName);
            project.ChangeDescription(request.ProjectDescription);
            project.ChangePieceWorkNumber(request.ProjectPieceWorkNumber);
            project.ChangeOrderNumber(request.ProjectOrderNumber);

            await _projectRepository.Update(project, cancellationToken);
            await _projectRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class UpdateProjectInformationCommandAuthorizer : IAuthorizer<UpdateProjectInformationCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public UpdateProjectInformationCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(UpdateProjectInformationCommand command,
            CancellationToken cancellationToken)
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
