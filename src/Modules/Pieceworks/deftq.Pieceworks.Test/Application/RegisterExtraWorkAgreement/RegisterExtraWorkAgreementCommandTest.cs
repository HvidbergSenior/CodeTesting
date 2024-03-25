using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterExtraWorkAgreement;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RegisterExtraWorkAgreement
{
    public class RegisterExtraWorkAgreementCommandTest
    {
        [Fact]
        public async Task WhenRegisteringExtraWorkAgreement_ExtraWorkAgreementIsInList()
        {
            var fakeUnitOfWork = new FakeUnitOfWork();
            var repository = new ProjectExtraWorkAgreementListInMemoryRepository();
            var projectId = Any.ProjectId();
            await repository.Add(ProjectExtraWorkAgreementList.Create(projectId, Any.ProjectExtraWorkAgreementListId()));

            var handler = new RegisterExtraWorkAgreementCommandHandler(repository, fakeUnitOfWork);
            var extraWorkAgreementId = Any.Guid();
            var extraWorkAgreementName = "this is a name";
            var extraWorkAgreementDescription = "this is a description";
            var extraWorkAgreementType = ExtraWorkAgreementType.AgreedPayment;
            var extraWorkAgreementNumber = "294850128";
            var extraWorkAgreementWorkTimeHours = ProjectExtraWorkAgreementHours.Create(20);
            var extraWorkAgreementWorkTimeMinutes = ProjectExtraWorkAgreementMinutes.Create(20);
            var extraWorkAgreementPaymentDkr = ProjectExtraWorkAgreementPaymentDkr.Create(29);
            var cmd = RegisterExtraWorkAgreementCommand.Create(projectId.Value, extraWorkAgreementId, extraWorkAgreementNumber, extraWorkAgreementName,
                extraWorkAgreementDescription, extraWorkAgreementType, extraWorkAgreementPaymentDkr.Value, extraWorkAgreementWorkTimeHours.Value, extraWorkAgreementWorkTimeMinutes.Value);
            await handler.Handle(cmd, CancellationToken.None);

            var extraWorkAgreementList = await repository.GetByProjectId(projectId.Value, CancellationToken.None);

            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(fakeUnitOfWork.IsCommitted);
            
            Assert.Equal(extraWorkAgreementId, extraWorkAgreementList.ExtraWorkAgreements[0].ProjectExtraWorkAgreementId.Value);
            Assert.Equal(extraWorkAgreementName, extraWorkAgreementList.ExtraWorkAgreements[0].ProjectExtraWorkAgreementName.Value);
            Assert.Equal(extraWorkAgreementNumber, extraWorkAgreementList.ExtraWorkAgreements[0].ProjectExtraWorkAgreementNumber.Value);
            Assert.Equal(extraWorkAgreementPaymentDkr.Value, extraWorkAgreementList.ExtraWorkAgreements[0].ProjectExtraWorkAgreementPaymentDkr?.Value);
            Assert.Equal(ProjectExtraWorkAgreementType.AgreedPayment(), extraWorkAgreementList.ExtraWorkAgreements[0].ProjectExtraWorkAgreementType);
        }
    }
}
