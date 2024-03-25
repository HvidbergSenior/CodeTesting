using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectSpecificOperation
{
    public sealed class ProjectSpecificOperationList : Entity
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectSpecificOperationListId ProjectSpecificOperationListId { get; private set; }
        public IList<ProjectSpecificOperation> ProjectSpecificOperations { get; private set; }

        private ProjectSpecificOperationList()
        {
            ProjectId = ProjectId.Empty();
            ProjectSpecificOperationListId = ProjectSpecificOperationListId.Empty();
            ProjectSpecificOperations = new List<ProjectSpecificOperation>();
        }

        private ProjectSpecificOperationList(ProjectId projectId, ProjectSpecificOperationListId projectSpecificOperationListId)
        {
            ProjectId = projectId;
            ProjectSpecificOperationListId = projectSpecificOperationListId;
            Id = ProjectSpecificOperationListId.Value;
            ProjectSpecificOperations = new List<ProjectSpecificOperation>();
        }

        public static ProjectSpecificOperationList Create(ProjectId projectId, ProjectSpecificOperationListId projectSpecificOperationListId)
        {
            return new ProjectSpecificOperationList(projectId, projectSpecificOperationListId);
        }

        public void AddProjectSpecificOperation(ProjectSpecificOperation projectSpecificOperation)
        {
            ProjectSpecificOperations.Add(projectSpecificOperation);
        }

        public void UpdateProjectSpecificOperation(ProjectSpecificOperationId projectSpecificOperationId,
            ProjectSpecificOperationExtraWorkAgreementNumber projectSpecificOperationExtraWorkAgreementNumber,
            ProjectSpecificOperationName projectSpecificOperationName, ProjectSpecificOperationDescription projectSpecificOperationDescription,
            ProjectSpecificOperationTime operationTime, ProjectSpecificOperationTime workingTime)
        {
            var found = FindProjectSpecificOperation(projectSpecificOperationId);
            found.Update(projectSpecificOperationExtraWorkAgreementNumber, projectSpecificOperationName, projectSpecificOperationDescription,
                operationTime, workingTime);
        }

        private void RemoveProjectSpecificOperation(ProjectSpecificOperationId projectSpecificOperationId)
        {
            var found = FindProjectSpecificOperation(projectSpecificOperationId);
            ProjectSpecificOperations.Remove(found);
        }

        public ProjectSpecificOperation FindProjectSpecificOperation(ProjectSpecificOperationId projectSpecificOperationId)
        {
            var found =
                ProjectSpecificOperations.FirstOrDefault(o => o.ProjectSpecificOperationId == projectSpecificOperationId);
            if (found is null)
            {
                throw new ProjectSpecificOperationNotFoundException(projectSpecificOperationId.Value);
            }

            return found;
        }

        public void RemoveProjectSpecificOperations(IList<ProjectSpecificOperationId> requestProjectSpecificOperationIds)
        {
            foreach (var requestProjectSpecificOperationId in requestProjectSpecificOperationIds)
            {
                RemoveProjectSpecificOperation(requestProjectSpecificOperationId);
            }
        }
    }
}
