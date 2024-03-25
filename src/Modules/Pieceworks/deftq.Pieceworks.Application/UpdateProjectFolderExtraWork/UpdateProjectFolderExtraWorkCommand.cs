using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.UpdateProjectFolderExtraWork
{
    public sealed class UpdateProjectFolderExtraWorkCommand : ICommand<ICommandResponse>
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectFolderId ProjectFolderId { get; private set; }
        public enum ExtraWork { ExtraWork, NormalWork}
        public ExtraWork FolderExtraWork { get; private set; }
        
        private UpdateProjectFolderExtraWorkCommand(ProjectId projectId, ProjectFolderId projectFolderId, ExtraWork extraWork)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            FolderExtraWork = extraWork;
        }

        public static UpdateProjectFolderExtraWorkCommand Create(Guid projectId, Guid folderId, ExtraWork extraWork)
        {
            return new UpdateProjectFolderExtraWorkCommand(ProjectId.Create(projectId), ProjectFolderId.Create(folderId), extraWork);
        }
    }

    internal class UpdateProjectFolderExtraWorkCommandHandler : ICommandHandler<UpdateProjectFolderExtraWorkCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProjectFolderExtraWorkCommandHandler(IProjectFolderRootRepository projectFolderRootRepository, IUnitOfWork unitOfWork)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<ICommandResponse> Handle(UpdateProjectFolderExtraWorkCommand request, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);
            
            if (request.FolderExtraWork == UpdateProjectFolderExtraWorkCommand.ExtraWork.ExtraWork)
            {
                projectFolderRoot.MarkAsExtraWork(request.ProjectFolderId);
            }
            else
            {
                projectFolderRoot.MarkAsNormalWork(request.ProjectFolderId);
            }
            
            await _projectFolderRootRepository.Update(projectFolderRoot, cancellationToken);
            await _projectFolderRootRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            
            return EmptyCommandResponse.Default;
        }
    }

    internal class UpdateProjectFolderExtraWorkCommandAuthorizer : IAuthorizer<UpdateProjectFolderExtraWorkCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public UpdateProjectFolderExtraWorkCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }
        
        public async Task<AuthorizationResult> Authorize(UpdateProjectFolderExtraWorkCommand command, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellation);
            if (project.IsOwner(_executionContext.UserId) || project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}
