using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.GetWorkItemsAsSpreadSheet
{
    public sealed class GetWorkItemsAsSpreadSheetQuery : IQuery<GetWorkItemsAsSpreadSheetQueryResponse>
    {
        public ProjectId ProjectId { get; private set; }

        private GetWorkItemsAsSpreadSheetQuery()
        {
            ProjectId = ProjectId.Empty();
        }

        private GetWorkItemsAsSpreadSheetQuery(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static GetWorkItemsAsSpreadSheetQuery Create(Guid projectId)
        {
            return new GetWorkItemsAsSpreadSheetQuery(ProjectId.Create(projectId));
        }
    }

    internal class GetWorkItemsAsSpreadSheetQueryHandler : IQueryHandler<GetWorkItemsAsSpreadSheetQuery, GetWorkItemsAsSpreadSheetQueryResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IProjectFolderWorkRepository _folderWorkRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;

        public GetWorkItemsAsSpreadSheetQueryHandler(IProjectRepository projectRepository, IProjectFolderRootRepository projectFolderRootRepository,
            IProjectFolderWorkRepository folderWorkRepository, IBaseRateAndSupplementRepository baseRateAndSupplementRepository)
        {
            _projectRepository = projectRepository;
            _projectFolderRootRepository = projectFolderRootRepository;
            _folderWorkRepository = folderWorkRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
        }

        public async Task<GetWorkItemsAsSpreadSheetQueryResponse> Handle(GetWorkItemsAsSpreadSheetQuery query, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellationToken);
            var folderRoot = await _projectFolderRootRepository.GetByProjectId(query.ProjectId.Value, cancellationToken);
            var folderAndSubFolders = folderRoot.RootFolder.GetFolderAndSubFolders().Select(f => f.ProjectFolderId.Value).ToList();
            var folderWorks = await _folderWorkRepository.GetByProjectAndFolderIds(project.ProjectId.Value, folderAndSubFolders, cancellationToken);

            using var openXmlExporter = new OpenXmlExporter(await _baseRateAndSupplementRepository.Get(cancellationToken));
            openXmlExporter.ExportWorkItems(project, folderRoot, folderWorks);
            var export = openXmlExporter.GetExport();

            return new GetWorkItemsAsSpreadSheetQueryResponse(export.FileName, export.Data);
        }
        
        internal class GetWorkItemsAsSpreadSheetQueryAuthorizer : IAuthorizer<GetWorkItemsAsSpreadSheetQuery>
        {
            private readonly IProjectRepository _projectRepository;
            private readonly IExecutionContext _executionContext;

            public GetWorkItemsAsSpreadSheetQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
            {
                _projectRepository = projectRepository;
                _executionContext = executionContext;
            }

            public async Task<AuthorizationResult> Authorize(GetWorkItemsAsSpreadSheetQuery query, CancellationToken cancellation)
            {
                var project = await _projectRepository.GetById(query.ProjectId.Value, cancellation);
                if (project.IsOwner(_executionContext.UserId) ||
                    project.IsParticipant(_executionContext.UserId) ||
                    project.IsProjectManager(_executionContext.UserId))
                {
                    return AuthorizationResult.Succeed();
                }

                return AuthorizationResult.Fail();
            }
        }
    }
}
