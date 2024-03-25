namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateProjectFolder
{
    public class CreateProjectFolderRequest
    {
        public string FolderName { get; }
        public string? FolderDescription { get; }
        public Guid? ParentFolderId { get; }

        public CreateProjectFolderRequest(string folderName, string? folderDescription, Guid? parentFolderId = null)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentException($"'{nameof(folderName)}' cannot be null or empty.", nameof(folderName));
            }
            FolderName = folderName;
            FolderDescription = folderDescription;
            ParentFolderId = parentFolderId;
        }
    }
}
