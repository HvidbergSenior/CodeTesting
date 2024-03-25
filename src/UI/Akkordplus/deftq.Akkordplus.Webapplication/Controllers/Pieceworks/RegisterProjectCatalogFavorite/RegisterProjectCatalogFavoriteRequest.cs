namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectCatalogFavorite
{
    public enum FavoriteCatalogType { Material, Operation }

    public class RegisterProjectCatalogFavoriteRequest
    {
        public Guid CatalogId { get; private set; }
        public FavoriteCatalogType CatalogType { get; private set; }

        public RegisterProjectCatalogFavoriteRequest(Guid catalogId, FavoriteCatalogType catalogType)
        {
            CatalogId = catalogId;
            CatalogType = catalogType;
        }
    }
}
