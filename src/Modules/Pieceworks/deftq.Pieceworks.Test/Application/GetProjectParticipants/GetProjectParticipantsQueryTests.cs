using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetProjectParticipants;
using deftq.Pieceworks.Domain.InvitationFlow;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using deftq.Pieceworks.Infrastructure.ProjectInvitation;
using FluentAssertions;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectParticipants
{
    public class GetProjectParticipantsQueryTests
    {
        private readonly InMemoryInvitationRepository _invitationRepository;
        public GetProjectParticipantsQueryTests()
        {
            _invitationRepository = new InMemoryInvitationRepository();
        }

        [Fact]
        public async Task Should_Return_Only_Owner()
        {
            var repository = new ProjectInMemoryRepository();
            var projectOwner = ProjectOwner.Create("John Doe", Guid.NewGuid());
            var project = Any.Project().OwnedBy(projectOwner);

            await repository.Add(project);

            var executionContext = new FakeExecutionContext(projectOwner.Id);
            var query = GetProjectParticipantsQuery.Create(project.ProjectId);
            var handler = new GetProjectParticipantsQueryHandler(repository, executionContext, _invitationRepository);

            var rsp = await handler.Handle(query, CancellationToken.None);

            rsp.Participants.Should().HaveCount(1);
            var participants = rsp.Participants.ToList().FindAll(p => p.Role == ProjectParticipantRole.ProjectParticipant);
            var owners = rsp.Participants.ToList().FindAll(p => p.Role == ProjectParticipantRole.ProjectOwner);
            var managers = rsp.Participants.ToList().FindAll(p => p.Role == ProjectParticipantRole.ProjectManager);
            participants.Count.Should().Be(0);
            owners.Count.Should().Be(1);
            managers.Count.Should().Be(0);
        }

        [Fact]
        public async Task Should_Return_2_Participants()
        {
            var repository = new ProjectInMemoryRepository();
            var projectOwner = ProjectOwner.Create("John Doe", Guid.NewGuid());
            var project = Any.Project().OwnedBy(projectOwner);

            var projectParticipant1 = Any.ProjectParticipant(ProjectParticipantName.Create("Fred Hanson"));
            project.AddProjectParticipant(projectParticipant1);
            var projectParticipant2 = Any.ProjectParticipant(ProjectParticipantName.Create("Sven Svenson"));
            project.AddProjectParticipant(projectParticipant2);

            await repository.Add(project);

            var executionContext = new FakeExecutionContext(projectOwner.Id);
            var query = GetProjectParticipantsQuery.Create(project.ProjectId);
            var handler = new GetProjectParticipantsQueryHandler(repository, executionContext, _invitationRepository);

            var rsp = await handler.Handle(query, CancellationToken.None);

            rsp.Participants.Should().HaveCount(3);
            var participants = rsp.Participants.ToList().FindAll(p => p.Role == ProjectParticipantRole.ProjectParticipant);
            var owners = rsp.Participants.ToList().FindAll(p => p.Role == ProjectParticipantRole.ProjectOwner);
            var managers = rsp.Participants.ToList().FindAll(p => p.Role == ProjectParticipantRole.ProjectManager);
            participants.Count.Should().Be(2);
            owners.Count.Should().Be(1);
            managers.Count.Should().Be(0);
        }

        [Fact]
        public async Task Should_Return_1_Manager()
        {
            var repository = new ProjectInMemoryRepository();
            var projectOwner = Any.ProjectOwner("John Doe");
            var project = Any.Project().OwnedBy(projectOwner);

            var manager = Any.ProjectManager("Fred Hanson");
            project.AddProjectManager(manager);

            await repository.Add(project);

            var query = CreateHandler(projectOwner, project, repository, _invitationRepository, out var handler);

            var rsp = await handler.Handle(query, CancellationToken.None);

            rsp.Participants.Should().HaveCount(2);
            var participants = rsp.Participants.ToList().FindAll(p => p.Role == ProjectParticipantRole.ProjectParticipant);
            var owners = rsp.Participants.ToList().FindAll(p => p.Role == ProjectParticipantRole.ProjectOwner);
            var managers = rsp.Participants.ToList().FindAll(p => p.Role == ProjectParticipantRole.ProjectManager);
            participants.Count.Should().Be(0);
            owners.Count.Should().Be(1);
            managers.Count.Should().Be(1);
        }

        private static GetProjectParticipantsQuery CreateHandler(ProjectOwner projectOwner, Project project, ProjectInMemoryRepository repository, InMemoryInvitationRepository repoInvitation,
            out GetProjectParticipantsQueryHandler handler)
        {
            var executionContext = new FakeExecutionContext(projectOwner.Id);
            var query = GetProjectParticipantsQuery.Create(project.ProjectId);
            handler = new GetProjectParticipantsQueryHandler(repository, executionContext, repoInvitation);
            return query;
        }
    }
}
