using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectCompensation;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.ProjectCompensation
{
    public class ProjectCompensationListTest
    {
        [Fact]
        public void RemoveSingleCompensationPaymentInList()
        {
            var project = Any.Project();
            var compensationPaymentList = ProjectCompensationList.Create(project.ProjectId, Any.ProjectCompensationListId());
            var compensationPayment1 = Any.ProjectCompensation();
            var compensationPayment2 = Any.ProjectCompensation();
            var compensationPayment3 = Any.ProjectCompensation();
            compensationPaymentList.AddCompensation(compensationPayment1);
            compensationPaymentList.AddCompensation(compensationPayment2);
            compensationPaymentList.AddCompensation(compensationPayment3);

            compensationPaymentList.RemoveCompensations(new List<ProjectCompensationId> { compensationPayment2.ProjectCompensationId });

            Assert.Equal(2, compensationPaymentList.Compensations.Count);
            Assert.DoesNotContain(compensationPayment2.ProjectCompensationId,
                compensationPaymentList.Compensations.Select(c => c.ProjectCompensationId));
        }

        [Fact]
        public void RemoveAllCompensationPaymentsInList()
        {
            var project = Any.Project();
            var compensationPaymentList = ProjectCompensationList.Create(project.ProjectId, Any.ProjectCompensationListId());
            var compensationPayment1 = Any.ProjectCompensation();
            var compensationPayment2 = Any.ProjectCompensation();
            var compensationPayment3 = Any.ProjectCompensation();
            compensationPaymentList.AddCompensation(compensationPayment1);
            compensationPaymentList.AddCompensation(compensationPayment2);
            compensationPaymentList.AddCompensation(compensationPayment3);

            compensationPaymentList.RemoveCompensations(new List<ProjectCompensationId>
            {
                compensationPayment1.ProjectCompensationId, compensationPayment2.ProjectCompensationId, compensationPayment3.ProjectCompensationId
            });
            Assert.Equal(0, compensationPaymentList.Compensations.Count);
        }

        [Fact]
        public void RemoveCompensationPaymentThatDoNotExists()
        {
            var project = Any.Project();
            var compensationPaymentList = ProjectCompensationList.Create(project.ProjectId, Any.ProjectCompensationListId());
            var compensationPayment1 = Any.ProjectCompensation();
            var compensationPayment2 = Any.ProjectCompensation();
            var compensationPayment3 = Any.ProjectCompensation();
            compensationPaymentList.AddCompensation(compensationPayment1);
            compensationPaymentList.AddCompensation(compensationPayment2);
            compensationPaymentList.AddCompensation(compensationPayment3);

            var toRemove = ProjectCompensationId.Create(Any.Guid());
            Assert.Throws<ProjectCompensationNotFoundException>(() =>
            {
                compensationPaymentList.RemoveCompensations(new List<ProjectCompensationId> { toRemove });
            });
        }
    }
}
