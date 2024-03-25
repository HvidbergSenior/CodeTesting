using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;

namespace deftq.Pieceworks.Domain.projectCompensation
{
    public sealed class ProjectCompensation : Entity
    {
        public ProjectCompensationId ProjectCompensationId { get; private set; }
        public ProjectCompensationPayment ProjectCompensationPayment { get; private set; }
        public ProjectCompensationPeriod ProjectCompensationPeriod { get; private set; }
        public IList<ProjectParticipantId> ProjectParticipantIds { get; private set; }

        private ProjectCompensation()
        {
            ProjectCompensationId = ProjectCompensationId.Create(Guid.NewGuid());
            ProjectCompensationPayment = ProjectCompensationPayment.Empty();
            ProjectCompensationPeriod = ProjectCompensationPeriod.Empty();
            ProjectParticipantIds = new List<ProjectParticipantId>();
        }

        private ProjectCompensation(ProjectCompensationId projectCompensationId, ProjectCompensationPayment projectCompensationPayment,
            ProjectCompensationPeriod projectCompensationPeriod, IList<ProjectParticipantId> projectParticipantIds)
        {
            Id = projectCompensationId.Id;
            ProjectCompensationId = projectCompensationId;
            ProjectCompensationPayment = projectCompensationPayment;
            ProjectCompensationPeriod = projectCompensationPeriod;
            ProjectParticipantIds = projectParticipantIds;
        }

        public static ProjectCompensation Create(ProjectCompensationId projectCompensationId, ProjectCompensationPayment projectCompensationPayment,
            ProjectCompensationPeriod projectCompensationPeriod, IList<ProjectParticipantId> projectParticipantIds)
        {
            var compensation = new ProjectCompensation(projectCompensationId, projectCompensationPayment, projectCompensationPeriod,
                projectParticipantIds);
            return compensation;
        }
        
        public bool IsDateIncluded(DateOnly dateOnly)
        {
            return ProjectCompensationPeriod.IsDateIncluded(dateOnly);
        }

        public bool IsParticipantIncluded(ProjectLogBookUser logBookUser)
        {
            return ProjectParticipantIds.Select(participantId => participantId.Value).Contains(logBookUser.UserId);
        }
    }
}
