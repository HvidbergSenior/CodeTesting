using System.Net;
using System.Net.Http.Json;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemMaterial;
using deftq.Pieceworks.Application.GetWorkItems;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Tests.End2End.CatalogTest;
using Xunit;

namespace deftq.Tests.End2End.ProjectWorkItems
{
    [Collection("End2End")]
    public class RegisterWorkItemMaterialTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public RegisterWorkItemMaterialTest(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task Should_Find_Registered_WorkItem()
        {
            //Import materials from catalog
            await CatalogTestData.ImportMaterials(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");

            // Register a work item
            await _api.RegisterWorkItemMaterial(projectId, folderId, CatalogTestData.MaterialWithReplacementId, 80, 1,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            // Get work items
            var workItemsResponse = await _api.GetWorkItems(projectId, folderId);
            Assert.Equal(1, workItemsResponse.WorkItems.Count);
            Assert.Equal(125000, workItemsResponse.WorkItems[0].WorkItemOperationTimeMilliseconds);
            Assert.Equal(219800m, workItemsResponse.WorkItems[0].WorkItemTotalOperationTimeMilliseconds);
            Assert.Equal(13.11m, workItemsResponse.WorkItems[0].WorkItemTotalPaymentDkr, 2);
        }

        [Fact]
        public async Task Register_Work_Item_Without_Mounting_And_Amount()
        {
            //Import materials from catalog
            await CatalogTestData.ImportMaterials(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");

            // Register a work item
            await _api.RegisterWorkItemMaterial(projectId, folderId, CatalogTestData.MaterialWithReplacementId, 0, 0,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            // Get work items
            var workItemsResponse = await _api.GetWorkItems(projectId, folderId);
            Assert.Equal(1, workItemsResponse.WorkItems.Count);
            Assert.Equal(-1, workItemsResponse.WorkItems[0].WorkItemMaterial?.WorkItemMountingCode);
            Assert.Equal(0, workItemsResponse.WorkItems[0].WorkItemTotalOperationTimeMilliseconds);
            Assert.Equal(0, workItemsResponse.WorkItems[0].WorkItemTotalPaymentDkr);
        }

        [Fact]
        public async Task Register_WorkItem_With_SupplementOperations()
        {
            //Import materials from catalog
            await CatalogTestData.ImportMaterials(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");

            // Register a work item
            var supplementOperationRequest1 = new MaterialSupplementOperationRequest(Guid.Parse("DF554BDD-90E0-4883-A485-C117CA27BC87"), 10);
            var supplementOperationRequest2 = new MaterialSupplementOperationRequest(Guid.Parse("3F65DA66-C36E-4BB7-9CB8-D968D4783AFF"), 12);
            await _api.RegisterWorkItemMaterial(projectId, folderId, CatalogTestData.TripleCableMaterialId, 3, 1,
                new List<MaterialSupplementOperationRequest> { supplementOperationRequest1, supplementOperationRequest2 },
                new List<MaterialSupplementRequest>());
            
            // Get work items
            var workItemsResponse = await _api.GetWorkItems(projectId, folderId);
            
            // Material has 3 supplement operations, but only two where specified in the request
            Assert.Equal(2, workItemsResponse.WorkItems[0].WorkItemMaterial?.SupplementOperations.Count);
            Assert.DoesNotContain(workItemsResponse.WorkItems[0].WorkItemMaterial!.SupplementOperations,
                op => op.Text.Equals("Do something unexpected", StringComparison.Ordinal));

            // Assert registered supplement operations
            var op1 = workItemsResponse.WorkItems[0].WorkItemMaterial!.SupplementOperations
                .First(op => op.Text.Equals("Just do something", StringComparison.Ordinal));
            var op2 = workItemsResponse.WorkItems[0].WorkItemMaterial!.SupplementOperations
                .First(op => op.Text.Equals("Dig a hole", StringComparison.Ordinal));
            Assert.Equal(10, op1.Amount);
            Assert.Equal(12, op2.Amount);
            Assert.Equal(WorkItemSupplementOperationResponse.WorkItemSupplementOperationType.AmountRelated, op1.OperationType);
            Assert.Equal(WorkItemSupplementOperationResponse.WorkItemSupplementOperationType.UnitRelated, op2.OperationType);
            Assert.Equal(29000, op1.OperationTimeMilliseconds);
            Assert.Equal(31000, op2.OperationTimeMilliseconds);
        }

        [Fact]
        public async Task Register_WorkItem_With_Unknown_SupplementOperation_Should_Return_Error()
        {
            //Import materials from catalog
            await CatalogTestData.ImportMaterials(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            //get root folder
            var foldersResponse = await _api.GetFolderRoot(projectId);
            var folderId = foldersResponse.RootFolder.ProjectFolderId;

            // Register a work item
            var materialId = CatalogTestData.TripleCableMaterialId;
            var workItemMountingCode = WorkItemMountingCode.FromCode(3).MountingCode;
            var supplementOperationRequest = new MaterialSupplementOperationRequest(Guid.Parse("AAAAAAAA-90E0-4883-A485-C117CA27BC87"), 10);
            var workItemRequest = new RegisterWorkItemMaterialRequest(materialId, Decimal.One, workItemMountingCode,
                new List<MaterialSupplementOperationRequest> { supplementOperationRequest }, new List<MaterialSupplementRequest>());
            var workItemResponse = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/folders/{folderId}/workitems/material",
                workItemRequest,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.NotFound, workItemResponse.StatusCode);
        }

        [Fact]
        public async Task Register_WorkItem_With_Unknown_Supplement_Should_Return_Error()
        {
            //Import materials from catalog
            await CatalogTestData.ImportMaterials(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();
            
            //get root folder
            var foldersResponse = await _api.GetFolderRoot(projectId);
            var folderId = foldersResponse.RootFolder.ProjectFolderId;

            // Register a work item
            var materialId = new Guid("4EAE982E-21BE-4596-8039-DA8A45BB7FD7");
            var workItemMountingCode = WorkItemMountingCode.FromCode(3).MountingCode;
            var workItemRequest = new RegisterWorkItemMaterialRequest(materialId, Decimal.One, workItemMountingCode,
                new List<MaterialSupplementOperationRequest>(),
                new List<MaterialSupplementRequest> { new MaterialSupplementRequest(Guid.Parse("B4FB06DC-C628-4FBF-850F-111111111111")) });
            var workItemResponse = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/folders/{folderId}/workitems/material",
                workItemRequest,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.NotFound, workItemResponse.StatusCode);
        }

        [Fact]
        public async Task Register_WorkItem_With_Supplements()
        {
            //Import materials from catalog
            await CatalogTestData.ImportMaterials(_fixture);
            await CatalogTestData.ImportSupplements(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            //get root folder
            var foldersResponse = await _api.GetFolderRoot(projectId);
            var folderId = foldersResponse.RootFolder.ProjectFolderId;

            // Register a work item
            var supplement1 = new MaterialSupplementRequest(Guid.Parse("C0C64FCC-D855-43ED-B808-9094C3D60646"));
            var supplement2 = new MaterialSupplementRequest(Guid.Parse("28AD88AD-C66B-49A8-88DF-CAFAB2A87290"));
            await _api.RegisterWorkItemMaterial(projectId, folderId, CatalogTestData.TripleCableMaterialId, 3, 1,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest> { supplement1, supplement2 });

            // Get work items
            var workItemsResponse = await _api.GetWorkItems(projectId, folderId);

            // Assert registered supplements
            Assert.Equal(1, workItemsResponse.WorkItems.Count);
            Assert.Contains(workItemsResponse.WorkItems[0].Supplements, s => s.SupplementNumber.Equals("bah25", StringComparison.Ordinal));
            Assert.Contains(workItemsResponse.WorkItems[0].Supplements, s => s.SupplementNumber.Equals("bah75", StringComparison.Ordinal));
        }
    }
}
