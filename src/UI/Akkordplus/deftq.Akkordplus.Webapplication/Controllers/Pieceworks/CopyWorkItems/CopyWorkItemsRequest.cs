namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CopyWorkItems
{
    public class CopyWorkItemsRequest
    {
        public Guid DestinationFolderId { get; private set; }
        public IList<Guid> WorkItemIds { get; private set; }
        
        public CopyWorkItemsRequest(Guid destinationFolderId, IList<Guid> workItemIds)
        {
            DestinationFolderId = destinationFolderId;
            WorkItemIds = workItemIds;
        }
    }
}
