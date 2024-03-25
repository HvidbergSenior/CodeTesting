using deftq.Catalog.Domain.MaterialImport;

namespace deftq.Catalog.Application.Import.GetMaterialImportStatus
{
    public static class ImportChecker
    {
        internal static bool IsImportComplete(IList<CatalogPageInfo> latestCatalogPageInfo)
        {
            if (latestCatalogPageInfo.Count == 0)
            {
                return false;
            }

            var pageInfo = latestCatalogPageInfo.ToArray();
            Array.Sort(pageInfo, (p1, p2) => p1.StartRow.CompareTo(p2.StartRow));
            
            var pageSize = pageInfo[0].PageSize;
            var lastPage = pageInfo[^1];
            
            for (var i = 0; i < pageInfo.Length; i++)
            {
                var page = pageInfo[i];
                if (page.IsLastPage && page != lastPage)
                {
                    // More pages exists, should not be last page
                    return false;
                }

                if (page.PageSize != pageSize)
                {
                    // Page size differs between pages
                    return false;
                }
                
                if (!page.IsLastPage && page.ContentSize < pageSize)
                {
                    // Expected page to be full
                    return false;
                }

                if (i > 0)
                {
                    var previousPage = pageInfo[i - 1];
                    if (page.StartRow != previousPage.StartRow + pageSize)
                    {
                        // Page not starting at expected row
                        return false;
                    }
                }
            }
            
            return lastPage.IsLastPage;
        }
    }
}
