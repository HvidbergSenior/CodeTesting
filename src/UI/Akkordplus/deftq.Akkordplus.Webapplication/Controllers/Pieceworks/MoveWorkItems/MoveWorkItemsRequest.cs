using System.Collections;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.MoveWorkItems
{
    public class MoveWorkItemsRequest
    {
        [SwaggerSchema("The destination folder, use '11111111-1111-1111-1111-111111111111' for root folder.")]
        public Guid DestinationFolderId { get; }
        
        [SwaggerSchema("The list of work items that are to be moved")]
        public IList<Guid> WorkItemIds { get;  }

        public MoveWorkItemsRequest(Guid destinationFolderId, IList<Guid> workItemIds)
        {
            DestinationFolderId = destinationFolderId;
            WorkItemIds = workItemIds;
        }
    }
}
