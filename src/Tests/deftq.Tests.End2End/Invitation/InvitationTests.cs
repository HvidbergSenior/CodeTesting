using System.Net;
using deftq.BuildingBlocks.Exceptions;
using FluentAssertions;
using Xunit;

namespace deftq.Tests.End2End.Invitation
{
    [Collection("End2End")]
    public class InvitationTests
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;
        
        public InvitationTests(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task Should_create_invitation_fully_valid()
        {
            var email = "me@me.dk";
            var projectId = await _api.CreateProject("MyProject", "Awesome Project");
            var invitationReponse = await _api.CreateProjectInvitation(projectId, email);
            invitationReponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task Should_throw_exception_for_invalid_email()
        {
            var email = "meme.dk";
            var projectId = await _api.CreateProject("MyProject", "Awesome Project");
            
            var response = await _api.CreateProjectInvitation(projectId, email);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
        [Fact]
        public async Task Should_throw_exception_for_invalid_project_id()
        {
            var email = "me@me.dk";
            var projectId = Guid.NewGuid();
            
            var response = await _api.CreateProjectInvitation(projectId, email);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
