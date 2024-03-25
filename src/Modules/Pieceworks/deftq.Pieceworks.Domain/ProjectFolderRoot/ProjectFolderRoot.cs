using System.Runtime.Serialization;
using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Domain;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderRoot : Entity
    {
        public static readonly ProjectFolderId RootFolderId = ProjectFolderId.Create(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        public ProjectId ProjectId { get; private set; }

        public ProjectFolderRootId ProjectFolderRootId { get; private set; }

        public ProjectFolder RootFolder { get; private set; }

        /// <summary>
        /// After project folder root has been deserialized, traverse the folder tree and set parent references.
        /// </summary>
        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            RootFolder.UpdateParent(null);
        }

        private ProjectFolderRoot()
        {
            ProjectFolderRootId = ProjectFolderRootId.Create(Guid.NewGuid());
            ProjectId = ProjectId.Empty();
            RootFolder = ProjectFolder.Create(RootFolderId, ProjectFolderName.Empty(), ProjectFolderDescription.Empty(),
                ProjectFolderCreatedBy.Empty(), FolderRateAndSupplement.InheritAll(), ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Normal());
        }

        private ProjectFolderRoot(ProjectId projectId, ProjectName projectName, ProjectFolderRootId projectFolderRootId,
            FolderRateAndSupplement folderRateAndSupplement)
        {
            Id = projectFolderRootId.Id;
            ProjectFolderRootId = projectFolderRootId;
            ProjectId = projectId;
            RootFolder = ProjectFolder.Create(RootFolderId, ProjectFolderName.Create(projectName.Value), ProjectFolderDescription.Empty(),
                ProjectFolderCreatedBy.Empty(), folderRateAndSupplement, ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Normal());
        }

        public static ProjectFolderRoot Create(ProjectId projectId, ProjectName projectName, ProjectFolderRootId projectFolderRootId,
            FolderRateAndSupplement folderRateAndSupplement)
        {
            var projectFolderRoot = new ProjectFolderRoot(projectId, projectName, projectFolderRootId, folderRateAndSupplement);
            return projectFolderRoot;
        }

        public void AddFolder(ProjectFolder folder)
        {
            RootFolder.AddSubFolder(ProjectId, folder);
            AddDomainEvent(ProjectFolderAddedDomainEvent.Create(ProjectId, folder.ProjectFolderId));
        }

        public void AddFolder(ProjectFolder folder, ProjectFolder parentFolder)
        {
            AddFolder(folder, parentFolder.ProjectFolderId);
        }
        
        public void AddFolder(ProjectFolder folder, ProjectFolderId parentFolderId)
        {
            var targetFolder = RootFolder.Find(parentFolderId);
            if (targetFolder != null)
            {
                targetFolder.AddSubFolder(ProjectId, folder);
                AddDomainEvent(ProjectFolderAddedDomainEvent.Create(ProjectId, folder.ProjectFolderId));
            }
            else
            {
                throw new ProjectFolderNotFoundException(parentFolderId.Value);
            }
        }

        public ProjectFolder GetFolder(ProjectFolderId projectFolderId)
        {
            var projectFolder = RootFolder.Find(projectFolderId);
            if (projectFolder is null)
            {
                throw new ProjectFolderNotFoundException(projectFolderId);
            }

            return projectFolder;
        }

        public void ChangeFolderName(ProjectFolderId projectFolderId, ProjectFolderName projectFolderName)
        {
            var folder = GetFolder(projectFolderId);
            var changeName = folder.ChangeName(projectFolderName);
            if (changeName)
            {
                AddDomainEvent(ProjectFolderNameUpdatedDomainEvent.Create(ProjectId, projectFolderId, folder.Name));
            }
        }

        public void ChangeFolderDescription(ProjectFolderId projectFolderId, ProjectFolderDescription projectFolderDescription)
        {
            var folder = GetFolder(projectFolderId);
            var changeDescription = folder.ChangeDescription(projectFolderDescription);
            if (changeDescription)
            {
                AddDomainEvent(ProjectFolderDescriptionUpdatedDomainEvent.Create(ProjectId, projectFolderId, projectFolderDescription));
            }
        }

        public void UpdateFolderSupplements(ProjectFolderId projectFolderId, IList<FolderSupplement> folderSupplements)
        {
            var folder = GetFolder(projectFolderId);
            var updateSupplements = folder.UpdateFolderSupplements(ProjectId, folderSupplements);
            
            foreach (var evt  in updateSupplements)
            {
                AddDomainEvent(evt);
            }
        }

        public void LockFolder(ProjectFolderId projectFolderId, bool recursive)
        {
            var folder = GetFolder(projectFolderId);
            folder.LockFolder(recursive);
        }

        public void MarkAsExtraWork(ProjectFolderId projectFolderId)
        {
            var folder = GetFolder(projectFolderId);
            folder.MarkAsExtraWork();
        }

        public void MarkAsNormalWork(ProjectFolderId projectFolderId)
        {
            var folder = GetFolder(projectFolderId);
            folder.MarkAsNormalWork();
        }

        public void UnlockFolder(ProjectFolderId projectFolderId, bool recursive)
        {
            var folder = GetFolder(projectFolderId);
            folder.UnlockFolder(recursive);
        }

        public bool RemoveFolder(ProjectFolderId projectFolderId)
        {
            var folder = GetFolder(projectFolderId);
            if (folder.ParentFolder is null)
            {
                throw new ProjectFolderNotFoundException(projectFolderId);
            }

            var removed = folder.ParentFolder.RemoveSubFolder(folder);
            if (removed)
            {
                AddDomainEvent(ProjectFolderRemovedDomainEvent.Create(ProjectId, projectFolderId));
            }

            return removed;
        }

        public void MoveFolder(ProjectFolderId folderId, ProjectFolderId destinationFolderId)
        {
            var folderToMove = GetFolder(folderId);
            if (folderToMove.ParentFolder is null)
            {
                throw new ProjectFolderNotFoundException(folderId);
            }

            if (destinationFolderId == folderToMove.ParentFolder.ProjectFolderId)
            {
                // Folder is already in destination folder
            }
            else if (destinationFolderId == RootFolderId)
            {
                // Move to top level
                if (folderToMove.ParentFolder != RootFolder)
                {
                    var from = folderToMove.ParentFolder;
                    from.RemoveSubFolder(folderToMove);
                    RootFolder.AddSubFolder(ProjectId, folderToMove);
                    AddDomainEvent(ProjectFolderMovedDomainEvent.Create(ProjectId, folderId, from.ProjectFolderId, RootFolderId));
                }
            }
            else
            {
                // Move into another folder
                var newParentFolder = GetFolder(destinationFolderId);
                if (newParentFolder == null || newParentFolder.ParentFolder == null)
                {
                    throw new ProjectFolderNotFoundException($"Unknown project folder id {destinationFolderId.Value}");
                }

                var from = folderToMove.ParentFolder;
                from.RemoveSubFolder(folderToMove);
                newParentFolder.AddSubFolder(ProjectId, folderToMove);
                AddDomainEvent(ProjectFolderMovedDomainEvent.Create(ProjectId, folderId, from.ProjectFolderId, newParentFolder.ProjectFolderId));
            }
        }

        public void AddDocument(ProjectFolderId projectFolderId, ProjectDocumentId projectDocumentId,
            ProjectDocumentName projectDocumentName, ProjectDocumentUploadedTimestamp uploadedTimestamp)
        {
            var folder = GetFolder(projectFolderId);
            folder.AddDocumentReference(projectDocumentId, projectDocumentName, uploadedTimestamp);
        }

        public void RemoveDocument(ProjectFolderId projectFolderId, ProjectDocumentId projectDocumentId)
        {
            var folder = GetFolder(projectFolderId);
            bool removed = folder.RemoveDocument(projectDocumentId);
            if (removed)
            {
                AddDomainEvent(ProjectDocumentRemovedDomainEvent.Create(ProjectId, projectFolderId, projectDocumentId));
            }
        }

        public void CopyFolder(ProjectFolderId folderId, ProjectFolderId destinationFolderId, IExecutionContext executionContext,
            ISystemTime systemTime)
        {
            var folderToCopy = GetFolder(folderId);
            var destinationFolder = GetFolder(destinationFolderId);
            var projectFolderCopyEvents = folderToCopy.CopyFolder(ProjectId, destinationFolder, executionContext, systemTime);
            
            foreach (var folderCopyEvent in projectFolderCopyEvents)
            {
                AddDomainEvent(folderCopyEvent);
            }
        }

        public void UpdateFolderBaseSupplement(ProjectFolderId folderId, FolderIndirectTimeSupplement folderIndirectTimeSupplement,
            FolderSiteSpecificTimeSupplement folderSiteSpecificTimeSupplement, Domain.BaseRateAndSupplement systemBaseRateAndSupplement)
        {
            var folder = GetFolder(folderId);
            
            var events = folder.UpdateFolderBaseSupplement(ProjectId, folderIndirectTimeSupplement, folderSiteSpecificTimeSupplement,
                FolderRateAndSupplement.Create(systemBaseRateAndSupplement));
            foreach (var evt in events)
            {
                AddDomainEvent(evt);
            }
        }

        public void UpdateFolderBaseRateRegulation(ProjectFolderId folderId, FolderBaseRateRegulation folderBaseRateRegulation,
            Domain.BaseRateAndSupplement systemBaseRateAndSupplement)
        {
            var folder = GetFolder(folderId);
            
            var events = folder.UpdateFolderBaseRateRegulation(ProjectId, folderBaseRateRegulation,
                FolderRateAndSupplement.Create(systemBaseRateAndSupplement));
            foreach (var evt in events)
            {
                AddDomainEvent(evt);
            }
        }

        public IList<ProjectFolder> GetFolderAndSubfolders(ProjectFolderId projectFolderId)
        {
            var projectFolder = GetFolder(projectFolderId);
            return projectFolder.GetFolderAndSubFolders();
        }
    }
}
