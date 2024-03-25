using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;

namespace deftq.Pieceworks.Application.CreateProjectFolder
{
    public sealed class CreateProjectFolderCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        internal ProjectFolderName FolderName { get; }
        internal ProjectFolderDescription FolderDescription { get; }
        internal ProjectFolderId? ParentFolderId { get; }

        private CreateProjectFolderCommand(ProjectId projectId, ProjectFolderId projectFolderId, ProjectFolderName folderName,
            ProjectFolderDescription folderDescription, ProjectFolderId? parentFolderId)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            FolderName = folderName;
            FolderDescription = folderDescription;
            ParentFolderId = parentFolderId;
        }

        public static CreateProjectFolderCommand Create(Guid projectId, Guid folderId, string folderName, string folderDescription,
            Guid? parentFolderId = null)
        {
            return new CreateProjectFolderCommand(ProjectId.Create(projectId), ProjectFolderId.Create(folderId), ProjectFolderName.Create(folderName),
                ProjectFolderDescription.Create(folderDescription), parentFolderId != null ? ProjectFolderId.Create(parentFolderId.Value) : null);
        }
    }

    internal class CreateProjectFolderCommandHandler : ICommandHandler<CreateProjectFolderCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;
        private readonly ISystemTime _systemTime;

        public CreateProjectFolderCommandHandler(IProjectFolderRootRepository projectFolderRootRepository, IUnitOfWork unitOfWork,
            IExecutionContext executionContext, ISystemTime systemTime)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
            _systemTime = systemTime;
        }

        public async Task<ICommandResponse> Handle(CreateProjectFolderCommand request, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);
            var createdBy = ProjectFolderCreatedBy.Create(_executionContext.UserId.ToString(), _systemTime.Now());

            if (request.ParentFolderId is not null)
            {
                projectFolderRoot.AddFolder(
                    ProjectFolder.Create(request.ProjectFolderId, request.FolderName, request.FolderDescription, createdBy,
                        FolderRateAndSupplement.InheritAll(), ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Normal()), request.ParentFolderId);
            }
            else
            {
                projectFolderRoot.AddFolder(ProjectFolder.Create(request.ProjectFolderId, request.FolderName, request.FolderDescription, createdBy,
                   FolderRateAndSupplement.InheritAll(), ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Normal()));
            }

            await _projectFolderRootRepository.Update(projectFolderRoot, cancellationToken);
            await _projectFolderRootRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class CreateProjectFolderCommandAuthorizer : IAuthorizer<CreateProjectFolderCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public CreateProjectFolderCommandAuthorizer(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(CreateProjectFolderCommand command, CancellationToken cancellation)
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
