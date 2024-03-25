using deftq.Pieceworks.Domain.project.Company;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.project.Company
{
    public class ProjectCompanyCvrNumberTest
    {
        [Fact]
        public void Valid_CVR_Numbers()
        {
            ProjectCompanyCvrNo.Create("");
            ProjectCompanyCvrNo.Create("12578970");
            Assert.Throws<ArgumentException>(() => ProjectCompanyCvrNo.Create("1"));
            Assert.Throws<ArgumentException>(() => ProjectCompanyCvrNo.Create("125789701"));
            Assert.Throws<ArgumentException>(() => ProjectCompanyCvrNo.Create("aaaaaaaa"));
        }
    }
}
