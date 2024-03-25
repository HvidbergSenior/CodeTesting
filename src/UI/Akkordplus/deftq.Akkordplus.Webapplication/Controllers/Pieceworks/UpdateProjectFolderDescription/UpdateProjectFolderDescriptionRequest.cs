
namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderDescription
{
    public class UpdateProjectFolderDescriptionRequest
    {
        public string ProjectFolderDescription{ get; }

        public UpdateProjectFolderDescriptionRequest(string projectFolderDescription)
        {
            ProjectFolderDescription = projectFolderDescription;
        }

    }
}
