using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;

namespace deftq.Pieceworks.Application.GetExtraWorkAgreementRates
{
    public sealed class GetExtraWorkAgreementRatesQuery  : IQuery<GetExtraWorkAgreementRatesQueryResponse>
    {
        public ProjectId ProjectId { get; }

        private GetExtraWorkAgreementRatesQuery(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static GetExtraWorkAgreementRatesQuery Create(ProjectId projectId)
        {
            return new GetExtraWorkAgreementRatesQuery(projectId);
        }
    }

    internal class GetExtraWorkAgreementRatesQueryHandler : IQueryHandler<GetExtraWorkAgreementRatesQuery, GetExtraWorkAgreementRatesQueryResponse>
    {
        private readonly IProjectExtraWorkAgreementListRepository _projectExtraWorkAgreementListRepository;

        public GetExtraWorkAgreementRatesQueryHandler(IProjectExtraWorkAgreementListRepository projectExtraWorkAgreementListRepository)
        {
            _projectExtraWorkAgreementListRepository = projectExtraWorkAgreementListRepository;
        }

        public async Task<GetExtraWorkAgreementRatesQueryResponse> Handle(GetExtraWorkAgreementRatesQuery query, CancellationToken cancellationToken)
        {
            var extraWorkAgreementList = await _projectExtraWorkAgreementListRepository.GetByProjectId(query.ProjectId.Value, cancellationToken);

            var customerRatePerHourDkr = extraWorkAgreementList.ProjectExtraWorkAgreementCustomerHourRate.Value;
            var companyRatePerHourDkr = extraWorkAgreementList.ProjectExtraWorkAgreementCompanyHourRate.Value;
            return new GetExtraWorkAgreementRatesQueryResponse(customerRatePerHourDkr, companyRatePerHourDkr);
        }
    }

    internal class GetExtraWorkAgreementRatesQueryAuthorizer : IAuthorizer<GetExtraWorkAgreementRatesQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetExtraWorkAgreementRatesQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetExtraWorkAgreementRatesQuery query, CancellationToken cancellation)
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
