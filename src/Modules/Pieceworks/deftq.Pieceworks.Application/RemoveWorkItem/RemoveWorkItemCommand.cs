using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.RemoveWorkItem
{
    public sealed class RemoveWorkItemCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        public IList<WorkItemId> WorkItemIds { get; }

        private RemoveWorkItemCommand(ProjectId projectId, ProjectFolderId projectFolderId, IList<WorkItemId> workItemIds)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            WorkItemIds = workItemIds;
        }

        public static RemoveWorkItemCommand Create(Guid projectId, Guid folderId, IList<Guid> workItemIds)
        {
            return new RemoveWorkItemCommand(ProjectId.Create(projectId), ProjectFolderId.Create(folderId), workItemIds.Select(WorkItemId.Create).ToList());
        }
    }

    internal class RemoveWorkItemCommandHandler : ICommandHandler<RemoveWorkItemCommand, ICommandResponse>
    {
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public RemoveWorkItemCommandHandler(IProjectFolderWorkRepository projectFolderWorkRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectFolderWorkRepository = projectFolderWorkRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<ICommandResponse> Handle(RemoveWorkItemCommand request, CancellationToken cancellationToken)
        {
            var projectFolderWork = await _projectFolderWorkRepository.GetByProjectAndFolderId(request.ProjectId.Value, request.ProjectFolderId.Value, cancellationToken);
            projectFolderWork.RemoveWorkItems(request.WorkItemIds);
            
            await _projectFolderWorkRepository.Update(projectFolderWork, cancellationToken);
            await _projectFolderWorkRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            
            return EmptyCommandResponse.Default;
        }
    }

    internal class RemoveWorkItemCommandAuthorizer : IAuthorizer<RemoveWorkItemCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public RemoveWorkItemCommandAuthorizer(IProjectRepository projectRepository, IProjectFolderRootRepository projectFolderRootRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _projectFolderRootRepository = projectFolderRootRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }
        
        public async Task<AuthorizationResult> Authorize(RemoveWorkItemCommand command, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellation);
            
            var folderRoot = await _projectFolderRootRepository.GetByProjectId(project.ProjectId.Value, cancellation);

            var folder = folderRoot.GetFolder(command.ProjectFolderId);

            if (project.IsOwner(_executionContext.UserId) || (project.IsParticipant(_executionContext.UserId) && folder.IsUnlocked()))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}
