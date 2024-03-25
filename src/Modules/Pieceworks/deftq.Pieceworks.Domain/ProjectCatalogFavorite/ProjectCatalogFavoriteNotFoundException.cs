using System.Runtime.Serialization;
using deftq.BuildingBlocks.Exceptions;

namespace deftq.Pieceworks.Domain.ProjectCatalogFavorite
{
    public class ProjectCatalogFavoriteNotFoundException : NotFoundException
    {
        public ProjectCatalogFavoriteNotFoundException(){}
        public ProjectCatalogFavoriteNotFoundException(string message) : base(message) { }
        public ProjectCatalogFavoriteNotFoundException(Guid entityId) : base($"Project catalog favorite with id {entityId} not found."){ }
        public ProjectCatalogFavoriteNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ProjectCatalogFavoriteNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context){ }
    }
}
