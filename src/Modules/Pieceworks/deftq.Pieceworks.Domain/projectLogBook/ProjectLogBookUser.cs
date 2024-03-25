using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class ProjectLogBookUser : Entity
    {
        public LogBookName Name { get; private set; }
        public Guid UserId { get; private set; }
        public IList<ProjectLogBookWeek> ProjectLogBookWeeks { get; private set; }

        ProjectLogBookUser()
        {
            Id = Guid.NewGuid();
            Name = LogBookName.Empty();
            UserId = Guid.Empty;
            ProjectLogBookWeeks = new List<ProjectLogBookWeek>();
        }

        private ProjectLogBookUser(LogBookName name, Guid userId)
        {
            Id = Guid.NewGuid();
            Name = name;
            UserId = userId;
            ProjectLogBookWeeks = new List<ProjectLogBookWeek>();
        }

        public static ProjectLogBookUser Create(Guid userId)
        {
            return Create(LogBookName.Empty(), userId);
        }

        public static ProjectLogBookUser Create(LogBookName name, Guid userId)
        {
            return new ProjectLogBookUser(name, userId);
        }

        public static ProjectLogBookUser Empty()
        {
            return new ProjectLogBookUser();
        }

        public ProjectLogBookWeek? FindWeek(LogBookYear year, LogBookWeek week)
        {
            return ProjectLogBookWeeks.FirstOrDefault(w => w.Year.Value == year.Value && w.Week.Value == week.Value);
        }

        public (ProjectLogBookWeek? start, ProjectLogBookWeek? end) FindFromAndToSalaryAdvance(LogBookYear year, LogBookWeek week)
        {
            if (ProjectLogBookWeeks.Count == 0)
            {
                return (null, null);
            }

            var weekStart = FindWeekMatchThisWeek(year, week);
            if (weekStart is null || weekStart.SalaryAdvance.IsEmpty())
            {
                weekStart = FindLastWeekOfWeeksBeforeThisWeek(year, week);
            }

            var weekEnd = FindFirstWeekOfWeeksAfterThisWeek(year, week);
            return (weekStart, weekEnd);
        }

        private ProjectLogBookWeek? FindWeekMatchThisWeek(LogBookYear year, LogBookWeek week)
        {
            return ProjectLogBookWeeks.FirstOrDefault(w => w.Year.Value == year.Value && w.Week.Value == week.Value);
        }

        private ProjectLogBookWeek? FindLastWeekOfWeeksBeforeThisWeek(LogBookYear year, LogBookWeek week)
        {
            var weeksBeforeDate =
                ProjectLogBookWeeks.Where(w =>
                    ((w.Year.Value == year.Value && w.Week.Value < week.Value) || w.Year.Value < year.Value) && !w.SalaryAdvance.IsEmpty());
            var weeksBeforeDateOrdered = weeksBeforeDate.OrderBy(w => w, new ProjectLogBookWeekComparer());
            return weeksBeforeDateOrdered.LastOrDefault();
        }

        private ProjectLogBookWeek? FindFirstWeekOfWeeksAfterThisWeek(LogBookYear year, LogBookWeek week)
        {
            var weeksAfterDate =
                ProjectLogBookWeeks.Where(w =>
                    ((w.Year.Value == year.Value && w.Week.Value > week.Value) || w.Year.Value > year.Value) && !w.SalaryAdvance.IsEmpty());
            var weeksAfterDateOrdered = weeksAfterDate.OrderBy(w => w, new ProjectLogBookWeekComparer());
            return weeksAfterDateOrdered.FirstOrDefault();
        }

        public void RegisterWeek(LogBookYear year, LogBookWeek week, LogBookNote note, IList<ProjectLogBookDay> logBookDays)
        {
            var logBookWeek = FindWeek(year, week);
            if (logBookWeek == null)
            {
                var closedHoursSummation = GetSumClosedHours(year, week);
                logBookWeek = ProjectLogBookWeek.Create(year, week, closedHoursSummation);
                ProjectLogBookWeeks.Add(logBookWeek);
            }

            logBookWeek.RegisterWeek(note, logBookDays);
        }

        public void UpdateSalaryAdvance(LogBookYear year, LogBookWeek week, LogBookSalaryAdvanceRole role, LogBookSalaryAdvanceAmount amount)
        {
            var logBookWeek = FindWeek(year, week);
            if (logBookWeek == null)
            {
                var closedHoursSummation = GetSumClosedHours(year, week);
                logBookWeek = ProjectLogBookWeek.Create(year, week, closedHoursSummation);
                ProjectLogBookWeeks.Add(logBookWeek);
            }

            logBookWeek.UpdateSalaryAdvance(role, amount);
        }

        public bool CloseWeek(LogBookYear year, LogBookWeek week)
        {
            var weekToClose = FindWeek(year, week);
            if (weekToClose is null)
            {
                RegisterWeek(year, week, LogBookNote.Empty(), new List<ProjectLogBookDay>());
                return FindWeek(year, week)!.CloseWeek();
            }

            return weekToClose.CloseWeek();
        }

        public bool OpenWeek(LogBookYear year, LogBookWeek week)
        {
            var weekToOpen = FindWeek(year, week);
            if (weekToOpen is not null)
            {
                return weekToOpen.OpenWeek();
            }

            return false;
        }

        public void SumClosedHours()
        {
            var sortedWeeks = GetWeeksSorted();

            LogBookTime summation = LogBookTime.Empty();
            foreach (var week in sortedWeeks.Values)
            {
                if (week.Closed)
                {
                    var closedHours = week.GetWeekSummation();
                    summation = summation.Add(closedHours);
                }

                week.UpdateClosedHoursSummation(summation);
            }
        }

        private SortedList<ProjectLogBookWeek, ProjectLogBookWeek> GetWeeksSorted()
        {
            var sortedWeeks = new SortedList<ProjectLogBookWeek, ProjectLogBookWeek>(new ProjectLogBookWeekComparer());
            foreach (var week in ProjectLogBookWeeks)
            {
                sortedWeeks.Add(week, week);
            }

            return sortedWeeks;
        }

        public LogBookTime GetSumClosedHours()
        {
            if (ProjectLogBookWeeks.Count == 0)
            {
                return LogBookTime.Empty();
            }

            return GetSumClosedHours(LogBookYear.Create(Int32.MaxValue), LogBookWeek.Create(Int32.MaxValue));
        }

        public LogBookTime GetSumClosedHoursInPeriod(DateOnly start, DateOnly end)
        {
            var weeksInPeriod = ProjectLogBookWeeks.Where(
                week => week.Closed && week.LogBookDays.Any(day => day.Date.Value >= start && day.Date.Value <= end));

            var sum = LogBookTime.Empty();
            foreach (var week in weeksInPeriod)
            {
                sum = sum.Add(week.GetSumHoursInPeriod(start, end));
            }

            return sum;
        }

        public LogBookTime GetSumClosedHours(LogBookYear year, LogBookWeek week)
        {
            var sortedWeeks = GetWeeksSorted();
            var result = LogBookTime.Empty();
            foreach (var currentWeek in sortedWeeks)
            {
                var currentWeekValue = currentWeek.Value;
                if (currentWeekValue.Year.Value < year.Value ||
                    (currentWeekValue.Year.Value == year.Value && currentWeekValue.Week.Value <= week.Value))
                {
                    result = currentWeekValue.ClosedHoursSummation;
                }
            }

            return result;
        }

        public bool IsApprentice(ProjectLogBookWeek week)
        {
            var salaryAdvance = FindFromAndToSalaryAdvance(week.Year, week.Week);

            return salaryAdvance.start is not null && salaryAdvance.start.SalaryAdvance.Role is not null &&
                               salaryAdvance.start.SalaryAdvance.Role == LogBookSalaryAdvanceRole.Apprentice;
        }
    }
}
