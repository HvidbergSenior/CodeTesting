using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;
using FluentValidation;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public sealed class ProjectExtraWorkAgreement : Entity
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectExtraWorkAgreementId ProjectExtraWorkAgreementId { get; private set; }
        public ProjectExtraWorkAgreementName ProjectExtraWorkAgreementName { get; private set; }
        public ProjectExtraWorkAgreementDescription ProjectExtraWorkAgreementDescription { get; private set; }
        public ProjectExtraWorkAgreementType ProjectExtraWorkAgreementType { get; private set; }
        public ProjectExtraWorkAgreementNumber ProjectExtraWorkAgreementNumber { get; private set; }
        public ProjectExtraWorkAgreementWorkTime? ProjectExtraWorkAgreementWorkTime { get; private set; }
        public ProjectExtraWorkAgreementPaymentDkr ProjectExtraWorkAgreementPaymentDkr { get; private set; }

        private ProjectExtraWorkAgreement()
        {
            ProjectId = ProjectId.Empty();
            ProjectExtraWorkAgreementId = ProjectExtraWorkAgreementId.Empty();
            Id = ProjectExtraWorkAgreementId.Value;
            ProjectExtraWorkAgreementName = ProjectExtraWorkAgreementName.Empty();
            ProjectExtraWorkAgreementDescription = ProjectExtraWorkAgreementDescription.Empty();
            ProjectExtraWorkAgreementType = ProjectExtraWorkAgreementType.Other();
            ProjectExtraWorkAgreementNumber = ProjectExtraWorkAgreementNumber.Empty();
            ProjectExtraWorkAgreementWorkTime = ProjectExtraWorkAgreementWorkTime.Empty();
            ProjectExtraWorkAgreementPaymentDkr = ProjectExtraWorkAgreementPaymentDkr.Empty();
        }

        private ProjectExtraWorkAgreement(ProjectId projectId, ProjectExtraWorkAgreementId projectExtraWorkAgreementId,
            ProjectExtraWorkAgreementName projectExtraWorkAgreementName, ProjectExtraWorkAgreementDescription projectExtraWorkAgreementDescription,
            ProjectExtraWorkAgreementType projectExtraWorkAgreementType, ProjectExtraWorkAgreementNumber projectExtraWorkAgreementNumber,
            ProjectExtraWorkAgreementWorkTime? projectExtraWorkAgreementWorkTime,
            ProjectExtraWorkAgreementPaymentDkr projectExtraWorkAgreementPaymentDkr)
        {
            ProjectId = projectId;
            ProjectExtraWorkAgreementId = projectExtraWorkAgreementId;
            Id = ProjectExtraWorkAgreementId.Value;
            ProjectExtraWorkAgreementName = projectExtraWorkAgreementName;
            ProjectExtraWorkAgreementDescription = projectExtraWorkAgreementDescription;
            ProjectExtraWorkAgreementType = projectExtraWorkAgreementType;
            ProjectExtraWorkAgreementNumber = projectExtraWorkAgreementNumber;
            ProjectExtraWorkAgreementWorkTime = projectExtraWorkAgreementWorkTime;
            ProjectExtraWorkAgreementPaymentDkr = projectExtraWorkAgreementPaymentDkr;
        }

        /// <summary>
        /// Create a new Extra Work Agreement of type company or customer hours
        /// </summary>
        public static ProjectExtraWorkAgreement Create(ProjectId projectId, ProjectExtraWorkAgreementId projectExtraWorkAgreementId,
            ProjectExtraWorkAgreementName projectExtraWorkAgreementName, ProjectExtraWorkAgreementDescription projectExtraWorkAgreementDescription,
            ProjectExtraWorkAgreementType projectExtraWorkAgreementType, ProjectExtraWorkAgreementNumber projectExtraWorkAgreementNumber,
            ProjectExtraWorkAgreementWorkTime? projectExtraWorkAgreementWorkTime)
        {
            if (projectExtraWorkAgreementType != ProjectExtraWorkAgreementType.CompanyHours() &&
                projectExtraWorkAgreementType != ProjectExtraWorkAgreementType.CustomerHours())
            {
                throw new ValidationException("Type must be company or customer hours");
            }

            return new ProjectExtraWorkAgreement(projectId, projectExtraWorkAgreementId, projectExtraWorkAgreementName,
                projectExtraWorkAgreementDescription, projectExtraWorkAgreementType, projectExtraWorkAgreementNumber,
                projectExtraWorkAgreementWorkTime, ProjectExtraWorkAgreementPaymentDkr.Empty());
        }

        /// <summary>
        /// Create a new Extra Work Agreement of type agreed payment
        /// </summary>
        public static ProjectExtraWorkAgreement Create(ProjectId projectId, ProjectExtraWorkAgreementId projectExtraWorkAgreementId,
            ProjectExtraWorkAgreementName projectExtraWorkAgreementName, ProjectExtraWorkAgreementDescription projectExtraWorkAgreementDescription,
            ProjectExtraWorkAgreementNumber projectExtraWorkAgreementNumber, ProjectExtraWorkAgreementPaymentDkr projectExtraWorkAgreementPaymentDkr)

        {
            return new ProjectExtraWorkAgreement(projectId, projectExtraWorkAgreementId, projectExtraWorkAgreementName,
                projectExtraWorkAgreementDescription, ProjectExtraWorkAgreementType.AgreedPayment(), projectExtraWorkAgreementNumber,
                ProjectExtraWorkAgreementWorkTime.Empty(), projectExtraWorkAgreementPaymentDkr);
        }

        /// <summary>
        /// Create a new Extra Work Agreement of type other
        /// </summary>
        public static ProjectExtraWorkAgreement Create(ProjectId projectId, ProjectExtraWorkAgreementId projectExtraWorkAgreementId,
            ProjectExtraWorkAgreementName projectExtraWorkAgreementName, ProjectExtraWorkAgreementDescription projectExtraWorkAgreementDescription,
            ProjectExtraWorkAgreementNumber projectExtraWorkAgreementNumber)
        {
            return new ProjectExtraWorkAgreement(projectId, projectExtraWorkAgreementId, projectExtraWorkAgreementName,
                projectExtraWorkAgreementDescription, ProjectExtraWorkAgreementType.Other(), projectExtraWorkAgreementNumber,
                ProjectExtraWorkAgreementWorkTime.Empty(), ProjectExtraWorkAgreementPaymentDkr.Empty());
        }

        public void UpdatePaymentDkr(ProjectExtraWorkAgreementPaymentDkr paymentDkr)
        {
            ProjectExtraWorkAgreementPaymentDkr = paymentDkr;
        }
    }
}
