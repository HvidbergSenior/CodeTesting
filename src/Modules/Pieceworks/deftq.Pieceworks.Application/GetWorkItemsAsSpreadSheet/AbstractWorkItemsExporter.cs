using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.GetWorkItemsAsSpreadSheet
{
    public abstract class AbstractWorkItemsExporter
    {
        protected abstract void VisitProject(Project project);
        protected abstract void VisitFolder(ProjectFolder folder);
        protected abstract void VisitWorkItem(ProjectFolder folder, WorkItem workItem);
        public abstract WorkItemsExport GetExport();
        
        public void ExportWorkItems(Project project, ProjectFolderRoot folderRoot, IList<ProjectFolderWork> folderWorks)
        {
            VisitProject(project);
            VisitFolders(folderRoot.RootFolder, folderWorks);
        }

        private void VisitFolders(ProjectFolder folder, IList<ProjectFolderWork> folderWorks)
        {
            // Visit current folder
            VisitFolder(folder);
            
            // Get folder work for current folder
            var folderWork = folderWorks.FirstOrDefault(folderWork => folderWork.ProjectFolderId == folder.ProjectFolderId);
            if (folderWork is null)
            {
                throw new InvalidOperationException("Unable to look up folderWork");
            }

            // Visit each work item
            foreach (var workItem in folderWork.WorkItems)
            {
                VisitWorkItem(folder, workItem);
            }

            // Visit each sub folder recursively
            foreach (var subFolder in folder.SubFolders)
            {
                VisitFolders(subFolder, folderWorks);
            }
        }
    }

    public class WorkItemsExport
    {
        public string FileName { get; private set; }
        public byte[] Data { get; private set; }

        public WorkItemsExport(string fileName, byte[] data)
        {
            FileName = fileName;
            Data = data;
        }
    }
}
