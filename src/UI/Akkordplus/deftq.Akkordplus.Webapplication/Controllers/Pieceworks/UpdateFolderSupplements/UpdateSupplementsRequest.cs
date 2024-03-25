namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateFolderSupplements
{
    public class UpdateFolderSupplementsRequest
    {
        public IEnumerable<Guid> FolderSupplements { get; }
        
        public UpdateFolderSupplementsRequest(IEnumerable<Guid> folderSupplements)
        {
            FolderSupplements = folderSupplements;
        }

        
    }
}
