using deftq.Pieceworks.Domain.Calculation.Expression;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using static deftq.Pieceworks.Domain.Calculation.Expression.DecimalNumber;
using static deftq.Pieceworks.Domain.Calculation.Expression.DecimalNumberUnit;
using static deftq.Pieceworks.Domain.Calculation.Expression.SumExpression;
using static deftq.Pieceworks.Domain.Calculation.Expression.DivideExpression;
using static deftq.Pieceworks.Domain.Calculation.Expression.MultiplyExpression;

namespace deftq.Pieceworks.Domain.Calculation
{
    public class ExtraWorkAgreementCalculator
    {
        public TotalExtraWorkAgreementPaymentResult SumTotalPayment(ProjectExtraWorkAgreementList projectExtraWorkAgreementList)
        {
            if (projectExtraWorkAgreementList.ExtraWorkAgreements.Count == 0)
            {
                return new TotalExtraWorkAgreementPaymentResult(Number(0, Dkr));
            }

            IExpression totalSum = Number(projectExtraWorkAgreementList.ExtraWorkAgreements.First().ProjectExtraWorkAgreementPaymentDkr.Value);
            foreach (var extraWorkAgreement in projectExtraWorkAgreementList.ExtraWorkAgreements.Skip(1))
            {
                IExpression extraWorkAgreementPayment = Number(extraWorkAgreement.ProjectExtraWorkAgreementPaymentDkr.Value);
                totalSum = Sum(totalSum, extraWorkAgreementPayment);
            }

            return new TotalExtraWorkAgreementPaymentResult(totalSum);
        }

        public TotalExtraWorkAgreementPaymentResult CalculatePayment(ProjectExtraWorkAgreementCompanyHourRate companyHourRate,
            ProjectExtraWorkCustomerHourRate customerHourRate, ProjectExtraWorkAgreement projectExtraWorkAgreement)
        {
            if (projectExtraWorkAgreement.ProjectExtraWorkAgreementType == ProjectExtraWorkAgreementType.AgreedPayment())
            {
                var payment = projectExtraWorkAgreement.ProjectExtraWorkAgreementPaymentDkr!.Value;
                return new TotalExtraWorkAgreementPaymentResult(Number(payment));
            }
            else if (projectExtraWorkAgreement.ProjectExtraWorkAgreementType == ProjectExtraWorkAgreementType.CustomerHours())
            {
                var workTimeHours = new decimal(projectExtraWorkAgreement.ProjectExtraWorkAgreementWorkTime!.Hours.Value);
                var workTimeMinutes = new decimal(projectExtraWorkAgreement.ProjectExtraWorkAgreementWorkTime!.Minutes.Value);

                var summedPayment =
                    Multiply(Sum(Number(workTimeHours), (Divide(Number(workTimeMinutes), Number(60)))),
                        Number(customerHourRate.Value));

                return new TotalExtraWorkAgreementPaymentResult(summedPayment);
            }
            else if (projectExtraWorkAgreement.ProjectExtraWorkAgreementType == ProjectExtraWorkAgreementType.CompanyHours())
            {
                var workTimeHours = new decimal(projectExtraWorkAgreement.ProjectExtraWorkAgreementWorkTime!.Hours.Value);
                var workTimeMinutes = projectExtraWorkAgreement.ProjectExtraWorkAgreementWorkTime!.Minutes.Value;

                decimal hours = workTimeHours;
                decimal minutes = workTimeMinutes;

                var summedPayment =
                    Multiply(Sum(Number(hours), (Divide(Number(minutes), Number(60)))),
                        Number(companyHourRate.Value));

                return new TotalExtraWorkAgreementPaymentResult(summedPayment);
            }

            return new TotalExtraWorkAgreementPaymentResult(Number(0, Dkr));
        }
    }

    public class TotalExtraWorkAgreementPaymentResult
    {
        public IExpression Result { get; private set; }

        public TotalExtraWorkAgreementPaymentResult(IExpression result)
        {
            Result = result;
        }
    }
}
