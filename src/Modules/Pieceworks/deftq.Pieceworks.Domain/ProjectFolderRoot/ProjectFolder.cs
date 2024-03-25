using Baseline;
using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Domain;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using Newtonsoft.Json;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolder : Entity
    {
        public ProjectFolderId ProjectFolderId { get; private set; }
        public ProjectFolderName Name { get; private set; }
        public ProjectFolderDescription Description { get; private set; }
        public IList<ProjectFolder> SubFolders { get; private set; }
        public ProjectFolderCreatedBy CreatedBy { get; private set; }
        public IList<DocumentReference> Documents { get; private set; }
        public FolderRateAndSupplement FolderRateAndSupplement { get; private set; }
        public ProjectFolderLock FolderLock { get; private set; }
        public ProjectFolderExtraWork ExtraWork { get; private set; }
        public IList<FolderSupplement> FolderSupplements { get; private set; }

        [JsonIgnore] public ProjectFolder? ParentFolder { get; private set; }

        private ProjectFolder()
        {
            Id = Guid.NewGuid();
            ProjectFolderId = ProjectFolderId.Create(Id);
            Name = ProjectFolderName.Empty();
            Description = ProjectFolderDescription.Empty();
            ParentFolder = null;
            SubFolders = new List<ProjectFolder>();
            CreatedBy = ProjectFolderCreatedBy.Empty();
            Documents = new List<DocumentReference>();
            FolderRateAndSupplement = FolderRateAndSupplement.InheritAll();
            FolderLock = ProjectFolderLock.Empty();
            ExtraWork = ProjectFolderExtraWork.Normal();
            FolderSupplements = new List<FolderSupplement>();
        }

        private ProjectFolder(ProjectFolderId projectFolderId, ProjectFolderName name, ProjectFolderDescription description,
            ProjectFolderCreatedBy createdBy, FolderRateAndSupplement folderRateAndSupplement, ProjectFolderLock folderFolderLock,
            ProjectFolderExtraWork extraWork)
        {
            Id = projectFolderId.Value;
            ProjectFolderId = projectFolderId;
            Name = name;
            Description = description;
            ParentFolder = null;
            SubFolders = new List<ProjectFolder>();
            CreatedBy = createdBy;
            Documents = new List<DocumentReference>();
            FolderRateAndSupplement = folderRateAndSupplement;
            FolderLock = folderFolderLock;
            ExtraWork = extraWork;
            FolderSupplements = new List<FolderSupplement>();
        }

        private ProjectFolder(ProjectFolderId projectFolderId, ProjectFolderName name, ProjectFolderDescription description,
            ProjectFolderCreatedBy createdBy, FolderRateAndSupplement folderRateAndSupplement, ProjectFolderLock folderFolderLock,
            ProjectFolderExtraWork extraWork, IList<FolderSupplement> supplements)
        {
            Id = projectFolderId.Value;
            ProjectFolderId = projectFolderId;
            Name = name;
            Description = description;
            ParentFolder = null;
            SubFolders = new List<ProjectFolder>();
            CreatedBy = createdBy;
            Documents = new List<DocumentReference>();
            FolderRateAndSupplement = folderRateAndSupplement;
            FolderLock = folderFolderLock;
            ExtraWork = extraWork;
            FolderSupplements = supplements;
        }

        public static ProjectFolder Create(ProjectFolderId projectFolderId, ProjectFolderName name, ProjectFolderDescription description,
            ProjectFolderCreatedBy createdBy, FolderRateAndSupplement folderRateAndSupplement, ProjectFolderLock folderLock,
            ProjectFolderExtraWork extraWork)
        {
            var projectFolder = new ProjectFolder(projectFolderId, name, description, createdBy, folderRateAndSupplement, folderLock, extraWork);
            projectFolder.AddDomainEvent(ProjectFolderCreatedDomainEvent.Create(projectFolderId));
            return projectFolder;
        }

        private static ProjectFolder Create(ProjectFolderId projectFolderId, ProjectFolderName name, ProjectFolderDescription description,
            ProjectFolderCreatedBy createdBy, FolderRateAndSupplement folderRateAndSupplement, ProjectFolderLock folderLock,
            ProjectFolderExtraWork extraWork, IList<FolderSupplement> supplements)
        {
            var projectFolder = new ProjectFolder(projectFolderId, name, description, createdBy, folderRateAndSupplement, folderLock, extraWork,
                supplements);
            projectFolder.AddDomainEvent(ProjectFolderCreatedDomainEvent.Create(projectFolderId));
            return projectFolder;
        }

        public bool IsRoot()
        {
            return ParentFolder == null;
        }

        public bool ChangeName(ProjectFolderName name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var changed = this.Name.Equals(name);
            if (changed)
            {
                return false;
            }

            this.Name = name;
            return changed;
        }

        public bool ChangeDescription(ProjectFolderDescription description)
        {
            if (description is null)
            {
                throw new ArgumentNullException(nameof(description));
            }

            var notChanged = this.Description.Equals(description);
            if (notChanged)
            {
                return false;
            }

            this.Description = description;
            return true;
        }

        public IList<ProjectFolderSupplementsUpdatedDomainEvent> UpdateFolderSupplements(ProjectId projectId, IList<FolderSupplement> supplements)
        {
            if (supplements is null)
            {
                throw new ArgumentNullException(nameof(supplements));
            }

            var newSupplementsIds = supplements.OrderBy(s => s.SupplementNumber.Value).Select(s => s.SupplementNumber.Value).ToList();
            var oldSupplementsIds = this.FolderSupplements.OrderBy(s => s.SupplementNumber.Value).Select(s => s.SupplementNumber.Value).ToList();

            if (newSupplementsIds == oldSupplementsIds)
            {
                //Nothing got updated
                return new List<ProjectFolderSupplementsUpdatedDomainEvent>();
            }

            this.FolderSupplements = supplements;

            return GetFolderAndSubFolders().Select(folder => ProjectFolderSupplementsUpdatedDomainEvent.Create(projectId, folder.ProjectFolderId)).ToList();
        }

        public ProjectFolder? Find(ProjectFolderId projectFolderId)
        {
            if (ProjectFolderId.Equals(projectFolderId))
            {
                return this;
            }

            return SubFolders.Select(child => child.Find(projectFolderId)).SkipWhile(child => child == null)
                .FirstOrDefault();
        }

        public bool RemoveSubFolder(ProjectFolder subFolder)
        {
            if (subFolder == null)
            {
                throw new ArgumentNullException(nameof(subFolder));
            }

            var removed = SubFolders.Remove(subFolder);
            if (removed)
            {
                subFolder.ParentFolder = null;
            }

            return removed;
        }

        public void AddSubFolder(ProjectId projectId, ProjectFolder otherFolder)
        {
            if (otherFolder == null)
            {
                throw new ArgumentNullException(nameof(otherFolder));
            }

            if (otherFolder.ParentFolder != null)
            {
                throw new InvalidOperationException(
                    "Folder is already part of the folder structure, remove folder before adding it again");
            }

            this.SubFolders.Add(otherFolder);
            otherFolder.ParentFolder = this;
            if (IsCyclic())
            {
                throw new InvalidOperationException("Invalid folder structure");
            }

            otherFolder.UpdateRateAndSupplementRecursively(projectId, FolderRateAndSupplement);
        }

        internal IList<ProjectFolderRateAndSupplementUpdatedDomainEvent> UpdateFolderBaseSupplement(ProjectId projectId,
            FolderIndirectTimeSupplement folderIndirectTimeSupplement,
            FolderSiteSpecificTimeSupplement folderSiteSpecificTimeSupplement, FolderRateAndSupplement systemFolderRateAndSupplement)
        {
            if (ParentFolder is null)
            {
                if (folderIndirectTimeSupplement.InheritStatus.IsInherited() || folderSiteSpecificTimeSupplement.InheritStatus.IsInherited())
                {
                    throw new InvalidOperationException("Unable to inherit values on root folder");
                }
            }

            var newRateAndSupplement = FolderRateAndSupplement.Create(folderIndirectTimeSupplement,
                folderSiteSpecificTimeSupplement, FolderRateAndSupplement.BaseRateRegulation);
            if (newRateAndSupplement == FolderRateAndSupplement)
            {
                // Nothing changed
                return new List<ProjectFolderRateAndSupplementUpdatedDomainEvent>();
            }

            FolderRateAndSupplement = newRateAndSupplement;
            return UpdateRateAndSupplementRecursively(projectId,
                ParentFolder is not null ? ParentFolder.FolderRateAndSupplement : systemFolderRateAndSupplement);
        }

        internal IList<ProjectFolderRateAndSupplementUpdatedDomainEvent> UpdateFolderBaseRateRegulation(ProjectId projectId,
            FolderBaseRateRegulation folderBaseRateRegulation, FolderRateAndSupplement systemFolderRateAndSupplement)
        {
            if (ParentFolder is null)
            {
                if (folderBaseRateRegulation.InheritStatus.IsInherited())
                {
                    throw new InvalidOperationException("Unable to inherit values on root folder");
                }
            }

            var newRateAndSupplement = FolderRateAndSupplement.Create(FolderRateAndSupplement.IndirectTimeSupplement,
                FolderRateAndSupplement.SiteSpecificTimeSupplement, folderBaseRateRegulation);
            if (newRateAndSupplement == FolderRateAndSupplement)
            {
                // Nothing changed
                return new List<ProjectFolderRateAndSupplementUpdatedDomainEvent>();
            }

            FolderRateAndSupplement = newRateAndSupplement;
            return UpdateRateAndSupplementRecursively(projectId,
                ParentFolder is not null ? ParentFolder.FolderRateAndSupplement : systemFolderRateAndSupplement);
        }

        private IList<ProjectFolderRateAndSupplementUpdatedDomainEvent> UpdateRateAndSupplementRecursively(ProjectId projectId,
            FolderRateAndSupplement parentFolderRateAndSupplement)
        {
            var baseRegulation = FolderRateAndSupplement.BaseRateRegulation;
            if (baseRegulation.InheritStatus.IsInherited())
            {
                baseRegulation = FolderBaseRateRegulation.Create(parentFolderRateAndSupplement.BaseRateRegulation.Value,
                    FolderValueInheritStatus.Inherit());
            }

            var indirectTimeSupplement = FolderRateAndSupplement.IndirectTimeSupplement;
            if (indirectTimeSupplement.InheritStatus.IsInherited())
            {
                indirectTimeSupplement = FolderIndirectTimeSupplement.Create(parentFolderRateAndSupplement.IndirectTimeSupplement.Value,
                    FolderValueInheritStatus.Inherit());
            }

            var siteSpecificTimeSupplement = FolderRateAndSupplement.SiteSpecificTimeSupplement;
            if (siteSpecificTimeSupplement.InheritStatus.IsInherited())
            {
                siteSpecificTimeSupplement = FolderSiteSpecificTimeSupplement.Create(
                    parentFolderRateAndSupplement.SiteSpecificTimeSupplement.Value,
                    FolderValueInheritStatus.Inherit());
            }

            FolderRateAndSupplement = FolderRateAndSupplement.Create(indirectTimeSupplement, siteSpecificTimeSupplement, baseRegulation);

            var events = new List<ProjectFolderRateAndSupplementUpdatedDomainEvent>();
            var evt = ProjectFolderRateAndSupplementUpdatedDomainEvent.Create(projectId, ProjectFolderId);
            events.Add(evt);
            foreach (var subFolder in SubFolders)
            {
                events.AddRange(subFolder.UpdateRateAndSupplementRecursively(projectId, FolderRateAndSupplement));
            }

            return events;
        }

        public IList<DomainEvent> CopyFolder(ProjectId projectId, ProjectFolder destinationFolder,
            IExecutionContext executionContext, ISystemTime systemTime)
        {
            var resultEvents = new List<DomainEvent>();
            CopyFolder(projectId, destinationFolder, executionContext, systemTime, out var copiedEvents);
            resultEvents.AddRange(copiedEvents);
            return resultEvents;
        }

        private void CopyFolder(ProjectId projectId, ProjectFolder destinationFolder,
            IExecutionContext executionContext, ISystemTime systemTime,
            out IList<ProjectFolderCopiedDomainEvent> copiedEvents)
        {
            copiedEvents = new List<ProjectFolderCopiedDomainEvent>();
            var copiedFolder = CreateCopy(this, executionContext, systemTime);
            copiedEvents.Add(ProjectFolderCopiedDomainEvent.Create(projectId, copiedFolder.ProjectFolderId, ProjectFolderId,
                destinationFolder.ProjectFolderId));

            foreach (var subFolder in SubFolders)
            {
                subFolder.CopyFolder(projectId, copiedFolder, executionContext, systemTime, out var copiedSubfolderEvents);
                copiedEvents.AddRange(copiedSubfolderEvents);
            }

            destinationFolder.AddSubFolder(projectId, copiedFolder);
        }

        private ProjectFolder CreateCopy(ProjectFolder folderToCopy, IExecutionContext executionContext, ISystemTime systemTime)
        {
            var createdBy = ProjectFolderCreatedBy.Create(executionContext.UserId.ToString(), systemTime.Now());
            var supplements = CreateSupplementCopies(folderToCopy.FolderSupplements);

            return Create(ProjectFolderId.Create(Guid.NewGuid()), ProjectFolderName.Create(folderToCopy.Name.Value),
                ProjectFolderDescription.Create(folderToCopy.Description.Value), createdBy, folderToCopy.FolderRateAndSupplement,
                folderToCopy.FolderLock, folderToCopy.ExtraWork, supplements);
        }

        private static IList<FolderSupplement> CreateSupplementCopies(IList<FolderSupplement> supplementsToCopy)
        {
            return supplementsToCopy.Select(FolderSupplement.Copy).ToList();
        }

        private bool IsCyclic()
        {
            return DepthFirstSearch(new HashSet<object>(), null);
        }

        private bool DepthFirstSearch(ISet<object> visited, ProjectFolder? from)
        {
            if (visited.Contains(this))
            {
                return true;
            }

            visited.Add(this);

            var itemsToVisit = SubFolders.ToList();
            if (ParentFolder != null)
            {
                itemsToVisit.Add(ParentFolder);
            }

            foreach (var item in itemsToVisit)
            {
                if (item == from)
                {
                    continue;
                }

                if (item.DepthFirstSearch(visited, this))
                {
                    return true;
                }
            }

            return false;
        }

        public void LockFolder(bool recursive)
        {
            if (!FolderLock.IsLocked())
            {
                FolderLock = ProjectFolderLock.Locked();
            }

            if (recursive)
            {
                foreach (var subFolder in SubFolders)
                {
                    subFolder.LockFolder(recursive);
                }
            }
        }

        public void UnlockFolder(bool recursive)
        {
            if (FolderLock.IsLocked())
            {
                FolderLock = ProjectFolderLock.Unlocked();
            }

            if (recursive)
            {
                foreach (var subFolder in SubFolders)
                {
                    subFolder.UnlockFolder(recursive);
                }
            }
        }

        public bool IsUnlocked()
        {
            return FolderLock == ProjectFolderLock.Unlocked();
        }

        public void AddDocumentReference(ProjectDocumentId projectDocumentId, ProjectDocumentName projectDocumentName,
            ProjectDocumentUploadedTimestamp uploadedTimestamp)
        {
            Documents.Add(DocumentReference.Create(projectDocumentId, projectDocumentName, uploadedTimestamp));
        }

        internal void UpdateParent(ProjectFolder? parent)
        {
            ParentFolder = parent;
            foreach (var subFolder in SubFolders)
            {
                subFolder.UpdateParent(this);
            }
        }

        public bool RemoveDocument(ProjectDocumentId projectDocumentId)
        {
            var documentReference = Documents.FirstOrDefault(d => d.ProjectDocumentId == projectDocumentId);
            if (documentReference is null)
            {
                throw new ProjectDocumentNotFoundException(projectDocumentId);
            }

            return Documents.Remove(documentReference);
        }

        public void MarkAsExtraWork()
        {
            ExtraWork = ProjectFolderExtraWork.ExtraWork();
        }

        public void MarkAsNormalWork()
        {
            ExtraWork = ProjectFolderExtraWork.Normal();
        }

        public bool IsExtraWork()
        {
            if (ExtraWork.IsExtraWork())
            {
                return true;
            }

            if (ParentFolder is not null)
            {
                return ParentFolder.IsExtraWork();
            }

            return false;
        }

        public IList<ProjectFolder> GetFolderAndSubFolders()
        {
            var folderIds = new List<ProjectFolder>();

            foreach (var subFolder in SubFolders)
            {
                folderIds.AddRange(subFolder.GetFolderAndSubFolders());
            }

            folderIds.Add(this);
            return folderIds;
        }

        public IList<FolderSupplement> GetEffectiveFolderSupplements()
        {
            var thisFoldersSupplements = FolderSupplements;
            var parentFoldersSupplements = ParentFolder?.GetEffectiveFolderSupplements() ?? new List<FolderSupplement>();

            var result = new List<FolderSupplement>();
            result.AddRange(parentFoldersSupplements);

            foreach (var folderSupplement in thisFoldersSupplements)
            {
                if (!result.Any(supplement => supplement.CatalogSupplementId.Equals(folderSupplement.CatalogSupplementId)))
                {
                    result.Add(folderSupplement);
                }
            }

            return result;
        }
    }
}
