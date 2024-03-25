using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.CopyProjectFolder
{
    public sealed class CopyProjectFolderCommand : ICommand<ICommandResponse>
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        public ProjectFolderId DestinationFolderId { get; }

        private CopyProjectFolderCommand(ProjectId projectId, ProjectFolderId projectFolderId, ProjectFolderId destinationFolderId)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            DestinationFolderId = destinationFolderId;
        }

        public static CopyProjectFolderCommand Create(Guid projectId, Guid folderId, Guid destinationFolderId)
        {
            return new CopyProjectFolderCommand(ProjectId.Create(projectId), ProjectFolderId.Create(folderId),
                ProjectFolderId.Create(destinationFolderId));
        }
    }

    internal class CopyProjectFolderCommandHandler : ICommandHandler<CopyProjectFolderCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;
        private readonly ISystemTime _systemTime;

        public CopyProjectFolderCommandHandler(IProjectFolderRootRepository projectFolderRootRepository, IUnitOfWork unitOfWork,
            IExecutionContext executionContext, ISystemTime systemTime)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
            _systemTime = systemTime;
        }

        public async Task<ICommandResponse> Handle(CopyProjectFolderCommand request, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);
            projectFolderRoot.CopyFolder(request.ProjectFolderId, request.DestinationFolderId, _executionContext, _systemTime );
            
            await _projectFolderRootRepository.Update(projectFolderRoot, cancellationToken);
            await _projectFolderRootRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }


    internal class CopyProjectFolderCommandAuthorizer : IAuthorizer<CopyProjectFolderCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public CopyProjectFolderCommandAuthorizer(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(CopyProjectFolderCommand command, CancellationToken cancellation)
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
