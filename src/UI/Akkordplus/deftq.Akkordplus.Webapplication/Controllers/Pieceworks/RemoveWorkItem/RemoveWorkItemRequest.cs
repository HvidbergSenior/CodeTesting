namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveWorkItem
{
    public class RemoveWorkItemRequest
    {
        public IList<Guid> WorkItemIds { get; }

        public RemoveWorkItemRequest(IList<Guid> workItemIds)
        {
            WorkItemIds = workItemIds;
        }
    }
}
