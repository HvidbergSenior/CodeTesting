using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateBaseRateRegulation;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateBaseRateRegulation
{
    public class UpdateBaseRateRegulationCommandTest
    {
        private readonly FakeUnitOfWork _unitOfWork;
        private readonly ProjectFolderRootInMemoryRepository _projectFolderRootRepository;
        private readonly BaseRateAndSupplementRepository _baseRateAndSupplementRepository;

        public UpdateBaseRateRegulationCommandTest()
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

            var handler = new UpdateBaseRateRegulationCommandHandler(_projectFolderRootRepository, _baseRateAndSupplementRepository, _unitOfWork);
            var cmd = UpdateBaseRateRegulationCommand.Create(projectFolderRoot.ProjectId.Value, projectFolderRoot.RootFolder.ProjectFolderId.Value,
                100, UpdateBaseRateRegulationCommand.UpdateBaseRateRegulationStatusEnum.Overwrite);
            await handler.Handle(cmd, CancellationToken.None);

            Assert.Single(_projectFolderRootRepository.Entities);
            Assert.True(_projectFolderRootRepository.SaveChangesCalled);
            Assert.True(_unitOfWork.IsCommitted);

            var projectFolderRootFromRepo = await _projectFolderRootRepository.GetByProjectId(projectFolderRoot.ProjectId.Value);
            Assert.Equal(100, projectFolderRootFromRepo.RootFolder.FolderRateAndSupplement.BaseRateRegulation.Value);
            Assert.Equal(FolderValueInheritStatus.Overwrite(),
                projectFolderRootFromRepo.RootFolder.FolderRateAndSupplement.BaseRateRegulation.InheritStatus);
        }
    }
}
