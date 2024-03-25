using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateWorkItem;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateWorkItem
{
    public class UpdateWorkItemCommandAuthorizerTest
    {
        private readonly ProjectInMemoryRepository _projectRepository;
        private readonly ProjectFolderRootInMemoryRepository _projectFolderRootRepository;
        private readonly FakeExecutionContext _executionContext;

        public UpdateWorkItemCommandAuthorizerTest()
        {
            _projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            _projectRepository = new ProjectInMemoryRepository();
            _executionContext = new FakeExecutionContext();
        }
        
        [Fact]
        public async Task UpdateWorkItemAsOwner_ShouldBeAuthorized()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectRepository.Add(project);
            
            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), FolderRateAndSupplement.Empty());
            await _projectFolderRootRepository.Add(folderRoot);

            var authorizer = new UpdateWorkItemCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _executionContext);
            var authorizationResult =
                await authorizer.Authorize(UpdateWorkItemCommand.Create(project.ProjectId.Value, folderRoot.RootFolder.ProjectFolderId.Value, Guid.NewGuid(), Decimal.Zero),
                    CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task UpdateWorkItemAsParticipantWhileFolderIsUnlocked_ShouldBeAuthorized()
        {
            var project = Any.Project().WithParticipant(_executionContext.UserId);
            await _projectRepository.Add(project);
            
            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), FolderRateAndSupplement.Empty());
            folderRoot.UnlockFolder(folderRoot.RootFolder.ProjectFolderId, false);
            await _projectFolderRootRepository.Add(folderRoot);

            var authorizer = new UpdateWorkItemCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _executionContext);
            var authorizationResult =
                await authorizer.Authorize(UpdateWorkItemCommand.Create(project.ProjectId.Value, folderRoot.RootFolder.ProjectFolderId.Value, Guid.NewGuid(), Decimal.Zero),
                    CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task UpdateWorkItemAsParticipantWhileFolderIsLocked_ShouldNotBeAuthorized()
        {
            var project = Any.Project().WithParticipant(_executionContext.UserId);
            await _projectRepository.Add(project);
            
            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), FolderRateAndSupplement.Empty());
            folderRoot.LockFolder(folderRoot.RootFolder.ProjectFolderId, false);
            await _projectFolderRootRepository.Add(folderRoot);

            var authorizer = new UpdateWorkItemCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _executionContext);
            var authorizationResult =
                await authorizer.Authorize(UpdateWorkItemCommand.Create(project.ProjectId.Value, folderRoot.RootFolder.ProjectFolderId.Value, Guid.NewGuid(), Decimal.Zero),
                    CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task UpdateWorkItemAsProjectManagerWhileFolderIsUnLocked_ShouldBeAuthorized()
        {
            var project = Any.Project().WithProjectManager(_executionContext.UserId);
            await _projectRepository.Add(project);
            
            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), FolderRateAndSupplement.Empty());
            folderRoot.UnlockFolder(folderRoot.RootFolder.ProjectFolderId, false);
            await _projectFolderRootRepository.Add(folderRoot);

            var authorizer = new UpdateWorkItemCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _executionContext);
            var authorizationResult =
                await authorizer.Authorize(UpdateWorkItemCommand.Create(project.ProjectId.Value, folderRoot.RootFolder.ProjectFolderId.Value, Guid.NewGuid(), Decimal.Zero),
                    CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task UpdateWorkItemAsProjectManagerWhileFolderIsLocked_ShouldNotBeAuthorized()
        {
            var project = Any.Project().WithProjectManager(_executionContext.UserId);
            await _projectRepository.Add(project);
            
            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), FolderRateAndSupplement.Empty());
            folderRoot.LockFolder(folderRoot.RootFolder.ProjectFolderId, false);
            await _projectFolderRootRepository.Add(folderRoot);

            var authorizer = new UpdateWorkItemCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _executionContext);
            var authorizationResult =
                await authorizer.Authorize(UpdateWorkItemCommand.Create(project.ProjectId.Value, folderRoot.RootFolder.ProjectFolderId.Value, Guid.NewGuid(), Decimal.Zero),
                    CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
