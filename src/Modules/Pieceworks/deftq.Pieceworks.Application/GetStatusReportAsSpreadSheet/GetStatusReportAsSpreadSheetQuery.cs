using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectLogBook;

namespace deftq.Pieceworks.Application.GetStatusReportAsSpreadSheet
{
    public sealed class GetStatusReportAsSpreadSheetQuery : IQuery<GetStatusReportAsSpreadSheetQueryResponse>
    {
        public ProjectId ProjectId { get; private set; }

        private GetStatusReportAsSpreadSheetQuery()
        {
            ProjectId = ProjectId.Empty();
        }

        public GetStatusReportAsSpreadSheetQuery(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static GetStatusReportAsSpreadSheetQuery Create(Guid projectId)
        {
            return new GetStatusReportAsSpreadSheetQuery(ProjectId.Create(projectId));
        }
    }

    internal class GetStatusReportAsSpreadSheetQueryHandler : IQueryHandler<GetStatusReportAsSpreadSheetQuery,
        GetStatusReportAsSpreadSheetQueryResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectCompensationListRepository _projectCompensationListRepository;
        private readonly IProjectLogBookRepository _projectLogBookRepository;
        private readonly IProjectExtraWorkAgreementListRepository _projectExtraWorkAgreementListRepository;
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;

        public GetStatusReportAsSpreadSheetQueryHandler(IProjectRepository projectRepository, IProjectLogBookRepository projectLogBookRepository,
            IProjectCompensationListRepository projectCompensationListRepository,
            IProjectExtraWorkAgreementListRepository projectExtraWorkAgreementListRepository,
            IProjectFolderRootRepository projectFolderRootRepository, IProjectFolderWorkRepository projectFolderWorkRepository)
        {
            _projectRepository = projectRepository;
            _projectLogBookRepository = projectLogBookRepository;
            _projectCompensationListRepository = projectCompensationListRepository;
            _projectExtraWorkAgreementListRepository = projectExtraWorkAgreementListRepository;
            _projectFolderRootRepository = projectFolderRootRepository;
            _projectFolderWorkRepository = projectFolderWorkRepository;
        }

        public async Task<GetStatusReportAsSpreadSheetQueryResponse> Handle(GetStatusReportAsSpreadSheetQuery query,
            CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellationToken);
            var projectCompensationList = await _projectCompensationListRepository.GetByProjectId(project.ProjectId.Value);
            var projectLogBook = await _projectLogBookRepository.GetByProjectId(project.ProjectId.Value, cancellationToken);
            var projectExtraWorkAgreementList =
                await _projectExtraWorkAgreementListRepository.GetByProjectId(project.ProjectId.Value, cancellationToken);

            var folderSummation = await GetFolderSummation(project.ProjectId, cancellationToken);

            using var openXmlExporter = new StatusReportExporter();
            openXmlExporter.ExportStatusReport(project, projectCompensationList, projectLogBook, projectExtraWorkAgreementList, folderSummation);
            var export = openXmlExporter.GetExport();

            return new GetStatusReportAsSpreadSheetQueryResponse(export.Data, export.FileName);
        }

        private async Task<TotalFolderWorkTimeCalculationResult> GetFolderSummation(ProjectId projectId, CancellationToken cancellationToken)
        {
            var folderRoot = await _projectFolderRootRepository.GetByProjectId(projectId.Value, cancellationToken);
            var folder = folderRoot.GetFolder(folderRoot.RootFolder.ProjectFolderId);

            var allFoldersInSubTree = folderRoot.GetFolderAndSubfolders(folder.ProjectFolderId);
            var allFoldersInSubTreeIds = allFoldersInSubTree.Select(f => f.ProjectFolderId.Value).ToList();
            var allFolderWorks = await _projectFolderWorkRepository.GetByProjectAndFolderIds(projectId.Value, allFoldersInSubTreeIds, cancellationToken);
            
            var folderCalculator = new FolderCalculator();
            return folderCalculator.CalculateTotalOperationTime(folderRoot, folder.ProjectFolderId, allFolderWorks);
        }

        internal class GetStatusReportAsSpreadSheetQueryAuthorizer : IAuthorizer<GetStatusReportAsSpreadSheetQuery>
        {
            private readonly IProjectRepository _projectRepository;
            private readonly IExecutionContext _executionContext;

            public GetStatusReportAsSpreadSheetQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
            {
                _projectRepository = projectRepository;
                _executionContext = executionContext;
            }

            public async Task<AuthorizationResult> Authorize(GetStatusReportAsSpreadSheetQuery query, CancellationToken cancellation)
            {
                var project = await _projectRepository.GetById(query.ProjectId.Value, cancellation);
                if (project.IsOwner(_executionContext.UserId) ||
                    project.IsProjectManager(_executionContext.UserId))
                {
                    return AuthorizationResult.Succeed();
                }

                return AuthorizationResult.Fail();
            }
        }
    }
}
