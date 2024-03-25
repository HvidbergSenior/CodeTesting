using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.projectFolderRoot.BaseRateAndSupplement
{
    public class FolderIndirectTimeSupplementTest
    {
        [Fact]
        public void ValidValues()
        {
            Assert.Throws<ArgumentException>(() => FolderIndirectTimeSupplement.Create(-1, FolderValueInheritStatus.Overwrite()));
            Assert.Throws<ArgumentException>(() => FolderIndirectTimeSupplement.Create(-0.1m, FolderValueInheritStatus.Overwrite()));
            FolderIndirectTimeSupplement.Create(0.1m, FolderValueInheritStatus.Overwrite());
            FolderIndirectTimeSupplement.Create(99.996m, FolderValueInheritStatus.Overwrite());
            FolderIndirectTimeSupplement.Create(102.996m, FolderValueInheritStatus.Overwrite());
        }
    }
}
