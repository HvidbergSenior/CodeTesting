using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.projectLogBook
{
    public class ProjectRemovedEventHandlerTest
    {
        [Fact]
        public async Task ShouldNotReturnLogBookWhenProjectIsDeleted()
        {
            var uow = new FakeUnitOfWork();
            var logBookRepository = new ProjectLogBookInMemoryRepository();

            var logBook = Any.ProjectLogBook();

            await logBookRepository.Add(logBook);
            
            var handler = new ProjectRemovedEventHandler(logBookRepository, uow);
            var evt = ProjectRemovedDomainEvent.Create(logBook.ProjectId);
            
            await handler.Handle(evt, CancellationToken.None);
            
            Assert.Empty(logBookRepository.Entities);
            Assert.True(logBookRepository.SaveChangesCalled);
        }
    }
}
