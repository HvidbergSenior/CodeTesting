using System.Net;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateProjectFolder;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemMaterial;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateFolderSupplements;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderExtraWork;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderLock;
using deftq.Pieceworks.Application.GetProjectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Tests.End2End.CatalogTest;
using Xunit;
using Guid = System.Guid;

namespace deftq.Tests.End2End.ProjectFolders
{
    [Collection("End2End")]
    public class ProjectFoldersTest
    {
        private readonly Api _api;
        private readonly WebAppFixture fixture;

        public ProjectFoldersTest(WebAppFixture webAppFixture)
        {
            fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task FolderName_ShouldUpdate()
        {
            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");

            // Update name
            await _api.UpdateFolderName(projectId, folderId, "test");

            //Assert name was updated
            var folderRoot = await _api.GetFolderRoot(projectId);
            Assert.Equal("test", folderRoot.RootFolder.SubFolders[0].ProjectFolderName);
        }

        [Fact]
        public async Task FolderDescription_ShouldUpdate()
        {
            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");

            // Update description
            await _api.UpdateFolderDescription(projectId, folderId, "12345");

            //Assert description was updated
            var folderRoot = await _api.GetFolderRoot(projectId);
            Assert.Equal("12345", folderRoot.RootFolder.SubFolders[0].ProjectFolderDescription);
        }

        [Fact]
        public async Task FolderSupplements_ShouldUpdate()
        {
            //Import supplements from catalog
            await CatalogTestData.ImportSupplements(fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");

            var supplement1 = Guid.Parse("C0C64FCC-D855-43ED-B808-9094C3D60646");
            var supplement2 = Guid.Parse("28AD88AD-C66B-49A8-88DF-CAFAB2A87290");

            await _api.UpdateFolderSupplements(projectId, folderId, new List<Guid> { supplement1, supplement2 });
            var folderRoot = await _api.GetFolderRoot(projectId);
            var supplements = folderRoot.RootFolder.SubFolders[0].FolderSupplements;
            Assert.Equal(2, supplements.Count);
            Assert.Equal(supplement1, supplements[0].SupplementId);
            Assert.Equal("bah25", supplements[0].SupplementNumber);
            Assert.Equal(supplement2, supplements[1].SupplementId);
            Assert.Equal("bah75", supplements[1].SupplementNumber);
        }

        [Fact]
        public async Task FolderSupplements_ShouldUpdateAndClearAgain()
        {
            //Import supplements from catalog
            await CatalogTestData.ImportSupplements(fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");

            var supplement1 = Guid.Parse("C0C64FCC-D855-43ED-B808-9094C3D60646");
            var supplement2 = Guid.Parse("28AD88AD-C66B-49A8-88DF-CAFAB2A87290");

            await _api.UpdateFolderSupplements(projectId, folderId, new List<Guid> { supplement1, supplement2 });
            var folderRoot = await _api.GetFolderRoot(projectId);
            var supplements = folderRoot.RootFolder.SubFolders[0].FolderSupplements;
            Assert.Equal(2, supplements.Count);

            await _api.UpdateFolderSupplements(projectId, folderId, new List<Guid>());
            folderRoot = await _api.GetFolderRoot(projectId);
            supplements = folderRoot.RootFolder.SubFolders[0].FolderSupplements;
            Assert.Equal(0, supplements.Count);
        }

        [Fact]
        public async Task FolderSupplements_ShouldUpdateOnUseTheSupplementsSend()
        {
            //Import supplements from catalog
            await CatalogTestData.ImportSupplements(fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");

            var supplement1 = Guid.Parse("C0C64FCC-D855-43ED-B808-9094C3D60646");
            var supplement2 = Guid.Parse("28AD88AD-C66B-49A8-88DF-CAFAB2A87290");

            await _api.UpdateFolderSupplements(projectId, folderId, new List<Guid> { supplement1 });
            var folderRoot = await _api.GetFolderRoot(projectId);
            var supplements = folderRoot.RootFolder.SubFolders[0].FolderSupplements;
            Assert.Equal(1, supplements.Count);
            Assert.Equal(supplement1, supplements[0].SupplementId);

            await _api.UpdateFolderSupplements(projectId, folderId, new List<Guid> { supplement2 });
            folderRoot = await _api.GetFolderRoot(projectId);
            supplements = folderRoot.RootFolder.SubFolders[0].FolderSupplements;
            Assert.Equal(1, supplements.Count);
            Assert.Equal(supplement2, supplements[0].SupplementId);
        }

        [Fact]
        public async Task FolderSupplements_ShouldFailWhenUnknownSupplement()
        {
            //Import supplements from catalog
            await CatalogTestData.ImportSupplements(fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");

            var supplement1 = Guid.Parse("C0C64FCC-D855-43ED-B808-9094C3D60642"); // This is error GUID
            var supplement2 = Guid.Parse("28AD88AD-C66B-49A8-88DF-CAFAB2A87290");

            var updateRequest = new UpdateFolderSupplementsRequest(new List<Guid> { supplement1, supplement2 });
            var updateResponse = await fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/folders/{folderId}/supplements", updateRequest);
            Assert.Equal(HttpStatusCode.NotFound, updateResponse.StatusCode);
        }

        [Fact]
        public async Task GivenProject_WhenCreatingFolder_FoldersShouldIncludeFolder()
        {
            // Create a project
            var projectId = await _api.CreateProject("myProject");

            // Create a folder
            await _api.CreateFolder(projectId, "myFolder", "description");

            // Get folder structure
            var folderRoot = await _api.GetFolderRoot(projectId);
            Assert.Equal(projectId, folderRoot.ProjectId);
            Assert.Equal(ProjectFolderRoot.RootFolderId.Value, folderRoot.RootFolder.ProjectFolderId);
            Assert.Equal("myProject", folderRoot.RootFolder.ProjectFolderName);
            Assert.Equal(1, folderRoot.RootFolder.SubFolders.Count);
        }

        [Fact]
        public async Task GivenProject_WhenCreatingSubFolder_FoldersShouldIncludeSubFolder()
        {
            // Create a project
            var projectId = await _api.CreateProject("myProject");

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "myFolderDescription");

            // Create a sub folder
            await _api.CreateFolder(projectId, "mySubFolder", "mySubFolderDescription", folderId);

            // Get folder structure
            var folderRoot = await _api.GetFolderRoot(projectId);
            Assert.Equal(projectId, folderRoot.ProjectId);
            Assert.Equal(1, folderRoot.RootFolder.SubFolders.Count);
            Assert.Equal("myFolder", folderRoot.RootFolder.SubFolders[0].ProjectFolderName);
            Assert.Equal(1, folderRoot.RootFolder.SubFolders[0].SubFolders.Count);
            Assert.Equal("mySubFolder", folderRoot.RootFolder.SubFolders[0].SubFolders[0].ProjectFolderName);
        }

        [Fact]
        public async Task GivenUnknownProject_WhenCreatingFolder_ShouldReturnNotFound()
        {
            // Create a folder
            var id = Guid.NewGuid();
            var foldersRequest = new CreateProjectFolderRequest("myFolder", "myFolderDescription");
            var createFoldersResponse = await fixture.Client.PostAsJsonAsync($"/api/projects/{id}/folders", foldersRequest);
            Assert.Equal(HttpStatusCode.NotFound, createFoldersResponse.StatusCode);
        }

        [Fact]
        public async Task GivenProject_WhenCreatingSubFolderInUnknownFolder_ShouldReturnNotFound()
        {
            // Create a project
            var projectId = await _api.CreateProject("myProject");

            // Create a folder
            var foldersRequest = new CreateProjectFolderRequest("myFolder", "myFolderDescription", Guid.NewGuid());
            var createFoldersResponse = await fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/folders", foldersRequest);
            Assert.Equal(HttpStatusCode.NotFound, createFoldersResponse.StatusCode);
        }

        [Fact]
        public async Task GivenProjectWithFolder_WhenRemovingFolder_FoldersShouldNotIncludeFolder()
        {
            // Create a project
            var projectId = await _api.CreateProject("myProject");

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "myFolderDescription");

            // Remove folder
            await _api.RemoveFolder(projectId, folderId);

            // Assert folder is removed
            var folderRoot = await _api.GetFolderRoot(projectId);
            Assert.Empty(folderRoot.RootFolder.SubFolders);
        }

        [Fact]
        public async Task GivenProjectWithFolder_WhenCopyingFolder_FolderIsReturned()
        {
            // Import operations from catalog
            await CatalogTestData.ImportMaterials(fixture);
            await CatalogTestData.ImportSupplements(fixture);

            // Create a project
            var projectId = await _api.CreateProject("myProject");

            //Create folder tree
            var folder1Id = await _api.CreateFolder(projectId, "folder1", "folder1 description");
            var folder2Id = await _api.CreateFolder(projectId, "folder2", "folder2 description", folder1Id);
            var folder3Id = await _api.CreateFolder(projectId, "folder3", "folder3 description", folder2Id);
            await _api.CreateFolder(projectId, "folder4", "folder4 description", folder2Id);
            var destinationFolderId = await _api.CreateFolder(projectId, "folderdestination", "folderdestination description", folder1Id);

            // Register work item in folder 1
            var materialSupplementRequests = new List<MaterialSupplementRequest> { new MaterialSupplementRequest(CatalogTestData.Bah25SupplementId) };
            await _api.RegisterWorkItemMaterial(projectId, folder1Id, CatalogTestData.MaterialWithMasterId, 7, 2,
                new List<MaterialSupplementOperationRequest>(), materialSupplementRequests);

            // Register work item in folder 3
            materialSupplementRequests = new List<MaterialSupplementRequest> { new MaterialSupplementRequest(CatalogTestData.Bah75SupplementId) };
            await _api.RegisterWorkItemMaterial(projectId, folder3Id, CatalogTestData.MaterialWithMasterId, 3, 8,
                new List<MaterialSupplementOperationRequest>(), materialSupplementRequests);

            // Copy folder
            await _api.CopyFolder(projectId, folder1Id, destinationFolderId);

            // Assert folder is copied
            var folderRoot = await _api.GetFolderRoot(projectId);
            var sourceFolder = FindFirstFolderWithName(folderRoot.RootFolder, "folder1");
            var destinationFolder = FindFirstFolderWithName(folderRoot.RootFolder, "folderdestination");
            var destinationSubFolder1 = FindFirstFolderWithName(destinationFolder, "folder1");
            Assert.Equal(1, destinationFolder.SubFolders.Count);
            Assert.Equal(sourceFolder.ProjectFolderDescription, destinationFolder.SubFolders[0].ProjectFolderDescription);

            //Assert work item is copied
            var workItemResponse = await _api.GetWorkItems(projectId, destinationSubFolder1.ProjectFolderId);
            Assert.Equal(1, workItemResponse.WorkItems.Count);
        }

        [Fact]
        public async Task GivenProjectWithFolders_WhenCopyingFolder_FolderSupplementsAreCalculated()
        {
            // Import operations from catalog
            await CatalogTestData.ImportMaterials(fixture);
            await CatalogTestData.ImportSupplements(fixture);
            
            // Create a project
            var projectId = await _api.CreateProject("myProject");
            
            //Create folder tree
            var folder1Id = await _api.CreateFolder(projectId, "folder1", "folder1 description");
            var folder2Id = await _api.CreateFolder(projectId, "folder2", "folder2 description");
            
            await _api.RegisterWorkItemMaterial(projectId, folder1Id, CatalogTestData.MaterialWithMasterId, 3, 8,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            await _api.UpdateFolderSupplements(projectId, folder2Id, new List<Guid> { CatalogTestData.Bah75SupplementId });

            await _api.CopyFolder(projectId, folder1Id, folder2Id);

            var folders = (await _api.GetFolderRoot(projectId)).RootFolder;
            var folder1CopyId = folders.SubFolders.First(folder => folder.ProjectFolderId == folder2Id).SubFolders[0].ProjectFolderId;
            var folder1CopyWorkItems = await _api.GetWorkItems(projectId, folder1CopyId);
            
            Assert.Equal(36.71m, folder1CopyWorkItems.WorkItems[0].WorkItemTotalPaymentDkr, 2);
        }
        
        [Fact]
        public async Task GivenProjectWithFolders_WhenMovingFolder_FolderSupplementsAreCalculated()
        {
            // Import operations from catalog
            await CatalogTestData.ImportMaterials(fixture);
            await CatalogTestData.ImportSupplements(fixture);
            
            // Create a project
            var projectId = await _api.CreateProject("myProject");
            
            //Create folder tree
            var folder1Id = await _api.CreateFolder(projectId, "folder1", "folder1 description");
            var folder2Id = await _api.CreateFolder(projectId, "folder2", "folder2 description");
            
            await _api.RegisterWorkItemMaterial(projectId, folder1Id, CatalogTestData.MaterialWithMasterId, 3, 8,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            await _api.UpdateFolderSupplements(projectId, folder2Id, new List<Guid> { CatalogTestData.Bah75SupplementId });

            await _api.MoveFolder(projectId, folder1Id, folder2Id);

            var folders = (await _api.GetFolderRoot(projectId)).RootFolder;
            var folder1CopyId = folders.SubFolders.First(folder => folder.ProjectFolderId == folder2Id).SubFolders[0].ProjectFolderId;
            var folder1CopyWorkItems = await _api.GetWorkItems(projectId, folder1CopyId);
            
            Assert.Equal(36.71m, folder1CopyWorkItems.WorkItems[0].WorkItemTotalPaymentDkr, 2);
        }

        private ProjectFolderResponse FindFirstFolderWithName(ProjectFolderResponse root, string name)
        {
            return Flatten(root).First(folder => folder.ProjectFolderName.Equals(name, StringComparison.Ordinal));
        }

        private IList<ProjectFolderResponse> Flatten(ProjectFolderResponse root)
        {
            var result = new List<ProjectFolderResponse> { root };
            foreach (var subFolder in root.SubFolders)
            {
                result.AddRange(Flatten(subFolder));
            }

            return result;
        }

        [Fact]
        public async Task GivenProjectWithFolder_WhenMovingFolder_FolderIsReturned()
        {
            // Create a project
            var projectId = await _api.CreateProject("myProject");

            // Create folders
            var folder1Id = await _api.CreateFolder(projectId, "myFolder1", "description1");
            var folder2Id = await _api.CreateFolder(projectId, "myFolder2", "description2");

            // Move folder
            await _api.MoveFolder(projectId, folder1Id, folder2Id);

            // Assert folder is moved
            var folderRoot = await _api.GetFolderRoot(projectId);
            Assert.Equal(1, folderRoot.RootFolder.SubFolders.Count);
            Assert.Equal(1, folderRoot.RootFolder.SubFolders[0].SubFolders.Count);
        }

        [Fact]
        public async Task GivenFolderWithSupplements_WhenUpdatingSupplements_ShouldCalculate()
        {
            // Import operations from catalog
            await CatalogTestData.ImportMaterials(fixture);
            await CatalogTestData.ImportSupplements(fixture);

            // Create a project
            var projectId = await _api.CreateProject("myProject");

            //Create folder tree
            var folder1Id = await _api.CreateFolder(projectId, "folder1", "folder1 description");

            await _api.RegisterWorkItemMaterial(projectId, folder1Id, CatalogTestData.MaterialWithMasterId, 3, 8,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            var folder1WorkItems = await _api.GetWorkItems(projectId, folder1Id);
            Assert.Equal(20.98m, folder1WorkItems.WorkItems[0].WorkItemTotalPaymentDkr, 2);

            var supplement1 = CatalogTestData.Bah25SupplementId;
            var supplement2 = CatalogTestData.Bah75SupplementId;

            await _api.UpdateFolderSupplements(projectId, folder1Id, new List<Guid> { supplement1, supplement2 });

            folder1WorkItems = await _api.GetWorkItems(projectId, folder1Id);

            Assert.Equal(41.96m, folder1WorkItems.WorkItems[0].WorkItemTotalPaymentDkr, 2);
        }

        [Fact]
        public async Task GivenProjectWithFolder_WhenMovingFolderToRoot_FolderIsReturned()
        {
            // Create a project
            var projectId = await _api.CreateProject("myProject");

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder1", "description1");

            // Create a sub folder
            var subFolderId = await _api.CreateFolder(projectId, "myFolder2", "description2", folderId);

            // Move sub folder to root
            await _api.MoveFolder(projectId, subFolderId, ProjectFolderRoot.RootFolderId.Value);

            // Assert folder is moved
            var folderRoot = await _api.GetFolderRoot(projectId);
            Assert.Equal(2, folderRoot.RootFolder.SubFolders.Count);
            Assert.Equal(0, folderRoot.RootFolder.SubFolders[0].SubFolders.Count);
            Assert.Equal(0, folderRoot.RootFolder.SubFolders[1].SubFolders.Count);
        }

        [Fact]
        public async Task Folder_Can_Be_Locked()
        {
            //Create a project
            var projectId = await _api.CreateProject("myProject");

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder1", "description1");

            // Lock folder
            await _api.UpdateFolderLock(projectId, folderId, UpdateLockProjectFolderRequest.Lock.Locked, false);

            // Lock root folder
            await _api.UpdateFolderLock(projectId, ProjectFolderRoot.RootFolderId.Value, UpdateLockProjectFolderRequest.Lock.Locked, false);

            // Assert folder and root folder is locked
            var folderRoot = await _api.GetFolderRoot(projectId);
            Assert.Equal(ProjectFolderResponse.ProjectFolderLock.Locked, folderRoot.RootFolder.SubFolders[0].ProjectFolderLocked);
            Assert.Equal(ProjectFolderResponse.ProjectFolderLock.Locked, folderRoot.RootFolder.ProjectFolderLocked);
        }

        [Fact]
        public async Task GivenFolder_WhenMarkedAsExtraWork_FolderAndSubFoldersAreExtraWork()
        {
            //Create a project
            var projectId = await _api.CreateProject("myProject");

            // Create folders
            var folderA = await _api.CreateFolder(projectId, "folderA");
            await _api.CreateFolder(projectId, "folderA1", "", folderA);
            await _api.CreateFolder(projectId, "folderA2", "", folderA);
            var folderB = await _api.CreateFolder(projectId, "folderB");
            await _api.CreateFolder(projectId, "folderB1", "", folderB);

            // Mark as extra work
            await _api.UpdateFolderExtraWork(projectId, folderA, UpdateProjectFolderExtraWorkRequest.ExtraWorkUpdate.ExtraWork);

            // Assert correct folders are marked as extra work
            var folderRoot = await _api.GetFolderRoot(projectId);
            Assert.Equal(ProjectFolderResponse.ExtraWork.NormalWork, folderRoot.RootFolder.FolderExtraWork);
            Assert.Equal(ProjectFolderResponse.ExtraWork.ExtraWork, FindFirstFolderWithName(folderRoot.RootFolder, "folderA").FolderExtraWork);
            Assert.Equal(ProjectFolderResponse.ExtraWork.ExtraWork, FindFirstFolderWithName(folderRoot.RootFolder, "folderA1").FolderExtraWork);
            Assert.Equal(ProjectFolderResponse.ExtraWork.ExtraWork, FindFirstFolderWithName(folderRoot.RootFolder, "folderA2").FolderExtraWork);
            Assert.Equal(ProjectFolderResponse.ExtraWork.NormalWork, FindFirstFolderWithName(folderRoot.RootFolder, "folderB").FolderExtraWork);
            Assert.Equal(ProjectFolderResponse.ExtraWork.NormalWork, FindFirstFolderWithName(folderRoot.RootFolder, "folderB1").FolderExtraWork);
        }

        [Fact]
        public async Task Mysterious_Issue_Folder_Cannot_Be_Locked_After_Root_Folder_Is_Locked()
        {
            //Create a project
            var projectId = await _api.CreateProject("myProject");

            // get folder id
            var folderRoot = await _api.GetFolderRoot(projectId);
            var rootFolderId = folderRoot.RootFolder.ProjectFolderId;

            // Create a folder
            var subFolderId = await _api.CreateFolder(projectId, "myFolder1", "description1", rootFolderId);

            // Lock root folder
            await _api.UpdateFolderLock(projectId, rootFolderId, UpdateLockProjectFolderRequest.Lock.Locked, false);

            // Lock subFolder folder
            await _api.UpdateFolderLock(projectId, subFolderId, UpdateLockProjectFolderRequest.Lock.Locked, false);

            //Assert folder and root folder is locked
            folderRoot = await _api.GetFolderRoot(projectId);
            Assert.Equal(ProjectFolderResponse.ProjectFolderLock.Locked, folderRoot.RootFolder.ProjectFolderLocked);
            Assert.Equal(ProjectFolderResponse.ProjectFolderLock.Locked, folderRoot.RootFolder.SubFolders[0].ProjectFolderLocked);
        }
    }
}
