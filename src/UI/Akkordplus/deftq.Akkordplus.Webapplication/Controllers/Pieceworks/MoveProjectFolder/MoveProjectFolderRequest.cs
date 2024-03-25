using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.MoveProjectFolder
{
    public class MoveProjectFolderRequest
    {
        [SwaggerSchema("The source folder to move.")]
        public Guid FolderId { get; }
        
        [SwaggerSchema("The destination folder, use '11111111-1111-1111-1111-111111111111' for root folder.")]
        public Guid DestinationFolderId { get; }

        public MoveProjectFolderRequest(Guid folderId, Guid destinationFolderId)
        {
            FolderId = folderId;
            DestinationFolderId = destinationFolderId;
        }
    }
}
