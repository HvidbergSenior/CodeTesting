using deftq.Catalog.Application.Import.GetMaterialImportStatus;
using deftq.Catalog.Domain.MaterialImport;
using Xunit;

namespace deftq.Catalog.Test.Application.Import.GetMaterialImportStatus
{
    public class ImportCheckerTest
    {
        private readonly DateTimeOffset _now = DateTimeOffset.Now;
        
        [Fact]
        public void GivenNoImport_ImportIsNotComplete()
        {
            Assert.False(ImportChecker.IsImportComplete(new List<CatalogPageInfo>()));
        }
        
        [Fact]
        public void GivenSingleHalfFullPage_ImportIsComplete()
        {
            var page = new CatalogPageInfo(_now, 1, 10, true,5);
            Assert.True(ImportChecker.IsImportComplete(new List<CatalogPageInfo> { page }));
        }
        
        [Fact]
        public void GivenSingleEmptyPage_ImportIsComplete()
        {
            var page = new CatalogPageInfo(_now, 1, 10, true,0);
            Assert.True(ImportChecker.IsImportComplete(new List<CatalogPageInfo> { page }));
        }
        
        [Fact]
        public void GivenLastPageInvalidOrder_ImportIsIncomplete()
        {
            var page1 = new CatalogPageInfo(_now, 11, 10, true,0);
            var page2 = new CatalogPageInfo(_now, 1, 10, false,0);
            Assert.False(ImportChecker.IsImportComplete(new List<CatalogPageInfo> { page1, page2 }));
        }
        
        [Fact]
        public void GivenEmptyLastPage_ImportIsComplete()
        {
            var page1 = new CatalogPageInfo(_now, 1, 10, false,10);
            var page2 = new CatalogPageInfo(_now, 11, 10, true,0);
            Assert.True(ImportChecker.IsImportComplete(new List<CatalogPageInfo> { page1, page2 }));
        }
        
        [Fact]
        public void GivenFullLastPage_ImportIsComplete()
        {
            var page1 = new CatalogPageInfo(_now, 1, 10, false,10);
            var page2 = new CatalogPageInfo(_now, 11, 10, true,10);
            Assert.True(ImportChecker.IsImportComplete(new List<CatalogPageInfo> { page1, page2 }));
        }
        
        [Fact]
        public void GivenFullPage_ButNotLast_ImportIsIncomplete()
        {
            var page1 = new CatalogPageInfo(_now, 1, 10, false,10);
            var page2 = new CatalogPageInfo(_now, 11, 10, false,10);
            Assert.False(ImportChecker.IsImportComplete(new List<CatalogPageInfo> { page1, page2 }));
        }
    }
}
