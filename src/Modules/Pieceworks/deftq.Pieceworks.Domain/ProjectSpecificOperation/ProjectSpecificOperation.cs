using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectSpecificOperation
{
    public sealed class ProjectSpecificOperation : Entity
    {
        public ProjectSpecificOperationId ProjectSpecificOperationId { get; private set; }
        public ProjectSpecificOperationExtraWorkAgreementNumber ProjectSpecificOperationExtraWorkAgreementNumber { get; private set; }
        public ProjectSpecificOperationName ProjectSpecificOperationName { get; private set; }
        public ProjectSpecificOperationDescription ProjectSpecificOperationDescription { get; private set; }
        public ProjectSpecificOperationTime OperationTime { get; private set; }
        public ProjectSpecificOperationTime WorkingTime { get; private set; }

        private ProjectSpecificOperation()
        {
            ProjectSpecificOperationId = ProjectSpecificOperationId.Empty();
            ProjectSpecificOperationExtraWorkAgreementNumber = ProjectSpecificOperationExtraWorkAgreementNumber.Empty();
            ProjectSpecificOperationName = ProjectSpecificOperationName.Empty();
            ProjectSpecificOperationDescription = ProjectSpecificOperationDescription.Empty();
            OperationTime = ProjectSpecificOperationTime.Empty();
            WorkingTime = ProjectSpecificOperationTime.Empty();
        }

        private ProjectSpecificOperation(ProjectSpecificOperationId projectSpecificOperationId,
            ProjectSpecificOperationExtraWorkAgreementNumber projectSpecificOperationExtraWorkAgreementNumber,
            ProjectSpecificOperationName projectSpecificOperationName, ProjectSpecificOperationDescription projectSpecificOperationDescription,
            ProjectSpecificOperationTime operationTime, ProjectSpecificOperationTime workingTime)
        {
            ProjectSpecificOperationId = projectSpecificOperationId;
            Id = projectSpecificOperationId.Value;
            ProjectSpecificOperationExtraWorkAgreementNumber = projectSpecificOperationExtraWorkAgreementNumber;
            ProjectSpecificOperationName = projectSpecificOperationName;
            ProjectSpecificOperationDescription = projectSpecificOperationDescription;
            OperationTime = operationTime;
            WorkingTime = workingTime;
        }

        public static ProjectSpecificOperation Create(ProjectSpecificOperationId projectSpecificOperationId,
            ProjectSpecificOperationExtraWorkAgreementNumber projectSpecificOperationExtraWorkAgreementNumber,
            ProjectSpecificOperationName projectSpecificOperationName, ProjectSpecificOperationDescription projectSpecificOperationDescription,
            ProjectSpecificOperationTime operationTime, ProjectSpecificOperationTime workingTime)
        {
            return new ProjectSpecificOperation(projectSpecificOperationId, projectSpecificOperationExtraWorkAgreementNumber,
                projectSpecificOperationName, projectSpecificOperationDescription, operationTime, workingTime);
        }

        public void UpdateOperationTime(ProjectSpecificOperationTime operationTime)
        {
            OperationTime = operationTime;
        }
        
        public void UpdateWorkingTime(ProjectSpecificOperationTime workingTime)
        {
            WorkingTime = workingTime;
        }

        public void Update(ProjectSpecificOperationExtraWorkAgreementNumber projectSpecificOperationExtraWorkAgreementNumber,
            ProjectSpecificOperationName projectSpecificOperationName, ProjectSpecificOperationDescription projectSpecificOperationDescription,
            ProjectSpecificOperationTime operationTime, ProjectSpecificOperationTime workingTime)
        {
            ProjectSpecificOperationExtraWorkAgreementNumber = projectSpecificOperationExtraWorkAgreementNumber;
            ProjectSpecificOperationName = projectSpecificOperationName;
            ProjectSpecificOperationDescription = projectSpecificOperationDescription;
            OperationTime = operationTime;
            WorkingTime = workingTime;
        }
    }
}
