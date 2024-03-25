namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateBaseRate
{
    public class UpdateBaseRateRequest
    {
        public BaseRateUpdate BaseRateRegulationPercentage { get; set; }
        
        public UpdateBaseRateRequest(BaseRateUpdate baseRateRegulationPercentage)
        {
            BaseRateRegulationPercentage = baseRateRegulationPercentage;
        }
    }

    public class BaseRateUpdate
    {
        public decimal Value { get; set; }
        
        public BaseRateStatusUpdate Status { get; set; }
        
        public BaseRateUpdate(decimal value, BaseRateStatusUpdate status)
        {
            Value = value;
            Status = status;
        }
    }

    public enum BaseRateStatusUpdate
    {
        Inherit,
        Overwrite
    }
}
