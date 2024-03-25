using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public sealed class ProjectExtraWorkAgreementList : Entity
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectExtraWorkAgreementListId ProjectExtraWorkAgreementListId { get; private set; }
        public ProjectExtraWorkAgreementCompanyHourRate ProjectExtraWorkAgreementCompanyHourRate { get; private set; }
        public ProjectExtraWorkCustomerHourRate ProjectExtraWorkAgreementCustomerHourRate { get; private set; }
        public ProjectExtraWorkAgreementTotalPaymentDkr ProjectExtraWorkAgreementTotalPaymentDkr { get; private set; }
        public IList<ProjectExtraWorkAgreement> ExtraWorkAgreements { get; private set; }

        private ProjectExtraWorkAgreementList()
        {
            ProjectId = ProjectId.Empty();
            ProjectExtraWorkAgreementListId = ProjectExtraWorkAgreementListId.Empty();
            Id = ProjectExtraWorkAgreementListId.Value;
            ProjectExtraWorkAgreementCompanyHourRate = ProjectExtraWorkAgreementCompanyHourRate.Empty();
            ProjectExtraWorkAgreementCustomerHourRate = ProjectExtraWorkCustomerHourRate.Empty();
            ProjectExtraWorkAgreementTotalPaymentDkr = ProjectExtraWorkAgreementTotalPaymentDkr.Empty();
            ExtraWorkAgreements = new List<ProjectExtraWorkAgreement>();
        }

        private ProjectExtraWorkAgreementList(ProjectId projectId, ProjectExtraWorkAgreementListId projectExtraWorkAgreementListId)
        {
            ProjectId = projectId;
            ProjectExtraWorkAgreementListId = projectExtraWorkAgreementListId;
            Id = ProjectExtraWorkAgreementListId.Value;
            ProjectExtraWorkAgreementCompanyHourRate = ProjectExtraWorkAgreementCompanyHourRate.Empty();
            ProjectExtraWorkAgreementCustomerHourRate = ProjectExtraWorkCustomerHourRate.Empty();
            ProjectExtraWorkAgreementTotalPaymentDkr = ProjectExtraWorkAgreementTotalPaymentDkr.Empty();
            ExtraWorkAgreements = new List<ProjectExtraWorkAgreement>();
        }

        public static ProjectExtraWorkAgreementList Create(ProjectId projectId, ProjectExtraWorkAgreementListId projectExtraWorkAgreementListId)
        {
            return new ProjectExtraWorkAgreementList(projectId, projectExtraWorkAgreementListId);
        }

        public static ProjectExtraWorkAgreementList Empty()
        {
            return new ProjectExtraWorkAgreementList();
        }

        public void SetCompanyAndCustomerRates(ProjectExtraWorkAgreementCompanyHourRate projectExtraWorkAgreementCompanyHourRate,
            ProjectExtraWorkCustomerHourRate projectExtraWorkCustomerHourRate)
        {
            ProjectExtraWorkAgreementCompanyHourRate = projectExtraWorkAgreementCompanyHourRate;
            ProjectExtraWorkAgreementCustomerHourRate = projectExtraWorkCustomerHourRate;

            foreach (var agreement in ExtraWorkAgreements)
            {
                UpdatePayment(agreement);
            }

            UpdateTotalPayment();
        }

        public void SetCompanyRate(ProjectExtraWorkAgreementCompanyHourRate projectExtraWorkAgreementCompanyHourRate)
        {
            SetCompanyAndCustomerRates(projectExtraWorkAgreementCompanyHourRate, this.ProjectExtraWorkAgreementCustomerHourRate);
        }

        public void SetCustomerRate(ProjectExtraWorkCustomerHourRate projectExtraWorkCustomerHourRate)
        {
            SetCompanyAndCustomerRates(this.ProjectExtraWorkAgreementCompanyHourRate, projectExtraWorkCustomerHourRate);
        }

        public void AddExtraWorkAgreement(ProjectExtraWorkAgreement extraWorkAgreement)
        {
            ExtraWorkAgreements.Add(extraWorkAgreement);
            UpdatePayment(extraWorkAgreement);
            UpdateTotalPayment();
        }

        public void RemoveExtraWorkAgreements(IList<ProjectExtraWorkAgreementId> extraWorkAgreementIds)
        {
            foreach (var extraWorkAgreementId in extraWorkAgreementIds)
            {
                RemoveExtraWorkAgreement(extraWorkAgreementId);
            }

            UpdateTotalPayment();
        }

        public void UpdateExtraWorkAgreement(ProjectId projectId, ProjectExtraWorkAgreementId projectExtraWorkAgreementId,
            ProjectExtraWorkAgreementNumber projectExtraWorkAgreementNumber, ProjectExtraWorkAgreementName projectExtraWorkAgreementName,
            ProjectExtraWorkAgreementDescription projectExtraWorkAgreementDescription, ProjectExtraWorkAgreementType projectExtraWorkAgreementType,
            ProjectExtraWorkAgreementPaymentDkr? projectExtraWorkAgreementPaymentDkr, ProjectExtraWorkAgreementHours? projectExtraWorkAgreementHours,
            ProjectExtraWorkAgreementMinutes? projectExtraWorkAgreementMinutes)
        {
            var foundExtraWorkAgreement =
                ExtraWorkAgreements.FirstOrDefault(
                    extraWorkAgreement => extraWorkAgreement.ProjectExtraWorkAgreementId == projectExtraWorkAgreementId);
            if (foundExtraWorkAgreement is null)
            {
                throw new ProjectExtraWorkAgreementNotFoundException(projectExtraWorkAgreementId.Value);
            }

            ProjectExtraWorkAgreement newExtraWorkAgreement;
            if (projectExtraWorkAgreementType == ProjectExtraWorkAgreementType.AgreedPayment())
            {
                newExtraWorkAgreement = ProjectExtraWorkAgreement.Create(projectId, projectExtraWorkAgreementId, projectExtraWorkAgreementName,
                    projectExtraWorkAgreementDescription, projectExtraWorkAgreementNumber, projectExtraWorkAgreementPaymentDkr!);
            } else if (projectExtraWorkAgreementType == ProjectExtraWorkAgreementType.CompanyHours()) 
            {
                newExtraWorkAgreement = ProjectExtraWorkAgreement.Create(projectId, projectExtraWorkAgreementId, projectExtraWorkAgreementName,
                    projectExtraWorkAgreementDescription, projectExtraWorkAgreementType, projectExtraWorkAgreementNumber,
                    ProjectExtraWorkAgreementWorkTime.Create(projectExtraWorkAgreementHours!, projectExtraWorkAgreementMinutes!));
            } else if (projectExtraWorkAgreementType == ProjectExtraWorkAgreementType.CustomerHours())
            {
                newExtraWorkAgreement = ProjectExtraWorkAgreement.Create(projectId, projectExtraWorkAgreementId, projectExtraWorkAgreementName,
                    projectExtraWorkAgreementDescription, projectExtraWorkAgreementType, projectExtraWorkAgreementNumber,
                    ProjectExtraWorkAgreementWorkTime.Create(projectExtraWorkAgreementHours!, projectExtraWorkAgreementMinutes!));
            } else if (projectExtraWorkAgreementType == ProjectExtraWorkAgreementType.Other())
            {
                newExtraWorkAgreement = ProjectExtraWorkAgreement.Create(projectId, projectExtraWorkAgreementId, projectExtraWorkAgreementName,
                    projectExtraWorkAgreementDescription, projectExtraWorkAgreementNumber);
            }
            else
            {
                throw new InvalidOperationException();
            }

            var index = ExtraWorkAgreements.IndexOf(foundExtraWorkAgreement);
            ExtraWorkAgreements[index] = newExtraWorkAgreement;
            UpdatePayment(newExtraWorkAgreement);
            UpdateTotalPayment();
        }

        private void RemoveExtraWorkAgreement(ProjectExtraWorkAgreementId extraWorkAgreementId)
        {
            var foundExtraWorkAgreement =
                ExtraWorkAgreements.FirstOrDefault(extraWorkAgreement => extraWorkAgreement.ProjectExtraWorkAgreementId == extraWorkAgreementId);
            if (foundExtraWorkAgreement is null)
            {
                throw new ProjectExtraWorkAgreementNotFoundException(extraWorkAgreementId.Value);
            }

            ExtraWorkAgreements.Remove(foundExtraWorkAgreement);
        }

        private void UpdatePayment(ProjectExtraWorkAgreement extraWorkAgreement)
        {
            var extraWorkAgreementCalculator = new ExtraWorkAgreementCalculator();
            var payment = extraWorkAgreementCalculator.CalculatePayment(ProjectExtraWorkAgreementCompanyHourRate,
                ProjectExtraWorkAgreementCustomerHourRate, extraWorkAgreement);
            extraWorkAgreement.UpdatePaymentDkr(ProjectExtraWorkAgreementPaymentDkr.Create(payment.Result.Evaluate().Value));
        }

        private void UpdateTotalPayment()
        {
            var extraWorkAgreementCalculator = new ExtraWorkAgreementCalculator();
            var calculateExtraWorkAgreements = extraWorkAgreementCalculator.SumTotalPayment(this);

            ProjectExtraWorkAgreementTotalPaymentDkr =
                ProjectExtraWorkAgreementTotalPaymentDkr.Create(calculateExtraWorkAgreements.Result.Evaluate().Value);
        }
    }
}
