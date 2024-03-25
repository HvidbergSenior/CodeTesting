using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemMaterial;
using deftq.Tests.End2End.CatalogTest;
using Xunit;

namespace deftq.Tests.End2End.GroupedWorkItem
{
    [Collection("End2End")]
    public class GroupedWorkItemsTests : IClassFixture<WebAppFixture>
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public GroupedWorkItemsTests(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task CanGetGroupedWorkItems_WithSuccess()
        {
            // Import operations from catalog
            await CatalogTestData.ImportMaterials(_fixture);
            await CatalogTestData.ImportSupplements(_fixture);

            // Create project
            var projectId = await _api.CreateProject();

            // Create Folder Structure
            var folderIdA = await _api.CreateFolder(projectId, "Folder_A");
            var folderIdB = await _api.CreateFolder(projectId, "Folder_B", "Folder_B_Description", folderIdA);
            var folderIdC = await _api.CreateFolder(projectId, "Folder_C", "Folder_C", folderIdA);

            // Register work item in folderA2
            await _api.RegisterWorkItemMaterial(projectId, folderIdB, CatalogTestData.MaterialWithReplacementId, 80, 10,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            // Fetch grouped work items
            var response = await _api.GetGroupedWorkItemsResult(projectId, folderIdB, 10);

            // Verify result
            Assert.Equal(1, response?.GroupedWorkItems.Count);
        }

        [Fact]
        public async Task CanGetCorrectGroupedWorkItemsAfterCopyingFolder()
        {
            // Import operations from catalog
            await CatalogTestData.ImportMaterials(_fixture);
            await CatalogTestData.ImportSupplements(_fixture);

            // Create project
            var projectId = await _api.CreateProject();

            // Create Folder Structure
            var parentFolderId = await _api.CreateFolder(projectId, "Folder_A", "Folder_A_Description");
            var folderAId = await _api.CreateFolder(projectId, "Folder_B", "Folder_B_Description", parentFolderId);
            var folderBId = await _api.CreateFolder(projectId, "Folder_C", "Folder_C_Description");

            // Register work item in folderA1
            await _api.RegisterWorkItemMaterial(projectId, folderAId, CatalogTestData.MaterialWithReplacementId, 80, 10,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            await _api.RegisterWorkItemMaterial(projectId, folderBId, CatalogTestData.MaterialWithReplacementId, 80, 5,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            await _api.CopyFolder(projectId, folderBId, parentFolderId);

            var groupedWorkItemsResult = await _api.GetGroupedWorkItemsResult(projectId, parentFolderId, 10);

            Assert.Equal(15, groupedWorkItemsResult.GroupedWorkItems[0].Amount);
        }

        [Fact]
        public async Task CanGetCorrectGroupedWorkItemsAfterMovingFolder()
        {
            // Import operations from catalog
            await CatalogTestData.ImportMaterials(_fixture);
            await CatalogTestData.ImportSupplements(_fixture);

            // Create project
            var projectId = await _api.CreateProject();

            // Create Folder Structure
            var parentFolderId = await _api.CreateFolder(projectId, "Folder_ParentFolder", "Folder_ParentFolder_Description");
            var folderAId = await _api.CreateFolder(projectId, "Folder_A", "Folder_A_Description", parentFolderId);
            var folderBId = await _api.CreateFolder(projectId, "Folder_B", "Folder_B_Description");

            // Register work item in folderA1
            await _api.RegisterWorkItemMaterial(projectId, folderAId, CatalogTestData.MaterialWithReplacementId, 80, 10,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            await _api.RegisterWorkItemMaterial(projectId, folderBId, CatalogTestData.MaterialWithReplacementId, 80, 5,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            await _api.MoveFolder(projectId, folderBId, parentFolderId);

            var groupedWorkItemsResult = await _api.GetGroupedWorkItemsResult(projectId, parentFolderId, 10);

            Assert.Equal(15, groupedWorkItemsResult.GroupedWorkItems[0].Amount);
        }

        [Fact]
        public async Task CanGetMaxHitsMostSignificant()
        {
            // Import operations from catalog
            await CatalogTestData.ImportMaterials(_fixture);
            await CatalogTestData.ImportSupplements(_fixture);

            // Create project
            var projectId = await _api.CreateProject();

            var folderAId = await _api.CreateFolder(projectId, "Folder_A");

            await _api.RegisterWorkItemMaterial(projectId, folderAId, CatalogTestData.MaterialWithReplacementId, 80, 10,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            await _api.RegisterWorkItemMaterial(projectId, folderAId, CatalogTestData.MaterialWithMasterId, 80, 5,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            var groupedWorkItemsResult = await _api.GetGroupedWorkItemsResult(projectId, folderAId, 1);

            Assert.Equal(1, groupedWorkItemsResult.GroupedWorkItems.Count);
            Assert.Equal(10, groupedWorkItemsResult.GroupedWorkItems[0].Amount);
        }
    }
}
