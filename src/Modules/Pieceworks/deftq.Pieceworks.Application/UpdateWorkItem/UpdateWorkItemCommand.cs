using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.UpdateWorkItem
{
    public sealed class UpdateWorkItemCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        internal ProjectFolderId ProjectFolderId { get; }
        public WorkItemId WorkItemId { get; }
        public WorkItemAmount WorkItemAmount { get; }
        
        public UpdateWorkItemCommand(ProjectId projectId, ProjectFolderId projectFolderId, WorkItemId workItemId, WorkItemAmount workItemAmount)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            WorkItemId = workItemId;
            WorkItemAmount = workItemAmount;
        }

        public static UpdateWorkItemCommand Create(Guid projectId, Guid projectFolderId, Guid workItemId, decimal workItemAmount)
        {
            return new UpdateWorkItemCommand(ProjectId.Create(projectId), ProjectFolderId.Create(projectFolderId), WorkItemId.Create(workItemId), WorkItemAmount.Create(workItemAmount));
        }
    }
    
    internal class UpdateWorkItemCommandHandler : ICommandHandler<UpdateWorkItemCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly IUnitOfWork _unitOfWork;


        public UpdateWorkItemCommandHandler(IProjectFolderRootRepository projectFolderRootRepository, IProjectFolderWorkRepository projectFolderWorkRepository, IBaseRateAndSupplementRepository baseRateAndSupplementRepository, IUnitOfWork unitOfWork)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _projectFolderWorkRepository = projectFolderWorkRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
            _unitOfWork = unitOfWork;
        }
 
        public async Task<ICommandResponse> Handle(UpdateWorkItemCommand request, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);
            var folderWork = await _projectFolderWorkRepository.GetByProjectAndFolderId(request.ProjectId.Value, request.ProjectFolderId.Value, cancellationToken);
            
            var systemBaseRateAndSupplement = await _baseRateAndSupplementRepository.Get(cancellationToken);
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(systemBaseRateAndSupplement, projectFolderRoot.GetFolder(request.ProjectFolderId));
            
            folderWork.UpdateWorkItem(request.WorkItemId, request.WorkItemAmount, baseRateAndSupplementProxy);

            await _projectFolderWorkRepository.Update(folderWork, cancellationToken);
            await _projectFolderWorkRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            
            return EmptyCommandResponse.Default;
        }
    }
    
    internal class UpdateWorkItemCommandAuthorizer : IAuthorizer<UpdateWorkItemCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IExecutionContext _executionContext;

        public UpdateWorkItemCommandAuthorizer(IProjectRepository projectRepository, IProjectFolderRootRepository projectFolderRootRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _projectFolderRootRepository = projectFolderRootRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(UpdateWorkItemCommand command, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellation);
            
            var folderRoot = await _projectFolderRootRepository.GetByProjectId(project.ProjectId.Value, cancellation);

            var folder = folderRoot.GetFolder(command.ProjectFolderId);
            
            if (project.IsOwner(_executionContext.UserId) || (project.IsProjectManager(_executionContext.UserId) && folder.IsUnlocked()) ||
                (project.IsParticipant(_executionContext.UserId) && folder.IsUnlocked()))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
