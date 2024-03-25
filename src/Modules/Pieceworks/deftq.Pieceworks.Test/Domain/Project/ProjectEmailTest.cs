using deftq.Pieceworks.Domain.project;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.project
{
    public class ProjectEmailTest
    {
        [Fact]
        public void WhenHaveValidEmail_ShouldNotThrowException()
        {
            var res = ProjectEmail.Create("brian@mjolner.dk");
        }
        
        [Fact]
        public void WhenHaveInvalidEmail_ShouldThrowException()
        {
            Assert.Throws<FormatException>(() => ProjectEmail.Create("@mjolner.dk"));
        }
    }
}
