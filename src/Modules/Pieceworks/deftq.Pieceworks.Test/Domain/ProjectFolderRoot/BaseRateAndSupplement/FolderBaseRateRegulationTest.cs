using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.projectFolderRoot.BaseRateAndSupplement
{
    public class FolderBaseRateRegulationTest
    {
        [Fact]
        public void ValidValues()
        {
            Assert.Throws<ArgumentException>(() => FolderBaseRateRegulation.Create(-1, FolderValueInheritStatus.Overwrite()));
            Assert.Throws<ArgumentException>(() => FolderBaseRateRegulation.Create(-0.1m, FolderValueInheritStatus.Overwrite()));
            FolderBaseRateRegulation.Create(0.1m, FolderValueInheritStatus.Overwrite());
            FolderBaseRateRegulation.Create(99.996m, FolderValueInheritStatus.Overwrite());
            FolderBaseRateRegulation.Create(102.996m, FolderValueInheritStatus.Overwrite());
        }
    }
}
