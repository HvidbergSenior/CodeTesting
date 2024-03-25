using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.UpdateProjectFolderDescription
{
    public sealed class UpdateProjectFolderDescriptionCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        internal ProjectFolderId ProjectFolderId { get; }
        internal ProjectFolderDescription ProjectFolderDescription { get; }

        private UpdateProjectFolderDescriptionCommand(ProjectId projectId, ProjectFolderId projectFolderId, ProjectFolderDescription projectFolderDescription)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            ProjectFolderDescription = projectFolderDescription;
        }

        public static UpdateProjectFolderDescriptionCommand Create(Guid projectId, Guid folderId, string descriptionValue)
        {
            return new UpdateProjectFolderDescriptionCommand(ProjectId.Create(projectId),
                ProjectFolderId.Create(folderId), ProjectFolderDescription.Create(descriptionValue));
        }
    }

    internal class UpdateProjectFolderDescriptionCommandHandler : ICommandHandler<UpdateProjectFolderDescriptionCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;
        
        public UpdateProjectFolderDescriptionCommandHandler(IProjectFolderRootRepository projectFolderRootRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<ICommandResponse> Handle(UpdateProjectFolderDescriptionCommand request, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);

            projectFolderRoot.ChangeFolderDescription(request.ProjectFolderId, request.ProjectFolderDescription);
            
            await _projectFolderRootRepository.Update(projectFolderRoot, cancellationToken);
            await _projectFolderRootRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            
            return EmptyCommandResponse.Default;
        }
    }
    
    internal class UpdateProjectFolderDescriptionCommandAuthorizer : IAuthorizer<UpdateProjectFolderDescriptionCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public UpdateProjectFolderDescriptionCommandAuthorizer(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(UpdateProjectFolderDescriptionCommand command, CancellationToken cancellation)
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
