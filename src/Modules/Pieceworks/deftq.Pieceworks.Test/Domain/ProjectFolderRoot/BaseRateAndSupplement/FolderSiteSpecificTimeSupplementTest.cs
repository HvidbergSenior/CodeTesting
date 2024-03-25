using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.projectFolderRoot.BaseRateAndSupplement
{
    public class FolderSiteSpecificTimeSupplementTest
    {
        [Fact]
        public void ValidValues()
        {
            Assert.Throws<ArgumentException>(() => FolderSiteSpecificTimeSupplement.Create(-1, FolderValueInheritStatus.Overwrite()));
            Assert.Throws<ArgumentException>(() => FolderSiteSpecificTimeSupplement.Create(-0.1m, FolderValueInheritStatus.Overwrite()));
            FolderSiteSpecificTimeSupplement.Create(0.1m, FolderValueInheritStatus.Overwrite());
            FolderSiteSpecificTimeSupplement.Create(99.996m, FolderValueInheritStatus.Overwrite());
            FolderSiteSpecificTimeSupplement.Create(102.996m, FolderValueInheritStatus.Overwrite());
        }
    }
}
