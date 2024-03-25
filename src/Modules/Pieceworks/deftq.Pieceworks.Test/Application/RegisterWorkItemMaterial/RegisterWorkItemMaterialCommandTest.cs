using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterWorkItemMaterial;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using Supplement = deftq.Pieceworks.Application.RegisterWorkItemMaterial.Supplement;
using SupplementOperation = deftq.Pieceworks.Application.RegisterWorkItemMaterial.SupplementOperation;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Application.RegisterWorkItemMaterial
{
    public class RegisterWorkItemMaterialCommandTest
    {
        private readonly FakeUnitOfWork _uow;
        private readonly FakeExecutionContext _executionContext;
        private readonly BaseRateAndSupplementRepository _baseRateRepository;
        private readonly ProjectFolderRootInMemoryRepository _projectFolderRootRepository;
        private readonly ProjectFolderWorkInMemoryRepository _folderWorkRepository;

        public RegisterWorkItemMaterialCommandTest()
        {
            _uow = new FakeUnitOfWork();
            _executionContext = new FakeExecutionContext();
            _baseRateRepository = new BaseRateAndSupplementRepository();
            _projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            _folderWorkRepository = new ProjectFolderWorkInMemoryRepository();
        }
        
        [Fact]
        public async Task Should_Register_WorkItem_Successfully()
        {
            var projectId = ProjectId.Create(Guid.NewGuid());
            var projectFolderRoot = ProjectFolderRoot.Create(projectId, ProjectName.Empty(), Any.ProjectFolderRootId(), GetDefaultFolderRateAndSupplement());

            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);
            await _projectFolderRootRepository.Add(projectFolderRoot);

            var initialFolderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectId, folder.ProjectFolderId);
            await _folderWorkRepository.Add(initialFolderWork);
            
            var handler = new RegisterWorkItemMaterialCommandHandler(_projectFolderRootRepository, _baseRateRepository, _folderWorkRepository, _uow, _executionContext);
            var cmd = RegisterWorkItemMaterialCommand.Create(projectId.Value, folder.Id, Guid.NewGuid(),
                Guid.NewGuid(), "testText", DateOnly.MaxValue, 60000m, WorkItemMountingCode.FromCode(3).MountingCode, 2,
                 "0000000000000", WorkItemUnit.Piece.Value, new List<SupplementOperation>(), new List<Supplement>());
            await handler.Handle(cmd, CancellationToken.None);

            var folderWork = await _folderWorkRepository.GetByProjectAndFolderId(projectId.Value, folder.ProjectFolderId.Value);

            Assert.Single(folderWork.WorkItems);
            var workItem = folderWork.WorkItems[0];
            Assert.Equal(workItem.WorkItemId, cmd.WorkItemId);
            Assert.Equal(workItem.WorkItemMaterial?.MountingCode, cmd.WorkItemMountingCode);
            Assert.Equal(workItem.WorkItemMaterial?.EanNumber, cmd.WorkItemEanNumber);
            Assert.Equal(workItem.WorkItemMaterial?.Unit, cmd.WorkItemUnit);
            Assert.Single(_folderWorkRepository.Entities);
            Assert.True(_folderWorkRepository.SaveChangesCalled);
            Assert.True(_uow.IsCommitted);
        }
        
        [Fact]
        public async Task Should_Register_WorkItem_With_Supplement_Operations()
        {
            var projectId = ProjectId.Create(Guid.NewGuid());
            var projectFolderRoot = ProjectFolderRoot.Create(projectId, ProjectName.Empty(), Any.ProjectFolderRootId(), GetDefaultFolderRateAndSupplement());

            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);
            await _projectFolderRootRepository.Add(projectFolderRoot);

            var initialFolderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectId, folder.ProjectFolderId);
            await _folderWorkRepository.Add(initialFolderWork);
            
            var handler = new RegisterWorkItemMaterialCommandHandler(_projectFolderRootRepository, _baseRateRepository, _folderWorkRepository, _uow, _executionContext);
            var supplementOperation = SupplementOperation.Create(Guid.NewGuid(), Guid.NewGuid(),"Dig a hole", SupplementOperation.SupplementOperationType.AmountRelated, 10.1m, 14.2m);
            var cmd = RegisterWorkItemMaterialCommand.Create(projectId.Value, folder.Id, Guid.NewGuid(),
                Guid.NewGuid(), "testText", DateOnly.MaxValue, 60000m, WorkItemMountingCode.FromCode(3).MountingCode, 2,
                "0000000000000", WorkItemUnit.Piece.Value, new List<SupplementOperation> { supplementOperation }, new List<Supplement>());
            await handler.Handle(cmd, CancellationToken.None);

            var folderWork = await _folderWorkRepository.GetByProjectAndFolderId(projectId.Value, folder.ProjectFolderId.Value);

            Assert.Single(folderWork.WorkItems[0].WorkItemMaterial?.SupplementOperations);
            var operation = folderWork.WorkItems[0].WorkItemMaterial?.SupplementOperations[0];
            Assert.Equal("Dig a hole", operation?.Text.Value);
            Assert.Equal(10.1m, operation?.OperationTime.Value);
            Assert.Equal(14.2m, operation?.OperationAmount.Value);
            Assert.Equal(SupplementOperationType.AmountRelated(), operation?.OperationType);
        }

        [Fact]
        public async Task When_WorkItemAddedToUnknownProject_ExceptionIsThrown()
        {
            var handler = new RegisterWorkItemMaterialCommandHandler(_projectFolderRootRepository, _baseRateRepository, _folderWorkRepository, _uow, _executionContext);
            var cmd = RegisterWorkItemMaterialCommand.Create(Any.Instance<Guid>(), Any.Instance<Guid>(), Guid.NewGuid(),
                Guid.NewGuid(), "testText", DateOnly.MaxValue, 60000m, WorkItemMountingCode.FromCode(3).MountingCode, 2,
                "0000000000000", WorkItemUnit.Piece.Value, new List<SupplementOperation>(), new List<Supplement>());
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));   
        }
        
        [Fact]
        public async Task When_WorkItemAddedToUnknownFolder_ExceptionIsThrown()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            await _projectFolderRootRepository.Add(projectFolderRoot);

            var handler = new RegisterWorkItemMaterialCommandHandler(_projectFolderRootRepository, _baseRateRepository, _folderWorkRepository, _uow, _executionContext);
            var cmd = RegisterWorkItemMaterialCommand.Create(projectFolderRoot.ProjectId.Value, Any.Instance<Guid>(), Guid.NewGuid(),
                Guid.NewGuid(), "testText", DateOnly.MaxValue, 60000m, WorkItemMountingCode.FromCode(3).MountingCode, 2,
                "0000000000000", WorkItemUnit.Piece.Value, new List<SupplementOperation>(), new List<Supplement>());
            
            await Assert.ThrowsAsync<ProjectFolderNotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));   
        }
    }
}
