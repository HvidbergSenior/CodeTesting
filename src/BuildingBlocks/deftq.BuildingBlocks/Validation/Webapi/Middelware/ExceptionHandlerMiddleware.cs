using System.Net;
using System.Text.Json;
using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Validation.Webapi.Errors;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;

namespace deftq.BuildingBlocks.Validation.Webapi.Middelware
{
    public class ExceptionHandlerMiddleware
    {
        private const string JsonContentType = "application/problem+json";
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly bool _hideCallStackInResponse;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger logger, IOptions<ExceptionHandlerOption> options)
        {
            _next = next;
            _logger = logger;
            _hideCallStackInResponse = !options.Value.ShowCallStackInHttpResponse;
        }

#pragma warning disable MA0051 // Method is too long
        public async Task Invoke(HttpContext context)
#pragma warning restore MA0051 // Method is too long
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                object? errorResult;
                var traceId = context.TraceIdentifier;
                var instance = context.Request.Path;
                var httpStatusCode = (int)HttpStatusCode.InternalServerError;

                LogError(ex, traceId, instance);

                switch (ex)
                {
                    // What type of Exception shall we throw from our application?
                    case ValidationException e:
                        var validationError = new ValidationError(e.Errors, traceId, instance);
                        httpStatusCode = validationError.Status;
                        errorResult = validationError;
                        break;
                    case NotFoundException e:
                        // not found error
                        var notFoundError = new NotFoundError(e, traceId, instance);
                        httpStatusCode = notFoundError.Status;
                        errorResult = notFoundError;
                        break;
                    case AlreadyExistingException:
                        // already exists
                        var existingError = new AlreadyExistingError(traceId, instance);
                        httpStatusCode = existingError.Status;
                        errorResult = existingError;
                        break;
                    case UnauthorizedException:
                        var authError = new ExceptionError(HttpStatusCode.Unauthorized, traceId, instance);
                        httpStatusCode = authError.Status;
                        errorResult = authError;
                        break;
                    case ArgumentException:
                        var error = new BadArgumentError(HttpStatusCode.BadRequest, traceId, instance);
                        httpStatusCode = error.Status;
                        errorResult = error;
                        break;
                    case Exception e:
                        // unhandled error
                        errorResult = new UnhandledError(e, traceId, instance);
                        break;
                    default:
                        // unhandled error
                        errorResult = new UnhandledError(traceId, instance);
                        break;
                }

                if (context.Request.Path.Value.StartsWith("/administration", StringComparison.OrdinalIgnoreCase))
                {
                    if (ex is UnauthorizedException)
                    {
                        context.Response.Redirect("/administration/unauthorized/" + traceId);
                    }
                    else
                    {
                        context.Response.Redirect("/administration/error/" + traceId);
                    }
                }
                else
                {
                    context.Response.ContentType = JsonContentType;
                    context.Response.StatusCode = httpStatusCode;
                    var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                    if (_hideCallStackInResponse)
                    {
                        //override errorResult in production
                        errorResult = new UnhandledError(traceId, instance);
                    }
                    var result = JsonSerializer.Serialize(errorResult, options);
                    await context.Response.WriteAsync(result, context.RequestAborted);
                }
            }
        }

        private void LogError(string traceId, PathString instance)
        {
            var message = "Unhandled exception caught in " + nameof(ExceptionHandlerMiddleware) + Environment.NewLine +
                      "traceId: " + traceId + Environment.NewLine +
                      "instance: " + instance;

            using (LogContext.PushProperty("TraceId", traceId))
            {
                _logger.Error(message);
            }
        }

        private void LogError(Exception ex, string traceId, PathString instance)
        {
            var message = ex.GetType().Name + " caught in " + nameof(ExceptionHandlerMiddleware) + Environment.NewLine +
                      "Source: " + ex.Source + Environment.NewLine +
                      "Message: " + ex.Message + Environment.NewLine +
                      "traceId: " + traceId + Environment.NewLine +
                      "instance: " + instance;
            using (LogContext.PushProperty("TraceId", traceId))
            {
                _logger.Error(ex, message);
            }
        }
    }

    public class ExceptionHandlerOption
    {
        public bool ShowCallStackInHttpResponse { get; set; }
    }
}
