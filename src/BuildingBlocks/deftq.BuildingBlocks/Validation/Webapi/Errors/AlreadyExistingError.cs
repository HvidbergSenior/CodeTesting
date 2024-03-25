using System.Net;

namespace deftq.BuildingBlocks.Validation.Webapi.Errors
{

    public sealed class AlreadyExistingError : Error
    {
        private const string HttpStatusMessage = "Conflict";
        private const string DefaultErrorMessage = "Entity exists";

        public AlreadyExistingError(string traceId, string instance) : base(HttpStatusCode.Conflict, traceId, instance)
        {
            Title = HttpStatusMessage;
            Detail = DefaultErrorMessage;
        }
    }
}
