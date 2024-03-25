using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectSpecificOperation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.ProjectSpecificOperation
{
    public class ProjectRemoveEventHandlerTest
    {
        [Fact]
        public async Task ShouldNotReturnProjectSpecificOperationListWhenProjectIsDeleted()
        {
            var uow = new FakeUnitOfWork();
            var projectSpecificOperationListRepository = new ProjectSpecificOperationListInMemoryRepository();

            var projectSpecificOperationList = Any.ProjectSpecificOperationList();

            await projectSpecificOperationListRepository.Add(projectSpecificOperationList);

            var handler = new ProjectRemovedEventHandler(projectSpecificOperationListRepository, uow);
            var evt = ProjectRemovedDomainEvent.Create(projectSpecificOperationList.ProjectId);

            await handler.Handle(evt, CancellationToken.None);

            Assert.Empty(projectSpecificOperationListRepository.Entities);
            Assert.True(projectSpecificOperationListRepository.SaveChangesCalled);
        }
    }
}
