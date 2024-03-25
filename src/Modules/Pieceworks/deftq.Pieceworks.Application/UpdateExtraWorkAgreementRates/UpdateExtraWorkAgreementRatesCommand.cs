using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;

namespace deftq.Pieceworks.Application.UpdateExtraWorkAgreementRates
{
    public sealed class UpdateExtraWorkAgreementRatesCommand : ICommand<ICommandResponse>
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectExtraWorkCustomerHourRate CustomerHourRate { get; private set; }
        public ProjectExtraWorkAgreementCompanyHourRate CompanyHourRate { get; private set; }

        public UpdateExtraWorkAgreementRatesCommand()
        {
            ProjectId = ProjectId.Empty();
            CustomerHourRate = ProjectExtraWorkCustomerHourRate.Empty();
            CompanyHourRate = ProjectExtraWorkAgreementCompanyHourRate.Empty();
        }

        private UpdateExtraWorkAgreementRatesCommand(ProjectId projectId, ProjectExtraWorkCustomerHourRate customerHourRate,
            ProjectExtraWorkAgreementCompanyHourRate companyHourRate)
        {
            ProjectId = projectId;
            CustomerHourRate = customerHourRate;
            CompanyHourRate = companyHourRate;
        }

        public static UpdateExtraWorkAgreementRatesCommand Create(Guid projectId, decimal customerHourRateDkr, decimal companyHourRateDkr)
        {
            return new UpdateExtraWorkAgreementRatesCommand(ProjectId.Create(projectId), ProjectExtraWorkCustomerHourRate.Create(customerHourRateDkr),
                ProjectExtraWorkAgreementCompanyHourRate.Create(companyHourRateDkr));
        }
    }

    internal class UpdateExtraWorkAgreementRatesCommandHandler : ICommandHandler<UpdateExtraWorkAgreementRatesCommand, ICommandResponse>
    {
        private readonly IProjectExtraWorkAgreementListRepository _projectExtraWorkAgreementListRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateExtraWorkAgreementRatesCommandHandler(IProjectExtraWorkAgreementListRepository projectExtraWorkAgreementListRepository,
            IUnitOfWork unitOfWork)
        {
            _projectExtraWorkAgreementListRepository = projectExtraWorkAgreementListRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(UpdateExtraWorkAgreementRatesCommand command, CancellationToken cancellationToken)
        {
            var extraWorkAgreementList = await _projectExtraWorkAgreementListRepository.GetByProjectId(command.ProjectId.Value, cancellationToken);
            extraWorkAgreementList.SetCompanyAndCustomerRates(command.CompanyHourRate, command.CustomerHourRate);

            await _projectExtraWorkAgreementListRepository.Update(extraWorkAgreementList, cancellationToken);

            await _projectExtraWorkAgreementListRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class UpdateExtraWorkAgreementRatesCommandAuthorizer : IAuthorizer<UpdateExtraWorkAgreementRatesCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public UpdateExtraWorkAgreementRatesCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(UpdateExtraWorkAgreementRatesCommand command, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellation);

            if (project.IsOwner(_executionContext.UserId) || project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }
            
            return AuthorizationResult.Fail();
        }
    }
}
