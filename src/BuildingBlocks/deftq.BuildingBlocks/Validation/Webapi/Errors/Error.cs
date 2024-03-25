using System.Net;

namespace deftq.BuildingBlocks.Validation.Webapi.Errors
{
    /// <summary>
    /// 
    /// </summary>
    public class Error
    {
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Instance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TraceId { get; set; }

        public Error()
        {
            Title = string.Empty;
            Detail = string.Empty;
            Instance = string.Empty;
            TraceId = string.Empty;
            Status = 500;
            Type = $"https://httpstatuses.com/{Status}";
        }

        public Error(string title, string detail, HttpStatusCode status, string traceId, string instance)
        {
            Title = title;
            Detail = detail;
            Status = (int)status;
            TraceId = traceId;
            Instance = instance;
            Type = $"https://httpstatuses.com/{Status}";
        }

        public Error(HttpStatusCode status, string traceId, string instance) : this()
        {
            Status = (int)status;
            TraceId = traceId;
            Instance = instance;
            Title = GetHttpStatusDescription(status);
            Type = $"https://httpstatuses.com/{Status}";
        }

        private static string GetHttpStatusDescription(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.BadRequest => "Bad Request",
                HttpStatusCode.NotFound => "Not Found",
                _ => "Bad Request",
            };
        }
    }
}
