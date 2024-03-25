using System.Collections.ObjectModel;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.ProjectCatalogFavorite
{
    public sealed class ProjectCatalogFavoriteList : Entity
    {
        public ProjectCatalogFavoriteListId ProjectCatalogFavoriteListId { get; private set; }
        public ProjectId ProjectId { get; private set; }

        private IList<CatalogFavorite> _favorites;
        public IReadOnlyList<CatalogFavorite> Favorites
        {
            get => new ReadOnlyCollection<CatalogFavorite>(_favorites);
            private set => _favorites = new List<CatalogFavorite>(value);
        }

        private ProjectCatalogFavoriteList()
        {
            ProjectCatalogFavoriteListId = ProjectCatalogFavoriteListId.Empty();
            Id = ProjectCatalogFavoriteListId.Value;
            ProjectId = ProjectId.Empty();
            _favorites = new List<CatalogFavorite>();
        }

        private ProjectCatalogFavoriteList(ProjectCatalogFavoriteListId projectCatalogFavoriteListId, ProjectId projectId)
        {
            ProjectCatalogFavoriteListId = projectCatalogFavoriteListId;
            Id = ProjectCatalogFavoriteListId.Value;
            ProjectId = projectId;
            _favorites = new List<CatalogFavorite>();
        }

        public static ProjectCatalogFavoriteList Create(ProjectCatalogFavoriteListId projectCatalogFavoriteListId, ProjectId projectId)
        {
            return new ProjectCatalogFavoriteList(projectCatalogFavoriteListId, projectId);
        }

        public void AddFavorite(CatalogFavorite favorite)
        {
            if(_favorites.Any(f => IsSameCatalogItem(f, favorite)))
            {
                throw new ProjectCatalogFavoriteAlreadyAddedException();
            }
            _favorites.Add(favorite);
        }

        private bool IsSameCatalogItem(CatalogFavorite a, CatalogFavorite b)
        {
            return a.CatalogItemType == b.CatalogItemType && a.CatalogItemId == b.CatalogItemId;
        }
        
        public void RemoveFavorites(IList<CatalogFavoriteId> catalogFavoriteIds)
        {
            foreach (var catalogFavoriteId in catalogFavoriteIds)
            {
                RemoveFavorite(catalogFavoriteId);
            }
        }
        
        public void RemoveFavorite(CatalogFavoriteId favoriteId)
        {
            var foundFavorite = _favorites.FirstOrDefault(f => f.CatalogFavoriteId == favoriteId);
            if(foundFavorite is null)
            {
                throw new ProjectCatalogFavoriteNotFoundException(favoriteId.Value);
            }
            _favorites.Remove(foundFavorite);
        }
        
    }
}
