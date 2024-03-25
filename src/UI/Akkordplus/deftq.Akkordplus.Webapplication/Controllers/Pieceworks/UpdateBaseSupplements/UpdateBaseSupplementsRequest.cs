namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateBaseSupplements
{
    public class UpdateBaseSupplementsRequest
    {
        public BaseSupplementUpdate IndirectTimePercentage { get; set; }

        public BaseSupplementUpdate SiteSpecificTimePercentage { get; set; }
        
        public UpdateBaseSupplementsRequest(BaseSupplementUpdate indirectTimePercentage, BaseSupplementUpdate siteSpecificTimePercentage)
        {
            IndirectTimePercentage = indirectTimePercentage;
            SiteSpecificTimePercentage = siteSpecificTimePercentage;
        }
    }

    public class BaseSupplementUpdate
    {
        public decimal Value { get; set; }
        
        public BaseSupplementStatusUpdate Status { get; set; }
        
        public BaseSupplementUpdate(decimal value, BaseSupplementStatusUpdate status)
        {
            Value = value;
            Status = status;
        }
    }

    public enum BaseSupplementStatusUpdate
    {
        Inherit,
        Overwrite
    }
}
