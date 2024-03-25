using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemMaterial;
using deftq.Tests.End2End.CatalogTest;
using FluentAssertions;
using Xunit;

namespace deftq.Tests.End2End.ProjectWorkItems
{
    [Collection("End2End")]
    public class RemoveWorkItemTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public RemoveWorkItemTest(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task Should_Remove_Work_Item()
        {
            //Import materials from catalog
            await CatalogTestData.ImportMaterials(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");

            //Register a work item
            await _api.RegisterWorkItemMaterial(projectId, folderId, CatalogTestData.MaterialWithReplacementId, 80, 1,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            
            // Get work items
            var workItemsResponse = await _api.GetWorkItems(projectId, folderId);
            var workItemId = workItemsResponse.WorkItems[0].WorkItemId;
            
            //Delete work items
            await _api.RemoveWorkItem(projectId, folderId, new List<Guid> { workItemId });
            
            //Assert work item is deleted
            workItemsResponse = await _api.GetWorkItems(projectId, folderId);
            workItemsResponse.WorkItems.Should().BeEmpty();
        }
    }
}
