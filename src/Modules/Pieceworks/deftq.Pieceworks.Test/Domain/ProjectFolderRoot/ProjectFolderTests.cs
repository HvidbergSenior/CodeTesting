using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Serialization;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Domain.projectFolderRoot
{
    public class ProjectFolderTest
    {
        private readonly BaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly ProjectFolderWorkInMemoryRepository _projectFolderWorkInMemoryRepository;

        public ProjectFolderTest()
        {
            _baseRateAndSupplementRepository = new BaseRateAndSupplementRepository();
            _projectFolderWorkInMemoryRepository = new ProjectFolderWorkInMemoryRepository();
        }

        [Fact]
        public void CanSerializeAndDeserialize()
        {
            var serializer = new JsonSerializer<ProjectFolder>();
            var folder = Any.ProjectFolder();
            var subFolder = Any.ProjectFolder();
            folder.AddSubFolder(Any.ProjectId(), subFolder);

            var json = serializer.Serialize(folder);
            var result = serializer.Deserialize(json);

            Assert.Equal(folder.ProjectFolderId.Value, result?.ProjectFolderId.Value);
            Assert.Equal(folder.SubFolders[0].ProjectFolderId.Value, result?.SubFolders[0].ProjectFolderId.Value);
        }

        [Fact]
        public void When_Created_ShouldHaveIdNameAndCreatedBy()
        {
            var id = Any.ProjectFolderId();
            var name = Any.ProjectFolderName();
            var description = Any.ProjectFolderDescription();
            var createdBy = Any.ProjectFolderCreatedBy();
            var folderRateAndSupplement = FolderRateAndSupplement.InheritAll();
            var folderLocked = ProjectFolderLock.Unlocked();
            var extraWork = ProjectFolderExtraWork.Normal();
            var folder = ProjectFolder.Create(id, name, description, createdBy, folderRateAndSupplement, folderLocked, extraWork);

            Assert.Equal(folder.ProjectFolderId.Value, id.Value);
            Assert.Equal(folder.Name, name);
            Assert.Equal(folder.CreatedBy.Name, createdBy.Name);
            Assert.Equal(folder.CreatedBy.Timestamp, createdBy.Timestamp);
        }

        [Fact]
        public void When_AddedToFolder_ShouldHaveSubFolder()
        {
            var parent = Any.ProjectFolder();
            var child = Any.ProjectFolder();

            parent.AddSubFolder(Any.ProjectId(), child);

            Assert.Contains(child, parent.SubFolders);
            Assert.Equal(child.ParentFolder?.ProjectFolderId.Value, parent.ProjectFolderId.Value);
        }

        [Fact]
        public void When_AddedToFolder_ShouldDetectLoops()
        {
            var folder1 = Any.ProjectFolder();
            var folder2 = Any.ProjectFolder();
            var folder3 = Any.ProjectFolder();
            var projectId = Any.ProjectId();
            folder1.AddSubFolder(projectId, folder2);
            folder1.AddSubFolder(projectId, folder3);

            Assert.Throws<InvalidOperationException>(() => folder1.AddSubFolder(projectId, folder1));
            Assert.Throws<InvalidOperationException>(() => folder2.AddSubFolder(projectId, folder2));
            Assert.Throws<InvalidOperationException>(() => folder2.AddSubFolder(projectId, folder1));
            Assert.Throws<InvalidOperationException>(() => folder3.AddSubFolder(projectId, folder1));
        }

        [Fact]
        public void When_FolderCreated_Should_Raise_ProjectFolderCreatedEvent()
        {
            var folder = Any.ProjectFolder();
            Assert.IsType<ProjectFolderCreatedDomainEvent>(folder.PublishedEvent<ProjectFolderCreatedDomainEvent>());
        }

        [Fact]
        public void When_RemovedFromFolder_ShouldNotHaveSubFolder()
        {
            var parent = Any.ProjectFolder();
            var child = Any.ProjectFolder();

            parent.AddSubFolder(Any.ProjectId(), child);
            parent.RemoveSubFolder(child);

            Assert.DoesNotContain(child, parent.SubFolders);
        }

        [Fact]
        public void When_UnknownFolderRemoved_ShouldReturnFalse()
        {
            var parent = Any.ProjectFolder();
            var child = Any.ProjectFolder();

            Assert.False(parent.RemoveSubFolder(child));
        }

        [Fact]
        public void When_DocumentAdded_FolderShouldContainDocument()
        {
            var folder = Any.ProjectFolder();
            var docId = Any.ProjectDocumentId();
            var docName = Any.ProjectDocumentName();
            var uploadedTimestamp = Any.ProjectDocumentUploadedTimestamp();
            folder.AddDocumentReference(docId, docName, uploadedTimestamp);

            Assert.Single(folder.Documents);
            Assert.Equal(docId, folder.Documents[0].ProjectDocumentId);
            Assert.Equal(docName, folder.Documents[0].ProjectDocumentName);
            Assert.Equal(uploadedTimestamp, folder.Documents[0].UploadedTimestamp);
        }

        [Fact]
        public void When_FolderIsLocked_LockFolder()
        {
            var folder = Any.ProjectFolder();
            folder.UnlockFolder(false);
            Assert.Equal(ProjectFolderLock.Unlocked(), folder.FolderLock);
            folder.LockFolder(false);
            Assert.Equal(ProjectFolderLock.Locked(), folder.FolderLock);
        }

        [Fact]
        public void When_FolderIsLockedRecursively_SubfoldersShouldBeLocked()
        {
            var rootFolder = Any.UnlockedFolder();
            var subFolder = Any.UnlockedFolder();
            var subFolder1 = Any.UnlockedFolder();

            rootFolder.AddSubFolder(Any.ProjectId(), subFolder);
            rootFolder.AddSubFolder(Any.ProjectId(), subFolder1);
            rootFolder.LockFolder(true);

            Assert.Equal(ProjectFolderLock.Locked(), rootFolder.FolderLock);
            Assert.Equal(ProjectFolderLock.Locked(), subFolder.FolderLock);
            Assert.Equal(ProjectFolderLock.Locked(), subFolder1.FolderLock);
        }

        [Fact]
        public void When_FolderIsLocked_RecursivelyLocking_ShouldLockSubFolders()
        {
            var rootFolder = Any.LockedFolder();
            var subFolder = Any.UnlockedFolder();
            var subFolder1 = Any.UnlockedFolder();

            rootFolder.AddSubFolder(Any.ProjectId(), subFolder);
            rootFolder.AddSubFolder(Any.ProjectId(), subFolder1);
            rootFolder.LockFolder(true);

            Assert.Equal(ProjectFolderLock.Locked(), rootFolder.FolderLock);
            Assert.Equal(ProjectFolderLock.Locked(), subFolder.FolderLock);
            Assert.Equal(ProjectFolderLock.Locked(), subFolder1.FolderLock);
        }

        [Fact]
        public void When_FolderIsLocked_UnlockingFolder_SubFoldersShouldRemainLocked()
        {
            var rootFolder = Any.LockedFolder();
            var subFolder = Any.LockedFolder();
            var subFolder1 = Any.LockedFolder();

            rootFolder.AddSubFolder(Any.ProjectId(), subFolder);
            rootFolder.AddSubFolder(Any.ProjectId(), subFolder1);
            rootFolder.UnlockFolder(false);

            Assert.Equal(ProjectFolderLock.Unlocked(), rootFolder.FolderLock);
            Assert.Equal(ProjectFolderLock.Locked(), subFolder.FolderLock);
            Assert.Equal(ProjectFolderLock.Locked(), subFolder1.FolderLock);
        }

        [Fact]
        public void When_FolderIsLocked_UnlockingFolderRecursively_SubFoldersShouldUnlock()
        {
            var rootFolder = Any.LockedFolder();
            var subFolder = Any.LockedFolder();
            var subFolder1 = Any.LockedFolder();

            rootFolder.AddSubFolder(Any.ProjectId(), subFolder);
            subFolder.AddSubFolder(Any.ProjectId(), subFolder1);
            rootFolder.UnlockFolder(true);

            Assert.Equal(ProjectFolderLock.Unlocked(), rootFolder.FolderLock);
            Assert.Equal(ProjectFolderLock.Unlocked(), subFolder.FolderLock);
            Assert.Equal(ProjectFolderLock.Unlocked(), subFolder1.FolderLock);
        }

        [Fact]
        public async Task When_FolderIsCopied_WorkItemsShouldBeCopied()
        {
            var folderRootRepository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var folderRoot = Any.ProjectFolderRoot();
            var folder = ProjectFolder.Create(ProjectFolderId.Create(Guid.NewGuid()), ProjectFolderName.Create("folder"),
                ProjectFolderDescription.Create("folder description"), ProjectFolderCreatedBy.Create("folder name", new DateTimeOffset()),
                FolderRateAndSupplement.InheritAll(), ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Normal());
            var workItem1 = Any.WorkItem();
            var workItem2 = Any.WorkItem();
            var folderWork = ProjectFolderWork.Create(ProjectFolderWorkId.Create(Guid.NewGuid()), folderRoot.ProjectId, folder.ProjectFolderId);
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder);
            folderWork.AddWorkItem(workItem1, baseRateAndSupplementProxy);
            folderWork.AddWorkItem(workItem2, baseRateAndSupplementProxy);
            await _projectFolderWorkInMemoryRepository.Add(folderWork);
            var subFolder = ProjectFolder.Create(ProjectFolderId.Create(Guid.NewGuid()), ProjectFolderName.Create("subfolder"),
                ProjectFolderDescription.Create("subfolder description"), ProjectFolderCreatedBy.Create("subfolder name", new DateTimeOffset()),
                FolderRateAndSupplement.InheritAll(), ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Normal());

            var destinationFolder = ProjectFolder.Create(ProjectFolderId.Create(Guid.NewGuid()), ProjectFolderName.Create("destinationfolder"),
                ProjectFolderDescription.Create("destinationfolder description"),
                ProjectFolderCreatedBy.Create("destinationfolder name", new DateTimeOffset()), FolderRateAndSupplement.InheritAll(),
                ProjectFolderLock.Unlocked(),
                ProjectFolderExtraWork.Normal());

            folderRoot.AddFolder(folder);
            folderRoot.AddFolder(destinationFolder);
            folder.AddSubFolder(folderRoot.ProjectId, subFolder);
            await folderRootRepository.Add(folderRoot);

            var handler = new ProjectFolderCopiedEventHandler(folderRootRepository, _projectFolderWorkInMemoryRepository,
                _baseRateAndSupplementRepository, executionContext);
            var evt = ProjectFolderCopiedDomainEvent.Create(folderRoot.ProjectId, destinationFolder.ProjectFolderId, folder.ProjectFolderId,
                destinationFolder.ProjectFolderId);

            await handler.Handle(evt, CancellationToken.None);
            var destinationFolderWork =
                await _projectFolderWorkInMemoryRepository.GetByProjectAndFolderId(folderRoot.ProjectId.Value,
                    evt.Copy.Value);
            Assert.Equal(2, destinationFolderWork.WorkItems.Count);
            Assert.Equal(folderWork.WorkItems[0].Amount.Value, destinationFolderWork.WorkItems[0].Amount.Value);
            Assert.Equal(folderWork.WorkItems[1].Text.Value, destinationFolderWork.WorkItems[1].Text.Value);
            Assert.NotEqual(folderWork.WorkItems[0].Date.Value, destinationFolderWork.WorkItems[0].Date.Value);
            Assert.NotEqual(folderWork.WorkItems[0].User.UserId, destinationFolderWork.WorkItems[0].User.UserId);
        }

        [Fact]
        public void WhenCopyingToSubFolder_RecursiveCopyingShouldStop()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            var folder2 = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            folder.AddSubFolder(folderRoot.ProjectId, folder2);

            folder.CopyFolder(Any.ProjectId(), folder2, new FakeExecutionContext(), new SystemTime());
            Assert.Equal(folder.Name, folder2.SubFolders[0].Name);
            Assert.Equal(folder2.Name, folder2.SubFolders[0].SubFolders[0].Name);

            folder.CopyFolder(Any.ProjectId(), folder2, new FakeExecutionContext(), new SystemTime());
        }

        [Fact]
        public void WhenCopyFolder_FolderSupplementsIsCopied()
        {
            var root = Any.ProjectFolderRoot();
            var folderA = Any.ProjectFolder();
            var folderASupplemnts = Any.FolderSupplements();

            root.AddFolder(folderA);
            folderA.UpdateFolderSupplements(root.ProjectId, folderASupplemnts);

            Assert.NotEmpty(folderA.FolderSupplements);
            Assert.Equal(2, folderA.FolderSupplements.Count);
            Assert.Empty(root.RootFolder.FolderSupplements);

            folderA.CopyFolder(root.ProjectId, root.RootFolder, new FakeExecutionContext(), new SystemTime());
            var folderB = root.RootFolder.SubFolders[1];

            Assert.NotEmpty(folderA.FolderSupplements);
            Assert.Equal(2, folderA.FolderSupplements.Count);
            Assert.Empty(root.RootFolder.FolderSupplements);
            Assert.NotEmpty(folderB.FolderSupplements);
            Assert.NotEqual(folderA.FolderSupplements[0].SupplementId.Value, folderB.FolderSupplements[0].SupplementId.Value);
            Assert.Equal(folderA.FolderSupplements[0].SupplementNumber.Value, folderB.FolderSupplements[0].SupplementNumber.Value);
            Assert.Equal(folderA.FolderSupplements[0].SupplementPercentage.Value, folderB.FolderSupplements[0].SupplementPercentage.Value);
            Assert.Equal(folderA.FolderSupplements[0].CatalogSupplementId.Value, folderB.FolderSupplements[0].CatalogSupplementId.Value);
            Assert.Equal(folderA.FolderSupplements[0].SupplementText.Value, folderB.FolderSupplements[0].SupplementText.Value);
            Assert.NotEqual(folderA.FolderSupplements[1].SupplementId.Value, folderB.FolderSupplements[1].SupplementId.Value);
            Assert.Equal(folderA.FolderSupplements[1].SupplementNumber.Value, folderB.FolderSupplements[1].SupplementNumber.Value);
            Assert.Equal(folderA.FolderSupplements[1].SupplementPercentage.Value, folderB.FolderSupplements[1].SupplementPercentage.Value);
            Assert.Equal(folderA.FolderSupplements[1].CatalogSupplementId.Value, folderB.FolderSupplements[1].CatalogSupplementId.Value);
            Assert.Equal(folderA.FolderSupplements[1].SupplementText.Value, folderB.FolderSupplements[1].SupplementText.Value);
        }

        [Fact]
        public void WhenCopyFolderWithSubFolders_FolderSupplementsIsCopied()
        {
            var root = Any.ProjectFolderRoot();
            var folderA = Any.ProjectFolder();
            var folderAA = Any.ProjectFolder();
            var folderAB = Any.ProjectFolder();
            var folderAASupplements = Any.FolderSupplements();

            root.AddFolder(folderA);
            folderA.AddSubFolder(root.ProjectId, folderAA);
            folderA.AddSubFolder(root.ProjectId, folderAB);
            folderAA.UpdateFolderSupplements(root.ProjectId, folderAASupplements);

            Assert.Empty(root.RootFolder.FolderSupplements);
            Assert.Empty(folderA.FolderSupplements);
            Assert.NotEmpty(folderAA.FolderSupplements);
            Assert.Equal(2, folderAA.FolderSupplements.Count);
            Assert.Empty(folderAB.FolderSupplements);

            folderA.CopyFolder(root.ProjectId, root.RootFolder, new FakeExecutionContext(), new SystemTime());
            var folderB = root.RootFolder.SubFolders[1];
            var folderBA = folderB.SubFolders[0];
            var folderBB = folderB.SubFolders[1];

            Assert.Empty(root.RootFolder.FolderSupplements);
            Assert.Empty(folderB.FolderSupplements);
            Assert.NotEmpty(folderBA.FolderSupplements);
            Assert.Equal(2, folderBA.FolderSupplements.Count);
            Assert.Empty(folderBB.FolderSupplements);

            Assert.NotEqual(folderAA.FolderSupplements[0].SupplementId.Value, folderBA.FolderSupplements[0].SupplementId.Value);
            Assert.Equal(folderAA.FolderSupplements[0].SupplementNumber.Value, folderBA.FolderSupplements[0].SupplementNumber.Value);
            Assert.Equal(folderAA.FolderSupplements[0].SupplementPercentage.Value, folderBA.FolderSupplements[0].SupplementPercentage.Value);
            Assert.Equal(folderAA.FolderSupplements[0].CatalogSupplementId.Value, folderBA.FolderSupplements[0].CatalogSupplementId.Value);
            Assert.Equal(folderAA.FolderSupplements[0].SupplementText.Value, folderBA.FolderSupplements[0].SupplementText.Value);
            Assert.NotEqual(folderAA.FolderSupplements[1].SupplementId.Value, folderBA.FolderSupplements[1].SupplementId.Value);
            Assert.Equal(folderAA.FolderSupplements[1].SupplementNumber.Value, folderBA.FolderSupplements[1].SupplementNumber.Value);
            Assert.Equal(folderAA.FolderSupplements[1].SupplementPercentage.Value, folderBA.FolderSupplements[1].SupplementPercentage.Value);
            Assert.Equal(folderAA.FolderSupplements[1].CatalogSupplementId.Value, folderBA.FolderSupplements[1].CatalogSupplementId.Value);
            Assert.Equal(folderAA.FolderSupplements[1].SupplementText.Value, folderBA.FolderSupplements[1].SupplementText.Value);
        }

        [Fact]
        public void GivenSubFolder_WhenParentIsSetToExtraWork_SubFolderIsExtraWork()
        {
            // root
            //  |- A
            //     |- A1
            //     |- A2
            //  |- B
            //     |- B1
            var rootFolder = Any.ProjectFolder();
            var folderA = Any.ProjectFolder();
            var folderA1 = Any.ProjectFolder();
            var folderA2 = Any.ProjectFolder();
            var folderB = Any.ProjectFolder();
            var folderB1 = Any.ProjectFolder();

            rootFolder.AddSubFolder(Any.ProjectId(), folderA);
            folderA.AddSubFolder(Any.ProjectId(), folderA1);
            folderA.AddSubFolder(Any.ProjectId(), folderA2);
            rootFolder.AddSubFolder(Any.ProjectId(), folderB);
            folderB.AddSubFolder(Any.ProjectId(), folderB1);

            folderA.MarkAsExtraWork();

            Assert.True(folderA.IsExtraWork());
            Assert.True(folderA1.IsExtraWork());
            Assert.True(folderA2.IsExtraWork());
            Assert.False(folderB.IsExtraWork());
            Assert.False(folderB1.IsExtraWork());
            Assert.False(rootFolder.IsExtraWork());
        }

        [Fact]
        public void GivenExtraWorkFolder_WhenRemovingExtraWorkFlag_folderIsNotExtraWork()
        {
            var folder = Any.ProjectFolder();

            folder.MarkAsExtraWork();
            Assert.True(folder.IsExtraWork());

            folder.MarkAsNormalWork();
            Assert.False(folder.IsExtraWork());
        }

        [Fact]
        public void GivenNoChangesInSupplements_FolderSupplementsShouldNotUpdate()
        {
            var folder = Any.ProjectFolder();
            var supplements = Any.FolderSupplements();
            var res = folder.UpdateFolderSupplements(Any.ProjectId(), supplements);
            Assert.NotEmpty(res); // Changes because no supplements was here before
            Assert.Equal(2, folder.FolderSupplements.Count);
            Assert.Equal(supplements[0].SupplementNumber.Value, folder.FolderSupplements[0].SupplementNumber.Value);
            Assert.Equal(supplements[1].SupplementNumber.Value, folder.FolderSupplements[1].SupplementNumber.Value);

            supplements = Any.FolderSupplements();
            res = folder.UpdateFolderSupplements(Any.ProjectId(), supplements);
            Assert.NotEmpty(res); // No changes made
            Assert.Equal(2, folder.FolderSupplements.Count);
            Assert.Equal(supplements[0].SupplementNumber.Value, folder.FolderSupplements[0].SupplementNumber.Value);
            Assert.Equal(supplements[1].SupplementNumber.Value, folder.FolderSupplements[1].SupplementNumber.Value);
        }

        [Fact]
        public void GivenChangesInSupplementsEmpty_FolderSupplementsShouldNotUpdate()
        {
            var folder = Any.ProjectFolder();
            var supplements = new List<FolderSupplement>();
            var res = folder.UpdateFolderSupplements(Any.ProjectId(), supplements);
            Assert.NotEmpty(res); // No changes made
            Assert.Equal(0, folder.FolderSupplements.Count);
        }

        [Fact]
        public void GivenChangesInSupplements_FolderSupplementsShouldUpdate()
        {
            var folder = Any.ProjectFolder();
            var supplements = Any.FolderSupplements();
            var res = folder.UpdateFolderSupplements(Any.ProjectId(), supplements);
            Assert.NotEmpty(res); // Changes because no supplements was here before
            Assert.Equal(2, folder.FolderSupplements.Count);
            Assert.Equal(supplements[0].SupplementNumber.Value, folder.FolderSupplements[0].SupplementNumber.Value);
            Assert.Equal(supplements[1].SupplementNumber.Value, folder.FolderSupplements[1].SupplementNumber.Value);

            supplements = Any.FolderSupplements();
            supplements[1] = FolderSupplement.Create(SupplementId.Create(Any.Guid()), CatalogSupplementId.Create(Any.Guid()),
                SupplementNumber.Create("X42"), SupplementText.Create("New unknown"), SupplementPercentage.Create(42));
            res = folder.UpdateFolderSupplements(Any.ProjectId(), supplements);
            Assert.NotEmpty(res); // have changes
            Assert.Equal(2, folder.FolderSupplements.Count);
            Assert.Equal(supplements[0].SupplementNumber.Value, folder.FolderSupplements[0].SupplementNumber.Value);
            Assert.Equal(supplements[1].SupplementNumber.Value, folder.FolderSupplements[1].SupplementNumber.Value);
        }

        [Fact]
        public void GivenChangesInSupplementsEmpty_FolderSupplementsShouldUpdate()
        {
            var folder = Any.ProjectFolder();
            var supplements = Any.FolderSupplements();
            var res = folder.UpdateFolderSupplements(Any.ProjectId(), supplements);
            // Changes because no supplements was here before
            Assert.Equal(2, folder.FolderSupplements.Count);
            Assert.Equal(supplements[0].SupplementNumber.Value, folder.FolderSupplements[0].SupplementNumber.Value);
            Assert.Equal(supplements[1].SupplementNumber.Value, folder.FolderSupplements[1].SupplementNumber.Value);

            supplements = new List<FolderSupplement>();
            res = folder.UpdateFolderSupplements(Any.ProjectId(), supplements);
            Assert.NotEmpty(res); // have changes
            Assert.Equal(0, folder.FolderSupplements.Count);
        }

        [Fact]
        public void GivenFolderWithSupplements_SubFolderShouldInheritSupplements()
        {
            var supplements = new List<FolderSupplement>
            {
                FolderSupplement.Create(SupplementId.Create(Any.Guid()), CatalogSupplementId.Create(Any.Guid()), SupplementNumber.Create("L42"),
                    SupplementText.Create("For lavt"), SupplementPercentage.Create(400)),
                FolderSupplement.Create(SupplementId.Create(Any.Guid()), CatalogSupplementId.Create(Any.Guid()), SupplementNumber.Create("H42"),
                    SupplementText.Create("Højde"), SupplementPercentage.Create(10)),
            };

            var folder = Any.ProjectFolder();
            folder.UpdateFolderSupplements(Any.ProjectId(), supplements);

            var folder2 = Any.ProjectFolder();
            folder.AddSubFolder(Any.ProjectId(), folder2);

            var folder2Work = ProjectFolderWork.Create(ProjectFolderWorkId.Create(Any.Guid()), Any.ProjectId(), folder2.ProjectFolderId);
            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), WorkItemDate.Empty(), WorkItemUser.Empty(),
                WorkItemText.Empty(), Any.WorkItemEanNumber(), WorkItemMountingCode.FromCode(11), WorkItemDuration.Create(10000),
                WorkItemAmount.Create(10), WorkItemUnit.Meter, new List<SupplementOperation>(),
                new List<Supplement>
                {
                    Supplement.Create(SupplementId.Empty(), CatalogSupplementId.Create(Any.Guid()), SupplementNumber.Empty(), SupplementText.Empty(),
                        SupplementPercentage.Create(100)),
                    Supplement.Create(SupplementId.Empty(), CatalogSupplementId.Create(Any.Guid()), SupplementNumber.Empty(), SupplementText.Empty(),
                        SupplementPercentage.Create(10))
                });
            folder2Work.AddWorkItem(workItem, new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder2));

            Assert.Equal(65.03m, workItem.TotalPayment.Value, 2);
        }

        [Fact]
        public void GivenSubFolderWithSupplementsAndParentWithNoSupplements_SubFolderShouldHaveSupplements()
        {
            var folder = Any.ProjectFolder();

            var folder2 = Any.ProjectFolder();
            folder.AddSubFolder(Any.ProjectId(), folder2);
            folder2.UpdateFolderSupplements(Any.ProjectId(), new List<FolderSupplement>
            {
                FolderSupplement.Create(SupplementId.Create(Any.Guid()), CatalogSupplementId.Create(Any.Guid()), SupplementNumber.Create("L42"),
                    SupplementText.Create("For lavt"), SupplementPercentage.Create(400))
            });

            var folder3 = Any.ProjectFolder();
            folder.AddSubFolder(Any.ProjectId(), folder3);
            folder3.UpdateFolderSupplements(Any.ProjectId(), new List<FolderSupplement>
            {
                FolderSupplement.Create(SupplementId.Create(Any.Guid()), CatalogSupplementId.Create(Any.Guid()), SupplementNumber.Create("H42"),
                    SupplementText.Create("Højde"), SupplementPercentage.Create(10))
            });

            var folder2Work = ProjectFolderWork.Create(ProjectFolderWorkId.Create(Any.Guid()), Any.ProjectId(), folder2.ProjectFolderId);
            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), WorkItemDate.Empty(), WorkItemUser.Empty(),
                WorkItemText.Empty(), Any.WorkItemEanNumber(), WorkItemMountingCode.FromCode(11), WorkItemDuration.Create(10000),
                WorkItemAmount.Create(10), WorkItemUnit.Meter, new List<SupplementOperation>(),
                new List<Supplement>
                {
                    Supplement.Create(SupplementId.Empty(), CatalogSupplementId.Create(Any.Guid()), SupplementNumber.Empty(), SupplementText.Empty(),
                        SupplementPercentage.Create(100)),
                    Supplement.Create(SupplementId.Empty(), CatalogSupplementId.Create(Any.Guid()), SupplementNumber.Empty(), SupplementText.Empty(),
                        SupplementPercentage.Create(10))
                });
            folder2Work.AddWorkItem(workItem, new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder2));

            Assert.Equal(63.99m, workItem.TotalPayment.Value, 2);
        }

        [Fact]
        public void GivenFolderWithIdenticalSupplements_SubFolderShouldHaveUniqueSupplementsOnly()
        {
            var folderSupplement = FolderSupplement.Create(SupplementId.Create(Any.Guid()), CatalogSupplementId.Create(Any.Guid()),
                SupplementNumber.Create("L42"), SupplementText.Create("For lavt"), SupplementPercentage.Create(400));

            var folder = Any.ProjectFolder();
            folder.UpdateFolderSupplements(Any.ProjectId(), new List<FolderSupplement> { folderSupplement });

            var folder2 = Any.ProjectFolder();
            folder.AddSubFolder(Any.ProjectId(), folder2);
            folder2.UpdateFolderSupplements(Any.ProjectId(), new List<FolderSupplement> { folderSupplement });

            var folder2Work = ProjectFolderWork.Create(ProjectFolderWorkId.Create(Any.Guid()), Any.ProjectId(), folder2.ProjectFolderId);
            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), WorkItemDate.Empty(), WorkItemUser.Empty(),
                WorkItemText.Empty(), Any.WorkItemEanNumber(), WorkItemMountingCode.FromCode(11), WorkItemDuration.Create(10000),
                WorkItemAmount.Create(10), WorkItemUnit.Meter, new List<SupplementOperation>(),
                new List<Supplement>
                {
                    Supplement.Create(SupplementId.Empty(), CatalogSupplementId.Create(Any.Guid()), SupplementNumber.Empty(), SupplementText.Empty(),
                        SupplementPercentage.Create(100)),
                    Supplement.Create(SupplementId.Empty(), CatalogSupplementId.Create(Any.Guid()), SupplementNumber.Empty(), SupplementText.Empty(),
                        SupplementPercentage.Create(10))
                });
            folder2Work.AddWorkItem(workItem, new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder2));

            Assert.Equal(63.99m, workItem.TotalPayment.Value, 2);
        }

        [Fact]
        public void WhenMovingWorkItemsFromFolderToAnotherFolder_WorkItemShouldHaveNewFoldersSupplements()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();

            // Create folder 1 and add supplements to it
            var folder1Supplements = new List<FolderSupplement> { Any.FolderSupplementWithPercentage(10), Any.FolderSupplementWithPercentage(20) };

            var folder1 = Any.ProjectFolder();
            folder1.UpdateFolderSupplements(projectFolderRoot.ProjectId, folder1Supplements);
            projectFolderRoot.RootFolder.AddSubFolder(Any.ProjectId(), folder1);

            var folder1Work = ProjectFolderWork.Create(ProjectFolderWorkId.Create(Any.Guid()), projectFolderRoot.ProjectId, folder1.ProjectFolderId);
            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), WorkItemDate.Empty(), WorkItemUser.Empty(),
                WorkItemText.Empty(), Any.WorkItemEanNumber(), WorkItemMountingCode.FromCode(11), WorkItemDuration.Create(10000),
                WorkItemAmount.Create(10), WorkItemUnit.Meter, new List<SupplementOperation>(),
                new List<Supplement>());

            folder1Work.AddWorkItem(workItem, new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder1));
            Assert.Equal(13.64m, workItem.TotalPayment.Value, 2);

            // Create folder 2 and add folder supplements to it
            var folder2Supplements = new List<FolderSupplement> { Any.FolderSupplementWithPercentage(42), Any.FolderSupplementWithPercentage(142) };

            var folder2 = Any.ProjectFolder();
            folder2.UpdateFolderSupplements(projectFolderRoot.ProjectId, folder1Supplements);
            var folder2Work = ProjectFolderWork.Create(ProjectFolderWorkId.Create(Any.Guid()), projectFolderRoot.ProjectId, folder2.ProjectFolderId);
            projectFolderRoot.RootFolder.AddSubFolder(projectFolderRoot.ProjectId, folder2);
            folder2.UpdateFolderSupplements(projectFolderRoot.ProjectId, folder2Supplements);

            // Move work item from folder 1 to folder 2 and assert supplements are calculated correctly
            folder1Work.MoveWorkItems(folder2Work, new List<WorkItemId> { workItem.WorkItemId },
                new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder2));

            Assert.Equal(29.79m, workItem.TotalPayment.Value, 2);
        }

        [Fact]
        public void WhenCopyingWorkItemsFromFolderToFolder_WorkItemShouldHaveNewSupplements()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();

            // Create folder 1
            var folder1 = ProjectFolder.Create(Any.ProjectFolderId(), Any.ProjectFolderName(), Any.ProjectFolderDescription(),
                Any.ProjectFolderCreatedBy(), FolderRateAndSupplement.Empty(), ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Empty());
            projectFolderRoot.RootFolder.AddSubFolder(projectFolderRoot.ProjectId, folder1);

            // Create supplements for folder 2
            var folder2Supplements = new List<FolderSupplement> { Any.FolderSupplementWithPercentage(10), Any.FolderSupplementWithPercentage(20) };

            // Create folder 2
            var folder2 = ProjectFolder.Create(Any.ProjectFolderId(), Any.ProjectFolderName(), Any.ProjectFolderDescription(),
                Any.ProjectFolderCreatedBy(), FolderRateAndSupplement.Empty(), ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Empty());
            folder1.AddSubFolder(Any.ProjectId(), folder2);

            var folder2Work = ProjectFolderWork.Create(ProjectFolderWorkId.Create(Any.Guid()), projectFolderRoot.ProjectId,
                folder2.ProjectFolderId);
            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), WorkItemDate.Empty(), WorkItemUser.Empty(),
                WorkItemText.Empty(), Any.WorkItemEanNumber(), WorkItemMountingCode.FromCode(11), WorkItemDuration.Create(10000),
                WorkItemAmount.Create(10), WorkItemUnit.Meter, new List<SupplementOperation>(),
                new List<Supplement>());

            folder2.UpdateFolderSupplements(projectFolderRoot.ProjectId, folder2Supplements);
            folder2Work.AddWorkItem(workItem, new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder2));
            Assert.Equal(13.64m, workItem.TotalPayment.Value, 2);

            // Create folder 3 and add folder supplements to it
            var supplements = new List<FolderSupplement> { Any.FolderSupplementWithPercentage(42), Any.FolderSupplementWithPercentage(1420) };

            var folder3 = Any.ProjectFolder();
            var folder3Work = ProjectFolderWork.Create(ProjectFolderWorkId.Create(Any.Guid()), projectFolderRoot.ProjectId,
                folder3.ProjectFolderId);
            projectFolderRoot.RootFolder.AddSubFolder(projectFolderRoot.ProjectId, folder3);
            folder3.UpdateFolderSupplements(projectFolderRoot.ProjectId, supplements);

            // Copy work item from folder 2 to folder 3 and assert supplements are calculated correctly
            folder2Work.CopyWorkItems(folder3Work, new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder3),
                new List<WorkItemId> { workItem.WorkItemId }, new FakeExecutionContext());

            Assert.Equal(163.84m, folder3Work.WorkItems[0].TotalPayment.Value, 2);
        }

        [Fact]
        public void WhenUpdatingWorkItemsInFolderWithSupplements_ShouldCalculateCorrectly()
        {
            // Create folder root and and folder
            var projectFolderRoot = Any.ProjectFolderRoot();

            var folder = Any.ProjectFolder();
            projectFolderRoot.RootFolder.AddSubFolder(projectFolderRoot.ProjectId, folder);

            // Add supplements to folder
            var supplements = new List<FolderSupplement> { Any.FolderSupplementWithPercentage(142), Any.FolderSupplementWithPercentage(42) };
            folder.UpdateFolderSupplements(projectFolderRoot.ProjectId, supplements);

            var folderWork = ProjectFolderWork.Create(ProjectFolderWorkId.Create(Any.Guid()), projectFolderRoot.ProjectId, folder.ProjectFolderId);
            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), WorkItemDate.Empty(), WorkItemUser.Empty(),
                WorkItemText.Empty(), Any.WorkItemEanNumber(), WorkItemMountingCode.FromCode(11), WorkItemDuration.Create(10000),
                WorkItemAmount.Create(10), WorkItemUnit.Meter, new List<SupplementOperation>(),
                new List<Supplement>());

            folderWork.AddWorkItem(workItem, new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder));
            Assert.Equal(29.79m, workItem.TotalPayment.Value, 2);

            // Update work item and calculate supplements
            var workItemAmount = 23;
            folderWork.UpdateWorkItem(workItem.WorkItemId, WorkItemAmount.Create(workItemAmount), new BaseRateAndSupplementProxy(
                GetDefaultBaseRateAndSupplement(), folder));
            Assert.Equal(68.52m, workItem.TotalPayment.Value, 2);
        }
    }
}
