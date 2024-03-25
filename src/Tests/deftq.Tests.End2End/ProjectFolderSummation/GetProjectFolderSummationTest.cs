using System.Diagnostics;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemMaterial;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderExtraWork;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Tests.End2End.CatalogTest;
using Xunit;
using Xunit.Abstractions;

namespace deftq.Tests.End2End.ProjectFolderSummation
{
    [Collection("End2End")]
    public class GetProjectFolderSummationTest
    {
        private readonly ITestOutputHelper _output;
        private readonly Api _api;

        public GetProjectFolderSummationTest(WebAppFixture webAppFixture, ITestOutputHelper output)
        {
            _output = output;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task GivenFolderWithWorkItem_WhenGettingSummation_WorkItemIsIncluded()
        {
            await CatalogTestData.ImportMaterials(_api.Fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");

            // Register work item
            await _api.RegisterWorkItemMaterial(projectId, folderId, CatalogTestData.MaterialWithReplacementId, 80, 10,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            // Get summation
            var folderSummation = await _api.GetFolderSummation(projectId, folderId);

            Assert.Equal(131.12m, folderSummation.TotalPaymentDkr, 2);
            Assert.Equal(2198000m, folderSummation.TotalWorkTimeMilliseconds, 2);
            Assert.Equal(0m, folderSummation.TotalExtraPaymentDkr, 2);
            Assert.Equal(0m, folderSummation.TotalExtraWorkTimeMilliseconds, 2);
        }
        
        [Fact]
        public async Task GivenFoldersWithExtraWorkItem_WhenGettingSummation_WorkItemIsIncluded()
        {
            await CatalogTestData.ImportMaterials(_api.Fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            // root
            //    |- A (extra work)
            //       |- A1
            //    |- B
            //       |- B1 (extra work)
            var folderAId = await _api.CreateFolder(projectId, "A");
            var folderA1Id = await _api.CreateFolder(projectId, "A1", String.Empty, folderAId);
            var folderBId = await _api.CreateFolder(projectId, "B");
            var folderB1Id = await _api.CreateFolder(projectId, "B1", String.Empty, folderBId);
            
            // Set extra work
            await _api.UpdateFolderExtraWork(projectId, folderAId, UpdateProjectFolderExtraWorkRequest.ExtraWorkUpdate.ExtraWork);
            await _api.UpdateFolderExtraWork(projectId, folderB1Id, UpdateProjectFolderExtraWorkRequest.ExtraWorkUpdate.ExtraWork);
            
            // Register work items
            await _api.RegisterWorkItemMaterial(projectId, folderAId, CatalogTestData.MaterialWithReplacementId, 80, 20,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            await _api.RegisterWorkItemMaterial(projectId, folderA1Id, CatalogTestData.MaterialWithReplacementId, 80, 20,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            await _api.RegisterWorkItemMaterial(projectId, folderBId, CatalogTestData.MaterialWithReplacementId, 80, 10,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            await _api.RegisterWorkItemMaterial(projectId, folderB1Id, CatalogTestData.MaterialWithReplacementId, 80, 10,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            // Get summation
            var folderSummation = await _api.GetFolderSummation(projectId, ProjectFolderRoot.RootFolderId.Value);

            Assert.Equal(262.23m + 262.23m + 131.12m + 131.12m, folderSummation.TotalPaymentDkr, 2);
            Assert.Equal(4396000m + 4396000m + 2198000m + 2198000m, folderSummation.TotalWorkTimeMilliseconds, 2);
            Assert.Equal(262.23m + 262.23m + 131.12m, folderSummation.TotalExtraPaymentDkr, 2);
            Assert.Equal(4396000m + 4396000m + 2198000m, folderSummation.TotalExtraWorkTimeMilliseconds, 2);
        }
        
        [Fact]
        public async Task GivenCopiedFolder_WhenGettingSummation_CopiesAreIncludedInSum()
        {
            await CatalogTestData.ImportMaterials(_api.Fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create folders
            // root
            //    |- parent
            //       |- source
            var parentFolderId = await _api.CreateFolder(projectId, "parent");
            var sourceFolderId = await _api.CreateFolder(projectId, "source", string.Empty, parentFolderId);

            // Register work item
            await _api.RegisterWorkItemMaterial(projectId, sourceFolderId, CatalogTestData.MaterialWithReplacementId, 80, 10,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            // Create copy of source folder
            // root
            //    |- parent
            //       |- source
            //       |- source
            await _api.CopyFolder(projectId, sourceFolderId, parentFolderId);
            
            // Get summation
            var folderSummation = await _api.GetFolderSummation(projectId, parentFolderId);

            Assert.Equal(131.1168m * 2, folderSummation.TotalPaymentDkr, 2);
            Assert.Equal(2198000m * 2, folderSummation.TotalWorkTimeMilliseconds, 2);
            Assert.Equal(0m, folderSummation.TotalExtraPaymentDkr, 2);
            Assert.Equal(0m, folderSummation.TotalExtraWorkTimeMilliseconds, 2);
        }
        
        [Fact]
        public async Task GivenMovedFolder_WhenGettingSummation_MovedFolderIsIncludedInSum()
        {
            await CatalogTestData.ImportMaterials(_api.Fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create folders
            // root
            //    |- parent
            //       |- A
            //    |- B
            var parentFolderId = await _api.CreateFolder(projectId, "parent");
            var folderAId = await _api.CreateFolder(projectId, "A", string.Empty, parentFolderId);
            var folderBId = await _api.CreateFolder(projectId, "B");

            // Register work items
            await _api.RegisterWorkItemMaterial(projectId, folderAId, CatalogTestData.MaterialWithReplacementId, 80, 10,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            await _api.RegisterWorkItemMaterial(projectId, folderBId, CatalogTestData.MaterialWithReplacementId, 80, 10,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            
            // Move folder B into parent folder
            // root
            //    |- parent
            //       |- A
            //       |- B
            await _api.MoveFolder(projectId, folderBId, parentFolderId);
            
            // Get summation
            var folderSummation = await _api.GetFolderSummation(projectId, parentFolderId);

            Assert.Equal(131.1168m * 2, folderSummation.TotalPaymentDkr, 2);
            Assert.Equal(2198000m * 2, folderSummation.TotalWorkTimeMilliseconds, 2);
            Assert.Equal(0m, folderSummation.TotalExtraPaymentDkr, 2);
            Assert.Equal(0m, folderSummation.TotalExtraWorkTimeMilliseconds, 2);
        }
        
        [Fact(Skip = "manual")]
        public async Task GivenBiggerProject_WhenGettingSummation_ShouldRespondInReasonableTime()
        {
            await CatalogTestData.ImportMaterials(_api.Fixture);

            // Create a project
            var projectId = await _api.CreateProject();
            
            var folderTreeWidth = 2;
            var folderTreeDepth = 5;
            var workItemsInFolders = 100;
            var rootFolderId = ProjectFolderRoot.RootFolderId.Value;

            var folderCount = Math.Pow(folderTreeWidth, folderTreeDepth + 1) - 1;
            var workItemCount = folderCount * workItemsInFolders;
            _output.WriteLine($"Folders count {folderCount}");
            _output.WriteLine($"Work items count {workItemCount}");
            
            // Create folders
            await CreateFolder(projectId, rootFolderId, 0, "0", folderTreeDepth, folderTreeWidth, workItemsInFolders);
            
            // Get folder root
            var folderRootSw = Stopwatch.StartNew();
            var folderRoot = await _api.GetFolderRoot(projectId);
            folderRootSw.Stop();
            _output.WriteLine($"Get folder root took {folderRootSw.ElapsedMilliseconds} ms");
            
            // Get summation
            var folderSummationSw = Stopwatch.StartNew();
            var folderSummation = await _api.GetFolderSummation(projectId, rootFolderId);
            folderSummationSw.Stop();
            _output.WriteLine($"Get summation for root folder took {folderSummationSw.ElapsedMilliseconds} ms");
            _output.WriteLine($"Summation was {folderSummation.TotalPaymentDkr} Dkr and {folderSummation.TotalWorkTimeMilliseconds} ms");
        }

        private async Task CreateFolder(Guid projectId, Guid parentFolderId, int depth, string folderName, int maxDepth, int maxSubFolders, int workItems, bool debugOutput = false)
        {
            for (int i = 0; i < maxSubFolders; i++)
            {
                var folder = $"{folderName}.{i}";

                if (debugOutput)
                {
                    _output.WriteLine($"Folder {folder}");    
                }
                
                var subFolderId = await _api.CreateFolder(projectId, folder, Guid.NewGuid().ToString(), parentFolderId);
                if (depth < maxDepth)
                {
                    await CreateFolder(projectId, subFolderId, depth + 1, folder, maxDepth, maxSubFolders, workItems);
                }

                for (int j = 0; j < workItems; j++)
                {
                    if (debugOutput)
                    {
                        _output.WriteLine($"Folder {folder} work item {j}");    
                    }
                    
                    await _api.RegisterWorkItemMaterial(projectId, subFolderId, CatalogTestData.MaterialWithReplacementId, 80, 10,
                        new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());    
                }
            }
        }
    }
}
