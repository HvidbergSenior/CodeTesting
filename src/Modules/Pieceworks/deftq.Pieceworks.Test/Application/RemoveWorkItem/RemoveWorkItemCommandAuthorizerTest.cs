using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveWorkItem;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Application.RemoveWorkItem
{
    public class RemoveWorkItemCommandAuthorizerTest
    {
        private readonly FakeUnitOfWork _uow;
        private readonly ProjectInMemoryRepository _projectRepository;
        private readonly ProjectFolderRootInMemoryRepository _projectFolderRootRepository;
        private readonly FakeExecutionContext _executionContext;

        public RemoveWorkItemCommandAuthorizerTest()
        {
            _uow = new FakeUnitOfWork();
            _projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            _projectRepository = new ProjectInMemoryRepository();
            _executionContext = new FakeExecutionContext();
        }

        [Fact]
        public async Task RemoveWorkItemAsParticipantWhileFolderIsUnlocked_ShouldBeAuthorized()
        {
            var project = Any.Project().WithParticipant(_executionContext.UserId);
            await _projectRepository.Add(project);

            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), GetDefaultFolderRateAndSupplement());
            await _projectFolderRootRepository.Add(folderRoot);

            var authorizer = new RemoveWorkItemCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _uow, _executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RemoveWorkItemCommand.Create(project.ProjectId.Value, folderRoot.RootFolder.ProjectFolderId.Value, new List<Guid>()),
                    CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RemoveWorkItemAsParticipantWhileFolderIsLocked_ShouldNotBeAuthorized()
        {
            var project = Any.Project().WithParticipant(_executionContext.UserId);
            await _projectRepository.Add(project);

            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), GetDefaultFolderRateAndSupplement());
            folderRoot.LockFolder(folderRoot.RootFolder.ProjectFolderId, false);
            await _projectFolderRootRepository.Add(folderRoot);
            
            var authorizer = new RemoveWorkItemCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _uow, _executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RemoveWorkItemCommand.Create(project.ProjectId.Value, folderRoot.RootFolder.ProjectFolderId.Value,  new List<Guid>()),
                    CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RemoveWorkItemAsOwner_ShouldBeAuthorized()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectRepository.Add(project);

            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), GetDefaultFolderRateAndSupplement());
            await _projectFolderRootRepository.Add(folderRoot);
            
            var authorizer = new RemoveWorkItemCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _uow, _executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RemoveWorkItemCommand.Create(project.ProjectId.Value, folderRoot.RootFolder.ProjectFolderId.Value, new List<Guid>()),
                    CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RemoveWorkItemAsNonOwnerAndNonParticipant_ShouldNotBeAuthorized()
        {
            var project = Any.Project();
            await _projectRepository.Add(project);
            
            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), GetDefaultFolderRateAndSupplement());
            await _projectFolderRootRepository.Add(folderRoot);
            
            var authorizer = new RemoveWorkItemCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _uow, _executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RemoveWorkItemCommand.Create(project.ProjectId.Value, folderRoot.RootFolder.ProjectFolderId.Value, new List<Guid>()),
                    CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
