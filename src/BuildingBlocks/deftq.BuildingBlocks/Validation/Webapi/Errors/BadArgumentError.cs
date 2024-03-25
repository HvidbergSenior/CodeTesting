using System.Net;

namespace deftq.BuildingBlocks.Validation.Webapi.Errors
{
    public sealed class BadArgumentError : Error
    {
        public BadArgumentError(HttpStatusCode _httpStatusCode, string traceId, string instance) : base(_httpStatusCode, traceId, instance)
        {
        }
    }
}
