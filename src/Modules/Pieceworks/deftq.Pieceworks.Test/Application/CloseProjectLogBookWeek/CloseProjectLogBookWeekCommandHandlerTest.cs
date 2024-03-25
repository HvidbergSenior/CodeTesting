using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.CloseProjectLogBookWeek;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.CloseProjectLogBookWeek
{
    public class CloseProjectLogBookWeekCommandHandlerTest
    {
        [Fact]
        public async Task CloseLogBookWeek()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            
            var project = Any.Project().OwnedBy(executionContext.UserId);
            
            var logBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            await projectRepository.Add(project);
            await logBookRepository.Add(logBook);

            var handler =
                new CloseProjectLogBookWeekCommandHandler(projectRepository, logBookRepository, uow, executionContext);
            
            await handler.Handle(
                CloseProjectLogBookWeekCommand.Create(logBook.ProjectId.Value, executionContext.UserId, 2022, 32),
                CancellationToken.None);

            logBook = await logBookRepository.GetByProjectId(project.ProjectId.Value);
            Assert.Single(logBook.ProjectLogBookUsers);
            var week = logBook.FindWeek(executionContext.UserId, LogBookYear.Create(2022), LogBookWeek.Create(32));
            Assert.NotNull(week);
            Assert.True(week!.Closed);

            Assert.Single(logBookRepository.Entities);
            Assert.True(logBookRepository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
        }
    }
}
