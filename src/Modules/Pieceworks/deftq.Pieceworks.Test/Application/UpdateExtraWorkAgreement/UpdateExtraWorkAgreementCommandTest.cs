using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterExtraWorkAgreement;
using deftq.Pieceworks.Application.UpdateExtraWorkAgreement;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateExtraWorkAgreement
{
    public class UpdateExtraWorkAgreementCommandTest
    {
        [Fact]
        public async Task UpdateExtraWorkAgreement_ShouldUpdate()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectExtraWorkAgreementListInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var projectId = Any.ProjectId();

            var extraWorkAgreementList = ProjectExtraWorkAgreementList.Create(projectId, Any.ProjectExtraWorkAgreementListId());
            var extraWorkAgreement = Any.ProjectExtraWorkAgreementIncludingWorkTime();
            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement);
            await repository.Add(extraWorkAgreementList);
            var handler = new UpdateExtraWorkAgreementCommandHandler(repository, uow, executionContext);
            var newDescription = "Updated Description";
            var cmd = UpdateExtraWorkAgreementCommand.Create(projectId.Value, extraWorkAgreement.ProjectExtraWorkAgreementId.Value,
                extraWorkAgreement.ProjectExtraWorkAgreementNumber.Value, extraWorkAgreement.ProjectExtraWorkAgreementName.Value,
                newDescription,UpdateExtraWorkAgreementType.CompanyHours,
                extraWorkAgreement.ProjectExtraWorkAgreementPaymentDkr?.Value, extraWorkAgreement.ProjectExtraWorkAgreementWorkTime?.Hours.Value,
                extraWorkAgreement.ProjectExtraWorkAgreementWorkTime?.Minutes.Value);
            await handler.Handle(cmd, CancellationToken.None);
            
            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);

            var projectExtraWorkAgreementList = await repository.GetByProjectId(projectId.Value, CancellationToken.None);
            Assert.Equal(newDescription, projectExtraWorkAgreementList.ExtraWorkAgreements[0].ProjectExtraWorkAgreementDescription.Value);
        }

        [Fact]
        public async Task UpdateExtraWorkAgreementType_ShouldUpdate()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectExtraWorkAgreementListInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var projectId = Any.ProjectId();
            
            var extraWorkAgreementList = ProjectExtraWorkAgreementList.Create(projectId, Any.ProjectExtraWorkAgreementListId());
            var extraWorkAgreement = Any.ProjectExtraWorkAgreementIncludingWorkTime();
            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement);
            await repository.Add(extraWorkAgreementList);
            var handler = new UpdateExtraWorkAgreementCommandHandler(repository, uow, executionContext);

            var newType = UpdateExtraWorkAgreementType.AgreedPayment;
            var cmd = UpdateExtraWorkAgreementCommand.Create(projectId.Value, extraWorkAgreement.ProjectExtraWorkAgreementId.Value,
                extraWorkAgreement.ProjectExtraWorkAgreementNumber.Value, extraWorkAgreement.ProjectExtraWorkAgreementName.Value,
                extraWorkAgreement.ProjectExtraWorkAgreementDescription.Value, newType,
                extraWorkAgreement.ProjectExtraWorkAgreementPaymentDkr?.Value, extraWorkAgreement.ProjectExtraWorkAgreementWorkTime?.Hours.Value,
                extraWorkAgreement.ProjectExtraWorkAgreementWorkTime?.Minutes.Value);
            
            await handler.Handle(cmd, CancellationToken.None);
            
            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);

            var projectExtraWorkAgreementList = await repository.GetByProjectId(projectId.Value, CancellationToken.None);
            Assert.Equal(ProjectExtraWorkAgreementType.AgreedPayment(), projectExtraWorkAgreementList.ExtraWorkAgreements[0].ProjectExtraWorkAgreementType);
        }
    }
}
