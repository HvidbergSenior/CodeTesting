using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.GetWorkItemOperationPreview
{
    public sealed class GetWorkItemOperationPreviewQuery : IQuery<GetWorkItemOperationPreviewQueryResponse>
    {
        internal ProjectId ProjectId { get; }
        internal ProjectFolderId ProjectFolderId { get; }
        internal WorkItemDate WorkItemDate { get; }
        internal WorkItemDuration WorkItemOperationTime { get; }
        internal WorkItemAmount WorkItemAmount { get; }
        internal IList<Supplement> Supplements { get; }

        private GetWorkItemOperationPreviewQuery(ProjectId projectId, ProjectFolderId projectFolderId, WorkItemDate workItemDate,
            WorkItemDuration workItemOperationTime, WorkItemAmount workItemAmount, IList<Supplement> supplements)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            WorkItemDate = workItemDate;
            WorkItemOperationTime = workItemOperationTime;
            WorkItemAmount = workItemAmount;
            Supplements = supplements;
        }

        public static GetWorkItemOperationPreviewQuery Create(Guid projectId, Guid projectFolderId, DateOnly date, decimal operationTimeMilliseconds,
            decimal workItemAmount, IList<Supplement> supplements)
        {
            return new GetWorkItemOperationPreviewQuery(ProjectId.Create(projectId), ProjectFolderId.Create(projectFolderId),
                WorkItemDate.Create(date), WorkItemDuration.Create(operationTimeMilliseconds), WorkItemAmount.Create(workItemAmount), supplements);
        }
    }

    public sealed class Supplement
    {
        internal SupplementPercentage SupplementPercentage { get; private set; }

        private Supplement(SupplementPercentage supplementPercentage)
        {
            SupplementPercentage = supplementPercentage;
        }

        public static Supplement Create(decimal supplementPercentage)
        {
            return new Supplement(SupplementPercentage.Create(supplementPercentage));
        }
    }

    public sealed class GetWorkItemOperationPreviewQueryResponse
    {
        public decimal OperationTimeMilliseconds { get; private set; }
        public decimal TotalWorkTimeMilliseconds { get; private set; }
        public decimal WorkItemTotalPaymentDkr { get; private set; }

        public GetWorkItemOperationPreviewQueryResponse(decimal operationTimeMilliseconds, decimal totalWorkTimeMilliseconds, decimal workItemTotalPaymentDkr)
        {
            OperationTimeMilliseconds = operationTimeMilliseconds;
            TotalWorkTimeMilliseconds = totalWorkTimeMilliseconds;
            WorkItemTotalPaymentDkr = workItemTotalPaymentDkr;
        }
    }

    internal class GetWorkItemOperationPreviewQueryHandler : IQueryHandler<GetWorkItemOperationPreviewQuery, GetWorkItemOperationPreviewQueryResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;

        public GetWorkItemOperationPreviewQueryHandler(IProjectFolderRootRepository projectFolderRootRepository,
            IBaseRateAndSupplementRepository baseRateAndSupplementRepository)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
        }

        public async Task<GetWorkItemOperationPreviewQueryResponse> Handle(GetWorkItemOperationPreviewQuery query,
            CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(query.ProjectId.Value, cancellationToken);
            var systemBaseRateAndSupplementId = Guid.NewGuid();
            
            var supplements = CreateSupplements(query);

            var workItem = WorkItem.Create(WorkItemId.Empty(), CatalogOperationId.Empty(), WorkItemOperationNumber.Empty(), query.WorkItemDate,
                WorkItemUser.Empty(), WorkItemText.Empty(), query.WorkItemOperationTime, query.WorkItemAmount, supplements);

            var systemBaseRateAndSupplement = await _baseRateAndSupplementRepository.Get(cancellationToken);
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(systemBaseRateAndSupplement, projectFolderRoot.GetFolder(query.ProjectFolderId));
            
            var workItemCalculator = new WorkItemCalculator(baseRateAndSupplementProxy);
            var calculationResult = workItemCalculator.CalculateTotalOperationTime(workItem);

            var operationTimeMilliseconds = query.WorkItemOperationTime.Value;
            var totalWorkTimeMilliseconds = calculationResult.TotalWorkTimeExpression.Evaluate().Value;
            decimal totalWorkTimePaymentDkr = calculationResult.TotalPaymentExpression.Evaluate().Value;
            return new GetWorkItemOperationPreviewQueryResponse(operationTimeMilliseconds, totalWorkTimeMilliseconds, totalWorkTimePaymentDkr);
        }

        private static IList<Domain.FolderWork.Supplements.Supplement> CreateSupplements(GetWorkItemOperationPreviewQuery request)
        {
            var supplements = new List<Domain.FolderWork.Supplements.Supplement>();
            foreach (var supplement in request.Supplements)
            {
                supplements.Add(Domain.FolderWork.Supplements.Supplement.Create(
                    SupplementId.Create(Guid.NewGuid()), CatalogSupplementId.Empty(), SupplementNumber.Empty(),
                    SupplementText.Empty(), supplement.SupplementPercentage));
            }

            return supplements;
        }
    }

    internal class GetWorkItemOperationPreviewQueryAuthorizer : IAuthorizer<GetWorkItemOperationPreviewQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetWorkItemOperationPreviewQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetWorkItemOperationPreviewQuery query, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellation);
            if (project.IsOwner(_executionContext.UserId) || (project.IsParticipant(_executionContext.UserId)))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
