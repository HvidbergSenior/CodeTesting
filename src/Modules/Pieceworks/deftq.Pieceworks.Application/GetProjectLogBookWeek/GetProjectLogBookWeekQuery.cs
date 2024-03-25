using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;

namespace deftq.Pieceworks.Application.GetProjectLogBookWeek
{
    public sealed class GetProjectLogBookWeekQuery : IQuery<GetProjectLogBookWeekQueryResponse>
    {
        public ProjectId ProjectId { get; private set; }
        public Guid UserId { get; private set; }
        public LogBookYear LogBookYear { get; private set; }
        public LogBookWeek LogBookWeek { get; private set; }

        private GetProjectLogBookWeekQuery(ProjectId projectId, Guid userId, LogBookYear logBookYear,
            LogBookWeek logBookWeek)
        {
            ProjectId = projectId;
            UserId = userId;
            LogBookYear = logBookYear;
            LogBookWeek = logBookWeek;
        }

        public static GetProjectLogBookWeekQuery Create(ProjectId projectId, Guid userId, int year, int week)
        {
            return new GetProjectLogBookWeekQuery(projectId, userId, LogBookYear.Create(year),
                LogBookWeek.Create(week));
        }
    }

    internal class
        GetProjectLogBookWeekQueryHandler : IQueryHandler<GetProjectLogBookWeekQuery,
            GetProjectLogBookWeekQueryResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;
        private readonly IProjectLogBookRepository _projectLogBookRepository;
        private readonly ISystemTime _systemTime;

        public GetProjectLogBookWeekQueryHandler(IProjectRepository projectRepository, IExecutionContext executionContext,
            IProjectLogBookRepository projectLogBookRepository, ISystemTime systemTime)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
            _projectLogBookRepository = projectLogBookRepository;
            _systemTime = systemTime;
        }

        public async Task<GetProjectLogBookWeekQueryResponse> Handle(GetProjectLogBookWeekQuery query,
            CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellationToken);
            var logbook = await _projectLogBookRepository.GetByProjectId(query.ProjectId.Value, cancellationToken);
            var week = logbook.FindWeek(query.UserId, query.LogBookYear, query.LogBookWeek);

            if (week is null)
            {
                return CreateDummyWeekResponse(project, _executionContext, _systemTime, logbook.FindUser(query.UserId), query.LogBookYear,
                    query.LogBookWeek);
            }

            var user = logbook.FindUser(query.UserId);
            var days = week.LogBookDays.Select(day =>
            {
                var dateInUtc = _systemTime.DateOnlyAsUtcTimestamp(day.Date.Value);
                return new GetProjectLogBookDayResponse(dateInUtc, LogBookTimeResponse.Create(day.Time));
            }).ToList();

            var weekSummation = week.GetWeekSummation();
            var weekSummationResponse =
                new LogBookTimeResponse(weekSummation.GetHours().Value, weekSummation.GetMinutes().Value);

            var closedWeekSummation = week.ClosedHoursSummation;
            var closedWeekSummationResponse =
                new LogBookTimeResponse(closedWeekSummation.GetHours().Value, closedWeekSummation.GetMinutes().Value);

            var salaryAdvance = GetSalaryAdvanceResponse(project, _executionContext, user, week.Year, week.Week);
            return new GetProjectLogBookWeekQueryResponse(week.Year.Value, week.Week.Value,
                user?.Name.Value ?? String.Empty, week.Note.Value, week.Closed, weekSummationResponse,
                closedWeekSummationResponse, days, salaryAdvance);
        }

        private static GetProjectLogBookWeekQueryResponse CreateDummyWeekResponse(Project project, IExecutionContext executionContext,
            ISystemTime systemTime, ProjectLogBookUser? logBookUser,
            LogBookYear year, LogBookWeek week)
        {
            var sumClosedHours = LogBookTime.Empty();

            if (logBookUser is not null)
            {
                sumClosedHours = logBookUser.GetSumClosedHours(year, week);
            }

            var daysInWeek = ProjectLogBookWeek.Create(year, week).LogBookDays.Select(day =>
            {
                var dateInUtc = systemTime.DateOnlyAsUtcTimestamp(day.Date.Value);
                return new GetProjectLogBookDayResponse(dateInUtc, LogBookTimeResponse.Create(day.Time));
            }).ToList();

            var logBookTimeResponse = LogBookTimeResponse.Create(sumClosedHours);
            var salaryAdvance = GetSalaryAdvanceResponse(project, executionContext, logBookUser, year, week);
            return new GetProjectLogBookWeekQueryResponse(year.Value, week.Value, String.Empty, string.Empty, false,
                LogBookTimeResponse.Empty(), logBookTimeResponse, daysInWeek, salaryAdvance);
        }

        private static LogBookSalaryAdvanceResponse GetSalaryAdvanceResponse(Project project, IExecutionContext executionContext,
            ProjectLogBookUser? logBookUser, LogBookYear year, LogBookWeek week)
        {
            if (logBookUser is null)
            {
                return LogBookSalaryAdvanceResponse.Empty();
            }
            
            if (project.IsOwner(executionContext.UserId) ||
                (project.IsParticipant(executionContext.UserId) && executionContext.UserId == logBookUser.UserId))
            {
                return CreateSalaryAdvanceResponse(logBookUser, year, week);
            }
            return LogBookSalaryAdvanceResponse.Empty();
        }

        private static LogBookSalaryAdvanceResponse CreateSalaryAdvanceResponse(ProjectLogBookUser logBookUser, LogBookYear year, LogBookWeek week)
        {
            (ProjectLogBookWeek? start, ProjectLogBookWeek? end) = logBookUser.FindFromAndToSalaryAdvance(year, week);
            var weekStart = start is null || start.SalaryAdvance.IsEmpty()
                ? null
                : new LogBookSalaryAdvanceTimeResponse(start.Year.Value, start.Week.Value);
            var weekEnd = end is null || end.SalaryAdvance.IsEmpty() ? null : new LogBookSalaryAdvanceTimeResponse(end.Year.Value, end.Week.Value);

            var amount = 0.0m;
            var role = LogBookSalaryAdvanceRoleResponse.Undefined;

            if (start is not null && !start.SalaryAdvance.IsEmpty())
            {
                amount = start.SalaryAdvance.Amount?.Value ?? 0;
                if (start.SalaryAdvance.Role is not null)
                {
                    role = (start.SalaryAdvance.Role.Value == LogBookSalaryAdvanceRole.Participant
                        ? LogBookSalaryAdvanceRoleResponse.Participant
                        : LogBookSalaryAdvanceRoleResponse.Apprentice);
                }
            }

            return new LogBookSalaryAdvanceResponse(weekStart, weekEnd, amount, role);
        }
    }

    public class GetProjectLogBookWeekQueryAuthorizer : IAuthorizer<GetProjectLogBookWeekQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetProjectLogBookWeekQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetProjectLogBookWeekQuery query,
            CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellationToken);
            if (project.IsOwner(_executionContext.UserId) || project.IsParticipant(_executionContext.UserId) || project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
