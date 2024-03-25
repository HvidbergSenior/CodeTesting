using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectUser
{
    public sealed class ProjectUser : Entity
    {
        public ProjectUserId ProjectUserId { get; private set; }
        public ISet<ProjectId> OwnedProjects { get; private set; }

        private ProjectUser()
        {
            OwnedProjects = new HashSet<ProjectId>();
            ProjectUserId = ProjectUserId.Unknown();
        }

        private ProjectUser(ProjectUserId projectUserId)
        {
            Id = projectUserId.Value;
            ProjectUserId = projectUserId;
            OwnedProjects = new HashSet<ProjectId>();
        }

        public static ProjectUser Create(ProjectUserId pieceworkUserId)
        {
            return new ProjectUser(pieceworkUserId);
        }

        public void RegisterOwnedProject(ProjectId projectId)
        {
            OwnedProjects.Add(projectId);
        }

        public bool Owns(ProjectId projectId)
        {
            return OwnedProjects.Contains(projectId);
        }
    }
}
