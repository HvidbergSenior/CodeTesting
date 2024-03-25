
namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveProjectFavorites
{
    public class RemoveProjectFavoritesRequest
    {
        public IList<Guid> FavoriteIds { get; private set; }

        public RemoveProjectFavoritesRequest(IList<Guid> favoriteIds)
        {
            FavoriteIds = favoriteIds;
        }
    }
}
