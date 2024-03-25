using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.UpdateFolderSupplements
{
    public sealed class UpdateFolderSupplementsCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        internal ProjectFolderId ProjectFolderId { get; }
        internal IList<FolderSupplement>  FolderSupplements { get; }

        private UpdateFolderSupplementsCommand(ProjectId projectId, ProjectFolderId projectFolderId,
            IList<FolderSupplement>  folderSupplements)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            FolderSupplements = folderSupplements;
        }

        public static UpdateFolderSupplementsCommand Create(Guid projectId, Guid folderId, IList<FolderSupplement> folderSupplements)
        {
            return new UpdateFolderSupplementsCommand(ProjectId.Create(projectId),
                ProjectFolderId.Create(folderId), folderSupplements);
        }
    }

    internal class UpdateFolderSupplementsCommandHandler : ICommandHandler<UpdateFolderSupplementsCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public UpdateFolderSupplementsCommandHandler(IProjectFolderRootRepository projectFolderRootRepository, IUnitOfWork unitOfWork,
            IExecutionContext executionContext)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<ICommandResponse> Handle(UpdateFolderSupplementsCommand request, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);

            projectFolderRoot.UpdateFolderSupplements(request.ProjectFolderId, request.FolderSupplements);

            await _projectFolderRootRepository.Update(projectFolderRoot, cancellationToken);
            await _projectFolderRootRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class UpdateFolderSupplementsCommandAuthorizer : IAuthorizer<UpdateFolderSupplementsCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public UpdateFolderSupplementsCommandAuthorizer(IProjectRepository projectRepository, IUnitOfWork unitOfWork,
            IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(UpdateFolderSupplementsCommand command, CancellationToken cancellation)
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
