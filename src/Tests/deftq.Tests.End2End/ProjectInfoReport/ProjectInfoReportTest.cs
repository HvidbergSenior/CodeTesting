using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectParticipant;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemMaterial;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemOperation;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectNameAndOrderNumber;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectType;
using deftq.Pieceworks.Application.CreateProject;
using deftq.Pieceworks.Application.GetProjectUsers;
using deftq.Tests.End2End.CatalogTest;
using FluentAssertions;
using Xunit;

namespace deftq.Tests.End2End.ProjectInfoReport
{
    [Collection("End2End")]
    public class ProjectInfoReportTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;
        private readonly Guid RootFolderId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        public ProjectInfoReportTest(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task Should_get_a_simple_project_info_report_type_12_2_just_created()
        {
            var projectId = await _api.CreateProject("Test Project", "En god beskrivelse", PieceworkType.TwelveTwo, 123456789);

            var rsp = await _api.GetProjectInfoReport(projectId);
            rsp.ExtraWorkAgreementsRates.Should().NotBeNull();
            rsp.GroupedWorkitems.Should().NotBeNull();
            rsp.GroupedWorkitems.Should().HaveCount(0);
            rsp.Users.Should().NotBeNull();
            rsp.Users.Should().HaveCount(1);
            rsp.Project.Should().NotBeNull();
            rsp.ProjectSummation.Should().NotBeNull();
            rsp.RootFolder.Should().NotBeNull();
            rsp.RootFolder.ProjectFolderId.Should().Be(RootFolderId);
        }

        [Fact]
        public async Task Should_get_a_simple_project_info_report_type_12a_project_info_updated()
        {
            var projectId = await _api.CreateProject();
            await _api.UpdateProjectInformation(projectId,
                new UpdateProjectInformationRequest("Dette er mit navn", "En eller andet beskrivelse", "123456", "0987654321"));
            await _api.UpdateProjectType(projectId,
                new UpdateProjectTypeRequest(UpdateProjectTypeRequest.UpdateProjectPieceworkType.TwelveTwo, 123456, DateOnly.MaxValue,
                    DateOnly.MaxValue));
            await _api.UpdateProjectCompany(projectId, "Mjølner Informatics", "Finlandsgade 10, Århus", "12345678", "1234567890");

            var rsp = await _api.GetProjectInfoReport(projectId);
            rsp.ExtraWorkAgreementsRates.Should().NotBeNull();
            rsp.GroupedWorkitems.Should().NotBeNull();
            rsp.GroupedWorkitems.Should().HaveCount(0);
            rsp.Users.Should().NotBeNull();
            rsp.Users.Should().HaveCount(1);
            rsp.Project.Should().NotBeNull();
            rsp.ProjectSummation.Should().NotBeNull();
            rsp.RootFolder.Should().NotBeNull();
            rsp.RootFolder.ProjectFolderId.Should().Be(RootFolderId);
        }

        [Fact]
        public async Task Should_get_a_simple_project_info_report_added_work_items()
        {
            // Import operations and materials from catalog
            await CatalogTestData.ImportOperations(_fixture);
            await CatalogTestData.ImportMaterials(_fixture);
            
            var projectId = await _api.CreateProject();
            await _api.RegisterWorkItemOperation(projectId, RootFolderId, CatalogTestData.RemoveCoverOperationId, 3,
                new List<OperationSupplementRequest>());
            await _api.RegisterWorkItemMaterial(projectId, RootFolderId, CatalogTestData.MaterialWithReplacementId, 80, 10,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());
            await _api.RegisterWorkItemMaterial(projectId, RootFolderId, CatalogTestData.TripleCableMaterialId, 3, 1,
                new List<MaterialSupplementOperationRequest>(), new List<MaterialSupplementRequest>());

            var rsp = await _api.GetProjectInfoReport(projectId);
            rsp.ExtraWorkAgreementsRates.Should().NotBeNull();
            rsp.GroupedWorkitems.Should().NotBeNull();
            rsp.GroupedWorkitems.Should().HaveCount(3);
            rsp.Users.Should().NotBeNull();
            rsp.Users.Should().HaveCount(1);
            rsp.Project.Should().NotBeNull();
            rsp.ProjectSummation.Should().NotBeNull();
            rsp.RootFolder.Should().NotBeNull();
            rsp.RootFolder.ProjectFolderId.Should().Be(RootFolderId);
        }
        
        [Fact]
        public async Task Should_get_a_simple_project_info_report_added_users()
        {
            var projectId = await _api.CreateProject();
            await _api.RegisterUser(projectId, "Hr Elsen", UserRole.ProjectParticipant, "el@sen.dk", "Elvej 42", "21222324");
            await _api.RegisterUser(projectId, "Man Agersen", UserRole.ProjectManager, "man@zen.dk", "Man Ager 42", "42322212");
            await _api.RegisterUser(projectId, "Hr Testsen", UserRole.ProjectParticipant, "test@sen.dk", "Elvej 24", "42424242");
            
            var rsp = await _api.GetProjectInfoReport(projectId);
            rsp.Users.Should().HaveCount(4);
            var participants = rsp.Users.Where(u => u.Role == ProjectUserRole.Participant);
            var managers = rsp.Users.Where(u => u.Role == ProjectUserRole.Manager);
            participants.Should().HaveCount(2);
            managers.Should().HaveCount(1);
        }
    }
}
