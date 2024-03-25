using System.Net;
using deftq.BuildingBlocks.Exceptions;

namespace deftq.BuildingBlocks.Validation.Webapi.Errors
{
    public sealed class NotFoundError : Error
    {
        private const string DefaultErrorMessage = "Entity not found";

        public NotFoundError()
        {
        }

        public NotFoundError(string traceId, string instance) : base(HttpStatusCode.NotFound, traceId, instance)
        {
            Detail = DefaultErrorMessage;
        }

        public NotFoundError(NotFoundException exception, string traceId, string instance)
            : base(HttpStatusCode.NotFound, traceId, instance)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Detail = exception.Message;
        }
    }
}
