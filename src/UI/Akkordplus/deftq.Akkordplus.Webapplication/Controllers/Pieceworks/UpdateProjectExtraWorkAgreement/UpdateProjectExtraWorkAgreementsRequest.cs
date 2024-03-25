
namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectExtraWorkAgreement
{
    public class UpdateProjectExtraWorkAgreementsRequest
    {
        public string ExtraWorkAgreementNumber { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public UpdateExtraWorkAgreementType ExtraWorkAgreementType { get; private set; }
        public decimal? PaymentDkr { get; private set; }
        public UpdateExtraWorkAgreementWorkTime? WorkTime { get; private set; }

        public UpdateProjectExtraWorkAgreementsRequest(string extraWorkAgreementNumber, string name, string? description, UpdateExtraWorkAgreementType extraWorkAgreementType, decimal? paymentDkr, UpdateExtraWorkAgreementWorkTime? workTime)
        {
            ExtraWorkAgreementNumber = extraWorkAgreementNumber;
            Name = name;
            Description = description;
            ExtraWorkAgreementType = extraWorkAgreementType;
            PaymentDkr = paymentDkr;
            WorkTime = workTime;
           
        }
        
        public enum UpdateExtraWorkAgreementType { CustomerHours, CompanyHours, AgreedPayment, Other }
    }
    
    public class UpdateExtraWorkAgreementWorkTime
    {
        public int Hours { get; private set; }
        public int Minutes { get; private set; }

        public UpdateExtraWorkAgreementWorkTime(int hours, int minutes)
        {
            Hours = hours;
            Minutes = minutes;
        }
    }
}
