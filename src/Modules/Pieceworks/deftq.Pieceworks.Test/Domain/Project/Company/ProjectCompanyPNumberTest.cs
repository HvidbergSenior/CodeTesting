using deftq.Pieceworks.Domain.project.Company;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.project.Company
{
    public class ProjectCompanyPNumberTest
    {
        [Fact]
        public void Valid_P_Numbers()
        {
            ProjectCompanyPNo.Create("");
            ProjectCompanyPNo.Create("1012360688");
            Assert.Throws<ArgumentException>(() => ProjectCompanyCvrNo.Create("1"));
            Assert.Throws<ArgumentException>(() => ProjectCompanyCvrNo.Create("10123606880"));
            Assert.Throws<ArgumentException>(() => ProjectCompanyCvrNo.Create("aaaaaaaaaa"));
        }
    }
}
