namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderExtraWork
{
    public class UpdateProjectFolderExtraWorkRequest
    {
        public enum ExtraWorkUpdate { ExtraWork, NormalWork }

        public ExtraWorkUpdate FolderExtraWorkUpdate { get; private set; }

        public UpdateProjectFolderExtraWorkRequest(ExtraWorkUpdate folderExtraWorkUpdate)
        {
            FolderExtraWorkUpdate = folderExtraWorkUpdate;
        }
    }
}
