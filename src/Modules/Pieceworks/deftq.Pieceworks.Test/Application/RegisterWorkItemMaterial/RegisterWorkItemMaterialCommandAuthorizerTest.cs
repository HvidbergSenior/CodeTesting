using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterWorkItemMaterial;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using SupplementOperation = deftq.Pieceworks.Application.RegisterWorkItemMaterial.SupplementOperation;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Application.RegisterWorkItemMaterial
{
    public class RegisterWorkItemMaterialCommandAuthorizerTest
    {
        private readonly FakeUnitOfWork _uow;
        private readonly ProjectInMemoryRepository _projectRepository;
        private readonly ProjectFolderRootInMemoryRepository _projectFolderRootRepository;
        private readonly FakeExecutionContext _executionContext;

        public RegisterWorkItemMaterialCommandAuthorizerTest()
        {
            _uow = new FakeUnitOfWork();
            _projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            _projectRepository = new ProjectInMemoryRepository();
            _executionContext = new FakeExecutionContext();
        }
            
        [Fact]
        public async Task RegisterWorkItemAsProjectParticipantWhileFolderIsUnlocked_ShouldBeAuthorized()
        {
            var project = Any.Project().WithParticipant(_executionContext.UserId);
            await _projectRepository.Add(project);

            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), GetDefaultFolderRateAndSupplement());
            await _projectFolderRootRepository.Add(folderRoot);
            
            var authorizer = new RegisterWorkItemMaterialCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _uow, _executionContext);
            var authorizationResult = await authorizer.Authorize(RegisterWorkItemMaterialCommand.Create(project.ProjectId.Value, folderRoot.RootFolder.ProjectFolderId.Value,
                Guid.NewGuid(), Guid.NewGuid(), "", DateOnly.MaxValue, Decimal.Zero, WorkItemMountingCode.FromCode(3).MountingCode, Decimal.MaxValue,
                "0000000000000", "stk",
                new List<SupplementOperation>(), new List<Supplement>()), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RegisterWorkItemAsProjectParticipantWhileFolderIsLocked_ShouldNotBeAuthorized()
        {
            var project = Any.Project().WithParticipant(_executionContext.UserId);
            await _projectRepository.Add(project);

            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), GetDefaultFolderRateAndSupplement());
            folderRoot.LockFolder(folderRoot.RootFolder.ProjectFolderId, true);
            await _projectFolderRootRepository.Add(folderRoot);

            var authorizer = new RegisterWorkItemMaterialCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _uow, _executionContext);
            var authorizationResult = await authorizer.Authorize(RegisterWorkItemMaterialCommand.Create(project.ProjectId.Value, folderRoot.RootFolder.ProjectFolderId.Value,
                Guid.NewGuid(), Guid.NewGuid(), "", DateOnly.MaxValue, Decimal.Zero, WorkItemMountingCode.FromCode(3).MountingCode, Decimal.MaxValue,
                "0000000000000", "stk",
                new List<SupplementOperation>(), new List<Supplement>()), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RegisterWorkItemsAsNonProjectParticipantAndNonOwner_ShouldNotBeAuthorized()
        {
            var project = Any.Project();
            await _projectRepository.Add(project);
            
            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), GetDefaultFolderRateAndSupplement());
            await _projectFolderRootRepository.Add(folderRoot);

            var authorizer = new RegisterWorkItemMaterialCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _uow, _executionContext);
            var authorizationResult = await authorizer.Authorize(RegisterWorkItemMaterialCommand.Create(project.ProjectId.Value, folderRoot.RootFolder.ProjectFolderId.Value,
                Guid.NewGuid(), Guid.NewGuid(), "", DateOnly.MaxValue, Decimal.Zero, WorkItemMountingCode.FromCode(3).MountingCode, Decimal.MaxValue,
                "0000000000000", "stk", new List<SupplementOperation>(), new List<Supplement>()), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RegisterWorkItemAsOwner_ShouldBeAuthorized()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectRepository.Add(project);

            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, project.ProjectName, ProjectFolderRootId.Empty(), GetDefaultFolderRateAndSupplement());
            await _projectFolderRootRepository.Add(folderRoot);
            
            var authorizer = new RegisterWorkItemMaterialCommandAuthorizer(_projectRepository, _projectFolderRootRepository, _uow, _executionContext);
            var authorizationResult = await authorizer.Authorize(RegisterWorkItemMaterialCommand.Create(project.ProjectId.Value, folderRoot.RootFolder.ProjectFolderId.Value,
                Guid.NewGuid(), Guid.NewGuid(), "", DateOnly.MaxValue, Decimal.Zero, WorkItemMountingCode.FromCode(3).MountingCode, Decimal.MaxValue,
                "0000000000000", "stk", new List<SupplementOperation>(), new List<Supplement>()), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }
    }
}
