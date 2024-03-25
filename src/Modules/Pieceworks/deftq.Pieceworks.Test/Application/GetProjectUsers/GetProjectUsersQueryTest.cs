using deftq.Pieceworks.Application.GetProjectUsers;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using ProjectUserRole = deftq.Pieceworks.Application.GetProjectUsers.ProjectUserRole;

namespace deftq.Pieceworks.Test.Application.GetProjectUsers
{
    public class GetProjectUsersQueryTest
    {
        private readonly IProjectRepository _projectInMemoryRepository;

        public GetProjectUsersQueryTest()
        {
            _projectInMemoryRepository = new ProjectInMemoryRepository();
        }

        [Fact]
        public async Task WhenNoUsersIsSavedOnProject_ShouldReturnOnlyOwner()
        {
            var project = Any.Project();
            await _projectInMemoryRepository.Add(project);

            var query = GetProjectUsersQuery.Create(project.ProjectId);
            var handler = new GetProjectUsersQueryHandler(_projectInMemoryRepository);

            var projectUsersQueryResponse = await handler.Handle(query, CancellationToken.None);
            
            Assert.Equal(1, projectUsersQueryResponse.Users.Count);
            Assert.Equal(ProjectUserRole.Owner, projectUsersQueryResponse.Users[0].Role);
            Assert.Equal(project.ProjectOwner.Id, projectUsersQueryResponse.Users[0].Id);
            Assert.Equal(project.ProjectOwner.Name, projectUsersQueryResponse.Users[0].Name);
            Assert.Equal("", projectUsersQueryResponse.Users[0].Email);
            Assert.Null(projectUsersQueryResponse.Users[0].Address);
            Assert.Null(projectUsersQueryResponse.Users[0].Phone);
        }
        
        [Fact]
        public async Task WhenParticipantIsSavedOnProject_ShouldReturnOwnerAndParticipant()
        {
            var project = Any.Project();
            var participant = Any.ProjectParticipant();
            project.AddProjectParticipant(participant);
            await _projectInMemoryRepository.Add(project);

            var query = GetProjectUsersQuery.Create(project.ProjectId);
            var handler = new GetProjectUsersQueryHandler(_projectInMemoryRepository);

            var projectUsersQueryResponse = await handler.Handle(query, CancellationToken.None);
            var savedOwner = projectUsersQueryResponse.Users.First(u => u.Role == ProjectUserRole.Owner);
            var savedParticpant = projectUsersQueryResponse.Users.First(u => u.Role == ProjectUserRole.Participant);
            Assert.Equal(2, projectUsersQueryResponse.Users.Count);
            Assert.Equal(project.ProjectOwner.Id, savedOwner.Id);
            Assert.Equal(participant.Id, savedParticpant.Id);
            Assert.Equal(participant.Name.Value, savedParticpant.Name);
            Assert.Equal(ProjectUserRole.Participant, savedParticpant.Role);
            Assert.Equal(participant.Email.Value, savedParticpant.Email);
            Assert.Equal(participant.Address?.Value, savedParticpant.Address);
            Assert.Equal(participant.PhoneNumber?.Value, savedParticpant.Phone);
        }
        
        [Fact]
        public async Task WhenManagerIsSavedOnProject_ShouldReturnOwnerAndManager()
        {
            var project = Any.Project();
            var manager = Any.ProjectManager();
            project.AddProjectManager(manager);
            await _projectInMemoryRepository.Add(project);

            var query = GetProjectUsersQuery.Create(project.ProjectId);
            var handler = new GetProjectUsersQueryHandler(_projectInMemoryRepository);

            var projectUsersQueryResponse = await handler.Handle(query, CancellationToken.None);
            var savedOwner = projectUsersQueryResponse.Users.First(u => u.Role == ProjectUserRole.Owner);
            var savedManager = projectUsersQueryResponse.Users.First(u => u.Role == ProjectUserRole.Manager);
            Assert.Equal(2, projectUsersQueryResponse.Users.Count);
            Assert.Equal(project.ProjectOwner.Id, savedOwner.Id);
            Assert.Equal(manager.Id, savedManager.Id);
            Assert.Equal(manager.Name, savedManager.Name);
            Assert.Equal(ProjectUserRole.Manager, savedManager.Role);
            Assert.Equal(manager.Email.Value, savedManager.Email);
            Assert.Equal(manager.Address, savedManager.Address);
            Assert.Equal(manager.Phone, savedManager.Phone);
        }
        
        [Fact]
        public async Task WhenMultipleUsersIsSavedOnProject_ShouldReturnThemAll()
        {
            var project = Any.Project();
            var participant1 = Any.ProjectParticipant();
            var participant2 = Any.ProjectParticipant();
            var participant3 = Any.ProjectParticipant();
            var manager1 = Any.ProjectManager();
            var manager2 = Any.ProjectManager();
            
            project.AddProjectManager(manager1);
            project.AddProjectParticipant(participant2);
            project.AddProjectParticipant(participant3);
            project.AddProjectManager(manager2);
            project.AddProjectParticipant(participant1);
            await _projectInMemoryRepository.Add(project);

            var query = GetProjectUsersQuery.Create(project.ProjectId);
            var handler = new GetProjectUsersQueryHandler(_projectInMemoryRepository);

            var projectUsersQueryResponse = await handler.Handle(query, CancellationToken.None);
            var savedOwners = projectUsersQueryResponse.Users.Where(u => u.Role == ProjectUserRole.Owner);
            var savedParticpants = projectUsersQueryResponse.Users.Where(u => u.Role == ProjectUserRole.Participant);
            var savedManagers = projectUsersQueryResponse.Users.Where(u => u.Role == ProjectUserRole.Manager);
            Assert.Equal(6, projectUsersQueryResponse.Users.Count);
            Assert.Single(savedOwners);
            Assert.Equal(3, savedParticpants.Count());
            Assert.Equal(2, savedManagers.Count());
        }
    }
}
