using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Application.GetProjectLogBookWeek;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.projectLogBook
{
    public class ProjectLogBookWeekTests
    {
        [Fact]
        public void Initial_Week()
        {
            var week = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            Assert.Equal(7, week.LogBookDays.Count);
        }

        [Fact]
        public void Can_Register_Week_With_Note()
        {
            var week = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            week.RegisterWeek(LogBookNote.Create(""), new List<ProjectLogBookDay>());
            Assert.Equal(LogBookNote.Create(""), week.Note);
        }
        
        [Fact]
        public void Given_Closed_Week_Cannot_Register()
        {
            var week = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            week.CloseWeek();
            Assert.Throws<InvalidOperationException>(() => week.RegisterWeek(LogBookNote.Create(""), new List<ProjectLogBookDay>()));
        }

        [Fact]
        public void Can_Update_Note_On_Week()
        {
            var week = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            week.RegisterWeek(LogBookNote.Create(""), new List<ProjectLogBookDay>());
            week.RegisterWeek(LogBookNote.Create("new"), new List<ProjectLogBookDay>());
            Assert.Equal(LogBookNote.Create("new"), week.Note);
        }
        
        [Fact]
        public void Find_Week_Days()
        {
            var projectLogBookWeek = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            var findWeekDays = ProjectLogBookWeek.FindWeekDays(LogBookYear.Create(2022), LogBookWeek.Create(34));

            Assert.Equal(7, findWeekDays.Count);
            Assert.Equal(new DateOnly(2022, 8, 22), findWeekDays[0]);
            Assert.Equal(new DateOnly(2022, 8, 23), findWeekDays[1]);
            Assert.Equal(new DateOnly(2022, 8, 24), findWeekDays[2]);
            Assert.Equal(new DateOnly(2022, 8, 25), findWeekDays[3]);
            Assert.Equal(new DateOnly(2022, 8, 26), findWeekDays[4]);
            Assert.Equal(new DateOnly(2022, 8, 27), findWeekDays[5]);
            Assert.Equal(new DateOnly(2022, 8, 28), findWeekDays[6]);
        }

        [Fact]
        public void Find_Week_Invalid_Parameters()
        {
            var projectLogBookWeek = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            Assert.Throws<ArgumentException>(() => ProjectLogBookWeek.FindWeekDays(LogBookYear.Create(-2022), LogBookWeek.Create(34)));
            Assert.Throws<ArgumentException>(() => ProjectLogBookWeek.FindWeekDays(LogBookYear.Create(2022), LogBookWeek.Create(0)));
            Assert.Throws<ArgumentException>(() => ProjectLogBookWeek.FindWeekDays(LogBookYear.Create(2022), LogBookWeek.Create(60)));
        }

        [Fact]
        public void Find_Short_Week()
        {
            var projectLogBookWeek = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            var findLastWeek2021 = ProjectLogBookWeek.FindWeekDays(LogBookYear.Create(2021), LogBookWeek.Create(52));
            var findFirstWeek2022 = ProjectLogBookWeek.FindWeekDays(LogBookYear.Create(2022), LogBookWeek.Create(1));
            Assert.Equal(7, findLastWeek2021.Count);
            Assert.Equal(7, findFirstWeek2022.Count);
            Assert.Equal(new DateOnly(2021, 12, 27), findLastWeek2021[0]);
            Assert.Equal(new DateOnly(2022, 1, 2), findLastWeek2021[6]);
            Assert.Equal(new DateOnly(2022, 1, 3), findFirstWeek2022[0]);
            Assert.Equal(new DateOnly(2022, 1, 9), findFirstWeek2022[6]);
        }

        [Fact]
        public void Week_53_Should_Be_Week_1_Next_Year_If_Week_53_Doesnt_Exist()
        {
            var projectLogBookWeek = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            var nonExistingWeekDays = ProjectLogBookWeek.FindWeekDays(LogBookYear.Create(2022), LogBookWeek.Create(53));
            var firstWeekNextYear = ProjectLogBookWeek.FindWeekDays(LogBookYear.Create(2023), LogBookWeek.Create(1));
            Assert.Equal(nonExistingWeekDays, firstWeekNextYear);
        }

        [Fact]
        public void More_Than_Seven_Week_Days_Should_Not_Be_Allowed()
        {
            var week = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            Assert.Throws<ArgumentException>(() =>week.RegisterWeek(LogBookNote.Create(""), Enumerable.Repeat(ProjectLogBookDay.Create(LogBookDate.Today(),  LogBookTime.Create(LogBookHours.Create(3), LogBookMinutes.Create(3))), 8).ToList()));
        }

        [Fact]
        public void Less_Than_Seven_Week_Days_Should_Be_Handled()
        {
            var week = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            week.RegisterWeek(LogBookNote.Create(""), new List<ProjectLogBookDay>(){ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 8, 22)), LogBookTime.Create(LogBookHours.Create(3), LogBookMinutes.Create(10)))});
            Assert.Equal(7,week.LogBookDays.Count);
            Assert.Equal(3, week.LogBookDays[0].Time.GetHours().Value);
            Assert.Equal(10, week.LogBookDays[0].Time.GetMinutes().Value);
            
            Assert.True(week.LogBookDays.Skip(1).All(day => day.Time.GetHours().Value == 0 && day.Time.GetMinutes().Value == 0));
        }
        
        [Fact]
        public void Less_Than_Seven_Week_Days_Should_Keep_Old_Registration()
        {
            var week = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            week.RegisterWeek(LogBookNote.Create(""), new List<ProjectLogBookDay>(){ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 8, 22)), LogBookTime.Create(LogBookHours.Create(3), LogBookMinutes.Create(10)))});
            week.RegisterWeek(LogBookNote.Create(""), new List<ProjectLogBookDay>(){ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 8, 23)), LogBookTime.Create(LogBookHours.Create(7), LogBookMinutes.Create(8)))});
            Assert.Equal(7,week.LogBookDays.Count);
            Assert.Equal(3, week.LogBookDays[0].Time.GetHours().Value);
            Assert.Equal(10, week.LogBookDays[0].Time.GetMinutes().Value);
            Assert.Equal(7, week.LogBookDays[1].Time.GetHours().Value);
            Assert.Equal(8, week.LogBookDays[1].Time.GetMinutes().Value);
            
            Assert.True(week.LogBookDays.Skip(2).All(day => day.Time.GetHours().Value == 0 && day.Time.GetMinutes().Value == 0));
        }

        [Fact]
        public void Invalid_Hours_And_Minutes_Not_Allowed()
        {
            Assert.Throws<ArgumentException>(() => ProjectLogBookDay.Create(LogBookDate.Today(), LogBookTime.Create(LogBookHours.Create(3), LogBookMinutes.Create(-3))));
            Assert.Throws<ArgumentException>(() => ProjectLogBookDay.Create(LogBookDate.Today(), LogBookTime.Create(LogBookHours.Create(-3), LogBookMinutes.Create(3))));
            Assert.Throws<ArgumentException>(() => ProjectLogBookDay.Create(LogBookDate.Today(), LogBookTime.Create(LogBookHours.Create(3), LogBookMinutes.Create(70))));
            Assert.Throws<ArgumentException>(() => ProjectLogBookDay.Create(LogBookDate.Today(), LogBookTime.Create(LogBookHours.Create(24), LogBookMinutes.Create(1)))); 
            ProjectLogBookDay.Create(LogBookDate.Today(), LogBookTime.Create(LogBookHours.Create(24), LogBookMinutes.Create(0))); 
            ProjectLogBookDay.Create(LogBookDate.Today(), LogBookTime.Create(LogBookHours.Create(0), LogBookMinutes.Create(0)));
        }

        [Fact]
        public void Created_Date_Not_MatchingWeek_Should_Throw_Exception()
        {
            var week = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            Assert.Throws<ArgumentException>(() =>week.RegisterWeek(LogBookNote.Create(""), new List<ProjectLogBookDay>(){ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 2, 23)), LogBookTime.Create(LogBookHours.Create(3), LogBookMinutes.Create(8)))}));
        }

        [Fact]
        public void Week_Should_Be_Registered_To_User()
        {
            var week = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            var logBookUser = ProjectLogBookUser.Create(LogBookName.Create("user1"), Guid.NewGuid()); 
            logBookUser.RegisterWeek(week.Year, week.Week, week.Note, week.LogBookDays);
            var newLogBookUser = ProjectLogBookUser.Create(LogBookName.Create("user2"), Guid.NewGuid());
            Assert.NotEqual(logBookUser.ProjectLogBookWeeks, newLogBookUser.ProjectLogBookWeeks);
        }

        [Fact]
        public void User_Should_Not_Be_Able_To_Override_Another_Users_Weeks()
        {
            var week = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            var logBookUser = ProjectLogBookUser.Create(LogBookName.Create("user1"), Guid.NewGuid()); 
            logBookUser.RegisterWeek(week.Year, week.Week, week.Note, week.LogBookDays);
            var newLogBookUser = ProjectLogBookUser.Create(LogBookName.Create("user2"), Guid.NewGuid());
            newLogBookUser.RegisterWeek(week.Year, week.Week, week.Note, week.LogBookDays);
            Assert.NotEqual(logBookUser.ProjectLogBookWeeks, newLogBookUser.ProjectLogBookWeeks);
        }

        [Fact]
        public void Get_Week_Summation()
        {
            var week = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            week.RegisterWeek(LogBookNote.Create(""), new List<ProjectLogBookDay>(){ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 8, 22)), LogBookTime.Create(LogBookHours.Create(3), LogBookMinutes.Create(51)))});
            week.RegisterWeek(LogBookNote.Create(""), new List<ProjectLogBookDay>(){ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 8, 23)), LogBookTime.Create(LogBookHours.Create(7), LogBookMinutes.Create(11)))});

            var summation = week.GetWeekSummation();
            Assert.Equal(11, summation.GetHours().Value);
            Assert.Equal(2, summation.GetMinutes().Value);
        }
        
        [Fact]
        public async Task Total_Hours_From_Last_Week_Should_Be_Visible_On_Empty_Current_Week()
        {
            var executionContext = new FakeExecutionContext();
            var projectRepository = new ProjectInMemoryRepository();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            var logBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var logBookUser = ProjectLogBookUser.Create(LogBookName.Create(executionContext.UserName), executionContext.UserId);
            var note = LogBookNote.Empty();

            //Register week before and after current week
            logBook.RegisterWeek(logBookUser, LogBookYear.Create(2022), LogBookWeek.Create(34), note, new List<ProjectLogBookDay>()
            {
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 8, 22)), LogBookTime.Create(LogBookHours.Create(5), LogBookMinutes.Create(15))),
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 8, 23)), LogBookTime.Create(LogBookHours.Create(8), LogBookMinutes.Create(30)))
            } );
            
            logBook.RegisterWeek(logBookUser, LogBookYear.Create(2022), LogBookWeek.Create(36), note, new List<ProjectLogBookDay>()
            {
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 9, 5)), LogBookTime.Create(LogBookHours.Create(4), LogBookMinutes.Create(5))),
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 9, 6)), LogBookTime.Create(LogBookHours.Create(9), LogBookMinutes.Create(10)))
            } );
            
            //Close both weeks and sum closed hours
            logBook.CloseWeek(logBookUser, LogBookYear.Create(2022), LogBookWeek.Create(34));
            logBook.CloseWeek(logBookUser, LogBookYear.Create(2022), LogBookWeek.Create(36));
            logBook.SumClosedHours(logBookUser);

            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await logBookRepository.Add(logBook);

            var query = GetProjectLogBookWeekQuery.Create(logBook.ProjectId, logBookUser.UserId, 2022, 35);
            var handler = new GetProjectLogBookWeekQueryHandler(projectRepository, executionContext, logBookRepository, new SystemTime());
            var response = await handler.Handle(query, CancellationToken.None);
            
            //Assert hours and minutes from earlier closed week are shown in current week
            Assert.NotNull(response);
            Assert.Equal(35, response.Week);
            Assert.Equal(13, response.ClosedWeeksSummation.Hours);
            Assert.Equal(45, response.ClosedWeeksSummation.Minutes);
        }
        
        [Fact]
        public void Update_Closed_Hours_Summation()
        {
            var logBook = Any.ProjectLogBook();
            var user = ProjectLogBookUser.Create(LogBookName.Create("Hans"), new Guid());
            var note = LogBookNote.Empty();
            
            // Register hours on 3 different weeks
            logBook.RegisterWeek(user, LogBookYear.Create(2022), LogBookWeek.Create(28), note, new List<ProjectLogBookDay>()
            {
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 7, 13)), LogBookTime.Create(LogBookHours.Create(5), LogBookMinutes.Create(15))),
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 7, 15)), LogBookTime.Create(LogBookHours.Create(7), LogBookMinutes.Create(45)))
            });
            
            logBook.RegisterWeek(user, LogBookYear.Create(2022), LogBookWeek.Create(32), note, new List<ProjectLogBookDay>()
            {
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 8, 10)), LogBookTime.Create(LogBookHours.Create(1), LogBookMinutes.Create(15))),
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 8, 12)), LogBookTime.Create(LogBookHours.Create(2), LogBookMinutes.Create(30)))
            });
            
            logBook.RegisterWeek(user, LogBookYear.Create(2022), LogBookWeek.Create(23), note, new List<ProjectLogBookDay>()
            {
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 6, 7)), LogBookTime.Create(LogBookHours.Create(0), LogBookMinutes.Create(15))),
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 6, 8)), LogBookTime.Create(LogBookHours.Create(4), LogBookMinutes.Create(40)))
            });
            
            // Close two weeks and calculate summation
            logBook.CloseWeek(user, LogBookYear.Create(2022), LogBookWeek.Create(32));
            logBook.CloseWeek(user, LogBookYear.Create(2022), LogBookWeek.Create(28));
            logBook.SumClosedHours(user);
            
            // Assert summation on all three weeks
            var week23 = logBook.FindWeek(user.UserId, LogBookYear.Create(2022), LogBookWeek.Create(23));
            var week28 = logBook.FindWeek(user.UserId, LogBookYear.Create(2022), LogBookWeek.Create(28));
            var week32 = logBook.FindWeek(user.UserId, LogBookYear.Create(2022), LogBookWeek.Create(32));
            
            Assert.Equal(0, week23!.ClosedHoursSummation.GetHours().Value);
            Assert.Equal(0, week23!.ClosedHoursSummation.GetMinutes().Value);
            
            Assert.Equal(13, week28!.ClosedHoursSummation.GetHours().Value);
            Assert.Equal(0, week28!.ClosedHoursSummation.GetMinutes().Value);
            
            Assert.Equal(16, week32!.ClosedHoursSummation.GetHours().Value);
            Assert.Equal(45, week32!.ClosedHoursSummation.GetMinutes().Value);
            
            // Reopen single week
            logBook.OpenWeek(user, LogBookYear.Create(2022), LogBookWeek.Create(32));
            logBook.SumClosedHours(user);
            
            // Assert summation on all three weeks
            week23 = logBook.FindWeek(user.UserId, LogBookYear.Create(2022), LogBookWeek.Create(23));
            week28 = logBook.FindWeek(user.UserId, LogBookYear.Create(2022), LogBookWeek.Create(28));
            week32 = logBook.FindWeek(user.UserId, LogBookYear.Create(2022), LogBookWeek.Create(32));
            
            Assert.Equal(0, week23!.ClosedHoursSummation.GetHours().Value);
            Assert.Equal(0, week23!.ClosedHoursSummation.GetMinutes().Value);
            
            Assert.Equal(13, week28!.ClosedHoursSummation.GetHours().Value);
            Assert.Equal(0, week28!.ClosedHoursSummation.GetMinutes().Value);
            
            Assert.Equal(13, week32!.ClosedHoursSummation.GetHours().Value);
            Assert.Equal(0, week32!.ClosedHoursSummation.GetMinutes().Value);
        }

        [Fact]
        public void When_Week_Closed_Should_Raise_Event()
        {
            var logBook = Any.ProjectLogBook();
            var user = ProjectLogBookUser.Create(new Guid());
            var year = LogBookYear.Create(2022);
            var week = LogBookWeek.Create(29);
            logBook.CloseWeek(user, year, week);

            Assert.IsType<ProjectLogBookWeekClosedDomainEvent>(logBook.PublishedEvent<ProjectLogBookWeekClosedDomainEvent>());
        }
        
        [Fact]
        public void Close_Already_Closed_Week_Should_Not_Raise_Event()
        {
            var logBook = Any.ProjectLogBook();
            var user = ProjectLogBookUser.Create(new Guid());
            var year = LogBookYear.Create(2022);
            var week = LogBookWeek.Create(29);
            logBook.CloseWeek(user, year, week);
            logBook.CloseWeek(user, year, week);

            Assert.Equal(1, logBook.DomainEvents.Count(e => e is ProjectLogBookWeekClosedDomainEvent));
        }
        
        [Fact]
        public void ReOpen_Closed_Week_Should_Raise_Event()
        {
            var logBook = Any.ProjectLogBook();
            var user = ProjectLogBookUser.Create(new Guid());
            var year = LogBookYear.Create(2022);
            var week = LogBookWeek.Create(29);
            logBook.CloseWeek(user, year, week);
            logBook.OpenWeek(user, year, week);

            Assert.IsType<ProjectLogBookWeekOpenedDomainEvent>(logBook.PublishedEvent<ProjectLogBookWeekOpenedDomainEvent>());
        }
        
        [Fact]
        public void Register_Week_Should_Include_Closed_Hours_Summation()
        {
            var logBook = Any.ProjectLogBook();
            var user = ProjectLogBookUser.Create(new Guid());
            var year = LogBookYear.Create(2022);
            var week = LogBookWeek.Create(29);
            var projectLogBookDay = ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2022, 7, 18)), LogBookTime.Create(LogBookHours.Create(1), LogBookMinutes.Empty()));
            logBook.RegisterWeek(user, year, week, LogBookNote.Empty(), new List<ProjectLogBookDay>() { projectLogBookDay });
            logBook.CloseWeek(user, year, week);
            logBook.SumClosedHours(user);
            
            var weekAfter = LogBookWeek.Create(30);
            logBook.RegisterWeek(user, year, weekAfter, LogBookNote.Empty(), new List<ProjectLogBookDay>());
            var weekAfterRegistered = logBook.FindWeek(user.UserId, year, weekAfter);
            
            Assert.Equal(1, weekAfterRegistered!.ClosedHoursSummation.GetHours().Value);
        }
        
        [Fact]
        public void Not_Registered_Salary_Advance_on_Week()
        {
            var week = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            Assert.True(week.SalaryAdvance.IsEmpty());
        }

        [Fact]
        public void Log_Book_Salary_Amount_Can_Not_Less_Then_Zero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => LogBookSalaryAdvanceAmount.Create(-1));
        }

        [Fact]
        public void Register_Salary_Advance_on_Week_As_Participant()
        {
            var week = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            week.UpdateSalaryAdvance(LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(42));
            Assert.False(week.SalaryAdvance.IsEmpty());
            Assert.Equal(42, week.SalaryAdvance.Amount?.Value);
            Assert.Equal(LogBookSalaryAdvanceRole.Participant, week.SalaryAdvance.Role);
            
            // Overwrite parameters
            week.UpdateSalaryAdvance(LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(24));
            Assert.False(week.SalaryAdvance.IsEmpty());
            Assert.Equal(24, week.SalaryAdvance.Amount?.Value);
            Assert.Equal(LogBookSalaryAdvanceRole.Apprentice, week.SalaryAdvance.Role);
        }
        
        [Fact]
        public void Register_Salary_Advance_on_Week_As_Apprentice()
        {
            var week = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            week.UpdateSalaryAdvance(LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(42));
            Assert.False(week.SalaryAdvance.IsEmpty());
            Assert.Equal(42, week.SalaryAdvance.Amount?.Value);
            Assert.Equal(LogBookSalaryAdvanceRole.Apprentice, week.SalaryAdvance.Role);
            
            // Overwrite parameters
            week.UpdateSalaryAdvance(LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(24));
            Assert.False(week.SalaryAdvance.IsEmpty());
            Assert.Equal(24, week.SalaryAdvance.Amount?.Value);
            Assert.Equal(LogBookSalaryAdvanceRole.Participant, week.SalaryAdvance.Role);
        }

        [Fact]
        public void Register_And_Clear_Salary_Advance_On_Week()
        {
            var week = ProjectLogBookWeek.Create(LogBookYear.Create(2022), LogBookWeek.Create(34));
            week.UpdateSalaryAdvance(LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(42));
            Assert.False(week.SalaryAdvance.IsEmpty());
            Assert.Equal(42, week.SalaryAdvance.Amount?.Value);
            Assert.Equal(LogBookSalaryAdvanceRole.Apprentice, week.SalaryAdvance.Role);
            
            // Overwrite parameters
            week.UpdateSalaryAdvance(LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(0));
            Assert.True(week.SalaryAdvance.IsEmpty());
        }

        [Fact]
        public void Get_Summery_Period_For_Whole_Week()
        {
            var week = Any.ProjectLogBookWeek(2023, 4);
            week.RegisterWeek(LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 23, 7, 0),
                    Any.ProjectLogBookDay(2023, 1, 24, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 25, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 26, 7, 45),
                    Any.ProjectLogBookDay(2023, 1, 27, 6, 0)
                });
            var amount = week.GetSumHoursInPeriod(new DateOnly(2023, 1, 23), new DateOnly(2023, 1, 27));
            Assert.Equal(35, amount.GetHours().Value);
            Assert.Equal(45, amount.GetMinutes().Value);
        }
        
        [Fact]
        public void Get_Summery_Period_For_Whole_Week_Nothing_Registred()
        {
            var week = Any.ProjectLogBookWeek(2023, 4);
            var amount = week.GetSumHoursInPeriod(new DateOnly(2023, 1, 23), new DateOnly(2023, 1, 27));
            Assert.Equal(0, amount.GetHours().Value);
            Assert.Equal(0, amount.GetMinutes().Value);
        }
        
        [Fact]
        public void Get_Summery_Period_Single_Day()
        {
            var week = Any.ProjectLogBookWeek(2023, 4);
            week.RegisterWeek(LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 23, 7, 0),
                    Any.ProjectLogBookDay(2023, 1, 24, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 25, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 26, 7, 45),
                    Any.ProjectLogBookDay(2023, 1, 27, 6, 0)
                });
            var amount = week.GetSumHoursInPeriod(new DateOnly(2023, 1, 24), new DateOnly(2023, 1, 24));
            Assert.Equal(7, amount.GetHours().Value);
            Assert.Equal(30, amount.GetMinutes().Value);
        }
        
        [Fact]
        public void Get_Summery_Period_2_Days()
        {
            var week = Any.ProjectLogBookWeek(2023, 4);
            week.RegisterWeek(LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 23, 7, 0),
                    Any.ProjectLogBookDay(2023, 1, 24, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 25, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 26, 7, 45),
                    Any.ProjectLogBookDay(2023, 1, 27, 6, 0)
                });
            var amount = week.GetSumHoursInPeriod(new DateOnly(2023, 1, 24), new DateOnly(2023, 1, 25));
            Assert.Equal(15, amount.GetHours().Value);
            Assert.Equal(0, amount.GetMinutes().Value);
        }
        
        [Fact]
        public void Get_Summery_Period_In_Weekend()
        {
            var week = Any.ProjectLogBookWeek(2023, 4);
            week.RegisterWeek(LogBookNote.Empty(),
                new List<ProjectLogBookDay>()
                {
                    Any.ProjectLogBookDay(2023, 1, 23, 7, 0),
                    Any.ProjectLogBookDay(2023, 1, 24, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 25, 7, 30),
                    Any.ProjectLogBookDay(2023, 1, 26, 7, 45),
                    Any.ProjectLogBookDay(2023, 1, 27, 6, 0)
                });
            var amount = week.GetSumHoursInPeriod(new DateOnly(2023, 1, 28), new DateOnly(2023, 1, 29));
            Assert.Equal(0, amount.GetHours().Value);
            Assert.Equal(0, amount.GetMinutes().Value);
        }
    }
}
