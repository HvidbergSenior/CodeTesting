using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;

namespace deftq.Pieceworks.Application.GetExtraWorkAgreements
{
    public sealed class GetExtraWorkAgreementsQuery : IQuery<GetExtraWorkAgreementsQueryResponse>
    {
        public ProjectId ProjectId { get; }

        private GetExtraWorkAgreementsQuery(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static GetExtraWorkAgreementsQuery Create(ProjectId projectId)
        {
            return new GetExtraWorkAgreementsQuery(projectId);
        }
    }

    internal class GetExtraWorkAgreementsQueryHandler : IQueryHandler<GetExtraWorkAgreementsQuery, GetExtraWorkAgreementsQueryResponse>
    {
        private readonly IProjectExtraWorkAgreementListRepository _projectExtraWorkAgreementListRepository;

        public GetExtraWorkAgreementsQueryHandler(IProjectExtraWorkAgreementListRepository projectExtraWorkAgreementListRepository)
        {
            _projectExtraWorkAgreementListRepository = projectExtraWorkAgreementListRepository;
        }

        public async Task<GetExtraWorkAgreementsQueryResponse> Handle(GetExtraWorkAgreementsQuery query, CancellationToken cancellationToken)
        {
            var extraWorkAgreementList = await _projectExtraWorkAgreementListRepository.GetByProjectId(query.ProjectId.Value, cancellationToken);

            return MapResponse(extraWorkAgreementList);
        }

        private GetExtraWorkAgreementsQueryResponse MapResponse(ProjectExtraWorkAgreementList extraWorkAgreementList)
        {
            
            var result = extraWorkAgreementList.ExtraWorkAgreements.Select(extraWorkAgreement =>
            {
                var workTime = extraWorkAgreement.ProjectExtraWorkAgreementWorkTime is not null
                    ? new ExtraWorkAgreementWorkTime(extraWorkAgreement.ProjectExtraWorkAgreementWorkTime.Hours.Value,
                        extraWorkAgreement.ProjectExtraWorkAgreementWorkTime.Minutes.Value)
                    : null;
                
                return new GetExtraWorkAgreementQueryResponse(
                    extraWorkAgreement.ProjectExtraWorkAgreementId.Value, extraWorkAgreement.ProjectExtraWorkAgreementNumber.Value,
                    extraWorkAgreement.ProjectExtraWorkAgreementName.Value, extraWorkAgreement.ProjectExtraWorkAgreementDescription.Value,
                    MapExtraWorkAgreementType(extraWorkAgreement.ProjectExtraWorkAgreementType),
                    extraWorkAgreement.ProjectExtraWorkAgreementPaymentDkr?.Value,
                    workTime);
            }).ToList();

            return new GetExtraWorkAgreementsQueryResponse(extraWorkAgreementList.ProjectExtraWorkAgreementTotalPaymentDkr.Value, result);
        }

        private ExtraWorkAgreementTypeResponse MapExtraWorkAgreementType(ProjectExtraWorkAgreementType extraWorkAgreementType)
        {
            return extraWorkAgreementType switch
            {
                 _ when extraWorkAgreementType == ProjectExtraWorkAgreementType.Other() => ExtraWorkAgreementTypeResponse.Other,
                 _ when extraWorkAgreementType == ProjectExtraWorkAgreementType.AgreedPayment() => ExtraWorkAgreementTypeResponse.AgreedPayment,
                 _ when extraWorkAgreementType == ProjectExtraWorkAgreementType.CustomerHours() => ExtraWorkAgreementTypeResponse.CustomerHours,
                 _ when extraWorkAgreementType == ProjectExtraWorkAgreementType.CompanyHours() => ExtraWorkAgreementTypeResponse.CompanyHours,
                 _ => throw new ArgumentException("Invalid ExtraWorkAgreementType", nameof(extraWorkAgreementType))
            };
        }
    }

    internal class GetExtraWorkAgreementsQueryAuthorizer : IAuthorizer<GetExtraWorkAgreementsQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetExtraWorkAgreementsQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetExtraWorkAgreementsQuery query, CancellationToken cancellation)
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
