using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.Calculation
{
    public class ExtraWorkAgreementCalculatorTest
    {
        [Fact]
        public void GivenExtraWorkAgreementWithWorkTime_ShouldCalculateCorrectly()
        {
            var project = Any.Project();

            var calculator = new ExtraWorkAgreementCalculator();

            var extraWorkAgreementList = Any.ProjectExtraWorkAgreementList();
            extraWorkAgreementList.SetCompanyRate(ProjectExtraWorkAgreementCompanyHourRate.Create(100));
            var extraWorkAgreement = ProjectExtraWorkAgreement.Create(project.ProjectId, Any.ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName.Create("Extra work agreement 1"), ProjectExtraWorkAgreementDescription.Empty(),
                ProjectExtraWorkAgreementType.CompanyHours(), ProjectExtraWorkAgreementNumber.Create("4953028"),
                ProjectExtraWorkAgreementWorkTime.Create(ProjectExtraWorkAgreementHours.Create(7), ProjectExtraWorkAgreementMinutes.Create(30)));
            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement);

            var result = calculator.SumTotalPayment(extraWorkAgreementList);

            Assert.Equal(750, result.Result.Evaluate().Value);
        }

        [Fact]
        public void RegisteringExtraWorkAgreementWithNegativeWorkTimeMinutes_ShouldThrow()
        {
            var project = Any.Project();

            Assert.Throws<ArgumentException>(() => ProjectExtraWorkAgreement.Create(project.ProjectId, Any.ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName.Create("Extra work agreement 1"), ProjectExtraWorkAgreementDescription.Empty(),
                ProjectExtraWorkAgreementType.CompanyHours(), ProjectExtraWorkAgreementNumber.Create("4953028"),
                ProjectExtraWorkAgreementWorkTime.Create(ProjectExtraWorkAgreementHours.Create(-7), ProjectExtraWorkAgreementMinutes.Create(30))));
        }

        [Fact]
        public void RegisteringExtraWorkAgreementWithNegativeWorkTimeHours_ShouldThrow()
        {
            var project = Any.Project();

            Assert.Throws<ArgumentException>(() => ProjectExtraWorkAgreement.Create(project.ProjectId, Any.ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName.Create("Extra work agreement 1"), ProjectExtraWorkAgreementDescription.Empty(),
                ProjectExtraWorkAgreementType.CompanyHours(), ProjectExtraWorkAgreementNumber.Create("4953028"),
                ProjectExtraWorkAgreementWorkTime.Create(ProjectExtraWorkAgreementHours.Create(7), ProjectExtraWorkAgreementMinutes.Create(-30))));
        }

        [Fact]
        public void GivenExtraWorkAgreementWithPayment_ShouldCalculateCorrectly()
        {
            var project = Any.Project();

            var calculator = new ExtraWorkAgreementCalculator();

            var extraWorkAgreementList = Any.ProjectExtraWorkAgreementList();

            var extraWorkAgreement = ProjectExtraWorkAgreement.Create(project.ProjectId, Any.ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName.Create("Extra work agreement 1"), ProjectExtraWorkAgreementDescription.Empty(),
                ProjectExtraWorkAgreementNumber.Create("4953028"), ProjectExtraWorkAgreementPaymentDkr.Create(45.57m));
            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement);

            var result = calculator.SumTotalPayment(extraWorkAgreementList);

            Assert.Equal(45.57m, result.Result.Evaluate().Value);
        }

        [Fact]
        public void WhenRemovingExtraWorkAgreementFromList_ShouldCalculateCorrectly()
        {
            var project = Any.Project();

            var calculator = new ExtraWorkAgreementCalculator();

            var extraWorkAgreementList = Any.ProjectExtraWorkAgreementList();

            var extraWorkAgreement = ProjectExtraWorkAgreement.Create(project.ProjectId, Any.ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName.Create("Extra work agreement 1"), ProjectExtraWorkAgreementDescription.Empty(),
                ProjectExtraWorkAgreementNumber.Create("4953028"), ProjectExtraWorkAgreementPaymentDkr.Create(45.57m));

            var extraWorkAgreement2 = ProjectExtraWorkAgreement.Create(project.ProjectId, Any.ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName.Create("Extra work agreement 2"), ProjectExtraWorkAgreementDescription.Empty(),
                ProjectExtraWorkAgreementNumber.Create("65258"), ProjectExtraWorkAgreementPaymentDkr.Create(78.44m));

            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement);
            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement2);

            var result = calculator.SumTotalPayment(extraWorkAgreementList);
            Assert.Equal(124.01m, result.Result.Evaluate().Value);

            extraWorkAgreementList.RemoveExtraWorkAgreements(new List<ProjectExtraWorkAgreementId>
            {
                extraWorkAgreement.ProjectExtraWorkAgreementId
            });

            result = calculator.SumTotalPayment(extraWorkAgreementList);
            Assert.Equal(78.44m, result.Result.Evaluate().Value);
        }

        [Fact]
        public void GivenExtraWorkAgreementWithTypeOther_ResultShouldBeZero()
        {
            var calculator = new ExtraWorkAgreementCalculator();

            var extraWorkAgreementList = Any.ProjectExtraWorkAgreementList();
            var extraWorkAgreement = Any.ProjectExtraWorkAgreementOther();
            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement);

            var result = calculator.SumTotalPayment(extraWorkAgreementList);

            Assert.Equal(0, result.Result.Evaluate().Value);
        }

        [Fact]
        public void GivenExtraWorkAgreementWithNegativePayments_ShouldCalculateCorrectly()
        {
            var project = Any.Project();

            var calculator = new ExtraWorkAgreementCalculator();

            var extraWorkAgreementList = Any.ProjectExtraWorkAgreementList();
            var extraWorkAgreement1 = ProjectExtraWorkAgreement.Create(project.ProjectId, Any.ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName.Create("Extra work agreement 1"), ProjectExtraWorkAgreementDescription.Empty(),
                ProjectExtraWorkAgreementNumber.Create("4953028"), ProjectExtraWorkAgreementPaymentDkr.Create(-45.57m));
            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement1);

            var extraWorkAgreement2 = ProjectExtraWorkAgreement.Create(project.ProjectId, Any.ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName.Create("Extra work agreement 1"), ProjectExtraWorkAgreementDescription.Empty(),
                ProjectExtraWorkAgreementNumber.Create("4953028"), ProjectExtraWorkAgreementPaymentDkr.Create(-459.62m));
            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement2);

            var result = calculator.SumTotalPayment(extraWorkAgreementList);

            Assert.Equal(-505.19m, result.Result.Evaluate().Value);
        }

        [Fact]
        public void GivenMultipleDifferentExtraWorkAgreements_ShouldCalculateCorrectly()
        {
            var project = Any.Project();

            var calculator = new ExtraWorkAgreementCalculator();
            var extraWorkAgreementList = Any.ProjectExtraWorkAgreementList();

            var extraWorkAgreement1 = ProjectExtraWorkAgreement.Create(project.ProjectId, Any.ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName.Create("Extra work agreement 1"), ProjectExtraWorkAgreementDescription.Empty(),
                ProjectExtraWorkAgreementNumber.Create("4953028"), ProjectExtraWorkAgreementPaymentDkr.Create(45.53m));
            var extraWorkAgreement2 = ProjectExtraWorkAgreement.Create(project.ProjectId, Any.ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName.Create("Extra work agreement 2"), ProjectExtraWorkAgreementDescription.Empty(),
                ProjectExtraWorkAgreementNumber.Create("4953028"), ProjectExtraWorkAgreementPaymentDkr.Create(730));
            var extraWorkAgreement3 = ProjectExtraWorkAgreement.Create(project.ProjectId, Any.ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName.Create("Extra work agreement 3"), ProjectExtraWorkAgreementDescription.Empty(),
                ProjectExtraWorkAgreementNumber.Create("4953028"), ProjectExtraWorkAgreementPaymentDkr.Create(500.64m));

            extraWorkAgreementList.SetCompanyRate(ProjectExtraWorkAgreementCompanyHourRate.Create(100));
            var extraWorkAgreement4 = ProjectExtraWorkAgreement.Create(project.ProjectId, Any.ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName.Create("Extra work agreement 4"), ProjectExtraWorkAgreementDescription.Empty(),
                ProjectExtraWorkAgreementType.CompanyHours(), ProjectExtraWorkAgreementNumber.Create("4953028"),
                ProjectExtraWorkAgreementWorkTime.Create(ProjectExtraWorkAgreementHours.Create(8), ProjectExtraWorkAgreementMinutes.Create(30)));

            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement1);
            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement2);
            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement3);
            extraWorkAgreementList.AddExtraWorkAgreement(extraWorkAgreement4);

            var result = calculator.SumTotalPayment(extraWorkAgreementList);

            Assert.Equal(2126.17m, result.Result.Evaluate().Value);
        }

        [Fact]
        public void GivenNoExtraWorkAgreements_ResultShouldBeZero()
        {
            var calculator = new ExtraWorkAgreementCalculator();

            var extraWorkAgreementList = Any.ProjectExtraWorkAgreementList();

            var result = calculator.SumTotalPayment(extraWorkAgreementList);

            Assert.Equal(0, result.Result.Evaluate().Value);
        }
    }
}
