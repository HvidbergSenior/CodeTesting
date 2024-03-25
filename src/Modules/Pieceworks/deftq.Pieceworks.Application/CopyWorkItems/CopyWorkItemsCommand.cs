using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.Calculation;

namespace deftq.Pieceworks.Application.CopyWorkItems
{
    public sealed class CopyWorkItemsCommand : ICommand<ICommandResponse>
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId SourceFolderId { get; }
        public ProjectFolderId DestinationFolderId { get; }
        public IList<WorkItemId> WorkItemIds { get; }

        private CopyWorkItemsCommand(ProjectId projectId, ProjectFolderId sourceFolderId, ProjectFolderId destinationFolderId,
            IList<WorkItemId> workItemIds)
        {
            ProjectId = projectId;
            SourceFolderId = sourceFolderId;
            DestinationFolderId = destinationFolderId;
            WorkItemIds = workItemIds;
        }

        public static CopyWorkItemsCommand Create(Guid projectId, Guid sourceFolderId, Guid destinationFolderId, IList<Guid> workItemIds)
        {
            return new CopyWorkItemsCommand(ProjectId.Create(projectId), ProjectFolderId.Create(sourceFolderId),
                ProjectFolderId.Create(destinationFolderId), workItemIds.Select(WorkItemId.Create).ToList());
        }
    }

    internal class CopyWorkItemsCommandHandler : ICommandHandler<CopyWorkItemsCommand, ICommandResponse>
    {
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public CopyWorkItemsCommandHandler(IProjectFolderWorkRepository projectFolderWorkRepository,
            IProjectFolderRootRepository projectFolderRootRepository, IBaseRateAndSupplementRepository baseRateAndSupplementRepository,
            IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectFolderWorkRepository = projectFolderWorkRepository;
            _projectFolderRootRepository = projectFolderRootRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<ICommandResponse> Handle(CopyWorkItemsCommand command, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(command.ProjectId.Value, cancellationToken);
            var destinationFolder = GetDestinationFolder(command, projectFolderRoot);
            
            var systemBaseRateAndSupplement = await _baseRateAndSupplementRepository.Get(cancellationToken);
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(systemBaseRateAndSupplement, destinationFolder);

            ProjectFolderWork sourceFolderWork =
                await _projectFolderWorkRepository.GetByProjectAndFolderId(command.ProjectId.Value, command.SourceFolderId.Value, cancellationToken);

            ProjectFolderWork destinationFolderWork =
                await _projectFolderWorkRepository.GetByProjectAndFolderId(command.ProjectId.Value, command.DestinationFolderId.Value,
                    cancellationToken);

            sourceFolderWork.CopyWorkItems(destinationFolderWork, baseRateAndSupplementProxy, command.WorkItemIds, _executionContext);

            await _projectFolderWorkRepository.Update(destinationFolderWork, cancellationToken);
            await _projectFolderWorkRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }

        private static ProjectFolder GetDestinationFolder(CopyWorkItemsCommand command, ProjectFolderRoot projectFolderRoot)
        {
            return projectFolderRoot.GetFolder(command.DestinationFolderId);
        }
    }

    internal class CopyWorkItemsCommandAuthorizer : IAuthorizer<CopyWorkItemsCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IExecutionContext _executionContext;

        public CopyWorkItemsCommandAuthorizer(IProjectRepository projectRepository, IProjectFolderRootRepository projectFolderRootRepository,
            IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _projectFolderRootRepository = projectFolderRootRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(CopyWorkItemsCommand command, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellation);
            var currentUser = _executionContext.UserId;

            // Owner is authorized
            if (project.IsOwner(currentUser))
            {
                return AuthorizationResult.Succeed();
            }

            // Participant is authorized if destination folder is unlocked
            if (project.IsParticipant(currentUser))
            {
                var folderRoot = await _projectFolderRootRepository.GetByProjectId(project.ProjectId.Value, cancellation);
                var destinationFolder = folderRoot.RootFolder.Find(command.DestinationFolderId);
                if (destinationFolder is not null && destinationFolder.IsUnlocked())
                {
                    return AuthorizationResult.Succeed();
                }
            }

            return await Task.FromResult(AuthorizationResult.Fail());
        }
    }
}
