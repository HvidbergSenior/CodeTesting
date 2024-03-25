using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectLogBook;

namespace deftq.Pieceworks.Application.GetProjectSummation
{
    public sealed class GetProjectSummationQuery : IQuery<GetProjectSummationQueryResponse>
    {
        public ProjectId ProjectId { get; }

        private GetProjectSummationQuery(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static GetProjectSummationQuery Create(ProjectId projectId)
        {
            return new GetProjectSummationQuery(projectId);
        }
    }

    internal class GetProjectSummationQueryHandler : IQueryHandler<GetProjectSummationQuery, GetProjectSummationQueryResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;
        private readonly IProjectLogBookRepository _projectLogBookRepository;
        private readonly IProjectExtraWorkAgreementListRepository _extraWorkAgreementListRepository;

        public GetProjectSummationQueryHandler(IProjectRepository projectRepository, IProjectFolderRootRepository projectFolderRootRepository,
            IProjectFolderWorkRepository projectFolderWorkRepository, IProjectLogBookRepository projectLogBookRepository,
            IProjectExtraWorkAgreementListRepository extraWorkAgreementListRepository)
        {
            _projectRepository = projectRepository;
            _projectFolderRootRepository = projectFolderRootRepository;
            _projectFolderWorkRepository = projectFolderWorkRepository;
            _projectLogBookRepository = projectLogBookRepository;
            _extraWorkAgreementListRepository = extraWorkAgreementListRepository;
        }

        public async Task<GetProjectSummationQueryResponse> Handle(GetProjectSummationQuery query, CancellationToken cancellationToken)
        {
            var projectId = query.ProjectId.Value;

            // Work item summation
            var folderRoot = await _projectFolderRootRepository.GetByProjectId(projectId, cancellationToken);
            var folder = folderRoot.RootFolder;
            var allFoldersInSubTree = folderRoot.GetFolderAndSubfolders(folder.ProjectFolderId);
            var allFoldersInSubTreeIds = allFoldersInSubTree.Select(f => f.ProjectFolderId.Value).ToList();
            var allFolderWorks = await _projectFolderWorkRepository.GetByProjectAndFolderIds(projectId, allFoldersInSubTreeIds, cancellationToken);
            var folderCalculator = new FolderCalculator();
            var folderResult = folderCalculator.CalculateTotalOperationTime(folderRoot, folder.ProjectFolderId, allFolderWorks);
            var totalWorkItemPaymentDkr = folderResult.TotalPaymentExpression.Evaluate().Value;
            var totalWorkItemExtraWorkPaymentDkr = folderResult.TotalExtraWorkPaymentExpression.Evaluate().Value;

            // Log book summation
            var logBook = await _projectLogBookRepository.GetByProjectId(projectId, cancellationToken);
            var totalClosedLogBookMinutes =
                logBook.ProjectLogBookUsers.Select(logBookUser => logBookUser.GetSumClosedHours().Value.TotalMinutes).Sum();
            var totalClosedLogBookHours = new decimal(totalClosedLogBookMinutes) / 60.0m;

            // Extra work agreement summation
            var extraWorkAgreementList = await _extraWorkAgreementListRepository.GetByProjectId(projectId, cancellationToken);
            var totalExtraWorkAgreementTotalPaymentDkr = extraWorkAgreementList.ProjectExtraWorkAgreementTotalPaymentDkr.Value;

            // Lump sum
            var totalLumpSumPaymentDkr = 0.0m;
            var project = await _projectRepository.GetById(projectId, cancellationToken);
            if (project.ProjectPieceworkType == ProjectPieceworkType.TwelveTwo)
            {
                totalLumpSumPaymentDkr = project.ProjectLumpSumPaymentDkr.Value;
            }

            // Calculation
            var totalCalculationSumDkr = 0.0m;

            // Total payment
            var totalPaymentDkr = totalWorkItemPaymentDkr + totalExtraWorkAgreementTotalPaymentDkr + totalLumpSumPaymentDkr;

            return new GetProjectSummationQueryResponse(totalWorkItemPaymentDkr, totalWorkItemExtraWorkPaymentDkr,
                totalExtraWorkAgreementTotalPaymentDkr, totalClosedLogBookHours, totalPaymentDkr, totalLumpSumPaymentDkr, totalCalculationSumDkr);
        }
    }

    internal class GetProjectSummationQueryAuthorizer : IAuthorizer<GetProjectSummationQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetProjectSummationQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetProjectSummationQuery query, CancellationToken cancellation)
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
