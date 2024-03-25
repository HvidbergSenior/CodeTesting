using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveExtraWorkAgreement;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveExtraWorkAgreement
{
    public class RemoveExtraWorkAgreementCommandTest
    {
        private readonly FakeUnitOfWork _uow;
        private readonly FakeExecutionContext _executionContext;
        private readonly ProjectExtraWorkAgreementListInMemoryRepository _projectExtraWorkAgreementListInMemoryRepository;

        public RemoveExtraWorkAgreementCommandTest()
        {
            _uow = new FakeUnitOfWork();
            _executionContext = new FakeExecutionContext();
            _projectExtraWorkAgreementListInMemoryRepository = new ProjectExtraWorkAgreementListInMemoryRepository();
        }

        [Fact]
        public async Task RemoveMultipleExtraWorkAgreements_ShouldNotReturnExtraWorkAgreements()
        {
            var project = Any.Project();

            var extraWorkAgreementList = ProjectExtraWorkAgreementList.Create(project.ProjectId, Any.ProjectExtraWorkAgreementListId());

            var extraWorkAgreement1 = Any.ProjectExtraWorkAgreementIncludingPaymentDkr();
            var extraWorkAgreement2 = Any.ProjectExtraWorkAgreementIncludingWorkTime();

            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement1);
            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement2);

            await _projectExtraWorkAgreementListInMemoryRepository.Add(extraWorkAgreementList);

            var handler = new RemoveExtraWorkAgreementCommandHandler(_projectExtraWorkAgreementListInMemoryRepository, _uow, _executionContext);
            var cmd = RemoveExtraWorkAgreementCommand.Create(project.ProjectId.Value,
                new List<Guid> { extraWorkAgreement1.ProjectExtraWorkAgreementId.Value, extraWorkAgreement2.ProjectExtraWorkAgreementId.Value });

            await handler.Handle(cmd, CancellationToken.None);

            Assert.Single(_projectExtraWorkAgreementListInMemoryRepository.Entities);
            Assert.True(_projectExtraWorkAgreementListInMemoryRepository.SaveChangesCalled);
            Assert.True(_uow.IsCommitted);

            extraWorkAgreementList =
                await _projectExtraWorkAgreementListInMemoryRepository.GetByProjectId(project.ProjectId.Value, CancellationToken.None);
            Assert.Empty(extraWorkAgreementList.ExtraWorkAgreements);
        }

        [Fact]
        public async Task RemoveUnknownExtraWorkAgreement_ShouldThrow()
        {
            var project = Any.Project();
            var extraWorkAgreementList = ProjectExtraWorkAgreementList.Create(project.ProjectId, Any.ProjectExtraWorkAgreementListId());

            await _projectExtraWorkAgreementListInMemoryRepository.Add(extraWorkAgreementList);

            var handler = new RemoveExtraWorkAgreementCommandHandler(_projectExtraWorkAgreementListInMemoryRepository, _uow, _executionContext);
            var cmd = RemoveExtraWorkAgreementCommand.Create(project.ProjectId.Value,
                new List<Guid> { Any.ProjectExtraWorkAgreementId().Value, Any.ProjectExtraWorkAgreementId().Value });

            await Assert.ThrowsAsync<ProjectExtraWorkAgreementNotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }
    }
}
