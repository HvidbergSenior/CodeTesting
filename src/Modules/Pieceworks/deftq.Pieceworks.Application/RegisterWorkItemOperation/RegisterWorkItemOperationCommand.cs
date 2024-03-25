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

namespace deftq.Pieceworks.Application.RegisterWorkItemOperation
{
    public sealed class RegisterWorkItemOperationCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        internal ProjectFolderId ProjectFolderId { get; }
        internal CatalogOperationId CatalogOperationId { get; }
        internal WorkItemOperationNumber WorkItemOperationNumber { get; }
        internal WorkItemId WorkItemId { get; }
        internal WorkItemText WorkItemText { get; }
        internal WorkItemDate WorkItemDate { get; }
        internal WorkItemDuration WorkItemDuration { get; }
        internal WorkItemAmount WorkItemAmount { get; }
        internal IList<Supplement> Supplements { get; }

        private RegisterWorkItemOperationCommand(ProjectId projectId, ProjectFolderId projectFolderId, CatalogOperationId catalogOperationId,
            WorkItemOperationNumber workItemOperationNumber, WorkItemId workItemId, WorkItemText workItemText, WorkItemDate workItemDate, WorkItemDuration workItemDuration,
            WorkItemAmount workItemAmount, IList<Supplement> supplements)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            CatalogOperationId = catalogOperationId;
            WorkItemOperationNumber = workItemOperationNumber;
            WorkItemId = workItemId;
            WorkItemText = workItemText;
            WorkItemDate = workItemDate;
            WorkItemDuration = workItemDuration;
            WorkItemAmount = workItemAmount;
            Supplements = supplements;
        }

        public static RegisterWorkItemOperationCommand Create(Guid projectId, Guid projectFolderId, Guid catalogOperationId, string workItemOperationNumber, Guid workItemId,
            string workItemText, DateOnly date, decimal operationTimeMilliseconds, decimal workItemAmount, IList<Supplement> supplements)
        {
            return new RegisterWorkItemOperationCommand(ProjectId.Create(projectId), ProjectFolderId.Create(projectFolderId),
                CatalogOperationId.Create(catalogOperationId), WorkItemOperationNumber.Create(workItemOperationNumber), 
                WorkItemId.Create(workItemId), WorkItemText.Create(workItemText), WorkItemDate.Create(date),
                WorkItemDuration.Create(operationTimeMilliseconds),
                WorkItemAmount.Create(workItemAmount), supplements);
        }
    }
    
    public sealed class Supplement
    {
        internal SupplementId SupplementId { get; private set; }
        internal CatalogSupplementId CatalogSupplementId { get; private set; }
        internal SupplementNumber SupplementNumber { get; private set; }
        internal SupplementText SupplementText { get; private set; }
        internal SupplementPercentage SupplementPercentage { get; private set; }

        private Supplement(SupplementId supplementId, CatalogSupplementId catalogSupplementId, SupplementNumber supplementNumber,
            SupplementText supplementText,
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
            return new Supplement(SupplementId.Create(supplementId), CatalogSupplementId.Create(catalogSupplementId),
                SupplementNumber.Create(supplementNumber), SupplementText.Create(supplementText), SupplementPercentage.Create(supplementPercentage));
        }
    }

    internal class RegisterWorkItemOperationCommandHandler : ICommandHandler<RegisterWorkItemOperationCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public RegisterWorkItemOperationCommandHandler(IProjectFolderRootRepository projectFolderRootRepository,
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

        public async Task<ICommandResponse> Handle(RegisterWorkItemOperationCommand request, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);
            var systemBaseRateAndSupplementId = Guid.NewGuid();

            var user = WorkItemUser.Create(_executionContext.UserId, _executionContext.UserName);
            var supplements = CreateSupplements(request);

            var workItem = WorkItem.Create(request.WorkItemId, request.CatalogOperationId, request.WorkItemOperationNumber, request.WorkItemDate, user, request.WorkItemText,
                request.WorkItemDuration, request.WorkItemAmount, supplements);
            
            var folderWork = await _projectFolderWorkRepository.GetByProjectAndFolderId(request.ProjectId.Value, request.ProjectFolderId.Value, cancellationToken);

            var systemBaseRateAndSupplement = await _baseRateAndSupplementRepository.Get(cancellationToken);
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(systemBaseRateAndSupplement, projectFolderRoot.GetFolder(request.ProjectFolderId));
            
            folderWork.AddWorkItem(workItem, baseRateAndSupplementProxy);

            await _projectFolderWorkRepository.Update(folderWork, cancellationToken);
            await _projectFolderWorkRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }

        private static IList<Domain.FolderWork.Supplements.Supplement> CreateSupplements(RegisterWorkItemOperationCommand request)
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
    }

    internal class RegisterWorkItemOperationCommandAuthorizer : IAuthorizer<RegisterWorkItemOperationCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public RegisterWorkItemOperationCommandAuthorizer(IProjectRepository projectRepository, IProjectFolderRootRepository projectFolderRootRepository, IUnitOfWork unitOfWork,
            IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _projectFolderRootRepository = projectFolderRootRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(RegisterWorkItemOperationCommand command, CancellationToken cancellation)
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
