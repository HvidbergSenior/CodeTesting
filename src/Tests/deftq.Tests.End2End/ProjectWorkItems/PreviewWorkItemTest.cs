using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetWorkItemMaterialPreview;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetWorkItemOperationPreview;
using deftq.Tests.End2End.CatalogTest;
using Xunit;

namespace deftq.Tests.End2End.ProjectWorkItems
{
    [Collection("End2End")]
    public class PreviewWorkItemTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public PreviewWorkItemTest(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task Preview_WorkItem_Material()
        {
            // Import materials from catalog
            await CatalogTestData.ImportMaterials(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");

            // Preview a work item
            var previewResponse = await _api.PreviewWorkItemMaterial(projectId, folderId, CatalogTestData.MaterialWithReplacementId, 80, 2,
                new List<GetWorkItemMaterialPreviewSupplementOperationRequest>(), new List<GetWorkItemMaterialPreviewSupplementRequest>());
            Assert.Equal(125000, previewResponse.OperationTimeMilliseconds);
            Assert.Equal(439600m, previewResponse.TotalWorkTimeMilliseconds);
            Assert.Equal(26.22m, previewResponse.WorkItemTotalPaymentDkr, 2);

            // Assert no work item was created
            var workItemsResponse = await _api.GetWorkItems(projectId, folderId);
            Assert.Empty(workItemsResponse.WorkItems);
        }

        [Fact]
        public async Task Preview_WorkItem_Operation()
        {
            // Import operations from catalog
            await CatalogTestData.ImportOperations(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");

            // Preview a work item
            var previewResponse = await _api.PreviewWorkItemOperation(projectId, folderId, CatalogTestData.RemoveCoverOperationId, 2,
                new List<GetWorkItemOperationPreviewSupplementRequest>());
            Assert.Equal(269, previewResponse.OperationTimeMilliseconds);
            Assert.Equal(946.0192m, previewResponse.TotalWorkTimeMilliseconds);
            Assert.Equal(0.0564m, previewResponse.WorkItemTotalPaymentDkr, 4);

            // Assert no work item was created
            var workItemsResponse = await _api.GetWorkItems(projectId, folderId);
            Assert.Empty(workItemsResponse!.WorkItems);
        }
    }
}
