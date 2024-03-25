using Baseline;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectCompensation
{
    public sealed class ProjectCompensationList : Entity
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectCompensationListId ProjectCompensationListId { get; private set; }
        public IList<ProjectCompensation> Compensations { get; private set; }

        private ProjectCompensationList()
        {
            ProjectId = ProjectId.Empty();
            ProjectCompensationListId = ProjectCompensationListId.Empty();
            Id = ProjectCompensationListId.Value;
            Compensations = new List<ProjectCompensation>();
        }

        private ProjectCompensationList(ProjectId projectId, ProjectCompensationListId projectCompensationListId)
        {
            ProjectId = projectId;
            ProjectCompensationListId = projectCompensationListId;
            Id = ProjectCompensationListId.Value;
            Compensations = new List<ProjectCompensation>();
        }

        public static ProjectCompensationList Create(ProjectId projectId, ProjectCompensationListId projectCompensationListId)
        {
            return new ProjectCompensationList(projectId, projectCompensationListId);
        }

        public static ProjectCompensationList Empty()
        {
            return new ProjectCompensationList();
        }

        public void AddCompensation(ProjectCompensation compensation)
        {
            if (compensation.ProjectParticipantIds.Count <= 0)
            {
                throw new InvalidOperationException("compensation has to have at least one participant");
            }
            Compensations.Add(compensation);
        }

        public void RemoveCompensations(IList<ProjectCompensationId> compensationPaymentIds)
        {
            foreach (ProjectCompensationId requestCompensationPaymentId in compensationPaymentIds)
            {
                if (Compensations.All(c => c.ProjectCompensationId.Id != requestCompensationPaymentId.Id))
                {
                    throw new ProjectCompensationNotFoundException(requestCompensationPaymentId.Id);
                }
            }
            Compensations.RemoveAll(c => compensationPaymentIds.Contains(c.ProjectCompensationId));
        } 
    }
}
