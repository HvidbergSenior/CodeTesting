using System.Runtime.Serialization;
using deftq.BuildingBlocks.Exceptions;

namespace deftq.Pieceworks.Domain.ProjectCatalogFavorite
{
    [Serializable]
    public class ProjectCatalogFavoriteListNotFoundException : NotFoundException
    {
        public ProjectCatalogFavoriteListNotFoundException() { }
        public ProjectCatalogFavoriteListNotFoundException(string message) : base(message) { }
        public ProjectCatalogFavoriteListNotFoundException(Guid entityId) : base($"Project catalog favorite id {entityId} not found.") { }
        public ProjectCatalogFavoriteListNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ProjectCatalogFavoriteListNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
