using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.CopyWorkItems;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using FluentAssertions;
using Xunit;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Application.CopyWorkItems
{
    public class CopyWorkItemsCommandAuthorizerTest
    {
        private readonly ProjectInMemoryRepository _projectRepository;
        private readonly ProjectFolderRootInMemoryRepository _projectFolderRootRepository;
        private readonly FakeExecutionContext _executionContext;

        public CopyWorkItemsCommandAuthorizerTest()
        {
            _projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            _projectRepository = new ProjectInMemoryRepository();
            _executionContext = new FakeExecutionContext();
        }

        [Fact]
        public async Task CopyWorkItemsAsOwner_ShouldBeAuthorized()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectRepository.Add(project);

            var authorizer = new CopyWorkItemsCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _executionContext);

            var authorizationResult =
                await authorizer.Authorize(
                    CopyWorkItemsCommand.Create(project.ProjectId.Value, Any.Instance<Guid>(), Any.Instance<Guid>(), new List<Guid>()),
                    CancellationToken.None);
            authorizationResult.IsAuthorized.Should().BeTrue();
        }

        [Fact]
        public async Task CopyWorkItemsAsNonProjectParticipantAndNonOwner_ShouldNotBeAuthorized()
        {
            var project = Any.Project();
            await _projectRepository.Add(project);

            var authorizer = new CopyWorkItemsCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _executionContext);

            var authorizationResult =
                await authorizer.Authorize(
                    CopyWorkItemsCommand.Create(project.ProjectId.Value, Any.Instance<Guid>(), Any.Instance<Guid>(), new List<Guid>()),
                    CancellationToken.None);
            authorizationResult.IsAuthorized.Should().BeFalse();
        }
        
        [Fact]
        public async Task CopyWorkItemsAsProjectParticipantWhileFolderIsLocked_ShouldNotBeAuthorized()
        {
            var project = Any.Project().WithParticipant(_executionContext.UserId);
            await _projectRepository.Add(project);

            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), GetDefaultFolderRateAndSupplement());
            folderRoot.LockFolder(folderRoot.RootFolder.ProjectFolderId, true);
            await _projectFolderRootRepository.Add(folderRoot);

            var authorizer = new CopyWorkItemsCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _executionContext);
            
            var authorizationResult =
                await authorizer.Authorize(
                    CopyWorkItemsCommand.Create(project.ProjectId.Value, Any.Instance<Guid>(), folderRoot.RootFolder.ProjectFolderId.Value, new List<Guid>()),
                    CancellationToken.None);
            authorizationResult.IsAuthorized.Should().BeFalse();
        }
        
        [Fact]
        public async Task CopyWorkItemsAsProjectParticipantWhileFolderIsUnlocked_ShouldBeAuthorized()
        {
            var project = Any.Project().WithParticipant(_executionContext.UserId);
            await _projectRepository.Add(project);

            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), GetDefaultFolderRateAndSupplement());
            await _projectFolderRootRepository.Add(folderRoot);

            var authorizer = new CopyWorkItemsCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _executionContext);
            
            var authorizationResult =
                await authorizer.Authorize(
                    CopyWorkItemsCommand.Create(project.ProjectId.Value, Any.Instance<Guid>(), folderRoot.RootFolder.ProjectFolderId.Value, new List<Guid>()),
                    CancellationToken.None);
            authorizationResult.IsAuthorized.Should().BeTrue();
        }
    }
}
