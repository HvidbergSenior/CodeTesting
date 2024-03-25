using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Application.GetProjectLogBookWeek;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectLogBookWeek
{
    public class GetProjectLogBookWeekQueryTests
    {
        [Fact]
        public Task UnknownUser_Should_Throw_NotFoundException()
        {
            var executionContext = new FakeExecutionContext();
            var projectRepository = new ProjectInMemoryRepository();
            var query = GetProjectLogBookWeekQuery.Create(Any.ProjectId(), Guid.NewGuid(), 2022, 35);
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            var handler = new GetProjectLogBookWeekQueryHandler(projectRepository, executionContext, logBookRepository, new SystemTime());

            return Assert.ThrowsAsync<NotFoundException>(
                async () => await handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task RegisteredWeek_Should_Return_ProjectLogBook()
        {
            var executionContext = new FakeExecutionContext();
            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var user = ProjectLogBookUser.Create(LogBookName.Create(executionContext.UserName), executionContext.UserId);
            projectLogBook.RegisterWeek(user, LogBookYear.Create(2022),
                LogBookWeek.Create(35), LogBookNote.Create("my note"), new List<ProjectLogBookDay>());
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(projectLogBook);

            var query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, user.UserId, 2022, 35);
            var handler = new GetProjectLogBookWeekQueryHandler(projectRepository, executionContext, logBookRepository, new SystemTime());
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(2022, response?.Year);
            Assert.Equal(35, response?.Week);
            Assert.Equal("my note", response?.Note);
            Assert.Equal(7, response?.Days.Count);
            Assert.Equal(0, response?.SalaryAdvance.Amount);
            Assert.Equal(LogBookSalaryAdvanceRoleResponse.Undefined, response?.SalaryAdvance.Role);
            Assert.Null(response?.SalaryAdvance.Start);
            Assert.Null(response?.SalaryAdvance.End);
        }
        
        [Fact]
        public async Task EmptyWeek_Should_Return_DummyWeek()
        {
            var executionContext = new FakeExecutionContext();
            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var user = ProjectLogBookUser.Create(LogBookName.Create(executionContext.UserName), executionContext.UserId);
            projectLogBook.RegisterWeek(user, LogBookYear.Create(2022),
                LogBookWeek.Create(35), Any.LogBookNote(), new List<ProjectLogBookDay>());
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(projectLogBook);

            var query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, user.UserId, 2022, 36);
            var handler = new GetProjectLogBookWeekQueryHandler(projectRepository, executionContext, logBookRepository, new SystemTime());
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(response);
            Assert.Equal(2022, response?.Year);
            Assert.Equal(36, response?.Week);
            Assert.Equal("", response?.Note);
            Assert.Equal(7, response?.Days.Count);
            Assert.Equal(0, response?.SalaryAdvance.Amount);
            Assert.Null(response?.SalaryAdvance.Start);
            Assert.Null(response?.SalaryAdvance.End);
            Assert.Equal(LogBookSalaryAdvanceRoleResponse.Undefined, response?.SalaryAdvance.Role);
        }

        [Fact]
        public async Task Register_Salary_Advance_Should_Return_A_Period_And_Amount()
        {
            var executionContext = new FakeExecutionContext();
            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            var year = LogBookYear.Create(2300);
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var user = ProjectLogBookUser.Create(LogBookName.Create(executionContext.UserName), executionContext.UserId);
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(user.UserId), year,LogBookWeek.Create(4), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(150));
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(user.UserId), year,LogBookWeek.Create(12), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(160));
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(user.UserId), year,LogBookWeek.Create(20), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(170));
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(user.UserId), year,LogBookWeek.Create(28), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(110));
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(projectLogBook);
            
            var handler = new GetProjectLogBookWeekQueryHandler(projectRepository, executionContext, logBookRepository, new SystemTime());
            
            var query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, user.UserId, year.Value, 8);
            var response = await handler.Handle(query, CancellationToken.None);
            
            Assert.Equal(4, response.SalaryAdvance.Start?.Week);
            Assert.Equal(year.Value, response.SalaryAdvance.Start?.Year);
            Assert.Equal(12, response.SalaryAdvance.End?.Week);
            Assert.Equal(year.Value, response.SalaryAdvance.End?.Year);
            Assert.Equal(150, response.SalaryAdvance.Amount);
            Assert.Equal(LogBookSalaryAdvanceRoleResponse.Participant, response.SalaryAdvance.Role);
            
            query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, user.UserId, year.Value, 16);
            response = await handler.Handle(query, CancellationToken.None);
            
            Assert.Equal(12, response.SalaryAdvance.Start?.Week);
            Assert.Equal(year.Value, response.SalaryAdvance.Start?.Year);
            Assert.Equal(20, response.SalaryAdvance.End?.Week);
            Assert.Equal(year.Value, response.SalaryAdvance.End?.Year);
            Assert.Equal(160, response.SalaryAdvance.Amount);
            Assert.Equal(LogBookSalaryAdvanceRoleResponse.Participant, response.SalaryAdvance.Role);
            
            query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, user.UserId, year.Value, 24);
            response = await handler.Handle(query, CancellationToken.None);
            
            Assert.Equal(20, response.SalaryAdvance.Start?.Week);
            Assert.Equal(year.Value, response.SalaryAdvance.Start?.Year);
            Assert.Equal(28, response.SalaryAdvance.End?.Week);
            Assert.Equal(year.Value, response.SalaryAdvance.End?.Year);
            Assert.Equal(170, response.SalaryAdvance.Amount);
            Assert.Equal(LogBookSalaryAdvanceRoleResponse.Participant, response.SalaryAdvance.Role);
        }
        
        [Fact]
        public async Task Register_Single_Salary_Advance_Should_Return_A_Period_And_Amount_Not_Closed()
        {
            var executionContext = new FakeExecutionContext();
            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            var year = LogBookYear.Create(2300);
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var user = ProjectLogBookUser.Create(LogBookName.Create(executionContext.UserName), executionContext.UserId);
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(user.UserId), year,LogBookWeek.Create(12), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(160));
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(projectLogBook);
            
            var query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, user.UserId, year.Value, 20);
            var handler = new GetProjectLogBookWeekQueryHandler(projectRepository, executionContext, logBookRepository, new SystemTime());
            var response = await handler.Handle(query, CancellationToken.None);
            
            Assert.Equal(12, response.SalaryAdvance.Start?.Week);
            Assert.Equal(year.Value, response.SalaryAdvance.Start?.Year);
            Assert.Null(response.SalaryAdvance.End);
            Assert.Equal(160, response.SalaryAdvance.Amount);
            Assert.Equal(LogBookSalaryAdvanceRoleResponse.Participant, response.SalaryAdvance.Role);
        }
        
        [Fact]
        public async Task Register_Single_Salary_Advance_Should_Return_A_Period_And_Amount_Not_Started()
        {
            var executionContext = new FakeExecutionContext();
            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            var year = LogBookYear.Create(2300);
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var user = ProjectLogBookUser.Create(LogBookName.Create(executionContext.UserName), executionContext.UserId);
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(user.UserId), year,LogBookWeek.Create(12), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(160));
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(projectLogBook);
            
            var query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, user.UserId, year.Value, 8);
            var handler = new GetProjectLogBookWeekQueryHandler(projectRepository, executionContext, logBookRepository, new SystemTime());
            var response = await handler.Handle(query, CancellationToken.None);
            
            Assert.Null(response.SalaryAdvance.Start);
            Assert.Equal(12, response.SalaryAdvance.End?.Week);
            Assert.Equal(year.Value, response.SalaryAdvance.End?.Year);
            Assert.Equal(0, response.SalaryAdvance.Amount);
            Assert.Equal(LogBookSalaryAdvanceRoleResponse.Undefined, response.SalaryAdvance.Role);
        }

        [Fact]
        public async Task Register_2_Salary_Advances_In_Different_Years_Should_Return_Period_And_Amount_In_Year_In_Between()
        {
            var executionContext = new FakeExecutionContext();
            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var user = ProjectLogBookUser.Create(LogBookName.Create(executionContext.UserName), executionContext.UserId);
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(user.UserId), LogBookYear.Create(2000),LogBookWeek.Create(4), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(150));
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(user.UserId), LogBookYear.Create(2025),LogBookWeek.Create(12), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(160));
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(projectLogBook);
            
            var handler = new GetProjectLogBookWeekQueryHandler(projectRepository, executionContext, logBookRepository, new SystemTime());
            
            var query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, user.UserId, 2023, 8);
            var response = await handler.Handle(query, CancellationToken.None);
            
            Assert.Equal(4, response.SalaryAdvance.Start?.Week);
            Assert.Equal(2000, response.SalaryAdvance.Start?.Year);
            Assert.Equal(12, response.SalaryAdvance.End?.Week);
            Assert.Equal(2025, response.SalaryAdvance.End?.Year);
            Assert.Equal(150, response.SalaryAdvance.Amount);
            Assert.Equal(LogBookSalaryAdvanceRoleResponse.Participant, response.SalaryAdvance.Role);
        }

        [Fact]
        public async Task Register_Salary_Advance_Get_Week_As_Owner()
        {
            const int week = 4;
            const int year = 2025;
            
            var owner = Any.ProjectOwner();
            var participant = Any.ProjectParticipant();
            var manager = Any.ProjectManager();

            var executionContext = new FakeExecutionContext(owner.Id);
            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(owner.Id);
            project.AddProjectParticipant(participant);
            project.AddProjectManager(manager);
            await projectRepository.Add(project);
            
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var logBookParticipant = ProjectLogBookUser.Create(LogBookName.Create(participant.Name.Value), participant.Id);
            var logBookOwner = ProjectLogBookUser.Create(LogBookName.Create(owner.Name), owner.Id);
            
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(owner.Id), LogBookYear.Create(year),LogBookWeek.Create(week), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(250));
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(participant.Id), LogBookYear.Create(year),LogBookWeek.Create(week), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(150));
            
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(projectLogBook);
            
            var handler = new GetProjectLogBookWeekQueryHandler(projectRepository, executionContext, logBookRepository, new SystemTime());
            
            var query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, owner.Id, year, week);
            var responseOwner = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(responseOwner.SalaryAdvance);
            Assert.False(SalaryAdvanceIsEmpty(responseOwner.SalaryAdvance));
            
            query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, participant.Id, year, week);
            var responseParticipant = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(responseParticipant.SalaryAdvance);
            Assert.False(SalaryAdvanceIsEmpty(responseParticipant.SalaryAdvance));
        }
        
        [Fact]
        public async Task Register_Salary_Advance_Get_Week_As_Participant()
        {
            const int week = 4;
            const int year = 2025;
            
            var owner = Any.ProjectOwner();
            var participant = Any.ProjectParticipant();
            var manager = Any.ProjectManager();

            var executionContext = new FakeExecutionContext(participant.Id);
            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(owner.Id);
            project.AddProjectParticipant(participant);
            project.AddProjectManager(manager);
            await projectRepository.Add(project);
            
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var logBookParticipant = ProjectLogBookUser.Create(LogBookName.Create(participant.Name.Value), participant.Id);
            var logBookOwner = ProjectLogBookUser.Create(LogBookName.Create(owner.Name), owner.Id);
            
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(owner.Id), LogBookYear.Create(year),LogBookWeek.Create(week), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(250));
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(participant.Id), LogBookYear.Create(year),LogBookWeek.Create(week), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(150));
            
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(projectLogBook);
            
            var handler = new GetProjectLogBookWeekQueryHandler(projectRepository, executionContext, logBookRepository, new SystemTime());
            
            var query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, owner.Id, year, week);
            var responseOwner = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(responseOwner.SalaryAdvance);
            Assert.True(SalaryAdvanceIsEmpty(responseOwner.SalaryAdvance));
            
            query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, participant.Id, year, week);
            var responseParticipant = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(responseParticipant.SalaryAdvance);
            Assert.False(SalaryAdvanceIsEmpty(responseParticipant.SalaryAdvance));
        }
        
        [Fact]
        public async Task Register_Salary_Advance_Get_Week_As_Other_Participant()
        {
            const int week = 4;
            const int year = 2025;
            
            var owner = Any.ProjectOwner();
            var participant = Any.ProjectParticipant();
            var otherParticipant = Any.ProjectParticipant();
            var manager = Any.ProjectManager();

            var executionContext = new FakeExecutionContext(otherParticipant.Id);
            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(owner.Id);
            project.AddProjectParticipant(participant);
            project.AddProjectParticipant(otherParticipant);
            project.AddProjectManager(manager);
            await projectRepository.Add(project);
            
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var logBookParticipant = ProjectLogBookUser.Create(LogBookName.Create(participant.Name.Value), participant.Id);
            var logBookOwner = ProjectLogBookUser.Create(LogBookName.Create(owner.Name), owner.Id);
            
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(owner.Id), LogBookYear.Create(year),LogBookWeek.Create(week), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(250));
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(participant.Id), LogBookYear.Create(year),LogBookWeek.Create(week), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(150));
            
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(projectLogBook);
            
            var handler = new GetProjectLogBookWeekQueryHandler(projectRepository, executionContext, logBookRepository, new SystemTime());
            
            var query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, owner.Id, year, week);
            var responseOwner = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(responseOwner.SalaryAdvance);
            Assert.True(SalaryAdvanceIsEmpty(responseOwner.SalaryAdvance));
            
            query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, participant.Id, year, week);
            var responseParticipant = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(responseParticipant.SalaryAdvance);
            Assert.True(SalaryAdvanceIsEmpty(responseParticipant.SalaryAdvance));
        }
        
        [Fact]
        public async Task Register_Salary_Advance_Get_Week_As_Manager()
        {
            const int week = 4;
            const int year = 2025;
            
            var owner = Any.ProjectOwner();
            var participant = Any.ProjectParticipant();
            var manager = Any.ProjectManager();

            var executionContext = new FakeExecutionContext(manager.Id);
            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(owner.Id);
            project.AddProjectParticipant(participant);
            project.AddProjectManager(manager);
            await projectRepository.Add(project);
            
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var logBookParticipant = ProjectLogBookUser.Create(LogBookName.Create(participant.Name.Value), participant.Id);
            var logBookOwner = ProjectLogBookUser.Create(LogBookName.Create(owner.Name), owner.Id);
            
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(owner.Id), LogBookYear.Create(year),LogBookWeek.Create(week), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(250));
            projectLogBook.UpdateSalaryAdvance(ProjectLogBookUser.Create(participant.Id), LogBookYear.Create(year),LogBookWeek.Create(week), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(150));
            
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(projectLogBook);
            
            var handler = new GetProjectLogBookWeekQueryHandler(projectRepository, executionContext, logBookRepository, new SystemTime());
            
            var query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, owner.Id, year, week);
            var responseOwner = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(responseOwner.SalaryAdvance);
            Assert.True(SalaryAdvanceIsEmpty(responseOwner.SalaryAdvance));
            
            query = GetProjectLogBookWeekQuery.Create(projectLogBook.ProjectId, participant.Id, year, week);
            var responseParticipant = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(responseParticipant.SalaryAdvance);
            Assert.True(SalaryAdvanceIsEmpty(responseParticipant.SalaryAdvance));
        }

        private bool SalaryAdvanceIsEmpty(LogBookSalaryAdvanceResponse salaryAdvance)
        {
            return salaryAdvance.Start is null && salaryAdvance.End is null && salaryAdvance.Amount == 0 &&
                   salaryAdvance.Role == LogBookSalaryAdvanceRoleResponse.Undefined;
        }
    }
}
