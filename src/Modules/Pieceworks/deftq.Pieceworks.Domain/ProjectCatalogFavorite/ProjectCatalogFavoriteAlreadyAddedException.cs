using System.Runtime.Serialization;
using deftq.BuildingBlocks.Exceptions;

namespace deftq.Pieceworks.Domain.ProjectCatalogFavorite
{
    [Serializable]
    public class ProjectCatalogFavoriteAlreadyAddedException : AlreadyExistingException
    {
        public ProjectCatalogFavoriteAlreadyAddedException() : base("Catalog item already added to favorite") { }
        public ProjectCatalogFavoriteAlreadyAddedException(string message) : base(message) { }
        public ProjectCatalogFavoriteAlreadyAddedException(string message, Exception inner) : base(message, inner) { }
        protected ProjectCatalogFavoriteAlreadyAddedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
