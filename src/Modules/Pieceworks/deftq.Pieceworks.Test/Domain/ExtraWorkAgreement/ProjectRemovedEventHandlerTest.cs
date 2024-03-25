using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.ExtraWorkAgreement
{
    public class ProjectRemovedEventHandlerTest
    {
        [Fact]
        public async Task ShouldNotReturnExtraWorkAgreementWhenProjectIsDeleted()
        {
            var uow = new FakeUnitOfWork();
            var extraWorkAgreementListRepository = new ProjectExtraWorkAgreementListInMemoryRepository();

            var extraWorkAgreementList = Any.ProjectExtraWorkAgreementList();

            await extraWorkAgreementListRepository.Add(extraWorkAgreementList);

            var handler = new ProjectRemovedEventHandler(extraWorkAgreementListRepository, uow);
            var evt = ProjectRemovedDomainEvent.Create(extraWorkAgreementList.ProjectId);

            await handler.Handle(evt, CancellationToken.None);

            Assert.Empty(extraWorkAgreementListRepository.Entities);
            Assert.True(extraWorkAgreementListRepository.SaveChangesCalled);
        }
    }
}
