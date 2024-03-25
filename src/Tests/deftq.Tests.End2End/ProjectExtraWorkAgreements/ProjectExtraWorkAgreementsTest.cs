using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateExtraWorkAgreement;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateExtraWorkAgreementRates;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectExtraWorkAgreement;
using deftq.Pieceworks.Application.GetExtraWorkAgreements;
using Xunit;

namespace deftq.Tests.End2End.ProjectExtraWorkAgreements
{
    [Collection("End2End")]
    public class ProjectExtraWorkAgreementsTest
    {
        private readonly Api _api;

        public ProjectExtraWorkAgreementsTest(WebAppFixture webAppFixture)
        {
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task GivenProject_WhenRegisteringExtraWorkAgreementWithTypeAgreedPayment_ExtraWorkAgreementIsAddedToList()
        {
            var projectId = await _api.CreateProject();

            await _api.RegisterExtraWorkAgreementWithPayment(projectId, "758294", "Test name", "Project description", 205,
                new CreateExtraWorkAgreementWorkTime(0, 0));

            var extraWorkAgreementsResponse = await _api.GetExtraWorkAgreements(projectId);
            Assert.Equal("Test name", extraWorkAgreementsResponse.ExtraWorkAgreements[0].Name);
            Assert.Equal("758294", extraWorkAgreementsResponse.ExtraWorkAgreements[0].ExtraWorkAgreementNumber);
            Assert.Equal(0, extraWorkAgreementsResponse.ExtraWorkAgreements[0].WorkTime!.Hours);
            Assert.Equal(0, extraWorkAgreementsResponse.ExtraWorkAgreements[0].WorkTime!.Minutes);
            Assert.Equal(205, extraWorkAgreementsResponse.ExtraWorkAgreements[0].PaymentDkr);
            Assert.Equal(ExtraWorkAgreementTypeResponse.AgreedPayment, extraWorkAgreementsResponse.ExtraWorkAgreements[0].ExtraWorkAgreementType);
        }

        [Fact]
        public async Task GivenProject_WhenRegisteringExtraWorkAgreementWithTypeOther_ExtraWorkAgreementDoesNotContainWorkTimeAndPayment()
        {
            var projectId = await _api.CreateProject();

            await _api.RegisterExtraWorkAgreementWithTypeOther(projectId, "758294", "Test name", "Project description", 20,
                new CreateExtraWorkAgreementWorkTime(15, 10));

            var extraWorkAgreementsResponse = await _api.GetExtraWorkAgreements(projectId);

            Assert.Equal("Test name", extraWorkAgreementsResponse.ExtraWorkAgreements[0].Name);
            Assert.Equal("758294", extraWorkAgreementsResponse.ExtraWorkAgreements[0].ExtraWorkAgreementNumber);
            Assert.Equal(0, extraWorkAgreementsResponse.ExtraWorkAgreements[0].WorkTime!.Hours);
            Assert.Equal(0, extraWorkAgreementsResponse.ExtraWorkAgreements[0].WorkTime!.Minutes);
            Assert.Equal(0, extraWorkAgreementsResponse.ExtraWorkAgreements[0].PaymentDkr);
            Assert.Equal(ExtraWorkAgreementTypeResponse.Other, extraWorkAgreementsResponse.ExtraWorkAgreements[0].ExtraWorkAgreementType);
        }

        [Fact]
        public async Task GivenProject_WhenRegisteringExtraWorkAgreementWithTypeCompanyHours_ExtraWorkAgreementContainsWorkTime()
        {
            var projectId = await _api.CreateProject();

            await _api.RegisterExtraWorkAgreementWithCompanyHoursWorkTime(projectId, "758294", "Test name", "", 20,
                new CreateExtraWorkAgreementWorkTime(15, 10));

            var extraWorkAgreementsResponse = await _api.GetExtraWorkAgreements(projectId);

            Assert.Equal("Test name", extraWorkAgreementsResponse.ExtraWorkAgreements[0].Name);
            Assert.Equal("758294", extraWorkAgreementsResponse.ExtraWorkAgreements[0].ExtraWorkAgreementNumber);
            Assert.Equal(15, extraWorkAgreementsResponse.ExtraWorkAgreements[0].WorkTime!.Hours);
            Assert.Equal(10, extraWorkAgreementsResponse.ExtraWorkAgreements[0].WorkTime!.Minutes);
            Assert.Equal(0, extraWorkAgreementsResponse.ExtraWorkAgreements[0].PaymentDkr);
            Assert.Equal(ExtraWorkAgreementTypeResponse.CompanyHours, extraWorkAgreementsResponse.ExtraWorkAgreements[0].ExtraWorkAgreementType);
        }

        [Fact]
        public async Task GivenProject_WhenRegisteringExtraWorkAgreementWithTypeCustomerHours_ExtraWorkAgreementContainsWorkTime()
        {
            var projectId = await _api.CreateProject();

            await _api.RegisterExtraWorkAgreementWithCustomerHoursWorkTime(projectId, "758294", "Test name", "", 20,
                new CreateExtraWorkAgreementWorkTime(15, 10));

            var extraWorkAgreementsResponse = await _api.GetExtraWorkAgreements(projectId);

            Assert.Equal("Test name", extraWorkAgreementsResponse.ExtraWorkAgreements[0].Name);
            Assert.Equal("758294", extraWorkAgreementsResponse.ExtraWorkAgreements[0].ExtraWorkAgreementNumber);
            Assert.Equal(15, extraWorkAgreementsResponse.ExtraWorkAgreements[0].WorkTime!.Hours);
            Assert.Equal(10, extraWorkAgreementsResponse.ExtraWorkAgreements[0].WorkTime!.Minutes);
            Assert.Equal(0, extraWorkAgreementsResponse.ExtraWorkAgreements[0].PaymentDkr);
            Assert.Equal(ExtraWorkAgreementTypeResponse.CustomerHours, extraWorkAgreementsResponse.ExtraWorkAgreements[0].ExtraWorkAgreementType);
        }

        [Fact]
        public async Task WhenRemovingExtraWorkAgreement_ExtraWorkAgreementShouldBeRemoved()
        {
            var projectId = await _api.CreateProject();

            await _api.RegisterExtraWorkAgreementWithPayment(projectId, "234098", "test name", "", 4392, new CreateExtraWorkAgreementWorkTime(5, 30));

            var extraWorkAgreementResponse = await _api.GetExtraWorkAgreements(projectId);

            await _api.RemoveExtraWorkAgreement(projectId, extraWorkAgreementResponse.ExtraWorkAgreements[0].ExtraWorkAgreementId);

            extraWorkAgreementResponse = await _api.GetExtraWorkAgreements(projectId);
            Assert.Empty(extraWorkAgreementResponse.ExtraWorkAgreements);
        }

        [Fact]
        public async Task WhenUpdatingExtraWorkAgreementWithWorkTime_ExtraWorkAgreementShouldBeUpdated()
        {
            var projectId = await _api.CreateProject();

            await _api.RegisterExtraWorkAgreementWithCompanyHoursWorkTime(projectId, "234098", "test name", "", 4392,
                new CreateExtraWorkAgreementWorkTime(7, 0));

            var extraWorkAgreementResponse = await _api.GetExtraWorkAgreements(projectId);
            Assert.Equal(7, extraWorkAgreementResponse.ExtraWorkAgreements[0].WorkTime!.Hours);

            await _api.UpdateExtraWorkAgreement(projectId, extraWorkAgreementResponse.ExtraWorkAgreements[0].ExtraWorkAgreementId, "Updated number",
                "Updated name", "Updated description", 3000, UpdateProjectExtraWorkAgreementsRequest.UpdateExtraWorkAgreementType.CompanyHours,
                new UpdateExtraWorkAgreementWorkTime(5, 0));

            extraWorkAgreementResponse = await _api.GetExtraWorkAgreements(projectId);
            Assert.Equal("Updated name", extraWorkAgreementResponse.ExtraWorkAgreements[0].Name);
            Assert.Equal("Updated description", extraWorkAgreementResponse.ExtraWorkAgreements[0].Description);
            Assert.Equal("Updated number", extraWorkAgreementResponse.ExtraWorkAgreements[0].ExtraWorkAgreementNumber);
            Assert.Equal(5, extraWorkAgreementResponse.ExtraWorkAgreements[0].WorkTime!.Hours);
        }

        [Fact]
        public async Task WhenUpdatingExtraWorkAgreementsWithAgreedPayment_ShouldNotDuplicate()
        {
            var projectId = await _api.CreateProject();

            await _api.RegisterExtraWorkAgreementWithCompanyHoursWorkTime(projectId, "111111", "nummer1", "", 0,
                new CreateExtraWorkAgreementWorkTime(5, 30));

            await _api.RegisterExtraWorkAgreementWithPayment(projectId, "22222", "nummer2", "", 100,
                new CreateExtraWorkAgreementWorkTime(0, 0));

            await _api.RegisterExtraWorkAgreementWithPayment(projectId, "33333", "nummer3", "", 200,
                new CreateExtraWorkAgreementWorkTime(0, 0));

            var extraWorkAgreementResponse = await _api.GetExtraWorkAgreements(projectId);

            await _api.UpdateExtraWorkAgreement(projectId, extraWorkAgreementResponse.ExtraWorkAgreements[0].ExtraWorkAgreementId, "100000",
                "new name1", "new description1", 3000, UpdateProjectExtraWorkAgreementsRequest.UpdateExtraWorkAgreementType.AgreedPayment,
                new UpdateExtraWorkAgreementWorkTime(0, 0));

            await _api.UpdateExtraWorkAgreement(projectId, extraWorkAgreementResponse.ExtraWorkAgreements[1].ExtraWorkAgreementId, "20000",
                "new name2", "new description2", 1000, UpdateProjectExtraWorkAgreementsRequest.UpdateExtraWorkAgreementType.AgreedPayment,
                new UpdateExtraWorkAgreementWorkTime(0, 0));

            extraWorkAgreementResponse = await _api.GetExtraWorkAgreements(projectId);

            Assert.Equal(3000, extraWorkAgreementResponse.ExtraWorkAgreements[0].PaymentDkr);
            Assert.Equal(1000, extraWorkAgreementResponse.ExtraWorkAgreements[1].PaymentDkr);
            Assert.Equal(200, extraWorkAgreementResponse.ExtraWorkAgreements[2].PaymentDkr);
        }

        [Fact]
        public async Task WhenUpdatingExtraWorkAgreementWithAgreedPayment_ExtraWorkAgreementShouldBeUpdated()
        {
            var projectId = await _api.CreateProject();

            await _api.RegisterExtraWorkAgreementWithPayment(projectId, "234098", "test name", "", 4392, new CreateExtraWorkAgreementWorkTime(7, 0));

            var extraWorkAgreementResponse = await _api.GetExtraWorkAgreements(projectId);
            Assert.Equal(4392, extraWorkAgreementResponse.ExtraWorkAgreements[0].PaymentDkr);

            await _api.UpdateExtraWorkAgreement(projectId, extraWorkAgreementResponse.ExtraWorkAgreements[0].ExtraWorkAgreementId, "Updated number",
                "Updated name", "Updated description", 3000, UpdateProjectExtraWorkAgreementsRequest.UpdateExtraWorkAgreementType.AgreedPayment,
                new UpdateExtraWorkAgreementWorkTime(5, 0));

            extraWorkAgreementResponse = await _api.GetExtraWorkAgreements(projectId);
            Assert.Equal("Updated name", extraWorkAgreementResponse.ExtraWorkAgreements[0].Name);
            Assert.Equal("Updated description", extraWorkAgreementResponse.ExtraWorkAgreements[0].Description);
            Assert.Equal("Updated number", extraWorkAgreementResponse.ExtraWorkAgreements[0].ExtraWorkAgreementNumber);
            Assert.Equal(3000, extraWorkAgreementResponse.ExtraWorkAgreements[0].PaymentDkr);
        }

        [Fact]
        public async Task GivenProject_WhenGettingExtraWorkAgreementRates_RatesAreReturned()
        {
            var projectId = await _api.CreateProject();

            var rates = await _api.GetExtraWorkAgreementRates(projectId);

            Assert.Equal(0, rates.CustomerRatePerHourDkr);
            Assert.Equal(0, rates.CompanyRatePerHourDkr);
        }

        [Fact]
        public async Task GivenProject_WhenUpdatingExtraWorkAgreementRates_NewRatesAreReturned()
        {
            var projectId = await _api.CreateProject();

            await _api.UpdateExtraWorkAgreementRates(projectId, new UpdateExtraWorkAgreementRatesRequest(567, 789));

            var rates = await _api.GetExtraWorkAgreementRates(projectId);

            Assert.Equal(567, rates.CustomerRatePerHourDkr);
            Assert.Equal(789, rates.CompanyRatePerHourDkr);
        }
    }
}
