using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterLogBookSalaryAdvance;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RegisterLogBookSalaryAdvance
{
    public class RegisterLogBookSalaryAdvanceCommandTest
    {
        [Fact]
        public async Task GivenLogBook_WhenRegisteringSalaryAdvance_ShouldRegister()
        {
            var year = 2023;
            var week = 3;
            var role = LogBookSalaryAdvanceRole.Participant;
            var amount = 142.5m;
            
            var uow = new FakeUnitOfWork();
            var executionContext = new FakeExecutionContext();
            
            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            await logBookRepository.Add(projectLogBook);
            
            var handler = new RegisterLogBookSalaryAdvanceCommandHandler(projectRepository, logBookRepository, uow, executionContext);
            var cmd = RegisterLogBookSalaryAdvanceCommand.Create(projectLogBook.ProjectId, executionContext.UserId, year, week, role, amount);
            await handler.Handle(cmd, CancellationToken.None);
            
            Assert.Single(logBookRepository.Entities);
            Assert.True(logBookRepository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
            
            var logbook = await logBookRepository.GetByProjectId(projectLogBook.ProjectId.Value, CancellationToken.None);
            var logbookWeek = logbook.FindWeek(executionContext.UserId, LogBookYear.Create(year), LogBookWeek.Create(week));
            Assert.NotNull(logbookWeek);
            Assert.False(logbookWeek?.SalaryAdvance.IsEmpty());
            Assert.Equal(amount, logbookWeek?.SalaryAdvance.Amount?.Value);
            Assert.Equal(role, logbookWeek?.SalaryAdvance.Role);
            
        }
        
        [Fact]
        public async Task GivenLogBook_WhenUpdateSalaryAdvance_ShouldHaveRegistering()
        {
            var year = 2023;
            var week = 3;

            var uow = new FakeUnitOfWork();
            var executionContext = new FakeExecutionContext();
            
            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            await logBookRepository.Add(projectLogBook);
            
            var handler = new RegisterLogBookSalaryAdvanceCommandHandler(projectRepository, logBookRepository, uow, executionContext);
            var cmd = RegisterLogBookSalaryAdvanceCommand.Create(projectLogBook.ProjectId, executionContext.UserId, year, week, LogBookSalaryAdvanceRole.Participant, 120);
            await handler.Handle(cmd, CancellationToken.None);
            
            var logbook = await logBookRepository.GetByProjectId(projectLogBook.ProjectId.Value, CancellationToken.None);
            var logbookWeek = logbook.FindWeek(executionContext.UserId, LogBookYear.Create(year), LogBookWeek.Create(week));
            Assert.NotNull(logbookWeek);
            Assert.False(logbookWeek?.SalaryAdvance.IsEmpty());
            Assert.Equal(120, logbookWeek?.SalaryAdvance.Amount?.Value);
            Assert.Equal(LogBookSalaryAdvanceRole.Participant, logbookWeek?.SalaryAdvance.Role);
            
            cmd = RegisterLogBookSalaryAdvanceCommand.Create(projectLogBook.ProjectId, executionContext.UserId, year, week, LogBookSalaryAdvanceRole.Apprentice, 95);
            await handler.Handle(cmd, CancellationToken.None);
            
            logbook = await logBookRepository.GetByProjectId(projectLogBook.ProjectId.Value, CancellationToken.None);
            logbookWeek = logbook.FindWeek(executionContext.UserId, LogBookYear.Create(year), LogBookWeek.Create(week));
            Assert.NotNull(logbookWeek);
            Assert.False(logbookWeek?.SalaryAdvance.IsEmpty());
            Assert.Equal(95, logbookWeek?.SalaryAdvance.Amount?.Value);
            Assert.Equal(LogBookSalaryAdvanceRole.Apprentice, logbookWeek?.SalaryAdvance.Role);
        }
        
        [Fact]
        public async Task GivenLogBook_WhenUpdateSalaryAdvanceWithZero_ShouldNotHaveRegistering()
        {
            var year = 2023;
            var week = 3;

            var uow = new FakeUnitOfWork();
            var executionContext = new FakeExecutionContext();
            
            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            await logBookRepository.Add(projectLogBook);
            
            var handler = new RegisterLogBookSalaryAdvanceCommandHandler(projectRepository, logBookRepository, uow, executionContext);
            var cmd = RegisterLogBookSalaryAdvanceCommand.Create(projectLogBook.ProjectId, executionContext.UserId, year, week, LogBookSalaryAdvanceRole.Participant, 120);
            await handler.Handle(cmd, CancellationToken.None);
            
            var logbook = await logBookRepository.GetByProjectId(projectLogBook.ProjectId.Value, CancellationToken.None);
            var logbookWeek = logbook.FindWeek(executionContext.UserId, LogBookYear.Create(year), LogBookWeek.Create(week));
            Assert.NotNull(logbookWeek);
            Assert.False(logbookWeek?.SalaryAdvance.IsEmpty());
            Assert.Equal(120, logbookWeek?.SalaryAdvance.Amount?.Value);
            Assert.Equal(LogBookSalaryAdvanceRole.Participant, logbookWeek?.SalaryAdvance.Role);
            
            cmd = RegisterLogBookSalaryAdvanceCommand.Create(projectLogBook.ProjectId, executionContext.UserId, year, week, LogBookSalaryAdvanceRole.Participant, 0);
            await handler.Handle(cmd, CancellationToken.None);
            
            logbook = await logBookRepository.GetByProjectId(projectLogBook.ProjectId.Value, CancellationToken.None);
            logbookWeek = logbook.FindWeek(executionContext.UserId, LogBookYear.Create(year), LogBookWeek.Create(week));
            Assert.NotNull(logbookWeek);
            Assert.True(logbookWeek?.SalaryAdvance.IsEmpty());
            Assert.Null(logbookWeek?.SalaryAdvance.Amount?.Value);
            Assert.Null(logbookWeek?.SalaryAdvance.Role);
        }
    }
}