﻿using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemMaterial;
using deftq.Tests.End2End.CatalogTest;
using Xunit;

namespace deftq.Tests.End2End.ProjectWorkItems
{
    [Collection("End2End")]
    public class UpdateWorkItemTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public UpdateWorkItemTest(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task Should_Update_Work_Item()
        {
            //Import materials from catalog
            await CatalogTestData.ImportMaterials(_fixture);
            
            //Create a project
            var projectId = await _api.CreateProject();

            //Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "folderDescription");

            //Register a work item
            await _api.RegisterWorkItemMaterial(projectId, folderId, CatalogTestData.MaterialWithReplacementId, 80, 1,
                    new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            //Get work items
            var workItemResponse = await _api.GetWorkItems(projectId, folderId);
            var workItemId = workItemResponse.WorkItems[0].WorkItemId;
            
            //Update amount on work item
            var newAmount = 40;
            await _api.UpdateWorkItem(projectId, folderId, workItemId, newAmount);
            
            //Assert amount on work item has been updated
            workItemResponse = await _api.GetWorkItems(projectId, folderId);
            Assert.Equal(40, workItemResponse.WorkItems[0].WorkItemAmount);
            Assert.Equal(524.47m, workItemResponse.WorkItems[0].WorkItemTotalPaymentDkr, 2);
            Assert.Equal(8792000.00m, workItemResponse.WorkItems[0].WorkItemTotalOperationTimeMilliseconds, 2);
        }
    }
}
