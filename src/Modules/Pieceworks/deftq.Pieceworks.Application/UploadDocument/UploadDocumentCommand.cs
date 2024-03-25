using System.Globalization;
using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Domain.projectFolderRoot;
using FluentValidation;

namespace deftq.Pieceworks.Application.UploadDocument
{
    public sealed class UploadDocumentCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        internal ProjectFolderId? ProjectFolderId { get; }
        internal ProjectDocumentId ProjectDocumentId { get; }
        internal ProjectDocumentName ProjectDocumentName { get; }
        internal Stream Data { get; }

        private UploadDocumentCommand(ProjectId projectId, ProjectFolderId? projectFolderId, ProjectDocumentId projectDocumentId, ProjectDocumentName projectDocumentName, Stream data)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            ProjectDocumentId = projectDocumentId;
            ProjectDocumentName = projectDocumentName;
            Data = data;
        }

        public static UploadDocumentCommand Create(Guid projectId, Guid? projectFolderId, Guid projectDocumentId, string documentName, Stream data)
        {
            var folderId = projectFolderId is not null ? ProjectFolderId.Create(projectFolderId.Value) : null;
            return new UploadDocumentCommand(ProjectId.Create(projectId), folderId, ProjectDocumentId.Create(projectDocumentId), ProjectDocumentName.Create(documentName), data);
        }
    }

    internal class UploadDocumentCommandValidator : AbstractCommandValidator<UploadDocumentCommand>
    {
        private IList<String> AllowedFileEndings = new List<string>() { "PDF", "JPG", "JPEG", "PNG"};
        
        public UploadDocumentCommandValidator()
        {
            RuleFor(cmd => cmd.ProjectDocumentName)
                .Must(name => HasAllowedFileEnding(name))
                .WithMessage("File type is not allowed");
        }

        private bool HasAllowedFileEnding(ProjectDocumentName name)
        {
            if (String.IsNullOrWhiteSpace(name.Value))
            {
                return false;
            }

            if (name.Value.Contains('.', StringComparison.Ordinal))
            {
                var fileEnding = name.Value.Split('.').Last().ToUpperInvariant();
                return AllowedFileEndings.Contains(fileEnding, StringComparer.Ordinal);    
            }

            return true;
        }
    } 
    
    internal class UploadDocumentCommandHandler : ICommandHandler<UploadDocumentCommand, ICommandResponse>
    {
        private readonly IProjectDocumentRepository _projectDocumentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;
        private readonly IFileStorage _fileStorage;
        private readonly ISystemTime _systemTime;

        public UploadDocumentCommandHandler(IProjectDocumentRepository projectDocumentRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext, IFileStorage fileStorage, ISystemTime systemTime)
        {
            _projectDocumentRepository = projectDocumentRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
            _fileStorage = fileStorage;
            _systemTime = systemTime;
        }

        public async Task<ICommandResponse> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            var projectFolderId = request.ProjectFolderId is not null ? ProjectFolderId.Create(request.ProjectFolderId.Value) : null;
            var reference = await UploadFile(request.ProjectId, projectFolderId, request.ProjectDocumentId, request.ProjectDocumentName, request.Data, cancellationToken);
            var document = ProjectDocument.Create(request.ProjectId, projectFolderId, request.ProjectDocumentId, request.ProjectDocumentName, ProjectDocumentUploadedTimestamp.Create(_systemTime.Now()), reference);
            
            await _projectDocumentRepository.Add(document, cancellationToken);
            await _projectDocumentRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }

        private async Task<IFileReference> UploadFile(ProjectId projectId, ProjectFolderId? projectFolderId, ProjectDocumentId id, ProjectDocumentName name, Stream data, CancellationToken cancellationToken)
        {
            if (projectFolderId is null)
            {
                return  await _fileStorage.StoreFileAsync(projectId, id, name, data, cancellationToken);    
            }
            return await _fileStorage.StoreFileAsync(projectId, projectFolderId, id, name, data, cancellationToken);
        }
    }
    
    internal class UploadDocumentCommandAuthorizer : IAuthorizer<UploadDocumentCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public UploadDocumentCommandAuthorizer(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(UploadDocumentCommand command, CancellationToken cancellation)
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
