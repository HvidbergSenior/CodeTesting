using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Serialization;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Domain.projectFolderRoot
{
    public class ProjectFolderRootTests
    {
        private readonly ITestOutputHelper output;

        public ProjectFolderRootTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void CanSerializeAndDeserialize()
        {
            var serializer = new JsonSerializer<ProjectFolderRoot>();

            var folderRoot = Any.ProjectFolderRoot();
            var subFolder = Any.ProjectFolder();
            folderRoot.AddFolder(subFolder);
            folderRoot.AddFolder(Any.ProjectFolder(), subFolder.ProjectFolderId);

            var json = serializer.Serialize(folderRoot);
            var result = serializer.Deserialize(json);

            Assert.Equal(folderRoot.ProjectFolderRootId.Id, result?.ProjectFolderRootId.Id);
            Assert.Single(folderRoot.RootFolder.SubFolders);
            Assert.Equal(folderRoot.RootFolder, folderRoot.RootFolder.SubFolders[0].ParentFolder);
            Assert.Equal(folderRoot.RootFolder.SubFolders[0], folderRoot.RootFolder.SubFolders[0].SubFolders[0].ParentFolder);
        }

        [Fact]
        public void When_Created_Should_Have_ProjectFolderRootId()
        {
            var projectId = Any.ProjectId();
            var projectName = Any.ProjectName();
            var projectFolderRootId = Any.ProjectFolderRootId();
            var projectFolderRoot = ProjectFolderRoot.Create(projectId, projectName, projectFolderRootId, GetDefaultFolderRateAndSupplement());

            Assert.Equal(projectFolderRootId, projectFolderRoot.ProjectFolderRootId);
            Assert.Equal(0, projectFolderRoot.RootFolder.SubFolders.Count);
        }

        [Fact]
        public void When_FolderAdded_ShouldContainFolder()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            var projectFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(projectFolder);

            Assert.Contains(projectFolder, projectFolderRoot.RootFolder.SubFolders);
        }

        [Fact]
        public void When_SubFolderAdded_ShouldContainHierarchy()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            var parentFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(parentFolder);

            var subFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(subFolder, parentFolder.ProjectFolderId);

            Assert.DoesNotContain(subFolder, projectFolderRoot.RootFolder.SubFolders);
            Assert.Contains(subFolder, parentFolder.SubFolders);
            Assert.NotNull(projectFolderRoot.GetFolder(subFolder.ProjectFolderId));
        }

        [Fact]
        public void When_FolderAdded_Should_Raise_ProjectFolderAddedEvent()
        {
            var folder = Any.ProjectFolder();
            var folderRoot = Any.ProjectFolderRoot();
            folderRoot.AddFolder(folder);
            Assert.IsType<ProjectFolderAddedDomainEvent>(folderRoot.PublishedEvent<ProjectFolderAddedDomainEvent>());
        }

        [Fact]
        public void When_GetSubFoldersOnFolder_ShouldContainAllFoldersInHierarchyBelow()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            var projectId = projectFolderRoot.ProjectId;
            var rootFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(rootFolder);

            var subFolderA = Any.ProjectFolder();
            projectFolderRoot.AddFolder(subFolderA);

            var subFolderB = Any.ProjectFolder();
            projectFolderRoot.AddFolder(subFolderB);

            var subFolderB1 = Any.ProjectFolder();
            projectFolderRoot.AddFolder(subFolderB1, subFolderB);

            var subFolderB2 = Any.ProjectFolder();
            projectFolderRoot.AddFolder(subFolderB2, subFolderB);

            var subFolderB2X = Any.ProjectFolder();
            projectFolderRoot.AddFolder(subFolderB2X, subFolderB2);

            var folderIds = projectFolderRoot.GetFolderAndSubfolders(subFolderB.ProjectFolderId);

            folderIds.Should().HaveCount(4);
            folderIds.Should().Contain(subFolderB)
                .And.Contain(subFolderB1)
                .And.Contain(subFolderB2)
                .And.Contain(subFolderB2X);
        }

        [Fact]
        public void When_RootFolderRemoved_ShouldThrow()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);

            // Removing root folder should not be allowed
            Assert.Throws<ProjectFolderNotFoundException>(() => projectFolderRoot.RemoveFolder(projectFolderRoot.RootFolder.ProjectFolderId));
        }

        [Fact]
        public void When_FolderRemoved_FolderIsNotReturned()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);

            Assert.Contains(folder, projectFolderRoot.RootFolder.SubFolders);
            projectFolderRoot.RemoveFolder(folder.ProjectFolderId);

            Assert.DoesNotContain(folder, projectFolderRoot.RootFolder.SubFolders);
        }

        [Fact]
        public void When_UnknownFolderDeleted_ShouldThrow()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            Assert.Throws<ProjectFolderNotFoundException>(() => projectFolderRoot.RemoveFolder(Any.ProjectFolderId()));
        }

        [Fact]
        public void When_FolderRemoved_Should_Raise_ProjectFolderRemovedEvent()
        {
            var folder = Any.ProjectFolder();
            var folderRoot = Any.ProjectFolderRoot();
            folderRoot.AddFolder(folder);
            folderRoot.RemoveFolder(folder.ProjectFolderId);
            Assert.IsType<ProjectFolderRemovedDomainEvent>(folderRoot.PublishedEvent<ProjectFolderRemovedDomainEvent>());
        }

        [Fact]
        public void When_UnknownFolderMoved_ShouldThrow()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            Assert.Throws<ProjectFolderNotFoundException>(() => projectFolderRoot.MoveFolder(Any.ProjectFolderId(), ProjectFolderRoot.RootFolderId));
        }

        [Fact]
        public void When_FolderMovedToUnknownFolder_ShouldThrow()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            var projectFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(projectFolder);
            Assert.Throws<ProjectFolderNotFoundException>(() => projectFolderRoot.MoveFolder(projectFolder.ProjectFolderId, Any.ProjectFolderId()));
        }

        [Fact]
        public void When_MovingFolders_CyclesAreDetected()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            var projectFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(projectFolder);
            projectFolderRoot.PrettyPrint(output);

            Assert.Throws<InvalidOperationException>(() =>
                projectFolderRoot.MoveFolder(projectFolder.ProjectFolderId, projectFolder.ProjectFolderId));
        }

        [Fact]
        public void When_MovingFolderToRoot_FolderIsReturned()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            var subFolder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            folderRoot.AddFolder(subFolder, folder.ProjectFolderId);
            folderRoot.PrettyPrint(output);

            folderRoot.MoveFolder(subFolder.ProjectFolderId, ProjectFolderRoot.RootFolderId);
            folderRoot.PrettyPrint(output);

            Assert.Contains(subFolder, folderRoot.RootFolder.SubFolders);
        }

        [Fact]
        public void When_FolderMoved_Should_Raise_ProjectFolderMovedEvent()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            var subFolder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            folderRoot.AddFolder(subFolder, folder.ProjectFolderId);
            folderRoot.MoveFolder(subFolder.ProjectFolderId, ProjectFolderRoot.RootFolderId);

            Assert.IsType<ProjectFolderMovedDomainEvent>(folderRoot.PublishedEvent<ProjectFolderMovedDomainEvent>());
        }

        [Fact]
        public void When_MovingFolderToSameFolder_NothingIsChanged()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            var subFolder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            folderRoot.AddFolder(subFolder, folder.ProjectFolderId);
            folderRoot.PrettyPrint(output);

            folderRoot.MoveFolder(subFolder.ProjectFolderId, folder.ProjectFolderId);
            folderRoot.PrettyPrint(output);

            Assert.Null(folderRoot.PublishedEvent<ProjectFolderMovedDomainEvent>());
            Assert.Contains(subFolder, folder.SubFolders);
        }

        [Fact]
        public void When_MovingFolderToAnotherFolder_FolderIsReturned()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var fromFolder = Any.ProjectFolder();
            var toFolder = Any.ProjectFolder();
            var subFolder = Any.ProjectFolder();
            folderRoot.AddFolder(fromFolder);
            folderRoot.AddFolder(toFolder);
            folderRoot.AddFolder(subFolder, fromFolder.ProjectFolderId);
            folderRoot.PrettyPrint(output);

            folderRoot.MoveFolder(subFolder.ProjectFolderId, toFolder.ProjectFolderId);
            folderRoot.PrettyPrint(output);

            Assert.DoesNotContain(subFolder, fromFolder.SubFolders);
            Assert.Contains(subFolder, toFolder.SubFolders);
        }

        [Fact]
        public void When_MovingFolder_SubItemsAreIncluded()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var sourceFolder = Any.ProjectFolder();
            var subFolder = Any.ProjectFolder();
            var destinationFolder = Any.ProjectFolder();
            folderRoot.AddFolder(sourceFolder);
            folderRoot.AddFolder(subFolder, sourceFolder.ProjectFolderId);
            folderRoot.AddFolder(destinationFolder);

            folderRoot.PrettyPrint(output);
            folderRoot.MoveFolder(sourceFolder.ProjectFolderId, destinationFolder.ProjectFolderId);
            folderRoot.PrettyPrint(output);

            Assert.DoesNotContain(sourceFolder, folderRoot.RootFolder.SubFolders);
            Assert.Contains(sourceFolder, destinationFolder.SubFolders);
            Assert.Contains(subFolder, destinationFolder.SubFolders[0].SubFolders);
        }

        [Fact]
        public void When_DocumentAddedToInvalidFolder_ExceptionIsThrown()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            var docId = Any.ProjectDocumentId();
            var docName = Any.ProjectDocumentName();
            var uploadedTimestamp = Any.ProjectDocumentUploadedTimestamp();

            Assert.Throws<ProjectFolderNotFoundException>(() => folderRoot.AddDocument(Any.ProjectFolderId(), docId, docName, uploadedTimestamp));
        }

        [Fact]
        public void When_DocumentAddedToFolder_FolderShouldContainDocument()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);

            var docId = Any.ProjectDocumentId();
            var docName = Any.ProjectDocumentName();
            var uploadedTimestamp = Any.ProjectDocumentUploadedTimestamp();
            folderRoot.AddDocument(folder.ProjectFolderId, docId, docName, uploadedTimestamp);

            Assert.Single(folder.Documents);
            Assert.Equal(docId, folder.Documents[0].ProjectDocumentId);
            Assert.Equal(docName, folder.Documents[0].ProjectDocumentName);
            Assert.Equal(uploadedTimestamp, folder.Documents[0].UploadedTimestamp);
        }

        [Fact]
        public void When_DocumentRemovedFromFolder_FolderShouldNotContainDocument()
        {
            // GIVEN
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            var projectDocument = Any.ProjectDocument(IFileReference.Empty());
            folderRoot.AddFolder(folder);
            folder.AddDocumentReference(projectDocument.ProjectDocumentId, projectDocument.ProjectDocumentName, projectDocument.UploadedTimestamp);

            // WHEN
            folderRoot.RemoveDocument(folder.ProjectFolderId, projectDocument.ProjectDocumentId);

            // THEN
            Assert.Empty(folder.Documents);
        }

        [Fact]
        public void When_DocumentRemovedFromWrongFolder_ExceptionIsThrown()
        {
            // GIVEN
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            var projectDocument = Any.ProjectDocument(IFileReference.Empty());
            folderRoot.AddFolder(folder);
            folder.AddDocumentReference(projectDocument.ProjectDocumentId, projectDocument.ProjectDocumentName, projectDocument.UploadedTimestamp);
            var nonExistingFolderId = Any.ProjectFolder().ProjectFolderId;
            var nonExistingDocumentId = Any.ProjectDocument(IFileReference.Empty()).ProjectDocumentId;

            // WHEN
            // THEN
            Assert.Throws<ProjectFolderNotFoundException>(
                () => folderRoot.RemoveDocument(nonExistingFolderId, projectDocument.ProjectDocumentId));
            Assert.Throws<ProjectDocumentNotFoundException>(
                () => folderRoot.RemoveDocument(folder.ProjectFolderId, nonExistingDocumentId));
        }

        [Fact]
        public void When_DocumentRemoved_Should_Raise_ProjectDocumentRemovedEvent()
        {
            // GIVEN
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            var projectDocument = Any.ProjectDocument(IFileReference.Empty());
            folderRoot.AddFolder(folder);
            folder.AddDocumentReference(projectDocument.ProjectDocumentId, projectDocument.ProjectDocumentName, projectDocument.UploadedTimestamp);

            // WHEN
            folderRoot.RemoveDocument(folder.ProjectFolderId, projectDocument.ProjectDocumentId);

            // THEN
            Assert.IsType<ProjectDocumentRemovedDomainEvent>(folderRoot.PublishedEvent<ProjectDocumentRemovedDomainEvent>());
        }

        [Fact]
        public void When_WorkItemsMoved_SourceFolderShouldNotContainWorkItem_AndDestinationFolderMustContainWorkItem()
        {
            // GIVEN
            var projectId = Any.ProjectId();
            var sourceFolderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectId, Any.ProjectFolderId());
            var destinationFolderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectId, Any.ProjectFolderId());
            var workItem1 = Any.WorkItem();
            var workItem2 = Any.WorkItem();
            sourceFolderWork.AddWorkItem(workItem1, GetDefaultBaseRateAndSupplementProxy());
            sourceFolderWork.AddWorkItem(workItem2, GetDefaultBaseRateAndSupplementProxy());

            var workItemIds = new[] { workItem1.WorkItemId, workItem2.WorkItemId }.ToList();

            // WHEN
            sourceFolderWork.MoveWorkItems(destinationFolderWork, workItemIds, GetDefaultBaseRateAndSupplementProxy());

            // THEN
            sourceFolderWork.WorkItems.Should().BeEmpty();
            destinationFolderWork.WorkItems.Should().HaveCount(2);
            destinationFolderWork.WorkItems.Select(wi => wi.WorkItemId).Should().Contain(workItem1.WorkItemId);
            destinationFolderWork.WorkItems.Select(wi => wi.WorkItemId).Should().Contain(workItem2.WorkItemId);
        }

        [Fact]
        public void When_WorkItemsDontBelongToSourceFolderAndMovingThem_ExceptionIsThrown()
        {
            // GIVEN
            var projectId = Any.ProjectId();
            var sourceFolderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectId, Any.ProjectFolderId());
            var destinationFolderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectId, Any.ProjectFolderId());

            var workItemIds = new[] { Any.WorkItemId(), Any.WorkItemId() }.ToList();

            // WHEN
            // THEN
            Assert.Throws<WorkItemNotFoundException>(
                () => sourceFolderWork.MoveWorkItems(destinationFolderWork, workItemIds, GetDefaultBaseRateAndSupplementProxy()));
        }

        [Fact]
        public void When_WorkItemsMovedToIllegalSourceFolder_ExceptionIsThrown()
        {
            // GIVEN
            var projectId = Any.ProjectId();
            var otherProjectId = Any.ProjectId();
            var sourceFolderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectId, Any.ProjectFolderId());
            var otherProjectFolderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), otherProjectId, Any.ProjectFolderId());
            var workItem1 = Any.WorkItem();
            var workItem2 = Any.WorkItem();
            sourceFolderWork.AddWorkItem(workItem1, GetDefaultBaseRateAndSupplementProxy());
            sourceFolderWork.AddWorkItem(workItem2, GetDefaultBaseRateAndSupplementProxy());

            var workItemIds = new[] { workItem1.WorkItemId, workItem2.WorkItemId }.ToList();

            // WHEN
            // THEN
            Assert.Throws<NotAllowedToMoveWorkItemToOtherProjectException>(
                () => sourceFolderWork.MoveWorkItems(otherProjectFolderWork, workItemIds, GetDefaultBaseRateAndSupplementProxy()));
        }

        [Fact]
        public void When_WorkItemsMoved_Should_Raise_ProjectWorkItemsMovedEvent()
        {
            // GIVEN
            var projectId = Any.ProjectId();
            var sourceFolderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectId, Any.ProjectFolderId());
            var destinationFolderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectId, Any.ProjectFolderId());
            var workItem1 = Any.WorkItem();
            var workItem2 = Any.WorkItem();
            sourceFolderWork.AddWorkItem(workItem1, GetDefaultBaseRateAndSupplementProxy());
            sourceFolderWork.AddWorkItem(workItem2, GetDefaultBaseRateAndSupplementProxy());

            var workItemIds = new[] { workItem1.WorkItemId, workItem2.WorkItemId }.ToList();

            // WHEN
            sourceFolderWork.MoveWorkItems(destinationFolderWork, workItemIds, GetDefaultBaseRateAndSupplementProxy());

            // THEN
            Assert.IsType<WorkItemMovedDomainEvent>(sourceFolderWork.PublishedEvent<WorkItemMovedDomainEvent>());
        }

        [Fact]
        public void GivenProjectFolderRoot_WhenOverwritingBaseRegulationAndSupplementsOnRoot_CorrectEffectiveValuesAreUsed()
        {
            // Create folders
            // Root (base rate regulation and supplements overwritten)
            //    |- folder
            //       |- subFolder
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            var subFolder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            folderRoot.AddFolder(subFolder, folder.ProjectFolderId);

            // Overwrite base rate regulation on root
            var defaultBaseRateAndSupplement = GetDefaultBaseRateAndSupplement();
            folderRoot.UpdateFolderBaseRateRegulation(ProjectFolderRoot.RootFolderId,
                FolderBaseRateRegulation.Create(66, FolderValueInheritStatus.Overwrite()),
                defaultBaseRateAndSupplement);
            folderRoot.UpdateFolderBaseSupplement(ProjectFolderRoot.RootFolderId,
                FolderIndirectTimeSupplement.Create(87, FolderValueInheritStatus.Overwrite()),
                FolderSiteSpecificTimeSupplement.Create(98, FolderValueInheritStatus.Overwrite()), defaultBaseRateAndSupplement);

            // Assert folder uses overwritten base rate regulation
            var folderRateAndSupplement = folder.FolderRateAndSupplement;
            Assert.False(folderRateAndSupplement.BaseRateRegulation.InheritStatus.IsOverwritten());
            Assert.False(folderRateAndSupplement.SiteSpecificTimeSupplement.InheritStatus.IsOverwritten());
            Assert.False(folderRateAndSupplement.IndirectTimeSupplement.InheritStatus.IsOverwritten());
            Assert.Equal(66, folderRateAndSupplement.BaseRateRegulation.Value);
            Assert.Equal(87, folderRateAndSupplement.IndirectTimeSupplement.Value);
            Assert.Equal(98, folderRateAndSupplement.SiteSpecificTimeSupplement.Value);

            // Assert subfolder uses overwritten base rate regulation
            var subFolderRateAndSupplement = subFolder.FolderRateAndSupplement;
            Assert.False(subFolderRateAndSupplement.BaseRateRegulation.InheritStatus.IsOverwritten());
            Assert.False(subFolderRateAndSupplement.SiteSpecificTimeSupplement.InheritStatus.IsOverwritten());
            Assert.False(subFolderRateAndSupplement.IndirectTimeSupplement.InheritStatus.IsOverwritten());
            Assert.Equal(66, subFolderRateAndSupplement.BaseRateRegulation.Value);
            Assert.Equal(87, subFolderRateAndSupplement.IndirectTimeSupplement.Value);
            Assert.Equal(98, subFolderRateAndSupplement.SiteSpecificTimeSupplement.Value);
        }

        [Fact]
        public void GivenProject_WhenUpdatingRates_SiblingFoldersDoesNotInheritFromEachOther()
        {
            // Create folders
            // Root (base rate regulation and supplements overwritten)
            //    |- folderA
            //    |- folderB
            var folderRoot = Any.ProjectFolderRoot();
            var folderA = Any.ProjectFolder();
            var folderB = Any.ProjectFolder();
            folderRoot.AddFolder(folderA);
            folderRoot.AddFolder(folderB);

            // Overwrite base rate regulation on root
            var defaultBaseRateAndSupplement = GetDefaultBaseRateAndSupplement();
            folderRoot.UpdateFolderBaseRateRegulation(folderA.ProjectFolderId,
                FolderBaseRateRegulation.Create(9, FolderValueInheritStatus.Overwrite()),
                defaultBaseRateAndSupplement);
            folderRoot.UpdateFolderBaseSupplement(folderA.ProjectFolderId,
                FolderIndirectTimeSupplement.Create(9, FolderValueInheritStatus.Overwrite()),
                FolderSiteSpecificTimeSupplement.Create(9, FolderValueInheritStatus.Overwrite()), defaultBaseRateAndSupplement);

            // Assert folderB does not inherit from folderA
            var folderRateAndSupplement = folderB.FolderRateAndSupplement;
            Assert.False(folderRateAndSupplement.BaseRateRegulation.InheritStatus.IsOverwritten());
            Assert.False(folderRateAndSupplement.SiteSpecificTimeSupplement.InheritStatus.IsOverwritten());
            Assert.False(folderRateAndSupplement.IndirectTimeSupplement.InheritStatus.IsOverwritten());
            Assert.Equal(0, folderRateAndSupplement.BaseRateRegulation.Value);
            Assert.Equal(64, folderRateAndSupplement.IndirectTimeSupplement.Value);
            Assert.Equal(2, folderRateAndSupplement.SiteSpecificTimeSupplement.Value);
        }
        
        [Fact]
        public void GivenProjectFolderRoot_WhenOverwritingBaseRegulationAndSupplementsOnMultipleLevels_CorrectEffectiveValuesAreUsed()
        {
            // Create folders
            // Root (base rate regulation and supplements overwritten)
            //    |- folder (base rate regulation and supplements overwritten)
            //       |- subFolder
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            var subFolder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            folderRoot.AddFolder(subFolder, folder.ProjectFolderId);

            // Overwrite base rate regulation on root
            var defaultBaseRateAndSupplement = GetDefaultBaseRateAndSupplement();
            folderRoot.UpdateFolderBaseRateRegulation(ProjectFolderRoot.RootFolderId,
                FolderBaseRateRegulation.Create(66, FolderValueInheritStatus.Overwrite()), defaultBaseRateAndSupplement);
            folderRoot.UpdateFolderBaseSupplement(ProjectFolderRoot.RootFolderId,
                FolderIndirectTimeSupplement.Create(87, FolderValueInheritStatus.Overwrite()),
                FolderSiteSpecificTimeSupplement.Create(98, FolderValueInheritStatus.Overwrite()), defaultBaseRateAndSupplement);
            folderRoot.UpdateFolderBaseRateRegulation(folder.ProjectFolderId,
                FolderBaseRateRegulation.Create(166, FolderValueInheritStatus.Overwrite()), defaultBaseRateAndSupplement);
            folderRoot.UpdateFolderBaseSupplement(folder.ProjectFolderId,
                FolderIndirectTimeSupplement.Create(187, FolderValueInheritStatus.Overwrite()),
                FolderSiteSpecificTimeSupplement.Create(198, FolderValueInheritStatus.Overwrite()), defaultBaseRateAndSupplement);

            // Assert folder uses overwritten base rate regulation
            var folderRateAndSupplement = folder.FolderRateAndSupplement;
            Assert.True(folderRateAndSupplement.BaseRateRegulation.InheritStatus.IsOverwritten());
            Assert.True(folderRateAndSupplement.SiteSpecificTimeSupplement.InheritStatus.IsOverwritten());
            Assert.True(folderRateAndSupplement.IndirectTimeSupplement.InheritStatus.IsOverwritten());
            Assert.Equal(166, folderRateAndSupplement.BaseRateRegulation.Value);
            Assert.Equal(198, folderRateAndSupplement.SiteSpecificTimeSupplement.Value);
            Assert.Equal(187, folderRateAndSupplement.IndirectTimeSupplement.Value);

            // Assert subfolder uses overwritten base rate regulation
            var subFolderRateAndSupplement = subFolder.FolderRateAndSupplement;
            Assert.False(subFolderRateAndSupplement.BaseRateRegulation.InheritStatus.IsOverwritten());
            Assert.False(subFolderRateAndSupplement.SiteSpecificTimeSupplement.InheritStatus.IsOverwritten());
            Assert.False(subFolderRateAndSupplement.IndirectTimeSupplement.InheritStatus.IsOverwritten());
            Assert.Equal(166, subFolderRateAndSupplement.BaseRateRegulation.Value);
            Assert.Equal(198, subFolderRateAndSupplement.SiteSpecificTimeSupplement.Value);
            Assert.Equal(187, subFolderRateAndSupplement.IndirectTimeSupplement.Value);
        }

        [Fact]
        public void GivenProjectFolderRoot_WhenOverwritingBaseRegulationAndSupplementsDifferentiated_OverwrittenValuesFromDifferentFoldersAreUsed()
        {
            // Create folders
            // Root (base rate regulation overwritten)
            //    |- folder (Indirect time supplement overwritten)
            //       |- subFolder (Site specific time supplement overwritten)
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            var subFolder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            folderRoot.AddFolder(subFolder, folder.ProjectFolderId);

            // Overwrite base rate regulation on root
            var defaultBaseRateAndSupplement = GetDefaultBaseRateAndSupplement();
            folderRoot.UpdateFolderBaseRateRegulation(ProjectFolderRoot.RootFolderId,
                FolderBaseRateRegulation.Create(77, FolderValueInheritStatus.Overwrite()), defaultBaseRateAndSupplement);
            folderRoot.UpdateFolderBaseSupplement(folder.ProjectFolderId,
                FolderIndirectTimeSupplement.Create(156, FolderValueInheritStatus.Overwrite()),
                FolderSiteSpecificTimeSupplement.Inherit(), defaultBaseRateAndSupplement);
            folderRoot.UpdateFolderBaseSupplement(subFolder.ProjectFolderId,
                FolderIndirectTimeSupplement.Inherit(),
                FolderSiteSpecificTimeSupplement.Create(44, FolderValueInheritStatus.Overwrite()), defaultBaseRateAndSupplement);

            // Assert root folder values
            var rootFolderRateAndSupplement = folderRoot.RootFolder.FolderRateAndSupplement;
            Assert.True(rootFolderRateAndSupplement.BaseRateRegulation.InheritStatus.IsOverwritten());
            Assert.False(rootFolderRateAndSupplement.SiteSpecificTimeSupplement.InheritStatus.IsOverwritten());
            Assert.False(rootFolderRateAndSupplement.IndirectTimeSupplement.InheritStatus.IsOverwritten());
            Assert.Equal(77, rootFolderRateAndSupplement.BaseRateRegulation.Value);
            Assert.Equal(defaultBaseRateAndSupplement.SiteSpecificTimeSupplement.Value, rootFolderRateAndSupplement.SiteSpecificTimeSupplement.Value);
            Assert.Equal(defaultBaseRateAndSupplement.IndirectTimeSupplement.Value, rootFolderRateAndSupplement.IndirectTimeSupplement.Value);

            // Assert folder values
            var folderRateAndSupplement = folder.FolderRateAndSupplement;
            Assert.False(folderRateAndSupplement.BaseRateRegulation.InheritStatus.IsOverwritten());
            Assert.False(folderRateAndSupplement.SiteSpecificTimeSupplement.InheritStatus.IsOverwritten());
            Assert.True(folderRateAndSupplement.IndirectTimeSupplement.InheritStatus.IsOverwritten());
            Assert.Equal(77, folderRateAndSupplement.BaseRateRegulation.Value);
            Assert.Equal(defaultBaseRateAndSupplement.SiteSpecificTimeSupplement.Value, rootFolderRateAndSupplement.SiteSpecificTimeSupplement.Value);
            Assert.Equal(156, folderRateAndSupplement.IndirectTimeSupplement.Value);

            // Assert subfolder uses overwritten base rate regulation
            var subFolderRateAndSupplement = subFolder.FolderRateAndSupplement;
            Assert.False(subFolderRateAndSupplement.BaseRateRegulation.InheritStatus.IsOverwritten());
            Assert.True(subFolderRateAndSupplement.SiteSpecificTimeSupplement.InheritStatus.IsOverwritten());
            Assert.False(subFolderRateAndSupplement.IndirectTimeSupplement.InheritStatus.IsOverwritten());
            Assert.Equal(77, subFolderRateAndSupplement.BaseRateRegulation.Value);
            Assert.Equal(44, subFolderRateAndSupplement.SiteSpecificTimeSupplement.Value);
            Assert.Equal(156, subFolderRateAndSupplement.IndirectTimeSupplement.Value);
        }

        [Fact]
        public void GivenProjectFolderRoot_WhenInheritingBaseRegulationAndSupplementsOnRootFolder_ExceptionIsThrown()
        {
            // Create folder
            var folderRoot = Any.ProjectFolderRoot();

            // Inherit base rate regulation on root folder
            var defaultBaseRateAndSupplement = GetDefaultBaseRateAndSupplement();
            var folderBaseRateRegulation = FolderBaseRateRegulation.Inherit();
            Assert.Throws<InvalidOperationException>(() =>
                folderRoot.UpdateFolderBaseRateRegulation(ProjectFolderRoot.RootFolderId, folderBaseRateRegulation, defaultBaseRateAndSupplement));

            // Overwrite base supplements on root folder
            Assert.Throws<InvalidOperationException>(() => folderRoot.UpdateFolderBaseSupplement(ProjectFolderRoot.RootFolderId,
                FolderIndirectTimeSupplement.Inherit(),
                FolderSiteSpecificTimeSupplement.Inherit(), defaultBaseRateAndSupplement));
        }
    }
}
