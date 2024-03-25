using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.RemoveDocument
{
    public sealed class RemoveProjectDocumentCommand: ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        public ProjectDocumentId ProjectDocumentId { get; }
        
        private RemoveProjectDocumentCommand(ProjectId projectId, ProjectFolderId projectFolderId, ProjectDocumentId projectDocumentId)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            ProjectDocumentId = projectDocumentId;
        }

        public static RemoveProjectDocumentCommand Create(Guid projectId, Guid folderId, Guid documentId)
        {
            return new RemoveProjectDocumentCommand(ProjectId.Create(projectId), ProjectFolderId.Create(folderId), ProjectDocumentId.Create(documentId));
        }
    }

    internal class RemoveDocumentCommandHandler : ICommandHandler<RemoveProjectDocumentCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveDocumentCommandHandler(IProjectFolderRootRepository projectFolderRootRepository, IUnitOfWork unitOfWork)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(RemoveProjectDocumentCommand request, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);
            projectFolderRoot.RemoveDocument(request.ProjectFolderId, request.ProjectDocumentId);
            
            await _projectFolderRootRepository.Update(projectFolderRoot, cancellationToken);
            await _projectFolderRootRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class RemoveProjectDocumentCommandAuthorizer : IAuthorizer<RemoveProjectDocumentCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public RemoveProjectDocumentCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(RemoveProjectDocumentCommand command, CancellationToken cancellation)
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
