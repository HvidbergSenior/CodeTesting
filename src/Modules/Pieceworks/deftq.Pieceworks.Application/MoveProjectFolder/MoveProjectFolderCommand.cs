using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.MoveProjectFolder
{
    public sealed class MoveProjectFolderCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        public ProjectFolderId DestinationFolderId { get; }

        private MoveProjectFolderCommand(ProjectId projectId, ProjectFolderId projectFolderId, ProjectFolderId destinationFolderId)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            DestinationFolderId = destinationFolderId;
        }
        
        public static MoveProjectFolderCommand Create(Guid projectId, Guid folderId, Guid destinationFolderId)
        {
            return new MoveProjectFolderCommand(ProjectId.Create(projectId), ProjectFolderId.Create(folderId), ProjectFolderId.Create(destinationFolderId));
        }
    }
    
    internal class MoveProjectFolderCommandHandler : ICommandHandler<MoveProjectFolderCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public MoveProjectFolderCommandHandler(IProjectFolderRootRepository projectFolderRootRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<ICommandResponse> Handle(MoveProjectFolderCommand request, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);
            projectFolderRoot.MoveFolder(request.ProjectFolderId, request.DestinationFolderId); 
            
            await _projectFolderRootRepository.Update(projectFolderRoot, cancellationToken);
            await _projectFolderRootRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }
    
    internal class MoveProjectFolderCommandAuthorizer : IAuthorizer<MoveProjectFolderCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public MoveProjectFolderCommandAuthorizer(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(MoveProjectFolderCommand command, CancellationToken cancellation)
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
