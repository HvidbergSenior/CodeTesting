
namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectNameAndOrderNumber
{
    public class UpdateProjectInformationRequest
    {
        public string Name { get; }
        public string? Description { get; }
        public string? OrderNumber { get; }
        public string? PieceworkNumber { get; }
        
        
        public UpdateProjectInformationRequest(string name, string? description, string? orderNumber, string? pieceworkNumber)
        {
            Name = name;
            Description = description;
            OrderNumber = orderNumber;
            PieceworkNumber = pieceworkNumber;
        }
    }
}
