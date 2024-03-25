using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.ProjectCatalogFavorite
{
    public sealed class CatalogFavorite : Entity
    {
        public CatalogFavoriteId CatalogFavoriteId { get; private set; }
        public CatalogItemType CatalogItemType { get; private set; }
        public CatalogItemId CatalogItemId { get; private set; }
        public CatalogItemNumber CatalogItemNumber { get; private set; }
        public CatalogItemText CatalogItemText { get; private set; }
        public CatalogItemUnit CatalogItemUnit { get; private set; }

        private CatalogFavorite()
        {
            CatalogFavoriteId = CatalogFavoriteId.Empty();
            Id = CatalogFavoriteId.Value;
            CatalogItemType = CatalogItemType.Material;
            CatalogItemId = CatalogItemId.Empty();
            CatalogItemNumber = CatalogItemNumber.Empty();
            CatalogItemText = CatalogItemText.Empty();
            CatalogItemUnit = CatalogItemUnit.Empty();
        }

        private CatalogFavorite(CatalogFavoriteId catalogFavoriteId, CatalogItemType catalogItemType, CatalogItemId catalogItemId,
            CatalogItemNumber catalogItemNumber, CatalogItemText catalogItemText, CatalogItemUnit catalogItemUnit)
        {
            CatalogFavoriteId = catalogFavoriteId;
            Id = CatalogFavoriteId.Value;
            CatalogItemType = catalogItemType;
            CatalogItemId = catalogItemId;
            CatalogItemNumber = catalogItemNumber;
            CatalogItemText = catalogItemText;
            CatalogItemUnit = catalogItemUnit;
        }

        public static CatalogFavorite Create(CatalogFavoriteId catalogFavoriteId, CatalogItemType catalogItemType, CatalogItemId catalogItemId,
            CatalogItemNumber catalogItemNumber, CatalogItemText catalogItemText, CatalogItemUnit catalogItemUnit)
        {
            return new CatalogFavorite(catalogFavoriteId, catalogItemType, catalogItemId, catalogItemNumber, catalogItemText, catalogItemUnit);
        }
    }
    public enum CatalogItemType { Material, Operation }
}
