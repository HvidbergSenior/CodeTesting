using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemMaterial;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateBaseRate;
using deftq.Pieceworks.Application.GetProjectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Tests.End2End.CatalogTest;
using Xunit;
using static deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateBaseSupplements.BaseSupplementStatusUpdate;

namespace deftq.Tests.End2End.BaseRateAndSupplements
{
    [Collection("End2End")]
    public class BaseRateAndSupplementsTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public BaseRateAndSupplementsTest(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task OverWriteBaseRateAndSupplementsOnProject()
        {
            // Create a project
            var projectId = await _api.CreateProject();

            // Create folders
            var folderId = await _api.CreateFolder(projectId, "myFolder");
            await _api.CreateFolder(projectId, "mySubFolder", String.Empty, folderId);

            // Update base rate regulation on project
            await _api.UpdateFolderBaseRateRegulation(projectId, ProjectFolderRoot.RootFolderId.Value, 12, BaseRateStatusUpdate.Overwrite);

            // Assert all folders have correct base rate regulation
            var folderRoot = await _api.GetFolderRoot(projectId);

            var rootFolderRateAndSupplements = folderRoot.RootFolder.BaseRateAndSupplements;
            Assert.Equal(12, rootFolderRateAndSupplements.BaseRateRegulationPercentage.Value);
            Assert.Equal(BaseRateAndSupplementsValueStatus.Overwrite, rootFolderRateAndSupplements.BaseRateRegulationPercentage.ValueStatus);

            var subFolderRateAndSupplements = folderRoot.RootFolder.SubFolders[0].BaseRateAndSupplements;
            Assert.Equal(12, subFolderRateAndSupplements.BaseRateRegulationPercentage.Value);
            Assert.Equal(BaseRateAndSupplementsValueStatus.Inherit, subFolderRateAndSupplements.BaseRateRegulationPercentage.ValueStatus);
        }

        [Fact]
        public async Task OverWriteBaseSupplementsOnProject()
        {
            // Create a project
            var projectId = await _api.CreateProject();

            // Create folders
            var folderId = await _api.CreateFolder(projectId, "myFolder");
            await _api.CreateFolder(projectId, "mySubFolder", String.Empty, folderId);

            // Update base rate regulation on project
            await _api.UpdateFolderBaseSupplements(projectId, ProjectFolderRoot.RootFolderId.Value, 19, Overwrite, 21, Overwrite);

            // Assert all folders have correct base rate regulation
            var folderRoot = await _api.GetFolderRoot(projectId);

            var rootFolderRateAndSupplements = folderRoot.RootFolder.BaseRateAndSupplements;
            Assert.Equal(19, rootFolderRateAndSupplements.IndirectTimeSupplementPercentage.Value);
            Assert.Equal(BaseRateAndSupplementsValueStatus.Overwrite, rootFolderRateAndSupplements.IndirectTimeSupplementPercentage.ValueStatus);
            Assert.Equal(21, rootFolderRateAndSupplements.SiteSpecificTimeSupplementPercentage.Value);
            Assert.Equal(BaseRateAndSupplementsValueStatus.Overwrite, rootFolderRateAndSupplements.SiteSpecificTimeSupplementPercentage.ValueStatus);

            var subFolderRateAndSupplements = folderRoot.RootFolder.SubFolders[0].BaseRateAndSupplements;
            Assert.Equal(19, subFolderRateAndSupplements.IndirectTimeSupplementPercentage.Value);
            Assert.Equal(BaseRateAndSupplementsValueStatus.Inherit, subFolderRateAndSupplements.IndirectTimeSupplementPercentage.ValueStatus);
            Assert.Equal(21, subFolderRateAndSupplements.SiteSpecificTimeSupplementPercentage.Value);
            Assert.Equal(BaseRateAndSupplementsValueStatus.Inherit, subFolderRateAndSupplements.SiteSpecificTimeSupplementPercentage.ValueStatus);
        }

        [Fact]
        public async Task GivenUser_WhenCreatingProject_SystemBaseBaseRateAndSupplementsAreUsed()
        {
            // Create a project
            var projectId = await _api.CreateProject();
            
            // Assert root folder has correct base rate regulation
            var folderRoot = await _api.GetFolderRoot(projectId);

            var rootFolderRateAndSupplements = folderRoot.RootFolder.BaseRateAndSupplements;
            Assert.Equal(0, rootFolderRateAndSupplements.BaseRateRegulationPercentage.Value);
            Assert.Equal(BaseRateAndSupplementsValueStatus.Overwrite, rootFolderRateAndSupplements.BaseRateRegulationPercentage.ValueStatus);
            Assert.Equal(64, rootFolderRateAndSupplements.IndirectTimeSupplementPercentage.Value);
            Assert.Equal(BaseRateAndSupplementsValueStatus.Overwrite, rootFolderRateAndSupplements.IndirectTimeSupplementPercentage.ValueStatus);
            Assert.Equal(2, rootFolderRateAndSupplements.SiteSpecificTimeSupplementPercentage.Value);
            Assert.Equal(BaseRateAndSupplementsValueStatus.Overwrite, rootFolderRateAndSupplements.SiteSpecificTimeSupplementPercentage.ValueStatus);
        }

        [Fact]
        public async Task GivenProject_WhenCopyingFolder_CorrectBaseAndSupplementIsUsedInCopy()
        {
            // Import operations from catalog
            await CatalogTestData.ImportMaterials(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create folders
            // Root (default: 64%, 2%, 0%)
            //    |- sourceFolder
            //       |- sourceSubFolder (overwrite: 0%, 0%, 0%)
            //    |- destination (overwrite: 5%, 5%, 5%)
            var sourceFolderId = await _api.CreateFolder(projectId, "sourceFolder");
            var sourceSubFolderId = await _api.CreateFolder(projectId, "sourceSubFolder", string.Empty, sourceFolderId);
            var destinationFolderId = await _api.CreateFolder(projectId, "destination");

            // Overwrite all values on project
            await _api.UpdateFolderBaseRateRegulation(projectId, sourceSubFolderId, 0, BaseRateStatusUpdate.Overwrite);
            await _api.UpdateFolderBaseSupplements(projectId, sourceSubFolderId, 0, Overwrite, 0, Overwrite);
            await _api.UpdateFolderBaseRateRegulation(projectId, destinationFolderId, 5, BaseRateStatusUpdate.Overwrite);
            await _api.UpdateFolderBaseSupplements(projectId, destinationFolderId, 5, Overwrite, 5, Overwrite);
            
            // Register work items
            await _api.RegisterWorkItemMaterial(projectId, sourceFolderId, CatalogTestData.MaterialWithReplacementId, 80, 5,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            await _api.RegisterWorkItemMaterial(projectId, sourceSubFolderId, CatalogTestData.MaterialWithReplacementId, 80, 5,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            // Copy sourceFolder to destinationFolder
            await _api.CopyFolder(projectId, sourceFolderId, destinationFolderId);

            // Assert work item copies are using new base rate regulation and supplements
            var folderRoot = await _api.GetFolderRoot(projectId);
            var copiedFolderId = folderRoot.RootFolder.SubFolders.Single(f => f.ProjectFolderId == destinationFolderId).SubFolders[0].ProjectFolderId;
            var copiedSubFolderId = folderRoot.RootFolder.SubFolders.Single(f => f.ProjectFolderId == destinationFolderId).SubFolders[0].SubFolders[0]
                .ProjectFolderId;

            var copiedFolderWorkItems = await _api.GetWorkItems(projectId, copiedFolderId);
            Assert.Single(copiedFolderWorkItems.WorkItems);
            Assert.Equal(45.53m, copiedFolderWorkItems.WorkItems[0].WorkItemTotalPaymentDkr, 2);

            var copiedSubFolderWorkItems = await _api.GetWorkItems(projectId, copiedSubFolderId);
            Assert.Single(copiedSubFolderWorkItems.WorkItems);
            Assert.Equal(39.52m, copiedSubFolderWorkItems.WorkItems[0].WorkItemTotalPaymentDkr, 2);
        }
        
        [Fact]
        public async Task GivenProject_WhenMovingFolder_CorrectBaseAndSupplementIsUsedAfterwards()
        {
            // Import operations from catalog
            await CatalogTestData.ImportMaterials(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create folders
            // Root (default: 64%, 2%, 0%)
            //    |- sourceFolder
            //       |- sourceSubFolder (overwrite: 0%, 0%, 0%)
            //    |- destination (overwrite: 5%, 5%, 5%)
            var sourceFolderId = await _api.CreateFolder(projectId, "sourceFolder");
            var sourceSubFolderId = await _api.CreateFolder(projectId, "sourceSubFolder", string.Empty, sourceFolderId);
            var destinationFolderId = await _api.CreateFolder(projectId, "destination");

            // Overwrite all values on project
            await _api.UpdateFolderBaseRateRegulation(projectId, sourceSubFolderId, 0, BaseRateStatusUpdate.Overwrite);
            await _api.UpdateFolderBaseSupplements(projectId, sourceSubFolderId, 0, Overwrite, 0, Overwrite);
            await _api.UpdateFolderBaseRateRegulation(projectId, destinationFolderId, 5, BaseRateStatusUpdate.Overwrite);
            await _api.UpdateFolderBaseSupplements(projectId, destinationFolderId, 5, Overwrite, 5, Overwrite);
            
            // Register work items
            await _api.RegisterWorkItemMaterial(projectId, sourceFolderId, CatalogTestData.MaterialWithReplacementId, 80, 5,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            await _api.RegisterWorkItemMaterial(projectId, sourceSubFolderId, CatalogTestData.MaterialWithReplacementId, 80, 5,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            // Copy sourceFolder to destinationFolder
            await _api.MoveFolder(projectId, sourceFolderId, destinationFolderId);

            // Assert work item copies are using new base rate regulation and supplements
            var movedFolderWorkItems = await _api.GetWorkItems(projectId, sourceFolderId);
            Assert.Single(movedFolderWorkItems.WorkItems);
            Assert.Equal(45.53m, movedFolderWorkItems.WorkItems[0].WorkItemTotalPaymentDkr, 2);

            var movedSubFolderWorkItems = await _api.GetWorkItems(projectId, sourceSubFolderId);
            Assert.Single(movedSubFolderWorkItems.WorkItems);
            Assert.Equal(39.52m, movedSubFolderWorkItems.WorkItems[0].WorkItemTotalPaymentDkr, 2);
        }
        
        [Fact]
        public async Task GivenProject_WhenResetting_CorrectBaseAndSupplementIsUsedInCopy()
        {
            // Import operations from catalog
            await CatalogTestData.ImportMaterials(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Create folders
            // Root (default: 64%, 2%, 0%)
            //    |- folder
            var folderId = await _api.CreateFolder(projectId, "folder");
            
            // Overwrite all values on project
            await _api.UpdateFolderBaseRateRegulation(projectId, folderId, 0, BaseRateStatusUpdate.Overwrite);
            await _api.UpdateFolderBaseSupplements(projectId, folderId, 0, Overwrite, 0, Overwrite);
            
            // Register work items
            await _api.RegisterWorkItemMaterial(projectId, folderId, CatalogTestData.MaterialWithReplacementId, 80, 5,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            
            // Resetting all values on project
            await _api.UpdateFolderBaseRateRegulation(projectId, folderId, 0, BaseRateStatusUpdate.Inherit);
            await _api.UpdateFolderBaseSupplements(projectId, folderId, 0, Inherit, 0, Inherit);

            // Assert work item copies are using new base rate regulation and supplements
            var workItems = await _api.GetWorkItems(projectId, folderId);
            Assert.Single(workItems.WorkItems);
            Assert.Equal(65.56m, workItems.WorkItems[0].WorkItemTotalPaymentDkr, 2);
        }
        
        [Fact]
        public async Task GivenProject_WhenSettingBaseSupplements_ShouldGetCorrectCombinedSupplementPercentage()
        {
            // Create a project
            var projectId = await _api.CreateProject();

            // Create folders
            var folderAId = await _api.CreateFolder(projectId, "folderA");
            var folderBId = await _api.CreateFolder(projectId, "folderB");
            var folderCId = await _api.CreateFolder(projectId, "folderC");

            // Overwrite base rate regulation and supplements
            await _api.UpdateFolderBaseSupplements(projectId, folderAId, 0, Overwrite, 0, Overwrite);
            await _api.UpdateFolderBaseSupplements(projectId, folderBId, 6, Overwrite, 7, Overwrite);
            await _api.UpdateFolderBaseSupplements(projectId, folderCId, 100, Overwrite, 100, Overwrite);
            
            // Get folder root
            var folderRoot = await _api.GetFolderRoot(projectId);
            
            // Assert combined supplement
            var folderARates = folderRoot.RootFolder.SubFolders.Single(f => f.ProjectFolderId == folderAId).BaseRateAndSupplements;
            var folderBRates = folderRoot.RootFolder.SubFolders.Single(f => f.ProjectFolderId == folderBId).BaseRateAndSupplements;
            var folderCRates = folderRoot.RootFolder.SubFolders.Single(f => f.ProjectFolderId == folderCId).BaseRateAndSupplements;
            Assert.Equal(6.00m, folderARates.CombinedSupplementPercentage, 2);
            Assert.Equal(19.36m, folderBRates.CombinedSupplementPercentage, 2);
            Assert.Equal(212.00m, folderCRates.CombinedSupplementPercentage, 2);
        }
    }
}
