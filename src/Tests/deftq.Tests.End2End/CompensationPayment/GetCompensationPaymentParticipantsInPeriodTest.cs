using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CloseProjectLogBookWeek;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectLogBookWeek;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectParticipant;
using Xunit;

namespace deftq.Tests.End2End.CompensationPayment
{
    [Collection("End2End")]
    public class GetCompensationPaymentParticipantsInPeriodTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public GetCompensationPaymentParticipantsInPeriodTest(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task GetCompensationPayment()
        {
            var firstDayOfWeek = new DateTimeOffset(2023, 1, 23, 0, 0, 0, TimeSpan.Zero);
            var projectId = await _api.CreateProject();
            var logBookWeekRequest = new RegisterProjectLogBookWeekRequest(_fixture.UserId, 2023, 4, "work work work",
                new List<RegisterProjectLogBookDay>()
                {
                    new RegisterProjectLogBookDay(firstDayOfWeek, 5, 45), new RegisterProjectLogBookDay(firstDayOfWeek.AddDays(2), 2, 15)
                });
            await _api.RegisterLogBookWeek(projectId, logBookWeekRequest);
            await _api.CloseLogBookWeek(projectId, new CloseProjectLogBookWeekRequest(_fixture.UserId, 2023, 4));

            var startDate = new DateOnly(firstDayOfWeek.Year, firstDayOfWeek.Month, firstDayOfWeek.Day);
            var endDate = new DateOnly(firstDayOfWeek.Year, firstDayOfWeek.Month, firstDayOfWeek.Day + 5);
            var response = await _api.GetCompensationPayment(projectId, startDate, endDate, 20);

            Assert.Equal(startDate, response.StartDate);
            Assert.Equal(endDate, response.EndDate);
            Assert.Equal(8, response.Participants.ToList()[0].Hours);
            Assert.Equal(160, response.Participants.ToList()[0].Payment);
        }
    }
}
