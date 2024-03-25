using deftq.Pieceworks.Domain.projectSpecificOperation;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.ProjectSpecificOperation
{
    public class UpdateProjectSpecificOperationTest
    {
        [Fact]
        public void WhenUpdateProjectSpecificOperationInList_ShouldHaveNewValues()
        {
            var project = Any.Project();

            var projectSpecificOperationList = ProjectSpecificOperationList.Create(project.ProjectId, Any.ProjectSpecificOperationListId());
            projectSpecificOperationList.AddProjectSpecificOperation(Any.ProjectSpecificOperation("12345", "Grav et hul", "Ud med jorden", 35000,
                25000));
            projectSpecificOperationList.AddProjectSpecificOperation(Any.ProjectSpecificOperation("34567", "Fyld et gravet hul", "Ind med jorden",
                53000, 25000));
            projectSpecificOperationList.AddProjectSpecificOperation(Any.ProjectSpecificOperation("98765", "Ryd op", "", 10000, 25000));

            var id2Update = projectSpecificOperationList.ProjectSpecificOperations[1].ProjectSpecificOperationId;

            projectSpecificOperationList.UpdateProjectSpecificOperation(id2Update, ProjectSpecificOperationExtraWorkAgreementNumber.Create("424242"),
                ProjectSpecificOperationName.Create("Find på noget andet at lave"), ProjectSpecificOperationDescription.Create("Noget helt andet"),
                ProjectSpecificOperationTime.Create(4200), ProjectSpecificOperationTime.Create(42000));

            var updatedOperation = projectSpecificOperationList.FindProjectSpecificOperation(id2Update);
            
            Assert.Equal(id2Update.Value, updatedOperation.ProjectSpecificOperationId.Value);
            Assert.Equal("424242", updatedOperation.ProjectSpecificOperationExtraWorkAgreementNumber.Value);
            Assert.Equal("Find på noget andet at lave", updatedOperation.ProjectSpecificOperationName.Value);
            Assert.Equal("Noget helt andet", updatedOperation.ProjectSpecificOperationDescription.Value);
            Assert.Equal(4200, updatedOperation.OperationTime.Value);
            Assert.Equal(42000, updatedOperation.WorkingTime.Value);
        }

        [Fact]
        public void WhenUpdateProjectSpecificOperationThatDoNotExists_ThenThrowException()
        {
            var project = Any.Project();

            var projectSpecificOperationList = ProjectSpecificOperationList.Create(project.ProjectId, Any.ProjectSpecificOperationListId());
            projectSpecificOperationList.AddProjectSpecificOperation(Any.ProjectSpecificOperation("12345", "Grav et hul", "Ud med jorden", 35000,
                25000));
            projectSpecificOperationList.AddProjectSpecificOperation(Any.ProjectSpecificOperation("34567", "Fyld et gravet hul", "Ind med jorden",
                53000, 25000));
            projectSpecificOperationList.AddProjectSpecificOperation(Any.ProjectSpecificOperation("98765", "Ryd op", "", 10000, 25000));
            
            Assert.Throws<ProjectSpecificOperationNotFoundException>(() => projectSpecificOperationList.UpdateProjectSpecificOperation(
                ProjectSpecificOperationId.Create(Any.Guid()), ProjectSpecificOperationExtraWorkAgreementNumber.Create("424242"),
                ProjectSpecificOperationName.Create("Find på noget andet at lave"), ProjectSpecificOperationDescription.Create("Noget helt andet"),
                ProjectSpecificOperationTime.Create(4200), ProjectSpecificOperationTime.Create(42000)));
        }
    }
}
