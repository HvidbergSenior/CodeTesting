using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project.Company
{
    public sealed class ProjectCompany : ValueObject
    {
        public ProjectCompanyName ProjectCompanyName { get; private set; }
        public ProjectAddress ProjectAddress { get; private set; }
        public ProjectCompanyCvrNo ProjectCompanyCvrNo { get; private set; }
        public ProjectCompanyPNo ProjectCompanyPNo { get; private set; }

        private ProjectCompany()
        {
            ProjectCompanyName = ProjectCompanyName.Empty();
            ProjectAddress = ProjectAddress.Empty();
            ProjectCompanyCvrNo = ProjectCompanyCvrNo.Empty();
            ProjectCompanyPNo = ProjectCompanyPNo.Empty();
        }
        
        private ProjectCompany(ProjectCompanyName projectCompanyName, ProjectAddress projectAddress,
            ProjectCompanyCvrNo projectCompanyCvrNo, ProjectCompanyPNo projectCompanyPNo)
        {
            ProjectCompanyName = projectCompanyName;
            ProjectAddress = projectAddress;
            ProjectCompanyCvrNo = projectCompanyCvrNo;
            ProjectCompanyPNo = projectCompanyPNo;
        }
        
        public static ProjectCompany Create(ProjectCompanyName projectCompanyName, ProjectAddress projectAddress,
            ProjectCompanyCvrNo projectCompanyCvrNo, ProjectCompanyPNo projectCompanyPNo)
        {
            return new ProjectCompany(projectCompanyName, projectAddress, projectCompanyCvrNo, projectCompanyPNo);
        }

        public static ProjectCompany Empty()
        {
            return new ProjectCompany();
        }
    }
}
