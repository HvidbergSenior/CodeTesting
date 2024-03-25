using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.ExtraWorkAgreement
{
    public class UpdateExtraWorkAgreementTest
    {
        [Fact]
        public void WhenUpdatingExtraWorkAgreement_ShouldGetUpdatedPayment()
        {
            var project = Any.Project();

            var extraWorkAgreementList = Any.ProjectExtraWorkAgreementList();

            var extraWorkAgreement = ProjectExtraWorkAgreement.Create(project.ProjectId, Any.ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName.Create("Extra work agreement 1"), ProjectExtraWorkAgreementDescription.Empty(),
                ProjectExtraWorkAgreementNumber.Create("4953028"), ProjectExtraWorkAgreementPaymentDkr.Create(45.57m));

            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement);

            var newPayment = ProjectExtraWorkAgreementPaymentDkr.Create(50);

            extraWorkAgreementList.UpdateExtraWorkAgreement(project.ProjectId, extraWorkAgreement.ProjectExtraWorkAgreementId,
                extraWorkAgreement.ProjectExtraWorkAgreementNumber, extraWorkAgreement.ProjectExtraWorkAgreementName,
                extraWorkAgreement.ProjectExtraWorkAgreementDescription, extraWorkAgreement.ProjectExtraWorkAgreementType,
                newPayment, extraWorkAgreement.ProjectExtraWorkAgreementWorkTime?.Hours,
                extraWorkAgreement.ProjectExtraWorkAgreementWorkTime?.Minutes);

            Assert.Equal(50, extraWorkAgreementList.ExtraWorkAgreements[0].ProjectExtraWorkAgreementPaymentDkr.Value);
        }
    }
}
