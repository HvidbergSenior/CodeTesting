namespace deftq.Pieceworks.Application.GetFavoriteList
{
    public enum CatalogItemType { Material, Operation }

    public class GetProjectFavoriteListQueryResponse
    {
        public IList<FavoritesResponse> Favorites { get; private set; }

        private GetProjectFavoriteListQueryResponse()
        {
            Favorites = new List<FavoritesResponse>();
        }

        public GetProjectFavoriteListQueryResponse(IList<FavoritesResponse> favorites)
        {
            Favorites = favorites;
        }
    }

    public class FavoritesResponse
    {
        public Guid FavoriteItemId { get; private set; }
        public Guid CatalogId { get; private set; }
        public string Text { get; private set; }
        public string Number { get; private set; }
        public string Unit { get; private set; }
        public CatalogItemType CatalogType { get; private set; }

        private FavoritesResponse()
        {
            FavoriteItemId = Guid.Empty;
            CatalogId = Guid.Empty;
            Text = string.Empty;
            Number = string.Empty;
            Unit = string.Empty;
            CatalogType = CatalogItemType.Material;
        }

        public FavoritesResponse(Guid favoriteItemId, Guid catalogId, string text, string number, string unit, CatalogItemType catalogType)
        {
            FavoriteItemId = favoriteItemId;
            CatalogId = catalogId;
            Text = text;
            Number = number;
            Unit = unit;
            CatalogType = catalogType;
        }
    }
}
