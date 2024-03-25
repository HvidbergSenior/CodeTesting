
namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjectExtraworkAgreements
{
    public class ExtraWorkAgreementsResponse
    {
        public decimal TotalPaymentDkr { get; private set; }
        public IList<ExtraWorkAgreementResponse> ExtraWorkAgreements { get; private set; }

        public ExtraWorkAgreementsResponse(decimal totalPaymentDkr, IList<ExtraWorkAgreementResponse> extraWorkAgreements)
        {
            TotalPaymentDkr = totalPaymentDkr;
            ExtraWorkAgreements = extraWorkAgreements;
        }
    }

    public class ExtraWorkAgreementResponse
    {
        public Guid ExtraWorkAgreementId { get; private set; }
        public string ExtraWorkAgreementNumber { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public ExtraWorkAgreementTypeResponse ExtraWorkAgreementType { get; private set; }
        public decimal? PaymentDkr { get; private set; }
        public ExtraWorkAgreementWorkTime? WorkTime { get; private set; }

        public ExtraWorkAgreementResponse(Guid extraWorkAgreementId, string extraWorkAgreementNumber, string name, string? description, ExtraWorkAgreementTypeResponse extraWorkAgreementType, decimal? paymentDkr, ExtraWorkAgreementWorkTime? workTime)
        {
            ExtraWorkAgreementId = extraWorkAgreementId;
            ExtraWorkAgreementNumber = extraWorkAgreementNumber;
            Name = name;
            Description = description;
            ExtraWorkAgreementType = extraWorkAgreementType;
            PaymentDkr = paymentDkr;
            WorkTime = workTime;
        }
    }
    
    public enum ExtraWorkAgreementTypeResponse { CustomerHours, CompanyHours, AgreedPayment, Other }

    public class ExtraWorkAgreementWorkTime
    {
        public int Hours { get; private set; }
        public int Minutes { get; private set; }

        public ExtraWorkAgreementWorkTime(int hours, int minutes)
        {
            Hours = hours;
            Minutes = minutes;
        }
    }
}
