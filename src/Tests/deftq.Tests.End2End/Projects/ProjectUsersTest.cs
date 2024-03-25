using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectParticipant;
using deftq.Pieceworks.Application.GetProjectUsers;
using Xunit;

namespace deftq.Tests.End2End.Projects
{
    [Collection("End2End")]
    public class ProjectUsersTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public ProjectUsersTest(WebAppFixture fixture)
        {
            _fixture = fixture;
            _api = new Api(fixture);
        }
        
        [Fact]
        public async Task CreateProjectOnlyGetOwner()
        {
            // Create a project
            var projectId = await _api.CreateProject("myProject", "Save participant on project");
            var projectUsersReponse = await _api.GetProjectUsers(projectId);
            var users = projectUsersReponse.Users;
            Assert.Equal(1, users.Count);
            Assert.Equal(ProjectUserRole.Owner, users[0].Role);
            Assert.Equal("test", users[0].Name);
            Assert.Equal("", users[0].Email);
            Assert.Null(users[0].Address);
            Assert.Null(users[0].Phone);
            
        }

        [Fact]
        public async Task SaveParticipantOnProjectGetOwnerAndParticipant()
        {
            const string name = "Testersen Elsen";
            const string email = "tester@elsen.dk";
            
            // Create a project
            var projectId = await _api.CreateProject("myProject", "Save participant on project");
            await _api.RegisterUser(projectId, name, UserRole.ProjectParticipant, email, null, null);
            var projectUsersReponse = await _api.GetProjectUsers(projectId);
            var users = projectUsersReponse.Users;
            var participant = users.First(u => u.Role == ProjectUserRole.Participant);
            Assert.Equal(2, users.Count);
            Assert.Equal(name, participant.Name);
            Assert.Equal(email, participant.Email);
            Assert.Equal("", participant.Address);
            Assert.Equal("", participant.Phone);
        }
        
        [Fact]
        public async Task SaveManagerOnProjectGetOwnerAndManager()
        {
            const string name = "Mr. Managerson";
            const string email = "manager@sonandson.dk";
            const string adr = "Man Ager 42";
            const string phone = "42434445";
            
            // Create a project
            var projectId = await _api.CreateProject("myProject", "Save manager on project");
            await _api.RegisterUser(projectId, name, UserRole.ProjectManager, email, adr, phone);
            var projectUsersReponse = await _api.GetProjectUsers(projectId);
            var users = projectUsersReponse.Users;
            var manager = users.First(u => u.Role == ProjectUserRole.Manager);
            Assert.Equal(2, users.Count);
            Assert.Equal(name, manager.Name);
            Assert.Equal(email, manager.Email);
            Assert.Equal(adr, manager.Address);
            Assert.Equal(phone, manager.Phone);
        }

        [Fact]
        public async Task SaveMultipleUsersOnProjectAndGetThemAll()
        {
            var projectId = await _api.CreateProject("myProject", "Save manager on project");
            await _api.RegisterUser(projectId, "manager 1", UserRole.ProjectManager, "m1@ager.dk", "Man Ager 42", "21212121");
            await _api.RegisterUser(projectId, "manager 2", UserRole.ProjectManager, "m2@ager.dk", "Man Ager 43", "22222222");
            await _api.RegisterUser(projectId, "elsen 1", UserRole.ProjectParticipant, "el1@strom.dk", "Elsenvej 41", "41414141");
            await _api.RegisterUser(projectId, "elsen 2", UserRole.ProjectParticipant, "el2@strom.dk", "Elsenvej 42", "42424242");
            await _api.RegisterUser(projectId, "elsen 3", UserRole.ProjectParticipant, "el3@strom.dk", "Elsenvej 43", "43434343");
            
            var projectUsersReponse = await _api.GetProjectUsers(projectId);
            var users = projectUsersReponse.Users;
            var owners = users.Where(u => u.Role == ProjectUserRole.Owner);
            var participants = users.Where(u => u.Role == ProjectUserRole.Participant);
            var managers = users.Where(u => u.Role == ProjectUserRole.Manager);
            
            Assert.Single(owners);
            Assert.Equal(3, participants.Count());
            Assert.Equal(2, managers.Count());
        }
    }
}
