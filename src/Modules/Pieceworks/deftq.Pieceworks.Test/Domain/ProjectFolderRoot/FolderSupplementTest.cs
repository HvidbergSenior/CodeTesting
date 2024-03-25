using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.projectFolderRoot;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.projectFolderRoot
{
    public class FolderSupplementTest
    {
        [Fact]
        public void CanCopyTheFolderSupplementExpectNewSupplementId()
        {
            var original = FolderSupplement.Create(SupplementId.Create(Any.Guid()), CatalogSupplementId.Create(Any.Guid()),
                SupplementNumber.Create("HLM42"), SupplementText.Create("Meget højt i lave steder 42%"), SupplementPercentage.Create(42));
            var copy = FolderSupplement.Copy(original);

            Assert.NotEqual(original.SupplementId.Value, copy.SupplementId.Value);
            Assert.Equal(original.SupplementNumber.Value, copy.SupplementNumber.Value);
            Assert.Equal(original.SupplementPercentage.Value, copy.SupplementPercentage.Value);
            Assert.Equal(original.CatalogSupplementId.Value, copy.CatalogSupplementId.Value);
            Assert.Equal(original.SupplementText.Value, copy.SupplementText.Value);
        }
    }
}
