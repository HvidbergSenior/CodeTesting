using deftq.BuildingBlocks.Domain;
using deftq.Catalog.Domain.MaterialCatalog;

namespace deftq.Catalog.Domain.MaterialImport
{
    public sealed class MaterialImportPage : Entity
    {
        public MaterialImportPageId MaterialImportPageId { get; private set; }

        public DateTimeOffset Published { get; private set; }
        
        public int StartRow { get; private set; }
        public int PageSize { get; private set; }
        public bool IsLastPage { get; private set; }
        public IList<Material> Content { get; private set; }
        public int ContentSize { get; private set; }

        private MaterialImportPage()
        {
            MaterialImportPageId = MaterialImportPageId.Empty();
            Id = MaterialImportPageId.Value;
            Published = DateTimeOffset.MinValue;
            StartRow = -1;
            PageSize = -1;
            IsLastPage = true;
            Content = new List<Material>();
            ContentSize = 0;
        }

        private MaterialImportPage(MaterialImportPageId materialImportPageId, DateTimeOffset published, int startRow, int pageSize, bool isLastPage,
            IList<Material> content, int contentSize)
        {
            MaterialImportPageId = materialImportPageId;
            Id = MaterialImportPageId.Value;
            Published = published;
            StartRow = startRow;
            PageSize = pageSize;
            IsLastPage = isLastPage;
            Content = content;
            ContentSize = contentSize;
        }

        public static MaterialImportPage Create(MaterialImportPageId materialImportPageId, DateTimeOffset published, int startRow, int pageSize,
            bool isLastPage, IList<Material> pageContent)
        {
            return new MaterialImportPage(materialImportPageId, published, startRow, pageSize, isLastPage, pageContent, pageContent.Count);
        }

        public static MaterialImportPage Empty()
        {
            return new MaterialImportPage();
        }
    }
}
