using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectCompany;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateProjectCompany
{
    public class UpdateProjectCompanyCommandTest
    {
        [Fact]
        public async Task GivenProject_WhenUpdatingCompany_ProjectIsUpdated()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var unitOfWork = new FakeUnitOfWork();
            var project = Any.Project();
            await projectRepository.Add(project);

            var companyName = "Acme Inc";
            var companyAddress = "Elm Street 12";
            var cvrNo = "12345678";
            var pNo = "0123456789";
            var command = UpdateProjectCompanyCommand.Create(project.ProjectId.Value, companyName, companyAddress, cvrNo, pNo);
            var handler = new UpdateProjectCompanyCommandHandler(projectRepository, unitOfWork);

            await handler.Handle(command, CancellationToken.None);

            Assert.Single(projectRepository.Entities);
            Assert.True(projectRepository.SaveChangesCalled);
            Assert.True(unitOfWork.IsCommitted);
            
            var updatedProject = await projectRepository.GetById(project.ProjectId.Value);

            Assert.Equal(companyName, updatedProject.ProjectCompany.ProjectCompanyName.Value);
            Assert.Equal(companyAddress, updatedProject.ProjectCompany.ProjectAddress.Value);
            Assert.Equal(cvrNo, updatedProject.ProjectCompany.ProjectCompanyCvrNo.Value);
            Assert.Equal(pNo, updatedProject.ProjectCompany.ProjectCompanyPNo.Value);
        }
    }
}
