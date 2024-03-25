using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemMaterial;
using deftq.Pieceworks.Application.GetWorkItems;
using deftq.Tests.End2End.CatalogTest;
using FluentAssertions;
using Xunit;

namespace deftq.Tests.End2End.ProjectWorkItems
{
    [Collection("End2End")]
    public class CopyWorkItemTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public CopyWorkItemTest(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task Copy_WorkItem()
        {
            // Import operations from catalog
            await CatalogTestData.ImportMaterials(_fixture);
            await CatalogTestData.ImportSupplements(_fixture);

            // Create a project
            var projectId = await _api.CreateProject("My pet project");

            // Create destination folder
            await _api.CreateFolder(projectId, "sourceFolder");

            // Get folder structure
            var folderRoot = await _api.GetFolderRoot(projectId);
            var sourceFolderId = folderRoot.RootFolder.ProjectFolderId;
            var destinationFolderId = folderRoot.RootFolder.SubFolders[0].ProjectFolderId;

            // Register work item in source folder
            var supplementRequest = new List<MaterialSupplementRequest> { new(CatalogTestData.Bah25SupplementId) };
            var supplementOperationRequest =
                new List<MaterialSupplementOperationRequest> { new(CatalogTestData.TripleCableEmbeddedSupplementOperationId, 10) };
            await _api.RegisterWorkItemMaterial(projectId, sourceFolderId, CatalogTestData.TripleCableMaterialId, 3, 1, supplementOperationRequest,
                supplementRequest);

            // Get source work item id
            var workItems = await _api.GetWorkItems(projectId, sourceFolderId);
            var workItemId = workItems.WorkItems[0].WorkItemId;

            // Copy work item to destination folder
            await _api.CopyWorkItems(projectId, sourceFolderId, new List<Guid> { workItemId }, destinationFolderId);

            // Assert new work item exists in destination folder
            var destinationWorkItems = await _api.GetWorkItems(projectId, destinationFolderId);
            destinationWorkItems.WorkItems.Should().HaveCount(1);
            destinationWorkItems.WorkItems[0].WorkItemType.Should().Be(WorkItemType.Material);
            destinationWorkItems.WorkItems[0].WorkItemTotalOperationTimeMilliseconds.Should().BeGreaterThan(0);
            destinationWorkItems.WorkItems[0].WorkItemTotalPaymentDkr.Should().BeGreaterThan(0);
        }
    }
}
