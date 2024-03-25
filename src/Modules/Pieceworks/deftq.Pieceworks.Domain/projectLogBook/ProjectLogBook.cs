using deftq.BuildingBlocks.Domain;
using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class ProjectLogBook : Entity
    {
        public ProjectId ProjectId { get; private set; }
        
        public ProjectLogBookId ProjectLogBookId { get; private set; }
        
        public IList<ProjectLogBookUser> ProjectLogBookUsers { get; private set; }

        private ProjectLogBook()
        {
            ProjectLogBookId = ProjectLogBookId.Create(Guid.NewGuid());
            ProjectId = ProjectId.Empty();
            ProjectLogBookUsers = new List<ProjectLogBookUser>();
        }

        private ProjectLogBook(ProjectId projectId, ProjectLogBookId projectLogBookId)
        {
            Id = projectLogBookId.Id;
            ProjectLogBookId = projectLogBookId;
            ProjectId = projectId;
            ProjectLogBookUsers = new List<ProjectLogBookUser>();
        }

        public static ProjectLogBook Create(ProjectId projectId, ProjectLogBookId projectLogBookId)
        {
            var projectLogBook = new ProjectLogBook(projectId, projectLogBookId);
            return projectLogBook;
        }

        public ProjectLogBookWeek? FindWeek(Guid userId, LogBookYear year, LogBookWeek week)
        {
            var projectLogBookUser = FindUser(userId);
            if (projectLogBookUser == null)
            {
                return null;
            }
            return projectLogBookUser.FindWeek(year, week);
        }

        public ProjectLogBookUser? FindUser(Guid userId)
        {
            return ProjectLogBookUsers.FirstOrDefault(u => u.UserId == userId);
        }

        public void RegisterWeek(ProjectLogBookUser user, LogBookYear year, LogBookWeek week, LogBookNote note, IList<ProjectLogBookDay> logBookDays)
        {
            var projectLogBookUser = FindUser(user.UserId);
            if (projectLogBookUser == null)
            { 
                projectLogBookUser = ProjectLogBookUser.Create(user.Name, user.UserId);
                ProjectLogBookUsers.Add(projectLogBookUser);
            }
            projectLogBookUser.RegisterWeek(year, week, note, logBookDays);
        }

        public void UpdateSalaryAdvance(ProjectLogBookUser user, LogBookYear year, LogBookWeek week, LogBookSalaryAdvanceRole role,
            LogBookSalaryAdvanceAmount amount)
        {
            var projectLogBookUser = FindUser(user.UserId);
            if (projectLogBookUser == null)
            { 
                projectLogBookUser = ProjectLogBookUser.Create(user.Name, user.UserId);
                ProjectLogBookUsers.Add(projectLogBookUser);
            }
            projectLogBookUser.UpdateSalaryAdvance(year, week, role, amount);
        }

        public void CloseWeek(ProjectLogBookUser user, LogBookYear year, LogBookWeek week)
        {
            var logBookUser = FindUser(user.UserId);
            if (logBookUser is null)
            {
                logBookUser = ProjectLogBookUser.Create(user.Name, user.UserId);
                ProjectLogBookUsers.Add(logBookUser);
            }
            var wasClosed = logBookUser.CloseWeek(year, week);
            if (wasClosed)
            {
                AddDomainEvent(ProjectLogBookWeekClosedDomainEvent.Create(ProjectId, user, year, week));
            }
        }
        
        public void OpenWeek(ProjectLogBookUser user, LogBookYear year, LogBookWeek week)
        {
            var logBookUser = FindUser(user.UserId);
            if (logBookUser is not null)
            {
                var wasOpened = logBookUser.OpenWeek(year, week);
                if (wasOpened)
                {
                    AddDomainEvent(ProjectLogBookWeekOpenedDomainEvent.Create(ProjectId, user, year, week));
                }
            }
        }

        public void SumClosedHours(ProjectLogBookUser user)
        {
            var logBookUser = FindUser(user.UserId);
            if (logBookUser is null)
            {
                throw new NotFoundException("Log book user not found");
            }
            logBookUser.SumClosedHours();
        }
    }
}
