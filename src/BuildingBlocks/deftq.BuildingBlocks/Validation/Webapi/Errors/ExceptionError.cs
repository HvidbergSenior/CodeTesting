using System.Net;

namespace deftq.BuildingBlocks.Validation.Webapi.Errors
{
    public sealed class ExceptionError : Error
    {
        public ExceptionError(HttpStatusCode _httpStatusCode, string traceId, string instance) : base(_httpStatusCode, traceId, instance)
        {
        }
    }
}
