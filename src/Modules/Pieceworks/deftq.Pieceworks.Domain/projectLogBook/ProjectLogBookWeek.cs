using System.Globalization;
using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class ProjectLogBookWeek : Entity
    {
        public LogBookYear Year { get; private set; }
        public LogBookWeek Week { get; private set; }
        public LogBookNote Note { get; private set; }
        public IList<ProjectLogBookDay> LogBookDays { get; private set; }
        public bool Closed { get; private set; }
        public LogBookTime ClosedHoursSummation { get; private set; }
        public LogBookSalaryAdvance SalaryAdvance { get; private set; }

        private ProjectLogBookWeek()
        {
            Id = Guid.NewGuid();
            Year = LogBookYear.Empty();
            Week = LogBookWeek.Empty();
            Note = LogBookNote.Empty();
            LogBookDays = new List<ProjectLogBookDay>();
            Closed = false;
            ClosedHoursSummation = LogBookTime.Empty();
            SalaryAdvance = LogBookSalaryAdvance.Empty();
        }

        private ProjectLogBookWeek(LogBookYear year, LogBookWeek week, LogBookTime closedHoursSummation)
        {
            Id = Guid.NewGuid();
            Year = year;
            Week = week;
            Note = LogBookNote.Empty();
            LogBookDays = FindWeekDays(Year, Week).Select(d => ProjectLogBookDay.Create(LogBookDate.Create(d), LogBookTime.Empty())).ToList();
            Closed = false;
            ClosedHoursSummation = closedHoursSummation;
            SalaryAdvance = LogBookSalaryAdvance.Empty();
        }

        public static ProjectLogBookWeek Create(LogBookYear year, LogBookWeek week, LogBookTime closedHoursSummation)
        {
            return new ProjectLogBookWeek(year, week, closedHoursSummation);
        }

        public static ProjectLogBookWeek Create(LogBookYear year, LogBookWeek week)
        {
            return new ProjectLogBookWeek(year, week, LogBookTime.Empty());
        }

        public void UpdateClosedHoursSummation(LogBookTime summation)
        {
            ClosedHoursSummation = summation;
        }

        public void RegisterWeek(LogBookNote note, IList<ProjectLogBookDay> logBookDays)
        {
            if (Closed)
            {
                throw new InvalidOperationException("Week is closed");
            }

            ValidateNoMoreThanSevenDays(logBookDays);

            var weekDays = FindWeekDays(Year, Week);

            ValidateAllDaysBelongToWeek(logBookDays, weekDays);

            Note = note;
            var weekDaysWithTime = weekDays.Select(day =>
            {
                var registeredDay = logBookDays.FirstOrDefault(d => day.Equals(d.Date.Value));
                return registeredDay ?? LogBookDays.First(previouslyDay => day.Equals(previouslyDay.Date.Value));
            }).ToList();
            LogBookDays = weekDaysWithTime;
        }

        public void UpdateSalaryAdvance(LogBookSalaryAdvanceRole role, LogBookSalaryAdvanceAmount amount)
        {
            SalaryAdvance = amount.Value <= 0 ? LogBookSalaryAdvance.Empty() : LogBookSalaryAdvance.Create(amount, role);
        }

        private static void ValidateAllDaysBelongToWeek(IList<ProjectLogBookDay> logBookDays, IList<DateOnly> weekDays)
        {
            if (!logBookDays.All(day => weekDays.Any(registerDay => registerDay.Equals(day.Date.Value))))
            {
                throw new ArgumentException($"{nameof(logBookDays)} Can not be outside given week", nameof(logBookDays));
            }
        }

        private static void ValidateNoMoreThanSevenDays(IList<ProjectLogBookDay> logBookDays)
        {
            if (logBookDays.Count > 7)
            {
                throw new ArgumentException($"{nameof(logBookDays)} can not be more than 7", nameof(logBookDays));
            }
        }

        public LogBookTime GetWeekSummation()
        {
            return LogBookDays.Aggregate(LogBookTime.Empty(), (sum, day) => sum.Add(day.Time));
        }

        public bool CloseWeek()
        {
            if (!Closed)
            {
                Closed = true;
                return true;
            }

            return false;
        }

        public bool OpenWeek()
        {
            if (Closed)
            {
                Closed = false;
                return true;
            }

            return false;
        }

        public static IList<DateOnly> FindWeekDays(LogBookYear year, LogBookWeek week)
        {
            if (year is null || year.Value < 1 || year.Value > 9999)
            {
                throw new ArgumentException($"{nameof(year)} must be between 1 and 9999", nameof(year));
            }

            if (week is null || week.Value < 1 || week.Value > 53)
            {
                throw new ArgumentException($"{nameof(week)} must be between 1 and 53", nameof(week));
            }

            var yearStart = ISOWeek.GetYearStart(year.Value);
            var weekStart = new GregorianCalendar().AddWeeks(yearStart, week.Value - 1);

            var weekDays = new List<DateOnly>();
            var weekStartDate = DateOnly.FromDateTime(weekStart);
            weekDays.Add(weekStartDate);
            weekDays.Add(weekStartDate.AddDays(1));
            weekDays.Add(weekStartDate.AddDays(2));
            weekDays.Add(weekStartDate.AddDays(3));
            weekDays.Add(weekStartDate.AddDays(4));
            weekDays.Add(weekStartDate.AddDays(5));
            weekDays.Add(weekStartDate.AddDays(6));

            return weekDays;
        }

        public LogBookTime GetSumHoursInPeriod(DateOnly start, DateOnly end)
        {
            var sum = LogBookTime.Empty();
            foreach (var dayInPeriod in LogBookDays.Where(day => day.Date.Value >= start && day.Date.Value <= end))
            {
                sum = sum.Add(dayInPeriod.Time);
            }
            return sum;
        }
    }

    public class ProjectLogBookWeekComparer : IComparer<ProjectLogBookWeek>
    {
        public int Compare(ProjectLogBookWeek? x, ProjectLogBookWeek? y)
        {
            if (x is null && y is null)
            {
                return 0;
            }

            if (x is null)
            {
                return -1;
            }

            if (y is null)
            {
                return 1;
            }

            if (x.Year.Value.CompareTo(y.Year.Value) != 0)
            {
                return x.Year.Value.CompareTo(y.Year.Value);
            }

            return x.Week.Value.CompareTo(y.Week.Value);
        }
    }
}
