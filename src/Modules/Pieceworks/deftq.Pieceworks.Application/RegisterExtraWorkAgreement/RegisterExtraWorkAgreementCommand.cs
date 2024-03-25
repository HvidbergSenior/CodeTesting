using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using FluentValidation;

namespace deftq.Pieceworks.Application.RegisterExtraWorkAgreement
{
    public sealed class RegisterExtraWorkAgreementCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public ProjectExtraWorkAgreementId ProjectExtraWorkAgreementId { get; set; }
        public ProjectExtraWorkAgreementNumber ProjectExtraWorkAgreementNumber { get; set; }
        public ProjectExtraWorkAgreementName ProjectExtraWorkAgreementName { get; set; }
        public ProjectExtraWorkAgreementDescription ProjectExtraWorkAgreementDescription { get; set; }
        public ProjectExtraWorkAgreementType ProjectExtraWorkAgreementType { get; set; }
        public ProjectExtraWorkAgreementPaymentDkr? ProjectExtraWorkAgreementPaymentDkr { get; set; }
        public ProjectExtraWorkAgreementHours? ProjectExtraWorkAgreementHours { get; set; }
        public ProjectExtraWorkAgreementMinutes? ProjectExtraWorkAgreementMinutes { get; set; }

        private RegisterExtraWorkAgreementCommand(ProjectId projectId, ProjectExtraWorkAgreementId projectExtraWorkAgreementId,
            ProjectExtraWorkAgreementNumber projectExtraWorkAgreementNumber, ProjectExtraWorkAgreementName projectExtraWorkAgreementName,
            ProjectExtraWorkAgreementDescription projectExtraWorkAgreementDescription, ProjectExtraWorkAgreementType projectExtraWorkAgreementType,
            ProjectExtraWorkAgreementPaymentDkr? projectExtraWorkAgreementPaymentDkr,
            ProjectExtraWorkAgreementHours? projectExtraWorkAgreementHours, ProjectExtraWorkAgreementMinutes? projectExtraWorkAgreementMinutes)
        {
            ProjectId = projectId;
            ProjectExtraWorkAgreementId = projectExtraWorkAgreementId;
            ProjectExtraWorkAgreementNumber = projectExtraWorkAgreementNumber;
            ProjectExtraWorkAgreementName = projectExtraWorkAgreementName;
            ProjectExtraWorkAgreementDescription = projectExtraWorkAgreementDescription;
            ProjectExtraWorkAgreementType = projectExtraWorkAgreementType;
            ProjectExtraWorkAgreementPaymentDkr = projectExtraWorkAgreementPaymentDkr;
            ProjectExtraWorkAgreementHours = projectExtraWorkAgreementHours;
            ProjectExtraWorkAgreementMinutes = projectExtraWorkAgreementMinutes;
        }

        public static RegisterExtraWorkAgreementCommand Create(Guid projectId, Guid projectExtraWorkAgreementId,
            string projectExtraWorkAgreementNumber, string projectExtraWorkAgreementName, string projectExtraWorkAgreementDescription,
            ExtraWorkAgreementType projectExtraWorkAgreementType, decimal? projectExtraWorkAgreementPaymentDkr,
            int? projectExtraWorkAgreementHours, int? projectExtraWorkAgreementMinutes)
        {
            var extraWorkAgreementHours = projectExtraWorkAgreementHours is not null
                ? ProjectExtraWorkAgreementHours.Create(projectExtraWorkAgreementHours.Value)
                : null;

            var extraWorkAgreementMinutes = projectExtraWorkAgreementMinutes is not null
                ? ProjectExtraWorkAgreementMinutes.Create(projectExtraWorkAgreementMinutes.Value)
                : null;

            var extraWorkAgreementPaymentDkr = projectExtraWorkAgreementPaymentDkr is not null
                ? ProjectExtraWorkAgreementPaymentDkr.Create(projectExtraWorkAgreementPaymentDkr.Value)
                : null;

            return new RegisterExtraWorkAgreementCommand(ProjectId.Create(projectId), ProjectExtraWorkAgreementId.Create(projectExtraWorkAgreementId),
                ProjectExtraWorkAgreementNumber.Create(projectExtraWorkAgreementNumber),
                ProjectExtraWorkAgreementName.Create(projectExtraWorkAgreementName),
                ProjectExtraWorkAgreementDescription.Create(projectExtraWorkAgreementDescription), projectExtraWorkAgreementType.ToDomain(),
                extraWorkAgreementPaymentDkr,
                extraWorkAgreementHours,
                extraWorkAgreementMinutes);
        }
    }

    internal class RegisterExtraWorkAgreementCommandHandler : ICommandHandler<RegisterExtraWorkAgreementCommand, ICommandResponse>
    {
        private readonly IProjectExtraWorkAgreementListRepository _projectExtraWorkAgreementListRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterExtraWorkAgreementCommandHandler(IProjectExtraWorkAgreementListRepository projectExtraWorkAgreementListRepository,
            IUnitOfWork unitOfWork)
        {
            _projectExtraWorkAgreementListRepository = projectExtraWorkAgreementListRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(RegisterExtraWorkAgreementCommand command, CancellationToken cancellationToken)
        {
            var extraWorkAgreementList = await _projectExtraWorkAgreementListRepository.GetByProjectId(command.ProjectId.Value, cancellationToken);

            if (command.ProjectExtraWorkAgreementType == ProjectExtraWorkAgreementType.AgreedPayment())
            {
                if (command.ProjectExtraWorkAgreementPaymentDkr is null)
                {
                    throw new ValidationException("Must include Payment");
                }

                var extraWorkAgreement = ProjectExtraWorkAgreement.Create(command.ProjectId, command.ProjectExtraWorkAgreementId,
                    command.ProjectExtraWorkAgreementName, command.ProjectExtraWorkAgreementDescription,
                    command.ProjectExtraWorkAgreementNumber, command.ProjectExtraWorkAgreementPaymentDkr);

                extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement);
            }
            else if (command.ProjectExtraWorkAgreementType == ProjectExtraWorkAgreementType.CustomerHours() ||
                     command.ProjectExtraWorkAgreementType == ProjectExtraWorkAgreementType.CompanyHours())
            {
                if (command.ProjectExtraWorkAgreementHours is null || command.ProjectExtraWorkAgreementMinutes is null)
                {
                    throw new ValidationException("Must include work time");
                }

                var workTime = ProjectExtraWorkAgreementWorkTime.Create(command.ProjectExtraWorkAgreementHours,
                    command.ProjectExtraWorkAgreementMinutes);

                var extraWorkAgreement = ProjectExtraWorkAgreement.Create(command.ProjectId, command.ProjectExtraWorkAgreementId,
                    command.ProjectExtraWorkAgreementName, command.ProjectExtraWorkAgreementDescription, command.ProjectExtraWorkAgreementType,
                    command.ProjectExtraWorkAgreementNumber, workTime);

                extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement);
            }
            else
            {
                var extraWorkAgreement = ProjectExtraWorkAgreement.Create(command.ProjectId, command.ProjectExtraWorkAgreementId,
                    command.ProjectExtraWorkAgreementName, command.ProjectExtraWorkAgreementDescription, command.ProjectExtraWorkAgreementNumber);

                extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement);
            }

            await _projectExtraWorkAgreementListRepository.Update(extraWorkAgreementList, cancellationToken);
            await _projectExtraWorkAgreementListRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class RegisterExtraWorkAgreementCommandAuthorizer : IAuthorizer<RegisterExtraWorkAgreementCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public RegisterExtraWorkAgreementCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(RegisterExtraWorkAgreementCommand command, CancellationToken cancellation)
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
