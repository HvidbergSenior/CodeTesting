using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectNameUpdatedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }

        private ProjectNameUpdatedDomainEvent(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static ProjectNameUpdatedDomainEvent Create(ProjectId projectId)
        {
            return new ProjectNameUpdatedDomainEvent(projectId);
        }
    }
}
