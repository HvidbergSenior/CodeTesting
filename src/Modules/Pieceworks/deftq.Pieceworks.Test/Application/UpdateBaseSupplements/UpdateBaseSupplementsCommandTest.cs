using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateBaseSupplements;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateBaseSupplements
{
    public class UpdateBaseSupplementsCommandTest
    {
        private readonly FakeUnitOfWork _unitOfWork;
        private readonly ProjectFolderRootInMemoryRepository _projectFolderRootRepository;
        private readonly BaseRateAndSupplementRepository _baseRateAndSupplementRepository;

        public UpdateBaseSupplementsCommandTest()
        {
            _unitOfWork = new FakeUnitOfWork();
            _projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            _baseRateAndSupplementRepository = new BaseRateAndSupplementRepository();
        }

        [Fact]
        public async Task GivenFolder_WhenUpdatingBaseRateRegulation_ShouldUpdateBaseRateRegulation()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            await _projectFolderRootRepository.Add(projectFolderRoot);

            var handler = new UpdateBaseSupplementsCommandHandler(_projectFolderRootRepository, _baseRateAndSupplementRepository, _unitOfWork);
            var cmd = UpdateBaseSupplementsCommand.Create(projectFolderRoot.ProjectId.Value, projectFolderRoot.RootFolder.ProjectFolderId.Value,
                100, UpdateBaseSupplementsCommand.UpdateBaseSupplementStatusEnum.Overwrite,
                120, UpdateBaseSupplementsCommand.UpdateBaseSupplementStatusEnum.Overwrite);
            await handler.Handle(cmd, CancellationToken.None);

            Assert.Single(_projectFolderRootRepository.Entities);
            Assert.True(_projectFolderRootRepository.SaveChangesCalled);
            Assert.True(_unitOfWork.IsCommitted);

            var projectFolderRootFromRepo = await _projectFolderRootRepository.GetByProjectId(projectFolderRoot.ProjectId.Value);
            Assert.Equal(100, projectFolderRootFromRepo.RootFolder.FolderRateAndSupplement.IndirectTimeSupplement.Value);
            Assert.Equal(FolderValueInheritStatus.Overwrite(),
                projectFolderRootFromRepo.RootFolder.FolderRateAndSupplement.IndirectTimeSupplement.InheritStatus);
            Assert.Equal(120, projectFolderRootFromRepo.RootFolder.FolderRateAndSupplement.SiteSpecificTimeSupplement.Value);
            Assert.Equal(FolderValueInheritStatus.Overwrite(),
                projectFolderRootFromRepo.RootFolder.FolderRateAndSupplement.SiteSpecificTimeSupplement.InheritStatus);
        }
    }
}
