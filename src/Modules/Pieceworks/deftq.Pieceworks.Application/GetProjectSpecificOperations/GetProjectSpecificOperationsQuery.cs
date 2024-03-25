using System.Reflection;
using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectSpecificOperation;

namespace deftq.Pieceworks.Application.GetProjectSpecificOperations
{
    public sealed class GetProjectSpecificOperationsQuery : IQuery<GetProjectSpecificOperationsListResponse>
    {
        public ProjectId ProjectId { get; }

        private GetProjectSpecificOperationsQuery(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static GetProjectSpecificOperationsQuery Create(ProjectId projectId)
        {
            return new GetProjectSpecificOperationsQuery(projectId);
        }
    }

    internal class GetProjectSpecificOperationsQueryHandler : IQueryHandler<GetProjectSpecificOperationsQuery, GetProjectSpecificOperationsListResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly IProjectSpecificOperationListRepository _repository;
        private readonly IExecutionContext _executionContext;
        private readonly ISystemTime _systemTime;

        public GetProjectSpecificOperationsQueryHandler(IProjectFolderRootRepository projectFolderRootRepository,
            IBaseRateAndSupplementRepository baseRateAndSupplementRepository, IProjectSpecificOperationListRepository repository,
            IExecutionContext executionContext, ISystemTime systemTime)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
            _repository = repository;
            _executionContext = executionContext;
            _systemTime = systemTime;
        }

        public async Task<GetProjectSpecificOperationsListResponse> Handle(GetProjectSpecificOperationsQuery query, CancellationToken cancellationToken)
        {
            var folderRoot = await _projectFolderRootRepository.GetByProjectId(query.ProjectId.Value, cancellationToken);
            var systemBaseRateAndSupplement = await _baseRateAndSupplementRepository.Get(cancellationToken);
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(systemBaseRateAndSupplement, folderRoot.RootFolder);
            var calc = new ProjectSpecificOperationCalculator(baseRateAndSupplementProxy, _systemTime);

            var operations = await _repository.GetByProjectId(query.ProjectId.Value, cancellationToken);
            var list = operations.ProjectSpecificOperations.Select(o => new GetProjectSpecificOperationResponse(o.ProjectSpecificOperationId.Value,
                o.ProjectSpecificOperationExtraWorkAgreementNumber.Value, o.ProjectSpecificOperationName.Value,
                o.ProjectSpecificOperationDescription.Value, o.OperationTime.Value, o.WorkingTime.Value, GetPayment(o, calc)));
            return new GetProjectSpecificOperationsListResponse(list);
        }

        private decimal GetPayment(ProjectSpecificOperation operation, ProjectSpecificOperationCalculator calc)
        {
            var result = calc.CalculateOperationPayment(operation.OperationTime.Value);
            return result.Payment.Evaluate().Value;
        }
    }

    internal class GetProjectSpecificOperationsQueryAuthorizer : IAuthorizer<GetProjectSpecificOperationsQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetProjectSpecificOperationsQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetProjectSpecificOperationsQuery query, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellation);
            if (project.IsOwner(_executionContext.UserId) || project.IsParticipant(_executionContext.UserId) ||
                project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
