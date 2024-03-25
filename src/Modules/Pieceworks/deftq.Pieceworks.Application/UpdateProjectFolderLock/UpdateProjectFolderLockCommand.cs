using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.UpdateProjectFolderLock
{
    public sealed class UpdateProjectFolderLockCommand : ICommand<ICommandResponse>
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectFolderId ProjectFolderId { get; private set; }
        public enum Lock {Locked, Unlocked}
        public Lock FolderLock { get; private set; }
        public bool Recursive { get; private set; }

        private UpdateProjectFolderLockCommand(ProjectId projectId, ProjectFolderId projectFolderId, Lock folderLock, bool recursive)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            FolderLock = folderLock;
            Recursive = recursive;
        }

        public static UpdateProjectFolderLockCommand Create(Guid projectId, Guid folderId, Lock folderLock, bool recursive)
        {
            return new UpdateProjectFolderLockCommand(ProjectId.Create(projectId), ProjectFolderId.Create(folderId), folderLock, recursive);
        }
    }

    internal class UpdateLockProjectFolderCommandHandler : ICommandHandler<UpdateProjectFolderLockCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public UpdateLockProjectFolderCommandHandler(IProjectFolderRootRepository projectFolderRootRepository, IUnitOfWork unitOfWork,
            IExecutionContext executionContext)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }
        
        public async Task<ICommandResponse> Handle(UpdateProjectFolderLockCommand request, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);
            if (request.FolderLock == UpdateProjectFolderLockCommand.Lock.Locked)
            {
                projectFolderRoot.LockFolder(request.ProjectFolderId, request.Recursive);
            }
            else
            {
                projectFolderRoot.UnlockFolder(request.ProjectFolderId, request.Recursive);
            }
            
            await _projectFolderRootRepository.Update(projectFolderRoot, cancellationToken);
            await _projectFolderRootRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            
            return EmptyCommandResponse.Default;
        }
    }

    internal class UpdateProjectFolderLockCommandAuthorizer : IAuthorizer<UpdateProjectFolderLockCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public UpdateProjectFolderLockCommandAuthorizer(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }
        
        public async Task<AuthorizationResult> Authorize(UpdateProjectFolderLockCommand command, CancellationToken cancellation)
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
