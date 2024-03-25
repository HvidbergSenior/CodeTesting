using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectCreatedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }
        public ProjectName ProjectName { get; }
        public ProjectDescription ProjectDescription { get; }
        public ProjectOwner ProjectOwner { get; }
        public ProjectPieceworkType ProjectPieceworkType { get; }


        private ProjectCreatedDomainEvent(ProjectId projectId, ProjectName projectName, ProjectDescription projectDescription, ProjectOwner projectOwner, ProjectPieceworkType projectPieceworkType)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            ProjectOwner = projectOwner;
            ProjectDescription = projectDescription;
            ProjectPieceworkType = projectPieceworkType;
        }

        public static ProjectCreatedDomainEvent Create(ProjectId projectId, ProjectName projectName, ProjectDescription projectDescription, ProjectOwner projectOwner, ProjectPieceworkType projectPieceworkType)
        {
            return new ProjectCreatedDomainEvent(projectId, projectName, projectDescription, projectOwner, projectPieceworkType);
        }
    }
}
