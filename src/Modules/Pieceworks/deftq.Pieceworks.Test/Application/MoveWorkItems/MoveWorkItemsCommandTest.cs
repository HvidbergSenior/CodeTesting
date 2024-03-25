using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.MoveWorkItems;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using FluentAssertions;
using Xunit;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Application.MoveWorkItems
{
    public class MoveWorkItemsCommandTest
    {
        [Fact]
        public async Task MoveWorkItemsCommand_ShouldMoveWorkItems()
        {
            // GIVEN
            var uow = new FakeUnitOfWork();
            var folderRootRepository = new ProjectFolderRootInMemoryRepository();
            var folderWorkRepository = new ProjectFolderWorkInMemoryRepository();
            var baseRateAndSupplementRepository = new BaseRateAndSupplementRepository();
            
            // Create project folder structure with source and destination folder
            ProjectId projectId = Any.ProjectId();
            var projectFolderRoot = ProjectFolderRoot.Create(projectId, Any.ProjectName(), Any.ProjectFolderRootId(), GetDefaultFolderRateAndSupplement());
            var sourceFolder = Any.ProjectFolder();
            var destinationFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(sourceFolder);
            projectFolderRoot.AddFolder(destinationFolder);
            await folderRootRepository.Add(projectFolderRoot);

            // Create project folder work structure for both the source and destination folder
            var sourceFolderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectId, sourceFolder.ProjectFolderId);
            var workItem1 = Any.WorkItem();
            var workItem2 = Any.WorkItem();
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), sourceFolder);
            sourceFolderWork.AddWorkItem(workItem1, baseRateAndSupplementProxy);
            sourceFolderWork.AddWorkItem(workItem2, baseRateAndSupplementProxy);
            await folderWorkRepository.Add(sourceFolderWork);
            
            var destinationFolderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectId, destinationFolder.ProjectFolderId);
            await folderWorkRepository.Add(destinationFolderWork);

            // List items to move
            var workItemIds = new[] { workItem1.WorkItemId.Value, workItem2.WorkItemId.Value }.ToList();

            var handler = new MoveWorkItemsCommandHandler(folderRootRepository, folderWorkRepository, baseRateAndSupplementRepository, uow);
            var cmd = MoveWorkItemsCommand.Create(projectFolderRoot.ProjectId.Value, sourceFolder.ProjectFolderId.Value, destinationFolder.ProjectFolderId.Value, workItemIds);
            
            // WHEN
            await handler.Handle(cmd, CancellationToken.None);

            // THEN
            folderWorkRepository.Entities.Should().HaveCount(2);
            folderWorkRepository.SaveChangesCalled.Should().BeTrue();
            uow.IsCommitted.Should().BeTrue();

            sourceFolderWork = await folderWorkRepository.GetByProjectAndFolderId(projectId.Value, sourceFolderWork.ProjectFolderId.Value);
            sourceFolderWork.WorkItems.Should().BeEmpty();

            destinationFolderWork = await folderWorkRepository.GetByProjectAndFolderId(projectId.Value, destinationFolderWork.ProjectFolderId.Value);
            destinationFolderWork.WorkItems.Should().HaveCount(2);
            destinationFolderWork.WorkItems.Select(wi => wi.WorkItemId.Value).Should().Contain(workItemIds[0]).And.Contain(workItemIds[1]);
        }
        
    }
}
