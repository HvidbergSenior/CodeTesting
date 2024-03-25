using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CloseProjectLogBookWeek;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectLogBookWeek;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemMaterial;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderExtraWork;
using deftq.Tests.End2End.CatalogTest;
using Xunit;

namespace deftq.Tests.End2End.Projects
{
    [Collection("End2End")]
    public class ProjectSummationTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public ProjectSummationTest(WebAppFixture fixture)
        {
            _fixture = fixture;
            _api = new Api(fixture);
        }

        [Fact]
        public async Task GetProjectSummation()
        {
            // Import materials from catalog
            await CatalogTestData.ImportMaterials(_fixture);
            
            // Create a project
            var projectId = await _api.CreateProject("myProject", "This is my project");
            
            // Register work
            var folderId = await _api.CreateFolder(projectId, "myFolder");
            await _api.RegisterWorkItemMaterial(projectId, folderId, CatalogTestData.MaterialWithReplacementId, 80, 5,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            // Register extra work
            var extraWorkFolderId = await _api.CreateFolder(projectId, "myExtraWorkFolder");
            await _api.UpdateFolderExtraWork(projectId, extraWorkFolderId, UpdateProjectFolderExtraWorkRequest.ExtraWorkUpdate.ExtraWork);
            await _api.RegisterWorkItemMaterial(projectId, extraWorkFolderId, CatalogTestData.MaterialWithReplacementId, 80, 9,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            // Register week in logbook
            var daysRequest = new List<RegisterProjectLogBookDay> { new(new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.Zero), 7, 30) };
            var weekRequest = new RegisterProjectLogBookWeekRequest(_fixture.UserId, 2023, 1, String.Empty, daysRequest);
            await _api.RegisterLogBookWeek(projectId, weekRequest);
            await _api.CloseLogBookWeek(projectId, new CloseProjectLogBookWeekRequest(_fixture.UserId, 2023, 1));

            // Register extra work agreement
            await _api.RegisterExtraWorkAgreementWithPayment(projectId, "1", "1", String.Empty, 124.95m);
            
            // Get project summation
            var projectSummation = await _api.GetProjectSummation(projectId);
            
            Assert.Equal(308.51m,projectSummation.TotalPaymentDkr, 2);
            Assert.Equal(0,projectSummation.TotalCalculationSumDkr, 2);
            Assert.Equal(7.5m,projectSummation.TotalLogBookHours, 2);
            Assert.Equal(0,projectSummation.TotalLumpSumDkr, 2);
            Assert.Equal(124.95m ,projectSummation.TotalExtraWorkAgreementDkr, 2);
            Assert.Equal(183.56m,projectSummation.TotalWorkItemPaymentDkr, 2);
            Assert.Equal(118.01m,projectSummation.TotalWorkItemExtraWorkPaymentDkr, 2);
        }
    }
}
