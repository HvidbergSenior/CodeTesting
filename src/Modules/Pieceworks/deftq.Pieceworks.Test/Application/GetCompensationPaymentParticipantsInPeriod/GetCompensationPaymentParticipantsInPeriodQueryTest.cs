using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetCompensationPaymentParticipantsInPeriod;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetCompensationPaymentParticipantsInPeriod
{
    public class GetCompensationPaymentParticipantsInPeriodQueryTest
    {
        [Fact]
        public async Task GetCompensationPaymentFor2OutOf3Participants()
        {
            var executionContext = new FakeExecutionContext();

            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId, "Owner");
            var participants = GenereateParticipants(project);
            await projectRepository.Add(project);

            var projectLogBook = GenerateLogBookWithRegistrationsClosedWeek(project, participants);
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(projectLogBook);

            var query = GetCompensationPaymentParticipantsInPeriodQuery.Create(projectLogBook.ProjectId, new DateOnly(2023, 1, 23),
                new DateOnly(2023, 1, 27),
                20);
            var handler = new GetCompensationPaymentParticipantsInPeriodQueryHandler(projectRepository, executionContext, logBookRepository);
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(3, response.Participants.ToList().Count);

            var ownerData = response.Participants.ToList().FirstOrDefault(u => u.ProjectParticipantId == executionContext.UserId);
            var participant1Data = response.Participants.ToList().FirstOrDefault(u => u.ProjectParticipantId == participants.p1.Id);
            var participant2Data = response.Participants.ToList().FirstOrDefault(u => u.ProjectParticipantId == participants.p2.Id);
            Assert.NotNull(ownerData);
            Assert.NotNull(participant1Data);
            Assert.NotNull(participant2Data);

            Assert.Equal(25, ownerData?.Hours);
            Assert.Equal(500, ownerData?.Payment);
            Assert.Equal("Owner", ownerData?.Name);
            Assert.Equal(25, participant1Data?.Hours);
            Assert.Equal(500, participant1Data?.Payment);
            Assert.Equal(participants.p1.Name.Value, participant1Data?.Name);
            Assert.Equal(participants.p1.Email.Value, participant1Data?.Email);
            Assert.Equal(0, participant2Data?.Hours);
            Assert.Equal(0, participant2Data?.Payment);
            Assert.Equal(participants.p2.Name.Value, participant2Data?.Name);
            Assert.Equal(participants.p2.Email.Value, participant2Data?.Email);
        }
        
        [Fact]
        public async Task GetCompensationPaymentPeriodSameDay()
        {
            var executionContext = new FakeExecutionContext();

            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId, "Owner");
            var participants = GenereateParticipants(project);
            await projectRepository.Add(project);

            var projectLogBook = GenerateLogBookWithRegistrationsClosedWeek(project, participants);
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(projectLogBook);

            var query = GetCompensationPaymentParticipantsInPeriodQuery.Create(projectLogBook.ProjectId, new DateOnly(2023, 1, 24),
                new DateOnly(2023, 1, 24),
                100);
            var handler = new GetCompensationPaymentParticipantsInPeriodQueryHandler(projectRepository, executionContext, logBookRepository);
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(3, response.Participants.ToList().Count);

            var ownerData = response.Participants.ToList().FirstOrDefault(u => u.ProjectParticipantId == executionContext.UserId);
            var participant1Data = response.Participants.ToList().FirstOrDefault(u => u.ProjectParticipantId == participants.p1.Id);
            var participant2Data = response.Participants.ToList().FirstOrDefault(u => u.ProjectParticipantId == participants.p2.Id);
            Assert.NotNull(ownerData);
            Assert.NotNull(participant1Data);
            Assert.NotNull(participant2Data);

            Assert.Equal(5.25m, ownerData?.Hours);
            Assert.Equal(525, ownerData?.Payment);
            Assert.Equal("Owner", ownerData?.Name);
            Assert.Equal(5.25m, participant1Data?.Hours);
            Assert.Equal(525, participant1Data?.Payment);
            Assert.Equal(participants.p1.Name.Value, participant1Data?.Name);
            Assert.Equal(participants.p1.Email.Value, participant1Data?.Email);
            Assert.Equal(0, participant2Data?.Hours);
            Assert.Equal(0, participant2Data?.Payment);
            Assert.Equal(participants.p2.Name.Value, participant2Data?.Name);
            Assert.Equal(participants.p2.Email.Value, participant2Data?.Email);
        }

        [Fact]
        public async Task GetCompensationPaymentNoRegistrations()
        {
            var executionContext = new FakeExecutionContext();

            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId, "Owner");
            var participants = GenereateParticipants(project);
            await projectRepository.Add(project);

            var projectLogBook = GenerateLogBookNoRegistrations(project, participants);
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(projectLogBook);

            var query = GetCompensationPaymentParticipantsInPeriodQuery.Create(projectLogBook.ProjectId, new DateOnly(2023, 1, 23),
                new DateOnly(2023, 1, 27),
                20);
            var handler = new GetCompensationPaymentParticipantsInPeriodQueryHandler(projectRepository, executionContext, logBookRepository);
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(3, response.Participants.ToList().Count);

            var ownerData = response.Participants.ToList().FirstOrDefault(u => u.ProjectParticipantId == executionContext.UserId);
            var participant1Data = response.Participants.ToList().FirstOrDefault(u => u.ProjectParticipantId == participants.p1.Id);
            var participant2Data = response.Participants.ToList().FirstOrDefault(u => u.ProjectParticipantId == participants.p2.Id);
            Assert.NotNull(ownerData);
            Assert.NotNull(participant1Data);
            Assert.NotNull(participant2Data);

            Assert.Equal(0, ownerData?.Hours);
            Assert.Equal(0, ownerData?.Payment);
            Assert.Equal(0, participant1Data?.Hours);
            Assert.Equal(0, participant1Data?.Payment);
            Assert.Equal(0, participant2Data?.Hours);
            Assert.Equal(0, participant2Data?.Payment);
        }

        [Fact]
        public async Task GetCompensationPaymentRegistrationsOutSideAskedPeriod()
        {
            var executionContext = new FakeExecutionContext();

            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId, "Owner");
            var participants = GenereateParticipants(project);
            await projectRepository.Add(project);

            var projectLogBook = GenerateLogBookWithRegistrationsClosedWeek(project, participants);
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(projectLogBook);

            var query = GetCompensationPaymentParticipantsInPeriodQuery.Create(projectLogBook.ProjectId, new DateOnly(2024, 1, 23),
                new DateOnly(2024, 1, 27),
                20);
            var handler = new GetCompensationPaymentParticipantsInPeriodQueryHandler(projectRepository, executionContext, logBookRepository);
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(3, response.Participants.ToList().Count);

            var ownerData = response.Participants.ToList().FirstOrDefault(u => u.ProjectParticipantId == executionContext.UserId);
            var participant1Data = response.Participants.ToList().FirstOrDefault(u => u.ProjectParticipantId == participants.p1.Id);
            var participant2Data = response.Participants.ToList().FirstOrDefault(u => u.ProjectParticipantId == participants.p2.Id);
            Assert.NotNull(ownerData);
            Assert.NotNull(participant1Data);
            Assert.NotNull(participant2Data);

            Assert.Equal(0, ownerData?.Hours);
            Assert.Equal(0, ownerData?.Payment);
            Assert.Equal(0, participant1Data?.Hours);
            Assert.Equal(0, participant1Data?.Payment);
            Assert.Equal(0, participant2Data?.Hours);
            Assert.Equal(0, participant2Data?.Payment);
        }
        
        [Fact]
        public async Task GetCompensationPaymentNotClosedWeek()
        {
            var executionContext = new FakeExecutionContext();

            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId, "Owner");
            var participants = GenereateParticipants(project);
            await projectRepository.Add(project);

            var projectLogBook = GenerateLogBookWithRegistrationsNotClosedWeek(project, participants);
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(projectLogBook);

            var query = GetCompensationPaymentParticipantsInPeriodQuery.Create(projectLogBook.ProjectId, new DateOnly(2024, 1, 23),
                new DateOnly(2024, 1, 27),
                20);
            var handler = new GetCompensationPaymentParticipantsInPeriodQueryHandler(projectRepository, executionContext, logBookRepository);
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(3, response.Participants.ToList().Count);

            var ownerData = response.Participants.ToList().FirstOrDefault(u => u.ProjectParticipantId == executionContext.UserId);
            var participant1Data = response.Participants.ToList().FirstOrDefault(u => u.ProjectParticipantId == participants.p1.Id);
            var participant2Data = response.Participants.ToList().FirstOrDefault(u => u.ProjectParticipantId == participants.p2.Id);
            Assert.NotNull(ownerData);
            Assert.NotNull(participant1Data);
            Assert.NotNull(participant2Data);

            Assert.Equal(0, ownerData?.Hours);
            Assert.Equal(0, ownerData?.Payment);
            Assert.Equal(0, participant1Data?.Hours);
            Assert.Equal(0, participant1Data?.Payment);
            Assert.Equal(0, participant2Data?.Hours);
            Assert.Equal(0, participant2Data?.Payment);
        }

        private (ProjectParticipant p1, ProjectParticipant p2) GenereateParticipants(Project project)
        {
            var participant1 = ProjectParticipant.Create(ProjectParticipantId.Create(Any.Guid()), ProjectParticipantName.Create("HaveRegistrations"),
                ProjectEmail.Create("k@k.dk"), ProjectParticipantAddress.Empty(), ProjectParticipantPhoneNumber.Empty());
            var participant2 = ProjectParticipant.Create(ProjectParticipantId.Create(Any.Guid()), ProjectParticipantName.Create("DoNotRegister"),
                ProjectEmail.Create("k@k.dk"), ProjectParticipantAddress.Empty(), ProjectParticipantPhoneNumber.Empty());
            project.AddProjectParticipant(participant1);
            project.AddProjectParticipant(participant2);
            return (participant1, participant2);
        }

        private ProjectLogBook GenerateLogBookNoRegistrations(Project project, (ProjectParticipant p1, ProjectParticipant p2) participants)
        {
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            return projectLogBook;
        }

        private ProjectLogBook GenerateLogBookWithRegistrationsClosedWeek(Project project, (ProjectParticipant p1, ProjectParticipant p2) participants)
        {
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var userOwner = ProjectLogBookUser.Create(LogBookName.Create(project.ProjectOwner.Name), project.ProjectOwner.Id);
            GenerateRegistrations(projectLogBook, userOwner);
            var savedOwner = projectLogBook.FindUser(userOwner.UserId); // Can not use "user", need to find the user saved on the logbook to close week 
            savedOwner?.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(4));
            var userParticipant = ProjectLogBookUser.Create(LogBookName.Create(participants.p1.Name.Value),
                participants.p1.ParticipantId.Value);
            GenerateRegistrations(projectLogBook, userParticipant);
            var savedParticipant = projectLogBook.FindUser(userParticipant.UserId); // Can not use "user", need to find the user saved on the logbook to close week 
            savedParticipant?.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(4));

            return projectLogBook;
        }
        
        private ProjectLogBook GenerateLogBookWithRegistrationsNotClosedWeek(Project project, (ProjectParticipant p1, ProjectParticipant p2) participants)
        {
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var userOwner = ProjectLogBookUser.Create(LogBookName.Create(project.ProjectOwner.Name), project.ProjectOwner.Id);
            GenerateRegistrations(projectLogBook, userOwner);
            var userParticipant = ProjectLogBookUser.Create(LogBookName.Create(participants.p1.Name.Value),
                participants.p1.ParticipantId.Value);
            GenerateRegistrations(projectLogBook, userParticipant);

            return projectLogBook;
        }

        private void GenerateRegistrations(ProjectLogBook projectLogBook, ProjectLogBookUser user)
        {
            projectLogBook.RegisterWeek(user, LogBookYear.Create(2023),
                LogBookWeek.Create(4), LogBookNote.Create("my note"),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 23, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 24, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 25, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 26, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 27, 4, 0)
                });
        }
    }
}
