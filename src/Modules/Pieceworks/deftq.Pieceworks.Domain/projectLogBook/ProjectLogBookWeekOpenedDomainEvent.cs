using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class ProjectLogBookWeekOpenedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectLogBookUser User { get; private set; }
        public LogBookYear Year { get; private set; }
        public LogBookWeek Week { get; private set; }

        private ProjectLogBookWeekOpenedDomainEvent(ProjectId projectId, ProjectLogBookUser user, LogBookYear year, LogBookWeek week)
        {
            ProjectId = projectId;
            User = user;
            Year = year;
            Week = week;
        }

        public static ProjectLogBookWeekOpenedDomainEvent Create(ProjectId projectId, ProjectLogBookUser user, LogBookYear year, LogBookWeek week)
        {
            return new ProjectLogBookWeekOpenedDomainEvent(projectId, user, year, week);
        }
    }
}
