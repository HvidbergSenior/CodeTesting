using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;

namespace deftq.Pieceworks.Application.RegisterWorkItemMaterial
{
    public sealed class RegisterWorkItemMaterialCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        internal ProjectFolderId ProjectFolderId { get; }
        internal CatalogMaterialId MaterialId { get; }
        internal WorkItemId WorkItemId { get; }
        internal WorkItemText WorkItemText { get; }
        internal WorkItemDate WorkItemDate { get; }
        internal WorkItemDuration WorkItemOperationTime { get; }
        internal WorkItemMountingCode WorkItemMountingCode { get; }
        internal WorkItemAmount WorkItemAmount { get; }
        internal WorkItemEanNumber WorkItemEanNumber { get; }
        internal WorkItemUnit WorkItemUnit { get; }
        internal IList<SupplementOperation> SupplementOperations { get; }
        internal IList<Supplement> Supplements { get; }

        private RegisterWorkItemMaterialCommand(ProjectId projectId, ProjectFolderId projectFolderId, CatalogMaterialId materialId, WorkItemId workItemId,
            WorkItemText workItemText, WorkItemDate workItemDate, WorkItemDuration workItemOperationTime,
            WorkItemMountingCode workItemMountingCode, WorkItemAmount workItemAmount, WorkItemEanNumber workItemEanNumber,
            WorkItemUnit workItemUnit, IList<SupplementOperation> supplementOperations, IList<Supplement> supplements)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            MaterialId = materialId;
            WorkItemId = workItemId;
            WorkItemText = workItemText;
            WorkItemDate = workItemDate;
            WorkItemOperationTime = workItemOperationTime;
            WorkItemMountingCode = workItemMountingCode;
            WorkItemAmount = workItemAmount;
            WorkItemEanNumber = workItemEanNumber;
            WorkItemUnit = workItemUnit;
            SupplementOperations = supplementOperations;
            Supplements = supplements;
        }

        public static RegisterWorkItemMaterialCommand Create(Guid projectId, Guid projectFolderId, Guid materialId, Guid workItemId, string workItemText,
            DateOnly date, decimal operationTimeMilliseconds, int workItemMountingCode, decimal workItemAmount,
            string workItemEanNumber, string workItemUnit, IList<SupplementOperation> supplementOperations, IList<Supplement> supplements)
        {
            return new RegisterWorkItemMaterialCommand(ProjectId.Create(projectId), ProjectFolderId.Create(projectFolderId),
                CatalogMaterialId.Create(materialId),
                WorkItemId.Create(workItemId), WorkItemText.Create(workItemText), WorkItemDate.Create(date),
                WorkItemDuration.Create(operationTimeMilliseconds), WorkItemMountingCode.FromCode(workItemMountingCode),
                WorkItemAmount.Create(workItemAmount),
                WorkItemEanNumber.Create(workItemEanNumber), WorkItemUnit.Create(workItemUnit), supplementOperations, supplements);
        }
    }

    public sealed class SupplementOperation
    {
        public enum SupplementOperationType { AmountRelated, UnitRelated }

        internal SupplementOperationId SupplementOperationId { get; private set; }
        internal CatalogSupplementOperationId CatalogSupplementOperationId { get; private set; }
        internal SupplementOperationText Text { get; private set; }
        internal SupplementOperationType Type { get; private set; }
        internal SupplementOperationTime OperationTime { get; private set; }
        internal SupplementOperationAmount Amount { get; private set; }

        private SupplementOperation(SupplementOperationId supplementOperationId, CatalogSupplementOperationId catalogSupplementOperationId, SupplementOperationText text,
            SupplementOperationType type, SupplementOperationTime operationTime, SupplementOperationAmount amount)
        {
            SupplementOperationId = supplementOperationId;
            CatalogSupplementOperationId = catalogSupplementOperationId;
            Text = text;
            Type = type;
            OperationTime = operationTime;
            Amount = amount;
        }

        public static SupplementOperation Create(Guid supplementOperationId, Guid catalogSupplementOperationId, string text, SupplementOperationType type,
            decimal operationTimeMilliseconds, decimal amount)
        {
            return new SupplementOperation(SupplementOperationId.Create(supplementOperationId), CatalogSupplementOperationId.Create(catalogSupplementOperationId), SupplementOperationText.Create(text),
                type, SupplementOperationTime.Create(operationTimeMilliseconds), SupplementOperationAmount.Create(amount));
        }
    }

    public sealed class Supplement
    {
        internal SupplementId SupplementId { get; private set; }
        internal CatalogSupplementId CatalogSupplementId { get; private set; }
        internal SupplementNumber SupplementNumber { get; private set; }
        internal SupplementText SupplementText { get; private set; }
        internal SupplementPercentage SupplementPercentage { get; private set; }

        private Supplement(SupplementId supplementId, CatalogSupplementId catalogSupplementId, SupplementNumber supplementNumber, SupplementText supplementText,
            SupplementPercentage supplementPercentage)
        {
            SupplementId = supplementId;
            CatalogSupplementId = catalogSupplementId;
            SupplementNumber = supplementNumber;
            SupplementText = supplementText;
            SupplementPercentage = supplementPercentage;
        }

        public static Supplement Create(Guid supplementId, Guid catalogSupplementId, string supplementNumber, string supplementText,
            decimal supplementPercentage)
        {
            return new Supplement(SupplementId.Create(supplementId), CatalogSupplementId.Create(catalogSupplementId), SupplementNumber.Create(supplementNumber), SupplementText.Create(supplementText), SupplementPercentage.Create(supplementPercentage));
        }
    }

    internal class RegisterWorkItemMaterialCommandHandler : ICommandHandler<RegisterWorkItemMaterialCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public RegisterWorkItemMaterialCommandHandler(IProjectFolderRootRepository projectFolderRootRepository,
            IBaseRateAndSupplementRepository baseRateAndSupplementRepository,
            IProjectFolderWorkRepository projectFolderWorkRepository,
            IUnitOfWork unitOfWork,
            IExecutionContext executionContext)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
            _projectFolderWorkRepository = projectFolderWorkRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<ICommandResponse> Handle(RegisterWorkItemMaterialCommand request, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);

            var user = WorkItemUser.Create(_executionContext.UserId, _executionContext.UserName);
            var supplementOperations = CreateSupplementOperations(request);
            var supplements = CreateSupplements(request);

            var workItem = WorkItem.Create(request.WorkItemId, request.MaterialId, request.WorkItemDate, user, request.WorkItemText,
                request.WorkItemEanNumber, request.WorkItemMountingCode,
                request.WorkItemOperationTime, request.WorkItemAmount, request.WorkItemUnit, supplementOperations, supplements);
            
            var folderWork = await _projectFolderWorkRepository.GetByProjectAndFolderId(request.ProjectId.Value, request.ProjectFolderId.Value, cancellationToken);
            
            var systemBaseRateAndSupplement = await _baseRateAndSupplementRepository.Get(cancellationToken);
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(systemBaseRateAndSupplement, projectFolderRoot.GetFolder(request.ProjectFolderId));
            
            folderWork.AddWorkItem(workItem, baseRateAndSupplementProxy);

            await _projectFolderWorkRepository.Update(folderWork, cancellationToken);
            await _projectFolderWorkRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }

        private static IList<Domain.FolderWork.Supplements.Supplement> CreateSupplements(RegisterWorkItemMaterialCommand request)
        {
            var supplements = new List<Domain.FolderWork.Supplements.Supplement>();
            foreach (var supplement in request.Supplements)
            {
                supplements.Add(Domain.FolderWork.Supplements.Supplement.Create(
                    SupplementId.Create(Guid.NewGuid()), supplement.CatalogSupplementId, supplement.SupplementNumber,
                    supplement.SupplementText, supplement.SupplementPercentage));
            }

            return supplements;
        }

        private static List<Domain.FolderWork.Supplements.SupplementOperation> CreateSupplementOperations(RegisterWorkItemMaterialCommand request)
        {
            var supplementOperations = new List<Domain.FolderWork.Supplements.SupplementOperation>();
            foreach (var operation in request.SupplementOperations)
            {
                var supplementOperationType = operation.Type == SupplementOperation.SupplementOperationType.AmountRelated
                    ? SupplementOperationType.AmountRelated()
                    : SupplementOperationType.UnitRelated();

                supplementOperations.Add(Domain.FolderWork.Supplements.SupplementOperation.Create(
                    operation.SupplementOperationId, operation.CatalogSupplementOperationId,
                    operation.Text, supplementOperationType, operation.OperationTime,
                    operation.Amount));
            }

            return supplementOperations;
        }
    }
    
    internal class RegisterWorkItemMaterialCommandAuthorizer : IAuthorizer<RegisterWorkItemMaterialCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public RegisterWorkItemMaterialCommandAuthorizer(IProjectRepository projectRepository, IProjectFolderRootRepository projectFolderRootRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _projectFolderRootRepository = projectFolderRootRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(RegisterWorkItemMaterialCommand command, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellation);
            
            var folderRoot = await _projectFolderRootRepository.GetByProjectId(project.ProjectId.Value, cancellation);

            var folder = folderRoot.GetFolder(command.ProjectFolderId);

            if (project.IsOwner(_executionContext.UserId) || (project.IsParticipant(_executionContext.UserId) && folder.IsUnlocked()))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}
