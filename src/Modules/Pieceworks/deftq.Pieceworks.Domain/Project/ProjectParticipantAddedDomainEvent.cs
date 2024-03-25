using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectParticipantAddedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectParticipant ProjectIdParticipant { get; private set; }
        
        private ProjectParticipantAddedDomainEvent(ProjectId projectId, ProjectParticipant projectIdParticipant)
        {
            ProjectId = projectId;
            ProjectIdParticipant = projectIdParticipant;
        }

        public static ProjectParticipantAddedDomainEvent Create(ProjectId projectId, ProjectParticipant projectIdParticipant)
        {
            return new ProjectParticipantAddedDomainEvent(projectId, projectIdParticipant);
        }
    }
}
