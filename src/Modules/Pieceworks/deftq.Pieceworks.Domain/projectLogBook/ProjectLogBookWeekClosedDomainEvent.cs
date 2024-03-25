using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class ProjectLogBookWeekClosedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectLogBookUser User { get; private set; }
        public LogBookYear Year { get; private set; }
        public LogBookWeek Week { get; private set; }

        private ProjectLogBookWeekClosedDomainEvent(ProjectId projectId, ProjectLogBookUser user, LogBookYear year, LogBookWeek week)
        {
            ProjectId = projectId;
            User = user;
            Year = year;
            Week = week;
        }

        public static ProjectLogBookWeekClosedDomainEvent Create(ProjectId projectId, ProjectLogBookUser user, LogBookYear year, LogBookWeek week)
        {
            return new ProjectLogBookWeekClosedDomainEvent(projectId, user, year, week);
        }
    }
}
