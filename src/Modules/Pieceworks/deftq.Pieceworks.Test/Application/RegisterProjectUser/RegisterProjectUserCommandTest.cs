using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterProjectUser;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RegisterProjectUser
{
    public class RegisterProjectUserCommandTest
    {
        private readonly IUnitOfWork _uow;
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public RegisterProjectUserCommandTest()
        {
            _uow = new FakeUnitOfWork();
            _projectRepository = new ProjectInMemoryRepository();
            _executionContext = new FakeExecutionContext();
        }

        [Fact]
        public async Task WhenSaveParticipant_ShouldSaveOnProject()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectRepository.Add(project);

            var participant = Any.ProjectParticipant("Tester Elsen", "tester@elsen.dk", "Eltestergade 42, 4200", "42424242");

            var cmd = RegisterProjectUserCommand.Create(project.ProjectId.Value, participant);
            var handler = new RegisterProjectUserCommandHandler(_projectRepository, _uow);
            
            await handler.Handle(cmd, CancellationToken.None);

            var updatedProject = await _projectRepository.GetById(project.ProjectId.Value);
            
            Assert.Equal(0, updatedProject.ProjectManagers.Count);
            Assert.Equal(1, updatedProject.ProjectParticipants.Count);

            var savedParticipant = updatedProject.ProjectParticipants[0];
            Assert.Equal(participant.Name, savedParticipant.Name);
            Assert.Equal(participant.Email.Value, savedParticipant.Email.Value);
            Assert.Equal(participant.Address, savedParticipant.Address);
            Assert.Equal(participant.PhoneNumber?.Value, savedParticipant.PhoneNumber?.Value);
        }
        
        [Fact]
        public async Task WhenSaveMultibleParticipants_ShouldSaveOnProject()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectRepository.Add(project);

            var participant1 = Any.ProjectParticipant(ProjectParticipantName.Create("Elsen 1"));
            var participant2 = Any.ProjectParticipant(ProjectParticipantName.Create("Elsen 2"));
            var participant3 = Any.ProjectParticipant(ProjectParticipantName.Create("Elsen 3"));

            var cmd1 = RegisterProjectUserCommand.Create(project.ProjectId.Value, participant1);
            var cmd2 = RegisterProjectUserCommand.Create(project.ProjectId.Value, participant2);
            var cmd3 = RegisterProjectUserCommand.Create(project.ProjectId.Value, participant3);
            
            var handler = new RegisterProjectUserCommandHandler(_projectRepository, _uow);
            
            await handler.Handle(cmd1, CancellationToken.None);
            await handler.Handle(cmd2, CancellationToken.None);
            await handler.Handle(cmd3, CancellationToken.None);

            var updatedProject = await _projectRepository.GetById(project.ProjectId.Value);
            
            Assert.Equal(0, updatedProject.ProjectManagers.Count);
            Assert.Equal(3, updatedProject.ProjectParticipants.Count);

            var savedParticipant1 = updatedProject.ProjectParticipants[0];
            var savedParticipant2 = updatedProject.ProjectParticipants[1];
            var savedParticipant3 = updatedProject.ProjectParticipants[2];
            Assert.Equal(participant1.Name, savedParticipant1.Name);
            Assert.Equal(participant2.Name, savedParticipant2.Name);
            Assert.Equal(participant3.Name, savedParticipant3.Name);
        }
        
        [Fact]
        public async Task WhenSaveManager_ShouldSaveOnProject()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectRepository.Add(project);

            var manager = Any.ProjectManager("Manage This", "manager@manga.dk", "Mangervej 24, 4200", "24242424");

            var cmd = RegisterProjectUserCommand.Create(project.ProjectId.Value, manager);
            var handler = new RegisterProjectUserCommandHandler(_projectRepository, _uow);
            
            await handler.Handle(cmd, CancellationToken.None);

            var updatedProject = await _projectRepository.GetById(project.ProjectId.Value);
            
            Assert.Equal(1, updatedProject.ProjectManagers.Count);
            Assert.Equal(0, updatedProject.ProjectParticipants.Count);
            
            var savedManager = updatedProject.ProjectManagers[0];
            Assert.Equal(manager.Name, savedManager.Name);
            Assert.Equal(manager.Email.Value, savedManager.Email.Value);
            Assert.Equal(manager.Address, savedManager.Address);
            Assert.Equal(manager.Phone, savedManager.Phone);
        }
        
        [Fact]
        public async Task WhenSaveMultibleManagers_ShouldSaveOnProject()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectRepository.Add(project);

            var manager1 = Any.ProjectParticipant(ProjectParticipantName.Create("Manage 1"));
            var manager2 = Any.ProjectParticipant(ProjectParticipantName.Create("Manage 2"));
            var manager3 = Any.ProjectParticipant(ProjectParticipantName.Create("Manage 3"));

            var cmd1 = RegisterProjectUserCommand.Create(project.ProjectId.Value, manager1);
            var cmd2 = RegisterProjectUserCommand.Create(project.ProjectId.Value, manager2);
            var cmd3 = RegisterProjectUserCommand.Create(project.ProjectId.Value, manager3);
            
            var handler = new RegisterProjectUserCommandHandler(_projectRepository, _uow);
            
            await handler.Handle(cmd1, CancellationToken.None);
            await handler.Handle(cmd2, CancellationToken.None);
            await handler.Handle(cmd3, CancellationToken.None);

            var updatedProject = await _projectRepository.GetById(project.ProjectId.Value);
            
            Assert.Equal(0, updatedProject.ProjectManagers.Count);
            Assert.Equal(3, updatedProject.ProjectParticipants.Count);

            var savedManager1 = updatedProject.ProjectParticipants[0];
            var savedManager2 = updatedProject.ProjectParticipants[1];
            var savedManager3 = updatedProject.ProjectParticipants[2];
            Assert.Equal(manager1.Name, savedManager1.Name);
            Assert.Equal(manager2.Name, savedManager2.Name);
            Assert.Equal(manager3.Name, savedManager3.Name);
        }
        
        [Fact]
        public async Task WhenSaveManagerAndParticipant_ShouldSaveOnProject()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectRepository.Add(project);

            var manager = Any.ProjectManager("Manage This", "manager@manga.dk", "Mangervej 24, 4200", "24242424");
            var cmdM = RegisterProjectUserCommand.Create(project.ProjectId.Value, manager);
            
            var participant = Any.ProjectParticipant("Tester Elsen", "tester@elsen.dk", "Eltestergade 42, 4200", "42424242");
            var cmdP = RegisterProjectUserCommand.Create(project.ProjectId.Value, participant);
            
            var handler = new RegisterProjectUserCommandHandler(_projectRepository, _uow);
            
            await handler.Handle(cmdM, CancellationToken.None);
            await handler.Handle(cmdP, CancellationToken.None);

            var updatedProject = await _projectRepository.GetById(project.ProjectId.Value);
            
            Assert.Equal(1, updatedProject.ProjectManagers.Count);
            Assert.Equal(1, updatedProject.ProjectParticipants.Count);
        }
    }
}
