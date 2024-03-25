using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.ProjectCatalogFavorite
{
    public sealed class ProjectCatalogFavoriteListId : ValueObject
    {
        public Guid Value { get; private set; }

        private ProjectCatalogFavoriteListId()
        {
            Value = Guid.Empty;
        }

        private ProjectCatalogFavoriteListId(Guid value)
        {
            Value = value;
        }

        public static ProjectCatalogFavoriteListId Create(Guid value)
        {
            return new ProjectCatalogFavoriteListId(value);
        }

        public static ProjectCatalogFavoriteListId Empty()
        {
            return new ProjectCatalogFavoriteListId();
        }
    }
}
