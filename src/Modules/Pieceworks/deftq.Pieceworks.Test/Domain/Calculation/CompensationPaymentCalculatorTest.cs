using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.projectLogBook;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.Calculation
{
    public class CompensationPaymentCalculatorTest
    {
        [Fact]
        public void GivenPeriodStartIsLaterThePeriodEnd_ThenThrowException()
        {
            var calculator = new CompensationPaymentCalculator();
            var userId = Any.Guid();
            var userName = Any.LogBookName();

            var projectLogBookUser = ProjectLogBookUser.Create(userName, userId);
            var users = new List<ProjectLogBookUser>() { projectLogBookUser };

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                calculator.CalculateUsersCompensationPayment(users, new DateOnly(2023, 9, 1), new DateOnly(2023, 1, 1), 20));
        }

        [Fact]
        public void GivenNoWeekRegistrations_TheReturnZero()
        {
            var calculator = new CompensationPaymentCalculator();
            var userId = Any.Guid();
            var userName = Any.LogBookName();

            var projectLogBookUser = ProjectLogBookUser.Create(userName, userId);
            var users = new List<ProjectLogBookUser>() { projectLogBookUser };

            var res = calculator.CalculateUsersCompensationPayment(users, new DateOnly(2023, 1, 23), new DateOnly(2023, 1, 27), 20);

            var user = res.Participants.ToList()[0];
            Assert.Equal(userId, user.ProjectParticipantId);
            Assert.Equal(0, user.Hours.Value);
            Assert.Equal(0, user.Payment.Value);
        }

        [Fact]
        public void GivenWeekRegistrations_ThenReturnHoursAndPayment()
        {
            var calculator = new CompensationPaymentCalculator();
            var userId = Any.Guid();
            var userName = Any.LogBookName();

            var projectLogBookUser = ProjectLogBookUser.Create(userName, userId);
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(4), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 23, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 24, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 25, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 26, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 27, 4, 0)
                });
            projectLogBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(4)); // closed 35h45m
            projectLogBookUser.SumClosedHours();

            var users = new List<ProjectLogBookUser>() { projectLogBookUser };

            var res = calculator.CalculateUsersCompensationPayment(users, new DateOnly(2023, 1, 23), new DateOnly(2023, 1, 27), 20);

            var user = res.Participants.ToList()[0];
            Assert.Equal(userId, user.ProjectParticipantId);
            Assert.Equal(25m, user.Hours.Value);
            Assert.Equal(500m, user.Payment.Value);
        }
        
        [Fact]
        public void GivenWeekRegistrations_WhenGet2Days_ThenReturnHoursAndPayment()
        {
            var calculator = new CompensationPaymentCalculator();
            var userId = Any.Guid();
            var userName = Any.LogBookName();

            var projectLogBookUser = ProjectLogBookUser.Create(userName, userId);
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(4), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 23, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 24, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 25, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 26, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 27, 4, 0)
                });
            projectLogBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(4)); // closed 35h45m
            projectLogBookUser.SumClosedHours();

            var users = new List<ProjectLogBookUser>() { projectLogBookUser };

            var res = calculator.CalculateUsersCompensationPayment(users, new DateOnly(2023, 1, 24), new DateOnly(2023, 1, 25), 20.5m);

            var user = res.Participants.ToList()[0];
            Assert.Equal(userId, user.ProjectParticipantId);
            Assert.Equal(10.5m, user.Hours.Value);
            Assert.Equal(215.25m, user.Payment.Value);
        }
        
        [Fact]
        public void GivenWeekRegistrations_WhenGetSameDay_ThenReturnHoursAndPayment()
        {
            var calculator = new CompensationPaymentCalculator();
            var userId = Any.Guid();
            var userName = Any.LogBookName();

            var projectLogBookUser = ProjectLogBookUser.Create(userName, userId);
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(4), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 23, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 24, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 25, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 26, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 27, 4, 0)
                });
            projectLogBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(4)); // closed 35h45m
            projectLogBookUser.SumClosedHours();

            var users = new List<ProjectLogBookUser>() { projectLogBookUser };

            var res = calculator.CalculateUsersCompensationPayment(users, new DateOnly(2023, 1, 24), new DateOnly(2023, 1, 24), 100);

            var user = res.Participants.ToList()[0];
            Assert.Equal(userId, user.ProjectParticipantId);
            Assert.Equal(5.25m, user.Hours.Value);
            Assert.Equal(525, user.Payment.Value);
        }
        
        [Fact]
        public void GivenWeekRegistrations_WhenNotClosedWeek_ThenReturnZeroHoursAndPayment()
        {
            var calculator = new CompensationPaymentCalculator();
            var userId = Any.Guid();
            var userName = Any.LogBookName();

            var projectLogBookUser = ProjectLogBookUser.Create(userName, userId);
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(4), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 23, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 24, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 25, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 26, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 27, 4, 0)
                });

            var users = new List<ProjectLogBookUser>() { projectLogBookUser };

            var res = calculator.CalculateUsersCompensationPayment(users, new DateOnly(2023, 1, 24), new DateOnly(2023, 1, 24), 100);

            var user = res.Participants.ToList()[0];
            Assert.Equal(userId, user.ProjectParticipantId);
            Assert.Equal(0, user.Hours.Value);
            Assert.Equal(0, user.Payment.Value);
        }

        [Fact]
        public void GivenWeekRegistrations_WhenAmountIsZero_ThenReturnHoursAndPaymentZero()
        {
            var calculator = new CompensationPaymentCalculator();
            var userId = Any.Guid();
            var userName = Any.LogBookName();

            var projectLogBookUser = ProjectLogBookUser.Create(userName, userId);
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(4), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 23, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 24, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 25, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 26, 5, 15),
                    Any.ProjectLogBookDay(2023, 1, 27, 4, 0)
                });
            projectLogBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(4)); // closed 35h45m
            projectLogBookUser.SumClosedHours();

            var users = new List<ProjectLogBookUser>() { projectLogBookUser };

            var res = calculator.CalculateUsersCompensationPayment(users, new DateOnly(2023, 1, 24), new DateOnly(2023, 1, 25), 0);

            var user = res.Participants.ToList()[0];
            Assert.Equal(userId, user.ProjectParticipantId);
            Assert.Equal(10.5m, user.Hours.Value);
            Assert.Equal(0, user.Payment.Value);
        }
    }
}
