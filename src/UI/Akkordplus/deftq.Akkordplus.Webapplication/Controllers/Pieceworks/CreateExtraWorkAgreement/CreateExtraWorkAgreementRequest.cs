using deftq.Pieceworks.Application.RegisterExtraWorkAgreement;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateExtraWorkAgreement
{
    public class CreateExtraWorkAgreementRequest
    {
        public string ExtraWorkAgreementNumber { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public ExtraWorkAgreementTypeRequest ExtraWorkAgreementType { get; private set; }
        public decimal? PaymentDkr { get; private set; }
        public CreateExtraWorkAgreementWorkTime? WorkTime { get; private set; }
        
        public enum ExtraWorkAgreementTypeRequest
        {
            CustomerHours,
            CompanyHours,
            AgreedPayment,
            Other
        }
        
        public CreateExtraWorkAgreementRequest(string extraWorkAgreementNumber, string name, string? description, ExtraWorkAgreementTypeRequest extraWorkAgreementType, decimal? paymentDkr, CreateExtraWorkAgreementWorkTime? workTime)
        {
            ExtraWorkAgreementNumber = extraWorkAgreementNumber;
            Name = name;
            Description = description;
            ExtraWorkAgreementType = extraWorkAgreementType;
            PaymentDkr = paymentDkr;
            WorkTime = workTime;
        }
    }

    public class CreateExtraWorkAgreementWorkTime
    {
        public int Hours { get; private set; }
        public int Minutes { get; private set; }

        public CreateExtraWorkAgreementWorkTime(int hours, int minutes)
        {
            Hours = hours;
            Minutes = minutes;
        }
    }
}
