using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectDescriptionUpdatedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }

        private ProjectDescriptionUpdatedDomainEvent(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static ProjectDescriptionUpdatedDomainEvent Create(ProjectId projectId)
        {
            return new ProjectDescriptionUpdatedDomainEvent(projectId);
        }
    }
}
