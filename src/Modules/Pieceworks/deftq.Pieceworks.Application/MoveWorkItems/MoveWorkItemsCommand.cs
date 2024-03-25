using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.MoveWorkItems
{
    public sealed class MoveWorkItemsCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public ProjectFolderId SourceFolderId { get; }
        public ProjectFolderId DestinationFolderId { get; }
        public IList<WorkItemId> WorkItemIds { get; }
        private MoveWorkItemsCommand(ProjectId projectId, ProjectFolderId sourceFolderId, ProjectFolderId destinationFolderId, IList<WorkItemId> workItemIds)
        {
            ProjectId = projectId;
            SourceFolderId = sourceFolderId;
            DestinationFolderId = destinationFolderId;
            WorkItemIds = workItemIds;
        }

        public static MoveWorkItemsCommand Create(Guid projectId, Guid SourceFolderId, Guid destinationFolderId, IList<Guid> workItemGuids)
        {
            return new MoveWorkItemsCommand(ProjectId.Create(projectId), ProjectFolderId.Create(SourceFolderId),
                ProjectFolderId.Create(destinationFolderId), workItemGuids.Select(WorkItemId.Create).ToList());
        }
    }
    internal class MoveWorkItemsCommandHandler : ICommandHandler<MoveWorkItemsCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MoveWorkItemsCommandHandler(IProjectFolderRootRepository projectFolderRootRepository,
            IProjectFolderWorkRepository projectFolderWorkRepository, IBaseRateAndSupplementRepository baseRateAndSupplementRepository,
            IUnitOfWork unitOfWork)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _projectFolderWorkRepository = projectFolderWorkRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(MoveWorkItemsCommand request, CancellationToken cancellationToken)
        {
            var sourceFolderWork = await _projectFolderWorkRepository.GetByProjectAndFolderId(request.ProjectId.Value, request.SourceFolderId.Value, cancellationToken);
            var destinationFolderWork = await _projectFolderWorkRepository.GetByProjectAndFolderId(request.ProjectId.Value, request.DestinationFolderId.Value, cancellationToken);

            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);

            var systemBaseRateAndSupplement = await _baseRateAndSupplementRepository.Get(cancellationToken);
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(systemBaseRateAndSupplement, projectFolderRoot.GetFolder(request.DestinationFolderId));
            
            sourceFolderWork.MoveWorkItems(destinationFolderWork, request.WorkItemIds, baseRateAndSupplementProxy);

            await _projectFolderWorkRepository.Update(sourceFolderWork, cancellationToken);
            await _projectFolderWorkRepository.Update(destinationFolderWork, cancellationToken);
            await _projectFolderWorkRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }
    
    internal class MoveWorkItemsCommandAuthorizer : IAuthorizer<MoveWorkItemsCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;

        public MoveWorkItemsCommandAuthorizer(IProjectRepository projectRepository, IProjectFolderRootRepository projectFolderRootRepository,
            IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _projectFolderRootRepository = projectFolderRootRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(MoveWorkItemsCommand command, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellation);
            if (project.IsOwner(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            if (project.IsParticipant(_executionContext.UserId))
            {
                var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(command.ProjectId.Value, cancellation);
                var sourceFolder = projectFolderRoot.GetFolder(command.SourceFolderId);
                var destinationFolder = projectFolderRoot.GetFolder(command.DestinationFolderId);
                if (sourceFolder.IsUnlocked() && destinationFolder.IsUnlocked())
                {
                    return AuthorizationResult.Succeed();
                }
            }
            return AuthorizationResult.Fail();
        }
    }
}
