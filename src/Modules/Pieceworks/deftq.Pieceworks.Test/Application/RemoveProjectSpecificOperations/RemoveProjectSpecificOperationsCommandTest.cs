using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveProjectSpecificOperations;
using deftq.Pieceworks.Domain.projectSpecificOperation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveProjectSpecificOperations
{
    public class RemoveProjectSpecificOperationsCommandTest
    {
        private readonly FakeUnitOfWork _unitOfWork;
        private readonly ProjectSpecificOperationListInMemoryRepository _projectSpecificOperationListRepository;

        public RemoveProjectSpecificOperationsCommandTest()
        {
            _unitOfWork = new FakeUnitOfWork();
            _projectSpecificOperationListRepository = new ProjectSpecificOperationListInMemoryRepository();
        }

        [Fact]
        public async Task RemoveMultipleProjectSpecificOperations_ShouldNotReturnProjectFavorites()
        {
            var project = Any.Project();
            var list = ProjectSpecificOperationList.Create(project.ProjectId, Any.ProjectSpecificOperationListId());
            var operation1 = Any.ProjectSpecificOperation();
            var operation2 = Any.ProjectSpecificOperation();
            list.AddProjectSpecificOperation(operation1);
            list.AddProjectSpecificOperation(operation2);
            await _projectSpecificOperationListRepository.Add(list);
            
            var handler = new RemoveProjectSpecificOperationsCommandHandler(_projectSpecificOperationListRepository, _unitOfWork);
            var cmd = RemoveProjectSpecificOperationsCommand.Create(project.ProjectId.Value, 
                new List<Guid> { operation1.ProjectSpecificOperationId.Value, operation2.ProjectSpecificOperationId.Value });
            await handler.Handle(cmd, CancellationToken.None);

            Assert.Single(_projectSpecificOperationListRepository.Entities);
            Assert.True(_projectSpecificOperationListRepository.SaveChangesCalled);
            Assert.True(_unitOfWork.IsCommitted);

            list = await _projectSpecificOperationListRepository.GetByProjectId(project.ProjectId.Value, CancellationToken.None);
            Assert.Empty(list.ProjectSpecificOperations);
        }
        
        [Fact]
        public async Task RemoveMultipleProjectSpecificOperations_ShouldThrow()
        {
            var project = Any.Project();
            var list = ProjectSpecificOperationList.Create(project.ProjectId, Any.ProjectSpecificOperationListId());
            var operation1 = Any.ProjectSpecificOperation();
            var operation2 = Any.ProjectSpecificOperation();
            list.AddProjectSpecificOperation(operation1);
            list.AddProjectSpecificOperation(operation2);
            await _projectSpecificOperationListRepository.Add(list);

            var handler = new RemoveProjectSpecificOperationsCommandHandler(_projectSpecificOperationListRepository, _unitOfWork);
            var cmd = RemoveProjectSpecificOperationsCommand.Create(project.ProjectId.Value, 
                new List<Guid> { Any.ProjectSpecificOperationId().Value, Any.ProjectSpecificOperationId().Value });
            await Assert.ThrowsAsync<ProjectSpecificOperationNotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }
    }
}
