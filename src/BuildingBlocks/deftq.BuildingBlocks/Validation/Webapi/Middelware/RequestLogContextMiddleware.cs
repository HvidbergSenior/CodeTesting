using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace deftq.BuildingBlocks.Validation.Webapi.Middelware
{
    public class RequestLogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        [SuppressMessage("Usage", "MA0100:Await task before disposing resources", Justification = "<Pending>")]
        public Task Invoke(HttpContext context)
        {
            var traceId = context.TraceIdentifier ?? "Missing Identity";

            using (LogContext.PushProperty("TraceId", traceId))
            {
                return _next.Invoke(context);
            }
        }
    }
}
