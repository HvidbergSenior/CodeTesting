using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Application.GetExtraWorkAgreementRates;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetExtraWorkAgreementRates
{
    public class GetExtraWorkAgreementRatesQueryTest
    {
        [Fact]
        public Task Should_Throw_NotFoundException()
        {
            var query = GetExtraWorkAgreementRatesQuery.Create(ProjectId.Create(Guid.NewGuid()));
            var repository = new ProjectExtraWorkAgreementListInMemoryRepository();
            var handler = new GetExtraWorkAgreementRatesQueryHandler(repository);

            return Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task GivenExtraWorkAgreementList_WhenGettingRates_RatesAreReturned()
        {
            var projectId = ProjectId.Create(Guid.NewGuid());
            var query = GetExtraWorkAgreementRatesQuery.Create(projectId);
            var repository = new ProjectExtraWorkAgreementListInMemoryRepository();
            var extraWorkAgreementList = ProjectExtraWorkAgreementList.Create(projectId, Any.ProjectExtraWorkAgreementListId());
            extraWorkAgreementList.SetCustomerRate(ProjectExtraWorkCustomerHourRate.Create(113));
            extraWorkAgreementList.SetCompanyRate(ProjectExtraWorkAgreementCompanyHourRate.Create(112));
            await repository.Add(extraWorkAgreementList);
            
            var handler = new GetExtraWorkAgreementRatesQueryHandler(repository);
            var resp = await handler.Handle(query, CancellationToken.None);
            
            Assert.Equal(113, resp.CustomerRatePerHourDkr);
            Assert.Equal(112, resp.CompanyRatePerHourDkr);
        }
    }
}
