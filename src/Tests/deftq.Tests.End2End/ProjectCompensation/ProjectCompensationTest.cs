using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CloseProjectLogBookWeek;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectCompensation;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectLogBookWeek;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectParticipant;
using Xunit;

namespace deftq.Tests.End2End.ProjectCompensation
{
    [Collection("End2End")]
    public class ProjectCompensationTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public ProjectCompensationTest(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task GivenProject_WhenRegisteringCompensation_CompensationIsAddedToList()
        {
            var projectId = await _api.CreateProject();

            await _api.RegisterUser(projectId, "Hans", UserRole.ProjectParticipant, String.Empty, String.Empty, String.Empty);
            var participants = await _api.GetProjectUsers(projectId);
            var participantId = participants.Users.First().Id;

            var day = new RegisterProjectLogBookDay(new DateTimeOffset(2023, 7, 4, 0, 0, 0, TimeSpan.Zero), 7, 45);
            var registerProjectLogBookDays = new List<RegisterProjectLogBookDay> { day };
            var registerWeekRequest = new RegisterProjectLogBookWeekRequest(participantId, 2023, 27, string.Empty, registerProjectLogBookDays);
            await _api.RegisterLogBookWeek(projectId, registerWeekRequest);
            await _api.CloseLogBookWeek(projectId, new CloseProjectLogBookWeekRequest(participantId, 2023, 27));
            
            var startDate = new DateOnly(2023, 7, 3);
            var endDate = new DateOnly(2023, 7, 5);
            var registerCompensationRequest = new RegisterCompensationRequest(40, startDate, endDate, new List<Guid> { participantId });
            await _api.RegisterProjectCompensation(projectId, registerCompensationRequest);

            var compensationList = await _api.GetProjectCompensationList(projectId);
            
            Assert.Single(compensationList.Compensations);
            Assert.Single(compensationList.Compensations[0].CompensationParticipant);
            Assert.Equal(7.75m,compensationList.Compensations[0].CompensationParticipant[0].ClosedHoursInPeriod);
            Assert.Equal(310m,compensationList.Compensations[0].CompensationParticipant[0].CompensationAmountDkr);
        }
    }
}
