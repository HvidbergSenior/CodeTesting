using System;
using System.Net;

namespace deftq.BuildingBlocks.Validation.Webapi.Errors
{
    public sealed class UnhandledError : Error
    {

        public UnhandledError(Exception exception, string traceId, string instance) : base(HttpStatusCode.InternalServerError, traceId, instance)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Title = "Unhandled error";
            Detail = exception.Message;
        }

        public UnhandledError(string traceId, string instance) : base(HttpStatusCode.InternalServerError, traceId, instance)
        {
        }
    }
}
