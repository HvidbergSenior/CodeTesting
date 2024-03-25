using deftq.Pieceworks.Domain.projectLogBook;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.projectLogBook
{
    public class ProjectLogBookUserTest
    {
        [Fact]
        public void GivenNoRegisteredWeeks_WhenGettingSum_GetsZeroHours()
        {
            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());
            var sum = projectLogBookUser.GetSumClosedHours();

            Assert.Equal(0, sum.Value.TotalHours);
        }

        [Fact]
        public void GivenNoClosedWeeks_WhenGettingSum_GetsSumOfClosedWeeks()
        {
            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());

            var year = LogBookYear.Create(2023);
            var week = LogBookWeek.Create(1);
            var hoursToRegister = new List<ProjectLogBookDay>
            {
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2023, 1, 2)),
                    LogBookTime.Create(LogBookHours.Create(5), LogBookMinutes.Create(45)))
            };

            projectLogBookUser.RegisterWeek(year, week, LogBookNote.Empty(), hoursToRegister);
            projectLogBookUser.SumClosedHours();

            var sum = projectLogBookUser.GetSumClosedHours();

            Assert.Equal(0.0, sum.Value.TotalHours);
        }

        [Fact]
        public void GivenClosedWeeks_WhenGettingSum_GetsSumOfClosedWeeks()
        {
            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());

            var year = LogBookYear.Create(2023);
            var week = LogBookWeek.Create(1);
            var hoursToRegister = new List<ProjectLogBookDay>
            {
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2023, 1, 2)),
                    LogBookTime.Create(LogBookHours.Create(5), LogBookMinutes.Create(45)))
            };

            projectLogBookUser.RegisterWeek(year, week, LogBookNote.Empty(), hoursToRegister);
            projectLogBookUser.CloseWeek(year, week);
            projectLogBookUser.SumClosedHours();

            var sum = projectLogBookUser.GetSumClosedHours();

            Assert.Equal(5.75, sum.Value.TotalHours);
        }

        [Fact]
        public void GivenClosedWeeksInTheFuture_WhenGettingSum_GetsSumOfAllClosedWeeks()
        {
            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());

            var year = LogBookYear.Create(2399);
            var week = LogBookWeek.Create(1);
            var hoursToRegister = new List<ProjectLogBookDay>
            {
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2399, 1, 5)),
                    LogBookTime.Create(LogBookHours.Create(7), LogBookMinutes.Create(15)))
            };

            projectLogBookUser.RegisterWeek(year, week, LogBookNote.Empty(), hoursToRegister);
            projectLogBookUser.CloseWeek(year, week);
            projectLogBookUser.SumClosedHours();

            var sum = projectLogBookUser.GetSumClosedHours();

            Assert.Equal(7.25, sum.Value.TotalHours);
        }

        [Fact]
        public void GivenWeekWithRegistrations_WhenSalaryAdvanceNotUpdate_ThenWeekHasInitialSalaryAdvance()
        {
            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());

            var year = LogBookYear.Create(2399);
            var week = LogBookWeek.Create(1);
            var hoursToRegister = new List<ProjectLogBookDay>
            {
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2399, 1, 5)),
                    LogBookTime.Create(LogBookHours.Create(7), LogBookMinutes.Create(15)))
            };

            projectLogBookUser.RegisterWeek(year, week, LogBookNote.Empty(), hoursToRegister);
            var savedWeek = projectLogBookUser.FindWeek(year, week);

            Assert.NotNull(savedWeek);
            Assert.True(savedWeek?.SalaryAdvance.IsEmpty());
            Assert.Null(savedWeek?.SalaryAdvance.Role);
            Assert.Null(savedWeek?.SalaryAdvance.Amount);
        }

        [Fact]
        public void GivenWeekWithRegistrations_WhenUpdateSalaryAdvanceWithApprentice_ThenWeekHasSalaryAdvance()
        {
            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());

            var year = LogBookYear.Create(2399);
            var week = LogBookWeek.Create(1);
            var hoursToRegister = new List<ProjectLogBookDay>
            {
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2399, 1, 5)),
                    LogBookTime.Create(LogBookHours.Create(7), LogBookMinutes.Create(15)))
            };

            projectLogBookUser.RegisterWeek(year, week, LogBookNote.Empty(), hoursToRegister);
            projectLogBookUser.UpdateSalaryAdvance(year, week, LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(42.42m));
            var savedWeek = projectLogBookUser.FindWeek(year, week);

            Assert.NotNull(savedWeek);
            Assert.False(savedWeek?.SalaryAdvance.IsEmpty());
            Assert.Equal(LogBookSalaryAdvanceRole.Apprentice, savedWeek?.SalaryAdvance.Role);
            Assert.Equal(42.42m, savedWeek?.SalaryAdvance.Amount?.Value);
        }

        [Fact]
        public void GivenWeekDoNotExists_WhenUpdateSalaryAdvance_ThenWeekExistsAndHasSalaryAdvance()
        {
            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());

            var year = LogBookYear.Create(2399);
            var week = LogBookWeek.Create(1);

            projectLogBookUser.UpdateSalaryAdvance(year, week, LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(42.42m));
            var savedWeek = projectLogBookUser.FindWeek(year, week);

            Assert.NotNull(savedWeek);
            Assert.False(savedWeek?.SalaryAdvance.IsEmpty());
            Assert.Equal(LogBookSalaryAdvanceRole.Apprentice, savedWeek?.SalaryAdvance.Role);
            Assert.Equal(42.42m, savedWeek?.SalaryAdvance.Amount?.Value);
        }

        [Fact]
        public void GivenWeekWithSalaryAdvance_WhenUpdateSalaryAdvanceWithOverwriteValues_ThenWeekHasNewSalaryAdvance()
        {
            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());

            var year = LogBookYear.Create(2399);
            var week = LogBookWeek.Create(1);
            var hoursToRegister = new List<ProjectLogBookDay>
            {
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2399, 1, 5)),
                    LogBookTime.Create(LogBookHours.Create(7), LogBookMinutes.Create(15)))
            };

            projectLogBookUser.RegisterWeek(year, week, LogBookNote.Empty(), hoursToRegister);
            projectLogBookUser.UpdateSalaryAdvance(year, week, LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(42.42m));
            var savedWeek = projectLogBookUser.FindWeek(year, week);

            Assert.NotNull(savedWeek);
            Assert.False(savedWeek?.SalaryAdvance.IsEmpty());
            Assert.Equal(LogBookSalaryAdvanceRole.Apprentice, savedWeek?.SalaryAdvance.Role);
            Assert.Equal(42.42m, savedWeek?.SalaryAdvance.Amount?.Value);

            projectLogBookUser.UpdateSalaryAdvance(year, week, LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(24.24m));
            savedWeek = projectLogBookUser.FindWeek(year, week);

            Assert.NotNull(savedWeek);
            Assert.False(savedWeek?.SalaryAdvance.IsEmpty());
            Assert.Equal(LogBookSalaryAdvanceRole.Participant, savedWeek?.SalaryAdvance.Role);
            Assert.Equal(24.24m, savedWeek?.SalaryAdvance.Amount?.Value);
        }

        [Fact]
        public void GivenWeekWithSalaryAdvance_WhenUpdateSalaryAdvanceWithZero_ThenWeekHasNoSalaryAdvance()
        {
            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());

            var year = LogBookYear.Create(2399);
            var week = LogBookWeek.Create(1);
            var hoursToRegister = new List<ProjectLogBookDay>
            {
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2399, 1, 5)),
                    LogBookTime.Create(LogBookHours.Create(7), LogBookMinutes.Create(15)))
            };

            projectLogBookUser.RegisterWeek(year, week, LogBookNote.Empty(), hoursToRegister);
            projectLogBookUser.UpdateSalaryAdvance(year, week, LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(42.42m));
            var savedWeek = projectLogBookUser.FindWeek(year, week);

            Assert.NotNull(savedWeek);
            Assert.False(savedWeek?.SalaryAdvance.IsEmpty());
            Assert.Equal(LogBookSalaryAdvanceRole.Apprentice, savedWeek?.SalaryAdvance.Role);
            Assert.Equal(42.42m, savedWeek?.SalaryAdvance.Amount?.Value);

            projectLogBookUser.UpdateSalaryAdvance(year, week, LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(0));
            savedWeek = projectLogBookUser.FindWeek(year, week);

            Assert.NotNull(savedWeek);
            Assert.True(savedWeek?.SalaryAdvance.IsEmpty());
            Assert.Null(savedWeek?.SalaryAdvance.Role);
            Assert.Null(savedWeek?.SalaryAdvance.Amount?.Value);
        }

        [Fact]
        public void Given3WeeksWithSalaryAdvance_GetSalaryAdvanceInTheWeeks()
        {
            var year = LogBookYear.Create(2399);
            var week1 = LogBookWeek.Create(4);
            decimal week1Amount = 150;
            var week2 = LogBookWeek.Create(8);
            decimal week2Amount = 160;
            var week3 = LogBookWeek.Create(12);
            decimal week3Amount = 170;

            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());

            projectLogBookUser.UpdateSalaryAdvance(year, week2, LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(week2Amount));
            projectLogBookUser.UpdateSalaryAdvance(year, week3, LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(week3Amount));
            projectLogBookUser.UpdateSalaryAdvance(year, week1, LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(week1Amount));

            var salaryAdvance1 = projectLogBookUser.FindFromAndToSalaryAdvance(year, week1);
            Assert.NotNull(salaryAdvance1.start);
            Assert.Equal(week1.Value, salaryAdvance1.start?.Week.Value);
            Assert.NotNull(salaryAdvance1.start?.SalaryAdvance.Amount);
            Assert.Equal(week1Amount, salaryAdvance1.start?.SalaryAdvance.Amount?.Value);
            Assert.Equal(LogBookSalaryAdvanceRole.Apprentice, salaryAdvance1.start?.SalaryAdvance.Role);
            Assert.NotNull(salaryAdvance1.end);
            Assert.Equal(week2.Value, salaryAdvance1.end?.Week.Value);

            var salaryAdvance2 = projectLogBookUser.FindFromAndToSalaryAdvance(year, week2);
            Assert.NotNull(salaryAdvance2.start);
            Assert.Equal(week2.Value, salaryAdvance2.start?.Week.Value);
            Assert.NotNull(salaryAdvance2.start?.SalaryAdvance.Amount);
            Assert.Equal(week2Amount, salaryAdvance2.start?.SalaryAdvance.Amount?.Value);
            Assert.Equal(LogBookSalaryAdvanceRole.Apprentice, salaryAdvance2.start?.SalaryAdvance.Role);
            Assert.NotNull(salaryAdvance2.end);
            Assert.Equal(week3.Value, salaryAdvance2.end?.Week.Value);
        }

        [Fact]
        public void Given3WeeksWithSalaryAdvance_GetSalaryAdvanceInTheWeeksBeforeAndAfter()
        {
            var year = LogBookYear.Create(2399);
            var week0 = LogBookWeek.Create(2);
            var week1 = LogBookWeek.Create(4);
            decimal week1Amount = 150;
            var week2 = LogBookWeek.Create(8);
            decimal week2Amount = 160;
            var week3 = LogBookWeek.Create(12);
            decimal week3Amount = 170;

            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());

            projectLogBookUser.UpdateSalaryAdvance(year, week2, LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(week2Amount));
            projectLogBookUser.UpdateSalaryAdvance(year, week3, LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(week3Amount));
            projectLogBookUser.UpdateSalaryAdvance(year, week1, LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(week1Amount));

            var salaryAdvanceBeforeWeek1 = projectLogBookUser.FindFromAndToSalaryAdvance(year, week0);
            Assert.Null(salaryAdvanceBeforeWeek1.start);
            Assert.NotNull(salaryAdvanceBeforeWeek1.end);
            Assert.Equal(week1.Value, salaryAdvanceBeforeWeek1.end?.Week.Value);

            var salaryAdvanceAfterWeek2 = projectLogBookUser.FindFromAndToSalaryAdvance(year, week3);
            Assert.NotNull(salaryAdvanceAfterWeek2.start);
            Assert.Equal(week3.Value, salaryAdvanceAfterWeek2.start?.Week.Value);
            Assert.Equal(LogBookSalaryAdvanceRole.Apprentice, salaryAdvanceAfterWeek2.start?.SalaryAdvance.Role);
            Assert.NotNull(salaryAdvanceAfterWeek2.start?.SalaryAdvance.Amount);
            Assert.Equal(week3Amount, salaryAdvanceAfterWeek2.start?.SalaryAdvance.Amount?.Value);
            Assert.Null(salaryAdvanceAfterWeek2.end);
        }

        [Fact]
        public void Given2WeeksWithSalaryAdvanceInDiffrenYears_GetSalaryAdvanceBetween()
        {
            var year1 = LogBookYear.Create(2100);
            var yearBetween = LogBookYear.Create(2200);
            var year2 = LogBookYear.Create(2300);

            var week = LogBookWeek.Create(4);

            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());

            projectLogBookUser.UpdateSalaryAdvance(year1, week, LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(150));
            projectLogBookUser.UpdateSalaryAdvance(year2, week, LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(250));

            var salaryAdvance = projectLogBookUser.FindFromAndToSalaryAdvance(yearBetween, week);
            Assert.NotNull(salaryAdvance.start);
            Assert.Equal(week.Value, salaryAdvance.start?.Week.Value);
            Assert.NotNull(salaryAdvance.start?.SalaryAdvance.Amount);
            Assert.Equal(150, salaryAdvance.start?.SalaryAdvance.Amount?.Value);
            Assert.Equal(LogBookSalaryAdvanceRole.Apprentice, salaryAdvance.start?.SalaryAdvance.Role);
            Assert.NotNull(salaryAdvance.end);
            Assert.Equal(week.Value, salaryAdvance.end?.Week.Value);
        }

        [Fact]
        public void GivenNoWeeksWithNoSalaryAdvance_GetSalaryAdvanceExpectNothing()
        {
            var year = LogBookYear.Create(2399);
            var week = LogBookWeek.Create(4);

            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());

            var salaryAdvance = projectLogBookUser.FindFromAndToSalaryAdvance(year, week);
            Assert.Null(salaryAdvance.start);
            Assert.Null(salaryAdvance.end);
        }

        [Fact]
        public void GivenNoWeeksWithNoRegistredLogBookDays_WhenGetSummaryInPeriod_SummationIsZero()
        {
            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());
            var summary = projectLogBookUser.GetSumClosedHoursInPeriod(new DateOnly(2023, 1, 23), new DateOnly(2023, 2, 10));
            Assert.Equal(0, summary.GetHours().Value);
            Assert.Equal(0, summary.GetMinutes().Value);
        }

        [Fact]
        public void GivenNoClosedWithRegistredLogBookDays_WhenGetSummaryInPeriod_SummationIsZero()
        {
            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(4), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 23, 7, 0),
                    Any.ProjectLogBookDay(2023, 1, 24, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 25, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 26, 7, 45),
                    Any.ProjectLogBookDay(2023, 1, 27, 6, 0)
                });
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(5), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 30, 7, 0),
                    Any.ProjectLogBookDay(2023, 1, 31, 7, 30),
                    Any.ProjectLogBookDay(2023, 2, 1, 7, 30),
                    Any.ProjectLogBookDay(2023, 2, 2, 7, 45),
                    Any.ProjectLogBookDay(2023, 2, 3, 6, 0)
                });
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(6), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 2, 6, 7, 0),
                    Any.ProjectLogBookDay(2023, 2, 7, 7, 30),
                    Any.ProjectLogBookDay(2023, 2, 8, 7, 30),
                    Any.ProjectLogBookDay(2023, 2, 9, 7, 45),
                    Any.ProjectLogBookDay(2023, 2, 10, 6, 0)
                });
            var summary = projectLogBookUser.GetSumClosedHoursInPeriod(new DateOnly(2023, 1, 23), new DateOnly(2023, 2, 10));
            Assert.Equal(0, summary.GetHours().Value);
            Assert.Equal(0, summary.GetMinutes().Value);
        }
        
        [Fact]
        public void GivenClosedWeeksWithRegistredLogBookDays_WhenGetSummaryInPeriod_SummationHasValue()
        {
            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(4), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 23, 7, 0),
                    Any.ProjectLogBookDay(2023, 1, 24, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 25, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 26, 7, 45),
                    Any.ProjectLogBookDay(2023, 1, 27, 6, 0)
                });
            projectLogBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(4)); // closed 35h45m
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(5), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 30, 7, 0),
                    Any.ProjectLogBookDay(2023, 1, 31, 7, 30),
                    Any.ProjectLogBookDay(2023, 2, 1, 7, 30),
                    Any.ProjectLogBookDay(2023, 2, 2, 7, 45),
                    Any.ProjectLogBookDay(2023, 2, 3, 6, 0)
                });
            projectLogBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(5)); // closed 35h45m
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(6), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 2, 6, 7, 0),
                    Any.ProjectLogBookDay(2023, 2, 7, 7, 30),
                    Any.ProjectLogBookDay(2023, 2, 8, 7, 30),
                    Any.ProjectLogBookDay(2023, 2, 9, 7, 45),
                    Any.ProjectLogBookDay(2023, 2, 10, 6, 0)
                });
            projectLogBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(6)); // closed 35h45m
            projectLogBookUser.SumClosedHours();
            
            var summary = projectLogBookUser.GetSumClosedHoursInPeriod(new DateOnly(2023, 1, 23), new DateOnly(2023, 2, 10));
            Assert.Equal(107, summary.GetHours().Value);
            Assert.Equal(15, summary.GetMinutes().Value);
        }
        
        [Fact]
        public void GivenClosedWeeksWithRegistredLogBookDays_WhenGetSummaryInPeriodCenterOfWeeks_SummationHasValue()
        {
            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(4), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 23, 7, 0),
                    Any.ProjectLogBookDay(2023, 1, 24, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 25, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 26, 7, 45),
                    Any.ProjectLogBookDay(2023, 1, 27, 6, 0)
                });
            projectLogBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(4)); // closed 35h45m
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(5), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 30, 7, 0),
                    Any.ProjectLogBookDay(2023, 1, 31, 7, 30),
                    Any.ProjectLogBookDay(2023, 2, 1, 7, 30),
                    Any.ProjectLogBookDay(2023, 2, 2, 7, 45),
                    Any.ProjectLogBookDay(2023, 2, 3, 6, 0)
                });
            projectLogBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(5)); // closed 35h45m
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(6), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 2, 6, 7, 0),
                    Any.ProjectLogBookDay(2023, 2, 7, 7, 30),
                    Any.ProjectLogBookDay(2023, 2, 8, 7, 30),
                    Any.ProjectLogBookDay(2023, 2, 9, 7, 45),
                    Any.ProjectLogBookDay(2023, 2, 10, 6, 0)
                });
            projectLogBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(6)); // closed 35h45m
            projectLogBookUser.SumClosedHours();
            
            var summary = projectLogBookUser.GetSumClosedHoursInPeriod(new DateOnly(2023, 1, 25), new DateOnly(2023, 2, 8));
            Assert.Equal(79, summary.GetHours().Value);
            Assert.Equal(0, summary.GetMinutes().Value);
        }
        
        [Fact]
        public void GivenClosedWeeksWithRegistredLogBookDays_WhenGetSummaryInSingleDay_SummationHasValue()
        {
            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(4), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 23, 7, 0),
                    Any.ProjectLogBookDay(2023, 1, 24, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 25, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 26, 7, 45),
                    Any.ProjectLogBookDay(2023, 1, 27, 6, 0)
                });
            projectLogBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(4)); // closed 35h45m
            projectLogBookUser.SumClosedHours();
            
            var summary = projectLogBookUser.GetSumClosedHoursInPeriod(new DateOnly(2023, 1, 25), new DateOnly(2023, 1, 25));
            Assert.Equal(7, summary.GetHours().Value);
            Assert.Equal(30, summary.GetMinutes().Value);
        }
        
        [Fact]
        public void GivenClosedWeeksWithRegistredLogBookDays_WhenGetSummaryIn2DaysPeriodInWeek_SummationHasValue()
        {
            var projectLogBookUser = ProjectLogBookUser.Create(Any.LogBookName(), Any.Guid());
            projectLogBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(4), LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 23, 7, 0),
                    Any.ProjectLogBookDay(2023, 1, 24, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 25, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 26, 7, 45),
                    Any.ProjectLogBookDay(2023, 1, 27, 6, 0)
                });
            projectLogBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(4));
            projectLogBookUser.SumClosedHours();
            
            var summary = projectLogBookUser.GetSumClosedHoursInPeriod(new DateOnly(2023, 1, 24), new DateOnly(2023, 1, 25));
            Assert.Equal(15, summary.GetHours().Value);
            Assert.Equal(0, summary.GetMinutes().Value);
        }
    }
}
