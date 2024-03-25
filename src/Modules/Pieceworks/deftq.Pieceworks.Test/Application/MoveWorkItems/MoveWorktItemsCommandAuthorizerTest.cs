using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.MoveWorkItems;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using FluentAssertions;
using Xunit;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Application.MoveWorkItems
{
    public class MoveWorktItemsCommandAuthorizerTest
    {
        [Fact]
        public async Task MoveWorkItemsAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new MoveWorkItemsCommandAuthorizer(projectRepository, projectFolderRootRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(
                MoveWorkItemsCommand.Create(project.ProjectId.Value, Guid.NewGuid(), Guid.NewGuid(), Any.Guids()), 
                CancellationToken.None);

            authorizationResult.IsAuthorized.Should().BeTrue();
        }

        [Fact]
        public async Task MoveWorkItemsAsParticipantAndFoldersUnlocked_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            
            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var projectFolderRoot = ProjectFolderRoot.Create(project.ProjectId, Any.ProjectName(), Any.ProjectFolderRootId(), GetDefaultFolderRateAndSupplement());
            var sourceFolder = Any.ProjectFolder();
            var destinationFolder = Any.ProjectFolder();
            sourceFolder.UnlockFolder(true);
            destinationFolder.UnlockFolder(true);
            projectFolderRoot.AddFolder(sourceFolder);
            projectFolderRoot.AddFolder(destinationFolder);
            await projectFolderRootRepository.Add(projectFolderRoot);

            var authorizer = new MoveWorkItemsCommandAuthorizer(projectRepository, projectFolderRootRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(
                MoveWorkItemsCommand.Create(project.ProjectId.Value, sourceFolder.ProjectFolderId.Value, destinationFolder.ProjectFolderId.Value,
                    Any.Guids()), CancellationToken.None);

            authorizationResult.IsAuthorized.Should().BeTrue();
        }

        [Fact]
        public async Task MoveWorkItemsAsParticipantAndFoldersLocked_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            
            // Create project
            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            // Create Folder structure
            var projectFolderRoot = ProjectFolderRoot.Create(project.ProjectId, Any.ProjectName(), Any.ProjectFolderRootId(), GetDefaultFolderRateAndSupplement());
            var sourceFolder = Any.ProjectFolder();
            var destinationFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(sourceFolder);
            projectFolderRoot.AddFolder(destinationFolder);
            await projectFolderRootRepository.Add(projectFolderRoot);

            // Test 1: Source folder locked
            sourceFolder.LockFolder(true);
            destinationFolder.UnlockFolder(true);
            await projectFolderRootRepository.Update(projectFolderRoot);

            // Verify that authorization fails
            var authorizer = new MoveWorkItemsCommandAuthorizer(projectRepository, projectFolderRootRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(
                MoveWorkItemsCommand.Create(project.ProjectId.Value, sourceFolder.ProjectFolderId.Value, destinationFolder.ProjectFolderId.Value,
                    Any.Guids()), CancellationToken.None);
            authorizationResult.IsAuthorized.Should().BeFalse();
            
            // Test 2: Destination folder locked
            sourceFolder.UnlockFolder(true);
            destinationFolder.LockFolder(true);
            await projectFolderRootRepository.Update(projectFolderRoot);
            
            // Verify that authorization fails
            authorizationResult = await authorizer.Authorize(
                MoveWorkItemsCommand.Create(project.ProjectId.Value, sourceFolder.ProjectFolderId.Value, destinationFolder.ProjectFolderId.Value,
                    Any.Guids()), CancellationToken.None);
            authorizationResult.IsAuthorized.Should().BeFalse();
            
            // Test 3: Both folders are locked
            sourceFolder.LockFolder(true);
            destinationFolder.LockFolder(true);
            await projectFolderRootRepository.Update(projectFolderRoot);
            
            // Verify that authorization fails
            authorizationResult = await authorizer.Authorize(
                MoveWorkItemsCommand.Create(project.ProjectId.Value, sourceFolder.ProjectFolderId.Value, destinationFolder.ProjectFolderId.Value,
                    Any.Guids()), CancellationToken.None);
            authorizationResult.IsAuthorized.Should().BeFalse();
        }

        [Fact]
        public async Task RemoveProjectDocumentAsUnaffiliatedUser_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new MoveWorkItemsCommandAuthorizer(projectRepository, projectFolderRootRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(
                MoveWorkItemsCommand.Create(project.ProjectId.Value, Guid.NewGuid(), Guid.NewGuid(), Any.Guids()), 
                CancellationToken.None);

            authorizationResult.IsAuthorized.Should().BeFalse();
        }
    }
}
