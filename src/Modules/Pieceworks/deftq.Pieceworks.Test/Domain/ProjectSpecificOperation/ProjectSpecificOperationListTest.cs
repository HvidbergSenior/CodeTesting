using deftq.Pieceworks.Domain.projectSpecificOperation;
using FluentAssertions;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.ProjectSpecificOperation
{
    public class ProjectSpecificOperationListTest
    {
        [Fact]
        public void ProjectSpecificOperationList_WhenRemovingOperation_OperationIsNotInList()
        {
            // Given
            var operations = ProjectSpecificOperationList.Create(Any.ProjectId(), Any.ProjectSpecificOperationListId());
            var operation = Any.ProjectSpecificOperation();
            operations.AddProjectSpecificOperation(operation);
            
            // When
            var list = new List<ProjectSpecificOperationId>{ operation.ProjectSpecificOperationId };
            operations.RemoveProjectSpecificOperations(list);

            // Then
            operations.ProjectSpecificOperations.Should().BeEmpty();
        }
        
        [Fact]
        public void ProjectSpecificOperationList_WhenRemovingUnknownOperation_ExceptionIsThrown()
        {
            // Given
            var operations = ProjectSpecificOperationList.Create(Any.ProjectId(), Any.ProjectSpecificOperationListId());
            
            // Then
            var list = new List<ProjectSpecificOperationId> { Any.ProjectSpecificOperationId() };
            Assert.Throws<ProjectSpecificOperationNotFoundException>(() => operations.RemoveProjectSpecificOperations(list));
        }
    }
}
