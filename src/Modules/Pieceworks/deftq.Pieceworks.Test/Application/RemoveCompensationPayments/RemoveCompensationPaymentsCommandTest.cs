using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveCompensationPayments;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveCompensationPayments
{
    public class RemoveCompensationPaymentsCommandTest
    {
        private readonly FakeUnitOfWork _uow;
        private readonly FakeExecutionContext _executionContext;
        private readonly ProjectCompensationListInMemoryRepository _projectCompensationListInMemoryRepository;

        public RemoveCompensationPaymentsCommandTest()
        {
            _uow = new FakeUnitOfWork();
            _executionContext = new FakeExecutionContext();
            _projectCompensationListInMemoryRepository = new ProjectCompensationListInMemoryRepository();
        }

        [Fact]
        public async Task RemoveSingleCompensationPaymentInList()
        {
            var compensationPaymentList = ProjectCompensationList.Create(Any.ProjectId(), Any.ProjectCompensationListId());
            var projectId = compensationPaymentList.ProjectId.Value;
            var compensationPayment1 = Any.ProjectCompensation();
            var compensationPayment2 = Any.ProjectCompensation();
            var compensationPayment3 = Any.ProjectCompensation();
            compensationPaymentList.AddCompensation(compensationPayment1);
            compensationPaymentList.AddCompensation(compensationPayment2);
            compensationPaymentList.AddCompensation(compensationPayment3);

            await _projectCompensationListInMemoryRepository.Add(compensationPaymentList);

            var handler = new RemoveCompensationPaymentsCommandHandler(_projectCompensationListInMemoryRepository, _uow, _executionContext);
            var idToRemove = compensationPayment2.ProjectCompensationId.Id;
            var cmd = RemoveCompensationPaymentsCommand.Create(projectId, new List<Guid> { idToRemove });
            await handler.Handle(cmd, CancellationToken.None);

            Assert.Single(_projectCompensationListInMemoryRepository.Entities);
            Assert.True(_projectCompensationListInMemoryRepository.SaveChangesCalled);
            Assert.True(_uow.IsCommitted);

            var savedCompensationPayments = await _projectCompensationListInMemoryRepository.GetByProjectId(projectId);
            Assert.Equal(2, savedCompensationPayments.Compensations.Count);
            Assert.DoesNotContain(idToRemove, savedCompensationPayments.Compensations.Select(c => c.ProjectCompensationId.Id));
        }
    }
}
