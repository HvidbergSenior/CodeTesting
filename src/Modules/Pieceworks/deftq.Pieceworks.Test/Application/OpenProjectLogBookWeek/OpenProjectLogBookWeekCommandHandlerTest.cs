using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.CloseProjectLogBookWeek;
using deftq.Pieceworks.Application.OpenProjectLogBookWeek;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.OpenProjectLogBookWeek
{
    public class OpenProjectLogBookWeekCommandHandlerTest
    {
        [Fact]
        public async Task OpenLogBookWeek()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            
            var project = Any.Project().OwnedBy(executionContext.UserId);
            var logBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var year = LogBookYear.Create(2022);
            var week = LogBookWeek.Create(32);
            logBook.CloseWeek(ProjectLogBookUser.Create(executionContext.UserId), year, week);
            
            await projectRepository.Add(project);
            await logBookRepository.Add(logBook);

            var handler = new OpenProjectLogBookWeekCommandHandler(projectRepository, logBookRepository, uow, executionContext);
            
            await handler.Handle(OpenProjectLogBookWeekCommand.Create(logBook.ProjectId.Value, executionContext.UserId, year.Value, week.Value),
                CancellationToken.None);

            logBook = await logBookRepository.GetById(logBook.Id);
            var openedWeek = logBook.FindWeek(executionContext.UserId, year, week);
            Assert.NotNull(openedWeek);
            Assert.False(openedWeek!.Closed);
            
            Assert.Single(logBookRepository.Entities);
            Assert.True(logBookRepository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
        }
    }
}
