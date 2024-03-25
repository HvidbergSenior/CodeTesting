namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateWorkItem
{
    public class UpdateWorkItemRequest
    {
        public decimal WorkItemAmount { get; private set; }

        public UpdateWorkItemRequest(decimal workItemAmount)
        {
            WorkItemAmount = workItemAmount;
        }
    }
}
