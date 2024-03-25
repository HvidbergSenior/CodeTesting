using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectDocument;

namespace deftq.Pieceworks.Application.GetDocument
{
    public sealed class GetDocumentQuery : IQuery<GetDocumentResponse>
    {
        public ProjectDocumentId ProjectDocumentId { get; private set; }

        private GetDocumentQuery(ProjectDocumentId projectDocumentId)
        {
            ProjectDocumentId = projectDocumentId;
        }

        public static GetDocumentQuery Create(ProjectDocumentId projectDocumentId)
        {
            return new GetDocumentQuery(projectDocumentId);
        }
    }

    public class GetDocumentResponse
    {
        public string Filename { get; private set; }

        private readonly byte[] _data;
        
        public GetDocumentResponse(string filename, byte[] data)
        {
            Filename = filename;
            _data = data;
        }

        public byte[] GetBuffer()
        {
            return _data;
        }
    }

    internal class GetDocumentQueryHandler : IQueryHandler<GetDocumentQuery, GetDocumentResponse>
    {
        private readonly IProjectDocumentRepository _projectDocumentRepository;
        private readonly IFileStorage _fileStorage;

        public GetDocumentQueryHandler(IProjectDocumentRepository projectDocumentRepository, IFileStorage fileStorage)
        {
            _projectDocumentRepository = projectDocumentRepository;
            _fileStorage = fileStorage;
        }

        public async Task<GetDocumentResponse> Handle(GetDocumentQuery query, CancellationToken cancellationToken)
        {
            var document = await _projectDocumentRepository.GetById(query.ProjectDocumentId.Value, cancellationToken);
            byte[] bytes = await _fileStorage.GetFileContentAsync(document.FileReference, cancellationToken);
            return new GetDocumentResponse(document.ProjectDocumentName.Value, bytes);
        }
    }

    public class GetDocumentQueryAuthorizer : IAuthorizer<GetDocumentQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectDocumentRepository _projectDocumentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public GetDocumentQueryAuthorizer(IProjectRepository projectRepository,
            IProjectDocumentRepository projectDocumentRepository, IUnitOfWork unitOfWork,
            IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _projectDocumentRepository = projectDocumentRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetDocumentQuery query, CancellationToken cancellationToken)
        {
            var document = await _projectDocumentRepository.GetById(query.ProjectDocumentId.Value, cancellationToken);
            var project = await _projectRepository.GetById(document.ProjectId.Value, cancellationToken);
            if (project.IsOwner(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
