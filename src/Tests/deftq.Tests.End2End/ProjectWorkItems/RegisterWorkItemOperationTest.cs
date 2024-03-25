using System.Net;
using System.Net.Http.Json;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemOperation;
using deftq.Pieceworks.Application.GetWorkItems;
using deftq.Tests.End2End.CatalogTest;
using Xunit;

namespace deftq.Tests.End2End.ProjectWorkItems
{
    [Collection("End2End")]
    public class RegisterWorkItemOperationTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public RegisterWorkItemOperationTest(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task Should_Find_Registered_WorkItem()
        {
            // Import materials from catalog
            await CatalogTestData.ImportOperations(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");

            // Register a work item
            await _api.RegisterWorkItemOperation(projectId, folderId, CatalogTestData.RemoveCoverOperationId, 2,
                new List<OperationSupplementRequest>());

            // Get work items
            var workItemsResponse = await _api.GetWorkItems(projectId, folderId);

            Assert.Equal(1, workItemsResponse.WorkItems.Count);
            Assert.Equal(269, workItemsResponse.WorkItems[0].WorkItemOperationTimeMilliseconds);
            Assert.Equal(946.0192m, workItemsResponse.WorkItems[0].WorkItemTotalOperationTimeMilliseconds);
            Assert.Equal(0.06m, workItemsResponse.WorkItems[0].WorkItemTotalPaymentDkr, 2);
        }

        [Fact]
        public async Task Register_Work_Item_Without_Amount()
        {
            //Import materials from catalog
            await CatalogTestData.ImportOperations(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");

            // Register a work item
            await _api.RegisterWorkItemOperation(projectId, folderId, CatalogTestData.RemoveCoverOperationId, 0,
                new List<OperationSupplementRequest>());

            // Get work items
            var workItemsResponse = await _api.GetWorkItems(projectId, folderId);
            Assert.Equal(1, workItemsResponse.WorkItems.Count);
            Assert.Equal(WorkItemType.Operation, workItemsResponse.WorkItems[0].WorkItemType);
            Assert.Equal("170141000001", workItemsResponse.WorkItems[0].WorkItemOperation?.OperationNumber);
            Assert.Equal(0, workItemsResponse.WorkItems[0].WorkItemTotalOperationTimeMilliseconds);
            Assert.Equal(0, workItemsResponse.WorkItems[0].WorkItemTotalPaymentDkr);
        }

        [Fact]
        public async Task Register_WorkItem_With_Unknown_Supplement_Should_Return_Error()
        {
            //Import materials from catalog
            await CatalogTestData.ImportOperations(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // get root folder
            var foldersResponse = await _api.GetFolderRoot(projectId);
            var folderId = foldersResponse.RootFolder.ProjectFolderId;

            // Register a work item
            var operationId = CatalogTestData.RemoveCoverOperationId;
            var workItemRequest = new RegisterWorkItemOperationRequest(operationId, Decimal.One,
                new List<OperationSupplementRequest> { new OperationSupplementRequest(Guid.Parse("B4FB06DC-C628-4FBF-850F-111111111111")) });
            var workItemResponse = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/folders/{folderId}/workitems/operation",
                workItemRequest,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.NotFound, workItemResponse.StatusCode);
        }

        [Fact]
        public async Task Register_WorkItem_With_Supplements()
        {
            //Import materials from catalog
            await CatalogTestData.ImportOperations(_fixture);
            await CatalogTestData.ImportSupplements(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            //get root folder
            var foldersResponse = await _api.GetFolderRoot(projectId);
            var folderId = foldersResponse.RootFolder.ProjectFolderId;

            // Register a work item
            var supplement1 = new OperationSupplementRequest(Guid.Parse("C0C64FCC-D855-43ED-B808-9094C3D60646"));
            var supplement2 = new OperationSupplementRequest(Guid.Parse("28AD88AD-C66B-49A8-88DF-CAFAB2A87290"));
            await _api.RegisterWorkItemOperation(projectId, folderId, CatalogTestData.RemoveCoverOperationId, 1,
                new List<OperationSupplementRequest> { supplement1, supplement2 });
            
            // Get work items
            var workItemsResponse = await _api.GetWorkItems(projectId, folderId);

            // Assert registered supplements
            Assert.Equal(1, workItemsResponse.WorkItems.Count);
            Assert.Contains(workItemsResponse.WorkItems[0].Supplements, s => s.SupplementNumber.Equals("bah25", StringComparison.Ordinal));
            Assert.Contains(workItemsResponse.WorkItems[0].Supplements, s => s.SupplementNumber.Equals("bah75", StringComparison.Ordinal));
        }
    }
}
