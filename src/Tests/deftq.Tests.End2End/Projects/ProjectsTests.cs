using System.Net;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectNameAndOrderNumber;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectType;
using deftq.BuildingBlocks.Serialization;
using deftq.BuildingBlocks.Validation.Webapi.Errors;
using deftq.Pieceworks.Application.CreateProject;
using deftq.Pieceworks.Application.GetProject;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectUser;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectType.UpdateProjectTypeRequest;

namespace deftq.Tests.End2End.Projects
{
    [Collection("End2End")]
    public class ProjectsTests
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;
        
        public ProjectsTests(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task Should_Find_Created_Project_And_ProjectUser_Owns_Project()
        {
            // Create a project
            var projectId = await _api.CreateProject("myProject", "This is my project");

            // Get the created project
            var projectResponse = await _api.GetProject(projectId);
            projectResponse.Id.Should().Be(projectId);
            projectResponse.Title.Should().Be("myProject");
            projectResponse.Description.Should().Be("This is my project");
            projectResponse.CurrentUserRole.Should().Be(ProjectRole.ProjectOwner);

            // Get the project user and verify that he owns the created project
            using var scope = _fixture.AppFactory.Services.CreateAsyncScope();
            var projectUserRepository = scope.ServiceProvider.GetRequiredService<IProjectUserRepository>();
            var projectUser = await projectUserRepository.GetById(_fixture.UserId);
            projectUser.Owns(ProjectId.Create(projectId)).Should().BeTrue();
        }

        [Fact]
        public async Task Should_Update_Project_Lump_Sum()
        {
            // Create a project
            var projectId = await _api.CreateProject(Guid.NewGuid().ToString());
            
            // Update lump sum
            await _api.UpdateProjectLumpSum(projectId, 667.95m);
            
            // Assert lump sum was updated
            var projectResponse = await _api.GetProjectSummation(projectId);
            Assert.Equal(667.95m, projectResponse.TotalLumpSumDkr);
        }

        [Fact]
        public async Task Should_Update_Project_Information()
        {
            var projectId = await _api.CreateProject(Guid.NewGuid().ToString());

            await _api.UpdateProjectInformation(projectId, new UpdateProjectInformationRequest("testName", "testDescription", "10000", "200000"));

            var projectResponse = await _api.GetProject(projectId);
            
            Assert.Equal("testName", projectResponse.Title);
            Assert.Equal("testDescription", projectResponse.Description);
            Assert.Equal("10000", projectResponse.OrderNumber);
            Assert.Equal("200000", projectResponse.PieceWorkNumber);
        }

        [Fact]
        public async Task Should_Update_Project_type()
        {
            var projectId = await _api.CreateProject(Guid.NewGuid().ToString());

            await _api.UpdateProjectType(projectId, new UpdateProjectTypeRequest(UpdateProjectPieceworkType.TwelveOneB, 100, DateOnly.MaxValue, DateOnly.MaxValue));

            var projectResponse = await _api.GetProject(projectId);
            
            Assert.Equal(PieceworkType.TwelveOneB, projectResponse.PieceworkType);
        }

        [Fact]
        public async Task Should_Remove_Project()
        {
            // Create a project
            var projectId = await _api.CreateProject(Guid.NewGuid().ToString());
            
            //Get the project
            var projectResponse = await _api.GetProject(projectId);
            Assert.Equal(projectId, projectResponse.Id);
            
            //Remove project
            await _api.RemoveProject(projectId);
            
            // Assert project is removed
            var response = await _fixture.Client.GetAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [Fact]
        public async Task Returns_NotFound_As_Error()
        {
            var id = Guid.NewGuid();
            var url = $"/api/projects/{id}";
            var response = await _fixture.Client.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var error = await response.Content.ReadAsStringAsync();
            var serializer = new JsonSerializer<Error>();
            var errorResponse = serializer.Deserialize(error);
            Assert.Equal(404, errorResponse?.Status);
            Assert.Equal(url, errorResponse?.Instance);
        }

        [Fact]
        public async Task Should_Create_With_Project_Lump_Sum()
        {
            // Create a project
            var projectId = await _api.CreateProject(pieceworkSum: 112.75m);
            
            // Assert lump sum was updated
            var projectResponse = await _api.GetProjectSummation(projectId);
            Assert.Equal(112.75m, projectResponse.TotalLumpSumDkr);
        }
        
        [Fact]
        public async Task GivenProject_WhenUpdatingCompany_CompanyIsReturned()
        {
            var projectId = await _api.CreateProject(Guid.NewGuid().ToString());
            await _api.UpdateProjectCompany(projectId, "Acme Inc.", "Nowhere", "01234567", "0123456789");
            
            var projectResponse = await _api.GetProject(projectId);
            Assert.Equal("Acme Inc.", projectResponse.CompanyName);
            Assert.Equal("Nowhere", projectResponse.CompanyAddress);
            Assert.Equal("01234567", projectResponse.CompanyCvrNo);
            Assert.Equal("0123456789", projectResponse.CompanyPNo);
        }
        
        [Fact]
        public async Task GivenProject_WhenUpdatingInformation_InformationIsReturned()
        {
            var projectId = await _api.CreateProject(Guid.NewGuid().ToString());
            await _api.UpdateProjectInformation(projectId, new UpdateProjectInformationRequest("newProject", "no news", "123", "321"));
            
            var projectResponse = await _api.GetProject(projectId);
            Assert.Equal("newProject", projectResponse.Title);
            Assert.Equal("no news", projectResponse.Description);
            Assert.Equal("123", projectResponse.OrderNumber);
            Assert.Equal("321", projectResponse.PieceWorkNumber);
        }
        
        [Fact]
        public async Task Project_Number_Is_Unique_And_Ascending()
        {
            // Create two projects
            var project1Id = await _api.CreateProject();
            var project2Id = await _api.CreateProject();
            
            // Get the projects
            var project1Response = await _api.GetProject(project1Id);
            var project2Response = await _api.GetProject(project2Id);
            
            // Assert project number is unique and ascending
            Assert.True(project1Response.ProjectNumber < project2Response.ProjectNumber);
        }
    }
}
