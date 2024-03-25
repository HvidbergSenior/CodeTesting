using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;

namespace deftq.Pieceworks.Application.UpdateBaseSupplements
{
    public sealed class UpdateBaseSupplementsCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        public FolderIndirectTimeSupplement FolderIndirectTimeSupplement { get; }
        public FolderSiteSpecificTimeSupplement FolderSiteSpecificTimeSupplement { get; }

        public enum UpdateBaseSupplementStatusEnum { Inherit, Overwrite }

        private UpdateBaseSupplementsCommand(ProjectId projectId, ProjectFolderId projectFolderId,
            FolderIndirectTimeSupplement folderIndirectTimeSupplement, FolderSiteSpecificTimeSupplement folderSiteSpecificTimeSupplement)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            FolderIndirectTimeSupplement = folderIndirectTimeSupplement;
            FolderSiteSpecificTimeSupplement = folderSiteSpecificTimeSupplement;
        }

        public static UpdateBaseSupplementsCommand Create(Guid projectId, Guid folderId, decimal folderIndirectTimeSupplementPercentage,
            UpdateBaseSupplementStatusEnum folderIndirectTimeSupplementStatus, decimal folderSiteSpecificTimeSupplementPercentage,
            UpdateBaseSupplementStatusEnum folderSiteSpecificTimeSupplementStatus)
        {
            var indirectTimeSupplementStatus = folderIndirectTimeSupplementStatus == UpdateBaseSupplementStatusEnum.Inherit
                ? FolderValueInheritStatus.Inherit()
                : FolderValueInheritStatus.Overwrite();

            var siteSpecificTimeSupplementStatus = folderSiteSpecificTimeSupplementStatus == UpdateBaseSupplementStatusEnum.Inherit
                ? FolderValueInheritStatus.Inherit()
                : FolderValueInheritStatus.Overwrite();

            return new UpdateBaseSupplementsCommand(ProjectId.Create(projectId), ProjectFolderId.Create(folderId),
                FolderIndirectTimeSupplement.Create(folderIndirectTimeSupplementPercentage, indirectTimeSupplementStatus),
                FolderSiteSpecificTimeSupplement.Create(folderSiteSpecificTimeSupplementPercentage, siteSpecificTimeSupplementStatus));
        }
    }

    internal class UpdateBaseSupplementsCommandHandler : ICommandHandler<UpdateBaseSupplementsCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBaseSupplementsCommandHandler(IProjectFolderRootRepository projectFolderRootRepository,
            IBaseRateAndSupplementRepository baseRateAndSupplementRepository, IUnitOfWork unitOfWork)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(UpdateBaseSupplementsCommand request, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);
            var systemBaseRateAndSupplement = await _baseRateAndSupplementRepository.Get(cancellationToken);
            var folderIndirectTimeSupplement = request.FolderIndirectTimeSupplement;
            var folderSiteSpecificTimeSupplement = request.FolderSiteSpecificTimeSupplement;

            projectFolderRoot.UpdateFolderBaseSupplement(request.ProjectFolderId, folderIndirectTimeSupplement, folderSiteSpecificTimeSupplement,
                systemBaseRateAndSupplement);

            await _projectFolderRootRepository.Update(projectFolderRoot, cancellationToken);
            await _projectFolderRootRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class UpdateBaseSupplementsCommandAuthorizer : IAuthorizer<UpdateBaseSupplementsCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public UpdateBaseSupplementsCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(UpdateBaseSupplementsCommand command, CancellationToken cancellation)
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
