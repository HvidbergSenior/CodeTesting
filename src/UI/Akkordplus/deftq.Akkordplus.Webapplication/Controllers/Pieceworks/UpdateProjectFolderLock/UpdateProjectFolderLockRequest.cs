namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderLock
{
    public class UpdateLockProjectFolderRequest
    {
        public enum Lock { Locked, Unlocked }
        public Lock FolderLock { get; private set; }
        public bool Recursive { get; private set; }
        public UpdateLockProjectFolderRequest(Lock folderLock, bool recursive)
        {
            FolderLock = folderLock;
            Recursive = recursive;
        }
    }
}
