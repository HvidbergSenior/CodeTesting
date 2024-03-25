namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectLumpSum
{
    public class UpdateProjectLumpSumRequest
    {
        public decimal LumpSumDkr { get; set; }
        
        public UpdateProjectLumpSumRequest(decimal lumpSumDkr)
        {
            LumpSumDkr = lumpSumDkr;
        }
    }
}
