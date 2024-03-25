using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace deftq.BuildingBlocks.PipelineBehaviour
{
    public class Loggingbehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<Loggingbehaviour<TRequest, TResponse>> _logger;
        private readonly IHttpContextAccessor httpContextAccessor;

        public Loggingbehaviour(ILogger<Loggingbehaviour<TRequest, TResponse>> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var traceId = httpContextAccessor.HttpContext?.TraceIdentifier ?? "Missing Identity";

            //Request
            var requestType = typeof(TRequest).Name;
            _logger.LogInformation("Handling {RequestType} with traceId {TraceId}", requestType, traceId);

            var response = await next();
            //Response
            var responseType = typeof(TResponse).Name;
            _logger.LogInformation("Handled {ResponseType} with traceId {TraceId}", responseType, traceId);
            return response;
        }
    }
}
