using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectRemovedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }

        private ProjectRemovedDomainEvent(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static ProjectRemovedDomainEvent Create(ProjectId projectId)
        {
            return new ProjectRemovedDomainEvent(projectId);
        }
    }
}
