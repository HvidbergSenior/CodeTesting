using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemMaterial;
using deftq.Tests.End2End.CatalogTest;
using FluentAssertions;
using Xunit;

namespace deftq.Tests.End2End.ProjectWorkItems
{
    [Collection("End2End")]
    public class MoveWorkItemsTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public MoveWorkItemsTest(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task Should_Move_Work_Item()
        {
            //Import materials from catalog
            await CatalogTestData.ImportMaterials(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();
            
            // Create a folder
            var sourceFolderId = await _api.CreateFolder(projectId, "name_1", "description 1");
            
            // Create another folder
            var destinationFolderId = await _api.CreateFolder(projectId, "name_2", "description 2");

            //Register a work item
            await _api.RegisterWorkItemMaterial(projectId, sourceFolderId, CatalogTestData.MaterialWithReplacementId, 80, 1,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            
            // Verify work items at source folder and extract work item id
            var workItemsResponse = await _api.GetWorkItems(projectId, sourceFolderId);
            var workItemId = workItemsResponse.WorkItems[0].WorkItemId;

            //Move work item
            await _api.MoveWorkItems(projectId, sourceFolderId, new List<Guid> { workItemId }, destinationFolderId);
            
            //Assert work item is moved from source folder
            workItemsResponse = await _api.GetWorkItems(projectId, sourceFolderId);
            workItemsResponse.WorkItems.Should().BeEmpty();
            
            //Assert work item is moved to source folder
            workItemsResponse = await _api.GetWorkItems(projectId, destinationFolderId);
            workItemsResponse.WorkItems[0].WorkItemId.Should().Be(workItemId);
        }
    }
}
