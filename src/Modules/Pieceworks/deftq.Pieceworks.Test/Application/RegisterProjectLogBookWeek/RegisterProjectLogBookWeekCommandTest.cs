using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterProjectLogBookWeek;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RegisterProjectLogBookWeek
{
    public class RegisterLogBookWeekCommandTest
    {
        [Fact]
        public async Task GivenLogBook_WhenRegisteringOwnWeekAsProjectOwner_ShouldRegisterWeek()
        {
            var uow = new FakeUnitOfWork();
            var executionContext = new FakeExecutionContext();
            
            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            await logBookRepository.Add(projectLogBook);
            
            var handler = new RegisterProjectLogBookCommandHandler(projectRepository, logBookRepository, uow, executionContext);
            var cmd = RegisterProjectLogBookWeekCommand.Create(projectLogBook.ProjectId.Value, executionContext.UserId, 2022, 3, "", new List<ProjectLogBookDay>());
            await handler.Handle(cmd, CancellationToken.None);
            
            Assert.Single(logBookRepository.Entities);
            Assert.True(logBookRepository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
        }
        
        [Fact]
        public async Task GivenLogBook_WhenRegisteringOwnWeekAsParticipant_ShouldRegisterWeek()
        {
            var uow = new FakeUnitOfWork();
            var executionContext = new FakeExecutionContext();
            
            var projectRepository = new ProjectInMemoryRepository();
            var participant = Any.ProjectParticipant(executionContext.UserId);
            var project = Any.Project().WithParticipant(participant.Id);
            await projectRepository.Add(project);
            
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            await logBookRepository.Add(projectLogBook);
            
            var handler = new RegisterProjectLogBookCommandHandler(projectRepository, logBookRepository, uow, executionContext);
            var cmd = RegisterProjectLogBookWeekCommand.Create(projectLogBook.ProjectId.Value, participant.Id, 2022, 3, "", new List<ProjectLogBookDay>());
            await handler.Handle(cmd, CancellationToken.None);
            
            Assert.Single(logBookRepository.Entities);
            Assert.True(logBookRepository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
        }
        
        [Fact]
        public async Task GivenLogBook_WhenRegisteringParticipantWeekAsParticipant_ShouldRegisterWeek()
        {
            var uow = new FakeUnitOfWork();
            var executionContext = new FakeExecutionContext();
            
            var projectRepository = new ProjectInMemoryRepository();
            var participant = Any.ProjectParticipant();
            var project = Any.Project().OwnedBy(executionContext.UserId).WithParticipant(participant.Id);
            await projectRepository.Add(project);
            
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            await logBookRepository.Add(projectLogBook);
            
            var handler = new RegisterProjectLogBookCommandHandler(projectRepository, logBookRepository, uow, executionContext);
            var cmd = RegisterProjectLogBookWeekCommand.Create(projectLogBook.ProjectId.Value, participant.Id, 2022, 3, "", new List<ProjectLogBookDay>());
            await handler.Handle(cmd, CancellationToken.None);
            
            Assert.Single(logBookRepository.Entities);
            Assert.True(logBookRepository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
        }
    }
}
