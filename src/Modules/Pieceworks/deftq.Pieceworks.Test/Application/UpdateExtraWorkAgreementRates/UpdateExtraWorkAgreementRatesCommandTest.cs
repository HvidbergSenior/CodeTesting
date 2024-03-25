using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateExtraWorkAgreementRates;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateExtraWorkAgreementRates
{
    public class UpdateExtraWorkAgreementRatesCommandTest
    {
        [Fact]
        public async Task GivenExtraWorkAgreementList_WhenUpdatingRates_NewRatesAreUsed()
        {
            var repository = new ProjectExtraWorkAgreementListInMemoryRepository();
            var fakeUnitOfWork = new FakeUnitOfWork();
            
            var projectId = ProjectId.Create(Guid.NewGuid());
            var extraWorkAgreementList = ProjectExtraWorkAgreementList.Create(projectId, Any.ProjectExtraWorkAgreementListId());
            extraWorkAgreementList.SetCustomerRate(ProjectExtraWorkCustomerHourRate.Create(113));
            extraWorkAgreementList.SetCompanyRate(ProjectExtraWorkAgreementCompanyHourRate.Create(112));
            await repository.Add(extraWorkAgreementList);

            var command = UpdateExtraWorkAgreementRatesCommand.Create(projectId.Value, 666, 667);
            var handler = new UpdateExtraWorkAgreementRatesCommandHandler(repository, fakeUnitOfWork);
            await handler.Handle(command, CancellationToken.None);

            var agreementListFromRepo = await repository.GetByProjectId(projectId.Value, CancellationToken.None);

            Assert.Equal(666, agreementListFromRepo.ProjectExtraWorkAgreementCustomerHourRate.Value);
            Assert.Equal(667, agreementListFromRepo.ProjectExtraWorkAgreementCompanyHourRate.Value);
        }
    }
}
