using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CopyProjectFolder
{
    public class CopyProjectFolderRequest
    {
        [SwaggerSchema("The destination folder, use '11111111-1111-1111-1111-111111111111' for root folder.")]
        public Guid DestinationFolderId { get; }

        public CopyProjectFolderRequest(Guid destinationFolderId)
        {
            DestinationFolderId = destinationFolderId;
        }
    }
}
