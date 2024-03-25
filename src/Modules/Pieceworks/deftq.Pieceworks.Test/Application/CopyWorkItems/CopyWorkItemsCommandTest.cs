using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.CopyWorkItems;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using deftq.Pieceworks.Test.Domain.Calculation;
using FluentAssertions;
using Xunit;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Application.CopyWorkItems
{
    public class CopyWorkItemsCommandTest
    {
        private readonly FakeUnitOfWork _uow;
        private readonly ProjectFolderRootInMemoryRepository _folderRootRepository;
        private readonly ProjectFolderWorkInMemoryRepository _folderWorkRepository;
        private readonly BaseRateAndSupplementRepository _baseRateAndSupplementInMemoryRepository;
        private readonly FakeExecutionContext _executionContext;

        public CopyWorkItemsCommandTest()
        {
            _uow = new FakeUnitOfWork();
            _folderRootRepository = new ProjectFolderRootInMemoryRepository();
            _folderWorkRepository = new ProjectFolderWorkInMemoryRepository();
            _baseRateAndSupplementInMemoryRepository = new BaseRateAndSupplementRepository();
            _executionContext = new FakeExecutionContext();
        }

        [Fact]
        public async Task GivenFolders_CopyToUnknownFolder_ThrowsException()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            var sourceFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(sourceFolder);
            await _folderRootRepository.Add(projectFolderRoot);
            await _folderWorkRepository.Add(ProjectFolderWork.Create(ProjectFolderWorkId.Create(Guid.NewGuid()), projectFolderRoot.ProjectId,
                sourceFolder.ProjectFolderId));

            var cmd = CopyWorkItemsCommand.Create(projectFolderRoot.ProjectId.Value, sourceFolder.ProjectFolderId.Value,
                Any.Guid(), new List<Guid>());

            var handler = new CopyWorkItemsCommandHandler(_folderWorkRepository, _folderRootRepository, _baseRateAndSupplementInMemoryRepository,
                _uow,
                _executionContext);

            await Assert.ThrowsAsync<ProjectFolderNotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }

        [Fact]
        public async Task GivenFolders_CopyFromUnknownFolder_ThrowsException()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            var destinationFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(destinationFolder);
            await _folderRootRepository.Add(projectFolderRoot);
            await _folderWorkRepository.Add(ProjectFolderWork.Create(ProjectFolderWorkId.Create(Guid.NewGuid()), projectFolderRoot.ProjectId,
                destinationFolder.ProjectFolderId));

            var cmd = CopyWorkItemsCommand.Create(projectFolderRoot.ProjectId.Value, Any.Guid(), destinationFolder.ProjectFolderId.Value,
                new List<Guid>());

            var handler = new CopyWorkItemsCommandHandler(_folderWorkRepository, _folderRootRepository, _baseRateAndSupplementInMemoryRepository,
                _uow,
                _executionContext);

            await Assert.ThrowsAsync<ProjectFolderNotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }

        [Fact]
        public async Task GivenFolders_CopyUnknownWorkItem_ThrowsException()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            var sourceFolder = Any.ProjectFolder();
            var destinationFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(sourceFolder);
            projectFolderRoot.AddFolder(destinationFolder);
            await _folderRootRepository.Add(projectFolderRoot);
            var sourceFolderWork = ProjectFolderWork.Create(ProjectFolderWorkId.Create(Guid.NewGuid()), projectFolderRoot.ProjectId,
                sourceFolder.ProjectFolderId);
            var workItem = Any.WorkItem();
            sourceFolderWork.AddWorkItem(workItem, new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), sourceFolder));
            await _folderWorkRepository.Add(sourceFolderWork);
            await _folderWorkRepository.Add(ProjectFolderWork.Create(ProjectFolderWorkId.Create(Guid.NewGuid()), projectFolderRoot.ProjectId,
                destinationFolder.ProjectFolderId));

            var cmd = CopyWorkItemsCommand.Create(projectFolderRoot.ProjectId.Value, sourceFolder.ProjectFolderId.Value,
                destinationFolder.ProjectFolderId.Value, new List<Guid> { Any.Guid() });

            var handler = new CopyWorkItemsCommandHandler(_folderWorkRepository, _folderRootRepository, _baseRateAndSupplementInMemoryRepository,
                _uow,
                _executionContext);

            await Assert.ThrowsAsync<WorkItemNotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }

        [Fact]
        public async Task GivenFolders_CopyWorkItem_WorkItemIsCopied()
        {
            // Given
            var projectFolderRoot = Any.ProjectFolderRoot();
            var sourceFolder = Any.ProjectFolder();
            var destinationFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(sourceFolder);
            projectFolderRoot.AddFolder(destinationFolder);
            await _folderRootRepository.Add(projectFolderRoot);
            var sourceFolderWork = ProjectFolderWork.Create(ProjectFolderWorkId.Create(Guid.NewGuid()), projectFolderRoot.ProjectId,
                sourceFolder.ProjectFolderId);
            var workItem1 = Any.WorkItem();
            var workItem2 = Any.WorkItem();
            sourceFolderWork.AddWorkItem(workItem1, GetDefaultBaseRateAndSupplementProxy());
            sourceFolderWork.AddWorkItem(workItem2, GetDefaultBaseRateAndSupplementProxy());
            await _folderWorkRepository.Add(sourceFolderWork);
            await _folderWorkRepository.Add(ProjectFolderWork.Create(ProjectFolderWorkId.Create(Guid.NewGuid()), projectFolderRoot.ProjectId,
                destinationFolder.ProjectFolderId));

            // When
            var cmd = CopyWorkItemsCommand.Create(projectFolderRoot.ProjectId.Value, sourceFolder.ProjectFolderId.Value,
                destinationFolder.ProjectFolderId.Value, new List<Guid> { workItem1.Id, workItem2.Id });

            var handler = new CopyWorkItemsCommandHandler(_folderWorkRepository, _folderRootRepository, _baseRateAndSupplementInMemoryRepository,
                _uow,
                _executionContext);
            
            await handler.Handle(cmd, CancellationToken.None);

            // Then
            var destinationFolderWork = await _folderWorkRepository.GetByProjectAndFolderId(projectFolderRoot.ProjectId.Value,
                destinationFolder.ProjectFolderId.Value);
            
            destinationFolderWork.WorkItems.Should().HaveCount(2);
            _folderWorkRepository.SaveChangesCalled.Should().BeTrue();
            _uow.IsCommitted.Should().BeTrue();
        }
    }
}
