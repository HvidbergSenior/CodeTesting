using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectManagerAddedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectManager ProjectManager { get; private set; }
        
        private ProjectManagerAddedDomainEvent(ProjectId projectId, ProjectManager projectManager)
        {
            ProjectId = projectId;
            ProjectManager = projectManager;
        }

        public static ProjectManagerAddedDomainEvent Create(ProjectId projectId, ProjectManager projectManager)
        {
            return new ProjectManagerAddedDomainEvent(projectId, projectManager);
        }
    }
}
