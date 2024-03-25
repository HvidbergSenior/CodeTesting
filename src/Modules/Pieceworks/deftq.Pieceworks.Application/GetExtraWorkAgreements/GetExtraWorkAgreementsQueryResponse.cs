
namespace deftq.Pieceworks.Application.GetExtraWorkAgreements
{
    public class GetExtraWorkAgreementsQueryResponse
    {
        public decimal TotalPaymentDkr { get; private set; }
        public IList<GetExtraWorkAgreementQueryResponse> ExtraWorkAgreements { get; private set; }

        public GetExtraWorkAgreementsQueryResponse(decimal totalPaymentDkr, IList<GetExtraWorkAgreementQueryResponse> extraWorkAgreements)
        {
            TotalPaymentDkr = totalPaymentDkr;
            ExtraWorkAgreements = extraWorkAgreements;
        }
    }

    public class GetExtraWorkAgreementQueryResponse
    {
        public Guid ExtraWorkAgreementId { get; private set; }
        public string ExtraWorkAgreementNumber { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public ExtraWorkAgreementTypeResponse ExtraWorkAgreementType { get; private set; }
        public decimal? PaymentDkr { get; private set; }
        public ExtraWorkAgreementWorkTime? WorkTime { get; private set; }

        public GetExtraWorkAgreementQueryResponse(Guid extraWorkAgreementId, string extraWorkAgreementNumber, string name, string? description, ExtraWorkAgreementTypeResponse extraWorkAgreementType, decimal? paymentDkr, ExtraWorkAgreementWorkTime? workTime)
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
