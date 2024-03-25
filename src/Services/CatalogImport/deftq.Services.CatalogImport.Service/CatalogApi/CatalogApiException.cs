namespace deftq.Catalog.Domain.CatalogApi
{
    public class CatalogApiException : Exception
    {
        public CatalogApiException() : base() { }
        public CatalogApiException(string message) : base(message) { }
        public CatalogApiException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
