using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CloseProjectLogBookWeek;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.OpenProjectLogBookWeek;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterLogbookSalaryAdvance;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectLogBookWeek;
using deftq.Pieceworks.Application.GetProjectLogBookWeek;
using Xunit;

namespace deftq.Tests.End2End.ProjectLogBookWeeks
{
    [Collection("End2End")]
    public class ProjectLogBookWeeksTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public ProjectLogBookWeeksTest(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task Should_Find_Registered_Logbook_Week_With_Registered_Days()
        {
            // Create a project
            var projectId = await _api.CreateProject();
            
            // Register logbook week with days
            var firstDayOfWeek = new DateTimeOffset(2022, 8, 8, 0, 0, 0, TimeSpan.Zero);
            var logBookWeekRequest = new RegisterProjectLogBookWeekRequest(_fixture.UserId, 2022, 32, "work work work",
                new List<RegisterProjectLogBookDay>()
                {
                    new RegisterProjectLogBookDay(firstDayOfWeek, 5, 45),
                    new RegisterProjectLogBookDay(firstDayOfWeek.AddDays(2), 2, 15)
                });
            await _api.RegisterLogBookWeek(projectId, logBookWeekRequest);
            
            // Close week
            await _api.CloseLogBookWeek(projectId, new CloseProjectLogBookWeekRequest(_fixture.UserId, 2022, 32));
            
            // Get logbook
            var weekResponse = await _api.GetLogBookWeek(projectId,_fixture.UserId, 2022, 32);
            
            Assert.Equal(2022, weekResponse.Year);
            Assert.Equal(32, weekResponse.Week);
            Assert.Equal("work work work", weekResponse.Note);
            Assert.Equal(8, weekResponse.WeekSummation.Hours);
            Assert.Equal(0, weekResponse.WeekSummation.Minutes);
            Assert.Equal(8, weekResponse.ClosedWeeksSummation.Hours);
            Assert.Equal(0, weekResponse.ClosedWeeksSummation.Minutes);
            Assert.Equal(7, weekResponse.Days.Count);
            Assert.Equal(8, weekResponse.Days[0].Date.Day);
            Assert.Equal(8, weekResponse.Days[0].Date.Month);
            Assert.Equal(2022, weekResponse.Days[0].Date.Year);
            Assert.Equal(5, weekResponse.Days[0].Time.Hours);
            Assert.Equal(45, weekResponse.Days[0].Time.Minutes);
            Assert.Equal(2, weekResponse.Days[2].Time.Hours);
            Assert.Equal(15, weekResponse.Days[2].Time.Minutes);
        }

        [Fact]
        public async Task Should_Return_Week_From_Date()
        {
            // Create a project
            var projectId = await _api.CreateProject();
            
            // Get log book from date
            var weekResponse = await _api.GetLogBookWeekFromDate(projectId, _fixture.UserId, 2022, 8, 12);
            
            Assert.Equal(2022, weekResponse.Year);
            Assert.Equal(32, weekResponse.Week);
            Assert.Equal("", weekResponse.Note);
            Assert.Equal(7, weekResponse.Days.Count);
        }
        
        [Fact]
        public async Task LogBook_Week_Can_Be_Closed()
        {
            // Create a project
            var projectId = await _api.CreateProject();

            // Close week
            await _api.CloseLogBookWeek(projectId, new CloseProjectLogBookWeekRequest(_fixture.UserId, 2022, 27));

            // Assert week was closed
            var weekResponse = await _api.GetLogBookWeek(projectId, _fixture.UserId, 2022, 27);
                
            Assert.True(weekResponse.Closed);
        }
        
        [Fact]
        public async Task Closed_LogBook_Week_Can_Be_Opened()
        {
            // Create a project
            var projectId = await _api.CreateProject();

            // Close week
            await _api.CloseLogBookWeek(projectId, new CloseProjectLogBookWeekRequest(_fixture.UserId, 2022, 27));
            
            // Open week again
            await _api.OpenLogBookWeek(projectId, new OpenProjectLogBookWeekRequest(_fixture.UserId, 2022, 27));
            
            // Assert week was opened again
            var weekResponse = await _api.GetLogBookWeek(projectId, _fixture.UserId, 2022, 27);
            
            Assert.False(weekResponse.Closed);
        }
        
        [Fact]
        public async Task Should_Register_Salary_Advance_Participant()
        {
            var year = 2023;
            var week1 = 9;
            var week2 = 10;

            var projectId = await _api.CreateProject();
            var request = new RegisterLogbookSalaryAdvanceRequest(_fixture.UserId, year, week1, LogBookSalaryAdvanceRoleRequest.Participant, 160);
            await _api.RegisterLogBookSalaryAdvance(projectId, request);
            request = new RegisterLogbookSalaryAdvanceRequest(_fixture.UserId, year, week2, LogBookSalaryAdvanceRoleRequest.Participant, 170);
            await _api.RegisterLogBookSalaryAdvance(projectId, request);

            var response = await _api.GetLogBookWeek(projectId, _fixture.UserId, year, week1);
            
            Assert.NotNull(response.SalaryAdvance.Start);
            Assert.Equal(year, response.SalaryAdvance.Start?.Year);
            Assert.Equal(week1, response.SalaryAdvance.Start?.Week);
            Assert.NotNull(response.SalaryAdvance.End);
            Assert.Equal(160, response.SalaryAdvance.Amount);
            Assert.Equal(LogBookSalaryAdvanceRoleResponse.Participant, response.SalaryAdvance.Role);
        }

        [Fact]
        public async Task Should_Register_Salary_Advance_Apprentice()
        {
            var year = 2023;
            var week1 = 9;
            var week2 = 10;

            var projectId = await _api.CreateProject();
            var request = new RegisterLogbookSalaryAdvanceRequest(_fixture.UserId, year, week1, LogBookSalaryAdvanceRoleRequest.Apprentice, 90);
            await _api.RegisterLogBookSalaryAdvance(projectId, request);
            request = new RegisterLogbookSalaryAdvanceRequest(_fixture.UserId, year, week2, LogBookSalaryAdvanceRoleRequest.Apprentice, 100);
            await _api.RegisterLogBookSalaryAdvance(projectId, request);

            var response = await _api.GetLogBookWeek(projectId, _fixture.UserId, year, week1);
            Assert.NotNull(response.SalaryAdvance.Start);
            Assert.Equal(year, response.SalaryAdvance.Start?.Year);
            Assert.Equal(week1, response.SalaryAdvance.Start?.Week);
            Assert.NotNull(response.SalaryAdvance.End);
            Assert.Equal(90, response.SalaryAdvance.Amount);
            Assert.Equal(LogBookSalaryAdvanceRoleResponse.Apprentice, response.SalaryAdvance.Role);
        }

        [Fact]
        public async Task Should_Register_Salary_Advance_2_Weeks_See_Period()
        {
            var year = 2023;
            var week1 = 9;
            var weekBetween = 12;
            var week2 = 18;

            var projectId = await _api.CreateProject();
            var request = new RegisterLogbookSalaryAdvanceRequest(_fixture.UserId, year, week1, LogBookSalaryAdvanceRoleRequest.Participant, 150);
            await _api.RegisterLogBookSalaryAdvance(projectId, request);
            request = new RegisterLogbookSalaryAdvanceRequest(_fixture.UserId, year, week2, LogBookSalaryAdvanceRoleRequest.Participant, 160);
            await _api.RegisterLogBookSalaryAdvance(projectId, request);

            var response = await _api.GetLogBookWeek(projectId, _fixture.UserId, year, weekBetween);
            Assert.NotNull(response.SalaryAdvance.Start);
            Assert.Equal(year, response.SalaryAdvance.Start?.Year);
            Assert.Equal(week1, response.SalaryAdvance.Start?.Week);
            Assert.NotNull(response.SalaryAdvance.End);
            Assert.Equal(year, response.SalaryAdvance.End?.Year);
            Assert.Equal(week2, response.SalaryAdvance.End?.Week);
            Assert.Equal(150, response.SalaryAdvance.Amount);
            Assert.Equal(LogBookSalaryAdvanceRoleResponse.Participant, response.SalaryAdvance.Role);
        }
        
        [Fact]
        public async Task Should_Register_Salary_Advance_2_Weeks_In_Different_Years()
        {
            var year1 = 2020;
            var yearBetween = 2023;
            var year2 = 2026;
            var week1 = 9;
            var weekBetween = 14;
            var week2 = 18;

            var projectId = await _api.CreateProject();
            var request = new RegisterLogbookSalaryAdvanceRequest(_fixture.UserId, year1, week1, LogBookSalaryAdvanceRoleRequest.Participant, 150);
            await _api.RegisterLogBookSalaryAdvance(projectId, request);
            request = new RegisterLogbookSalaryAdvanceRequest(_fixture.UserId, year2, week2, LogBookSalaryAdvanceRoleRequest.Participant, 160);
            await _api.RegisterLogBookSalaryAdvance(projectId, request);

            var response = await _api.GetLogBookWeek(projectId, _fixture.UserId, yearBetween, weekBetween);
            Assert.NotNull(response.SalaryAdvance.Start);
            Assert.Equal(year1, response.SalaryAdvance.Start?.Year);
            Assert.Equal(week1, response.SalaryAdvance.Start?.Week);
            Assert.NotNull(response.SalaryAdvance.End);
            Assert.Equal(year2, response.SalaryAdvance.End?.Year);
            Assert.Equal(week2, response.SalaryAdvance.End?.Week);
            Assert.Equal(150, response.SalaryAdvance.Amount);
            Assert.Equal(LogBookSalaryAdvanceRoleResponse.Participant, response.SalaryAdvance.Role);
        }

        [Fact]
        public async Task Should_No_Salary_Advance_Registred()
        {
            var projectId = await _api.CreateProject();
            var response = await _api.GetLogBookWeek(projectId, _fixture.UserId, 2012, 1);
            Assert.Null(response.SalaryAdvance.Start);
            Assert.Null(response.SalaryAdvance.End);
            Assert.Equal(0, response.SalaryAdvance.Amount);
            Assert.Equal(LogBookSalaryAdvanceRoleResponse.Undefined, response.SalaryAdvance.Role);
        }
        
        [Fact]
        public async Task Should_Reg_3_Different_Weeks_With_Salary_Advance_And_Clear_The_Middle()
        {
            var week1 = 9;
            var weekBetween = 12;
            var week2 = 18;
            var projectId = await _api.CreateProject();
            var request = new RegisterLogbookSalaryAdvanceRequest(_fixture.UserId, 2023, week1, LogBookSalaryAdvanceRoleRequest.Participant, 145);
            await _api.RegisterLogBookSalaryAdvance(projectId, request);
            request = new RegisterLogbookSalaryAdvanceRequest(_fixture.UserId, 2023, weekBetween, LogBookSalaryAdvanceRoleRequest.Participant, 150);
            await _api.RegisterLogBookSalaryAdvance(projectId, request);
            request = new RegisterLogbookSalaryAdvanceRequest(_fixture.UserId, 2023, week2, LogBookSalaryAdvanceRoleRequest.Participant, 160);
            await _api.RegisterLogBookSalaryAdvance(projectId, request);
            var response1 = await _api.GetLogBookWeek(projectId, _fixture.UserId, 2023, week1);
            var response2 = await _api.GetLogBookWeek(projectId, _fixture.UserId, 2023, weekBetween);
            var response3 = await _api.GetLogBookWeek(projectId, _fixture.UserId, 2023, week2);
            Assert.NotNull(response1.SalaryAdvance.Start);
            Assert.Equal(week1, response1.SalaryAdvance.Start?.Week);
            Assert.NotNull(response1.SalaryAdvance.End);
            Assert.Equal(weekBetween, response1.SalaryAdvance.End?.Week);
            Assert.Equal(145, response1.SalaryAdvance.Amount);
            Assert.NotNull(response2.SalaryAdvance.Start);
            Assert.Equal(weekBetween, response2.SalaryAdvance.Start?.Week);
            Assert.NotNull(response2.SalaryAdvance.End);
            Assert.Equal(week2, response2.SalaryAdvance.End?.Week);
            Assert.Equal(150, response2.SalaryAdvance.Amount);
            Assert.NotNull(response3.SalaryAdvance.Start);
            Assert.Null(response3.SalaryAdvance.End);
            Assert.Equal(160, response3.SalaryAdvance.Amount);
            request = new RegisterLogbookSalaryAdvanceRequest(_fixture.UserId, 2023, weekBetween, LogBookSalaryAdvanceRoleRequest.Participant, 0);
            await _api.RegisterLogBookSalaryAdvance(projectId, request);
            response1 = await _api.GetLogBookWeek(projectId, _fixture.UserId, 2023, week1);
            response2 = await _api.GetLogBookWeek(projectId, _fixture.UserId, 2023, weekBetween);
            Assert.NotNull(response1.SalaryAdvance.Start);
            Assert.Equal(week1, response1.SalaryAdvance.Start?.Week);
            Assert.NotNull(response1.SalaryAdvance.End);
            Assert.Equal(week2, response1.SalaryAdvance.End?.Week);
            Assert.Equal(145, response1.SalaryAdvance.Amount);
            Assert.NotNull(response2.SalaryAdvance.Start);
            Assert.Equal(week1, response2.SalaryAdvance.Start?.Week);
            Assert.NotNull(response2.SalaryAdvance.End);
            Assert.Equal(week2, response2.SalaryAdvance.End?.Week);
            Assert.Equal(145, response2.SalaryAdvance.Amount);
        }
    }
}
