using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.ProjectCompensation
{
    public class ProjectRemovedEventHandlerTest
    {
        [Fact]
        public async Task ShouldNotReturnFavoriteListWhenProjectIsDeleted()
        {
            var compensationListRepository = new ProjectCompensationListInMemoryRepository();

            var compensationList = Any.ProjectCompensationList();

            await compensationListRepository.Add(compensationList);

            var handler = new ProjectRemovedEventHandler(compensationListRepository);
            var evt = ProjectRemovedDomainEvent.Create(compensationList.ProjectId);

            await handler.Handle(evt, CancellationToken.None);

            Assert.Empty(compensationListRepository.Entities);
            Assert.True(compensationListRepository.SaveChangesCalled);
        }
    }
}
