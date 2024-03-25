using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;


namespace deftq.Pieceworks.Application.UpdateProjectFolderName
{
    public sealed class UpdateProjectFolderNameCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        internal ProjectFolderId ProjectFolderId { get; }
        internal ProjectFolderName ProjectFolderName { get; }

        private UpdateProjectFolderNameCommand(ProjectId projectId, ProjectFolderId projectFolderId, ProjectFolderName projectFolderName)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            ProjectFolderName = projectFolderName;
        }

        public static UpdateProjectFolderNameCommand Create(Guid projectId, Guid folderId, string nameValue)
        {
            return new UpdateProjectFolderNameCommand(ProjectId.Create(projectId),
                ProjectFolderId.Create(folderId), ProjectFolderName.Create(nameValue));
        }
    }

    internal class UpdateProjectFolderNameCommandHandler : ICommandHandler<UpdateProjectFolderNameCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public UpdateProjectFolderNameCommandHandler(IProjectFolderRootRepository projectFolderRootRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<ICommandResponse> Handle(UpdateProjectFolderNameCommand request, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);

            projectFolderRoot.ChangeFolderName(request.ProjectFolderId, request.ProjectFolderName);

            await _projectFolderRootRepository.Update(projectFolderRoot, cancellationToken);
            await _projectFolderRootRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            
            return EmptyCommandResponse.Default;
        }
    }

    internal class UpdateProjectFolderNameCommandAuthorizer : IAuthorizer<UpdateProjectFolderNameCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public UpdateProjectFolderNameCommandAuthorizer(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(UpdateProjectFolderNameCommand command, CancellationToken cancellation)
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
