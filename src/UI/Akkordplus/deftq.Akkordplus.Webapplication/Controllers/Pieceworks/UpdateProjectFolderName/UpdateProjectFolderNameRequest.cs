
namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderName
{
    public class UpdateProjectFolderNameRequest
    {
        public string ProjectFolderName { get; }

        public UpdateProjectFolderNameRequest(string projectFolderName)
        {
            ProjectFolderName = projectFolderName;
        }
    }
}
