using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.GetWorkItemMaterialPreview
{
    public sealed class GetWorkItemMaterialPreviewQuery : IQuery<GetWorkItemMaterialPreviewQueryResponse>
    {
        internal ProjectId ProjectId { get; }
        internal ProjectFolderId ProjectFolderId { get; }
        internal WorkItemDate WorkItemDate { get; }
        internal WorkItemDuration WorkItemOperationTime { get; }
        internal WorkItemAmount WorkItemAmount { get; }
        internal IList<SupplementOperation> SupplementOperations { get; }
        internal IList<Supplement> Supplements { get; }

        private GetWorkItemMaterialPreviewQuery(ProjectId projectId, ProjectFolderId projectFolderId, WorkItemDate workItemDate,
            WorkItemDuration workItemOperationTime, WorkItemAmount workItemAmount, IList<SupplementOperation> supplementOperations,
            IList<Supplement> supplements)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            WorkItemDate = workItemDate;
            WorkItemOperationTime = workItemOperationTime;
            WorkItemAmount = workItemAmount;
            SupplementOperations = supplementOperations;
            Supplements = supplements;
        }

        public static GetWorkItemMaterialPreviewQuery Create(Guid projectId, Guid projectFolderId, DateOnly date, decimal operationTimeMilliseconds,
            decimal workItemAmount, IList<SupplementOperation> supplementOperations, IList<Supplement> supplements)
        {
            return new GetWorkItemMaterialPreviewQuery(ProjectId.Create(projectId), ProjectFolderId.Create(projectFolderId),
                WorkItemDate.Create(date), WorkItemDuration.Create(operationTimeMilliseconds), WorkItemAmount.Create(workItemAmount),
                supplementOperations, supplements);
        }
    }

    public sealed class SupplementOperation
    {
        public enum SupplementOperationType { AmountRelated, UnitRelated }
        
        internal SupplementOperationType Type { get; private set; }
        internal SupplementOperationTime OperationTime { get; private set; }
        internal SupplementOperationAmount Amount { get; private set; }

        private SupplementOperation(SupplementOperationType type, SupplementOperationTime operationTime, SupplementOperationAmount amount)
        {
            Type = type;
            OperationTime = operationTime;
            Amount = amount;
        }

        public static SupplementOperation Create(SupplementOperationType type, decimal operationTimeMilliseconds, decimal amount)
        {
            return new SupplementOperation(type, SupplementOperationTime.Create(operationTimeMilliseconds), SupplementOperationAmount.Create(amount));
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

    public sealed class GetWorkItemMaterialPreviewQueryResponse
    {
        public decimal OperationTimeMilliseconds { get; private set; }
        public decimal TotalWorkTimeMilliseconds { get; private set; }
        public decimal WorkItemTotalPaymentDkr { get; private set; }

        public GetWorkItemMaterialPreviewQueryResponse(decimal operationTimeMilliseconds, decimal totalWorkTimeMilliseconds, decimal workItemTotalPaymentDkr)
        {
            OperationTimeMilliseconds = operationTimeMilliseconds;
            TotalWorkTimeMilliseconds = totalWorkTimeMilliseconds;
            WorkItemTotalPaymentDkr = workItemTotalPaymentDkr;
        }
    }

    internal class GetWorkItemMaterialPreviewQueryHandler : IQueryHandler<GetWorkItemMaterialPreviewQuery, GetWorkItemMaterialPreviewQueryResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        
        public GetWorkItemMaterialPreviewQueryHandler(IProjectFolderRootRepository projectFolderRootRepository,
            IBaseRateAndSupplementRepository baseRateAndSupplementRepository)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
        }

        public async Task<GetWorkItemMaterialPreviewQueryResponse> Handle(GetWorkItemMaterialPreviewQuery query,
            CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(query.ProjectId.Value, cancellationToken);
            var systemBaseRateAndSupplementId = Guid.NewGuid();
            
            var supplementOperations = CreateSupplementOperations(query);
            var supplements = CreateSupplements(query);

            var workItem = WorkItem.Create(WorkItemId.Empty(), CatalogMaterialId.Empty(), query.WorkItemDate, WorkItemUser.Empty(),
                WorkItemText.Empty(), WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(03), query.WorkItemOperationTime,
                query.WorkItemAmount, WorkItemUnit.Meter, supplementOperations, supplements);

            var systemBaseRateAndSupplement = await _baseRateAndSupplementRepository.Get( cancellationToken);
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(systemBaseRateAndSupplement, projectFolderRoot.GetFolder(query.ProjectFolderId));
            
            var workItemCalculator = new WorkItemCalculator(baseRateAndSupplementProxy);
            var calculationResult = workItemCalculator.CalculateTotalOperationTime(workItem);

            var operationTimeMilliseconds = query.WorkItemOperationTime.Value;
            var totalWorkTimeMilliseconds = calculationResult.TotalWorkTimeExpression.Evaluate().Value;
            decimal totalWorkTimePaymentDkr = calculationResult.TotalPaymentExpression.Evaluate().Value;
            return new GetWorkItemMaterialPreviewQueryResponse(operationTimeMilliseconds, totalWorkTimeMilliseconds, totalWorkTimePaymentDkr);
        }

        private static IList<Domain.FolderWork.Supplements.Supplement> CreateSupplements(GetWorkItemMaterialPreviewQuery request)
        {
            var supplements = new List<Domain.FolderWork.Supplements.Supplement>();
            foreach (var supplement in request.Supplements)
            {
                supplements.Add(Domain.FolderWork.Supplements.Supplement.Create(
                    SupplementId.Empty(), CatalogSupplementId.Empty(), SupplementNumber.Empty(),
                    SupplementText.Empty(), supplement.SupplementPercentage));
            }

            return supplements;
        }

        private static List<Domain.FolderWork.Supplements.SupplementOperation> CreateSupplementOperations(GetWorkItemMaterialPreviewQuery request)
        {
            var supplementOperations = new List<Domain.FolderWork.Supplements.SupplementOperation>();
            foreach (var operation in request.SupplementOperations)
            {
                var supplementOperationType = operation.Type == SupplementOperation.SupplementOperationType.AmountRelated
                    ? SupplementOperationType.AmountRelated()
                    : SupplementOperationType.UnitRelated();

                supplementOperations.Add(Domain.FolderWork.Supplements.SupplementOperation.Create(
                    SupplementOperationId.Empty(), CatalogSupplementOperationId.Empty(), SupplementOperationText.Empty(), 
                    supplementOperationType, operation.OperationTime, operation.Amount));
            }

            return supplementOperations;
        }
    }

    internal class GetWorkItemMaterialPreviewQueryAuthorizer : IAuthorizer<GetWorkItemMaterialPreviewQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetWorkItemMaterialPreviewQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetWorkItemMaterialPreviewQuery query, CancellationToken cancellation)
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
