using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterWorkItemOperation;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using Supplement = deftq.Pieceworks.Application.RegisterWorkItemOperation.Supplement;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Application.RegisterWorkItemOperation
{
    public class RegisterWorkItemOperationCommandAuthorizerTest
    {
        private readonly FakeUnitOfWork _uow;
        private readonly ProjectInMemoryRepository _projectRepository;
        private readonly ProjectFolderRootInMemoryRepository _projectFolderRootRepository;
        private readonly FakeExecutionContext _executionContext;

        public RegisterWorkItemOperationCommandAuthorizerTest()
        {
            _uow = new FakeUnitOfWork();
            _projectRepository = new ProjectInMemoryRepository();
            _projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            _executionContext = new FakeExecutionContext();
        }

        [Fact]
        public async Task RegisterWorkItemAsProjectParticipantWhileFolderIsUnlocked_ShouldBeAuthorized()
        {
            var project = Any.Project().WithParticipant(_executionContext.UserId);
            await _projectRepository.Add(project);
            
            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), GetDefaultFolderRateAndSupplement());
            await _projectFolderRootRepository.Add(folderRoot);

            var authorizer = new RegisterWorkItemOperationCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _uow, _executionContext);
            var authorizationResult = await authorizer.Authorize(RegisterWorkItemOperationCommand.Create(project.ProjectId.Value, ProjectFolderRoot.RootFolderId.Value,
                Guid.NewGuid(), "", Guid.NewGuid(), "", DateOnly.MaxValue, Decimal.Zero, Decimal.MaxValue,
                new List<Supplement>()), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RegisterWorkItemsAsProjectParticipantWhileFolderIsLocked_ShouldNotBeAuthorized()
        {
            var project = Any.Project().WithParticipant(_executionContext.UserId);
            await _projectRepository.Add(project);
            
            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), GetDefaultFolderRateAndSupplement());
            folderRoot.LockFolder(ProjectFolderRoot.RootFolderId, false);
            await _projectFolderRootRepository.Add(folderRoot);
            
            var authorizer = new RegisterWorkItemOperationCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _uow, _executionContext);
            var authorizationResult = await authorizer.Authorize(RegisterWorkItemOperationCommand.Create(project.ProjectId.Value, ProjectFolderRoot.RootFolderId.Value,
                Guid.NewGuid(), "", Guid.NewGuid(), "", DateOnly.MaxValue, Decimal.Zero, Decimal.MaxValue,
                new List<Supplement>()), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RegisterWorkItemsAsNonProjectParticipantAndNonOwner_ShouldNotBeAuthorized()
        {
            var project = Any.Project();
            await _projectRepository.Add(project);
            
            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), GetDefaultFolderRateAndSupplement());
            await _projectFolderRootRepository.Add(folderRoot);

            var authorizer = new RegisterWorkItemOperationCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _uow, _executionContext);
            var authorizationResult = await authorizer.Authorize(RegisterWorkItemOperationCommand.Create(project.ProjectId.Value, ProjectFolderRoot.RootFolderId.Value,
                Guid.NewGuid(), "", Guid.NewGuid(), "", DateOnly.MaxValue, Decimal.Zero,
                Decimal.MaxValue, new List<Supplement>()), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RegisterWorkItemAsOwner_ShouldBeAuthorized()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectRepository.Add(project);

            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), GetDefaultFolderRateAndSupplement());
            await _projectFolderRootRepository.Add(folderRoot);
            
            var authorizer = new RegisterWorkItemOperationCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _uow, _executionContext);
            var authorizationResult = await authorizer.Authorize(RegisterWorkItemOperationCommand.Create(project.ProjectId.Value, ProjectFolderRoot.RootFolderId.Value,
                Guid.NewGuid(), "", Guid.NewGuid(), "", DateOnly.MaxValue, Decimal.Zero,
                Decimal.MaxValue, new List<Supplement>()), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }
    }
}
