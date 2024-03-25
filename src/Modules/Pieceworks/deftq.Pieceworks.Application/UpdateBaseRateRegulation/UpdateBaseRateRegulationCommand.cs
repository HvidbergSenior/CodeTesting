using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;

namespace deftq.Pieceworks.Application.UpdateBaseRateRegulation
{
    public sealed class UpdateBaseRateRegulationCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        public FolderBaseRateRegulation FolderBaseRateRegulation { get; }

        public enum UpdateBaseRateRegulationStatusEnum { Inherit, Overwrite }

        private UpdateBaseRateRegulationCommand(ProjectId projectId, ProjectFolderId projectFolderId,
            FolderBaseRateRegulation folderBaseRateRegulation)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            FolderBaseRateRegulation = folderBaseRateRegulation;
        }

        public static UpdateBaseRateRegulationCommand Create(Guid projectId, Guid folderId, decimal folderBaseRateRegulationPercentage,
            UpdateBaseRateRegulationStatusEnum updateBaseRateRegulationStatus)
        {
            var inheritStatus = updateBaseRateRegulationStatus == UpdateBaseRateRegulationStatusEnum.Inherit
                ? FolderValueInheritStatus.Inherit()
                : FolderValueInheritStatus.Overwrite();

            return new UpdateBaseRateRegulationCommand(ProjectId.Create(projectId), ProjectFolderId.Create(folderId),
                FolderBaseRateRegulation.Create(folderBaseRateRegulationPercentage, inheritStatus));
        }
    }

    internal class UpdateBaseRateRegulationCommandHandler : ICommandHandler<UpdateBaseRateRegulationCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBaseRateRegulationCommandHandler(IProjectFolderRootRepository projectFolderRootRepository,
            IBaseRateAndSupplementRepository baseRateAndSupplementRepository, IUnitOfWork unitOfWork)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(UpdateBaseRateRegulationCommand request, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);
            var systemBaseRateAndSupplement = await _baseRateAndSupplementRepository.Get(cancellationToken);
            projectFolderRoot.UpdateFolderBaseRateRegulation(request.ProjectFolderId, request.FolderBaseRateRegulation, systemBaseRateAndSupplement);

            await _projectFolderRootRepository.Update(projectFolderRoot, cancellationToken);
            await _projectFolderRootRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class UpdateBaseRateRegulationCommandAuthorizer : IAuthorizer<UpdateBaseRateRegulationCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public UpdateBaseRateRegulationCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(UpdateBaseRateRegulationCommand command, CancellationToken cancellation)
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
