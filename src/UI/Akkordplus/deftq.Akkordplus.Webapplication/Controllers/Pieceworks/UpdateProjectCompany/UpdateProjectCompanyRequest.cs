namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectCompany
{
    public class UpdateProjectCompanyRequest
    {
        public string? Company { get; }
        public string? WorkplaceAdr { get; }
        public string? CvrNumber { get; }
        public string? PNumber { get; }

        public UpdateProjectCompanyRequest(string? company, string? workplaceAdr, string? cvrNumber, string? pNumber)
        {
            Company = company;
            WorkplaceAdr = workplaceAdr;
            CvrNumber = cvrNumber;
            PNumber = pNumber;
        }
    }
}
