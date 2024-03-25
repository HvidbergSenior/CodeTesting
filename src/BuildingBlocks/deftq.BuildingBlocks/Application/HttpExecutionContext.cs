using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace deftq.BuildingBlocks.Application
{
    public class HttpExecutionContext : IExecutionContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IIdentityResolver _identityResolver;
        private readonly ILogger<HttpExecutionContext> _logger;
        private readonly string HttpContextUserIdKey = "USER_ID";
        
        private readonly string ClaimsIssuer = "iss";
        private readonly string ClaimsName = "name";
        
        public HttpExecutionContext(IHttpContextAccessor httpContextAccessor, IIdentityResolver identityResolver, ILogger<HttpExecutionContext> logger)
        {
            this.httpContextAccessor = httpContextAccessor;
            _identityResolver = identityResolver;
            this._logger = logger;
        }
        
        public Guid UserId
        {
            get
            {
                var httpContext = httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    _logger.LogError("No http context");
                    return Guid.Empty;
                }

                if (httpContext.Items.ContainsKey(HttpContextUserIdKey))
                {
                    return (Guid)httpContext.Items[HttpContextUserIdKey];
                }
                
                var providerId = httpContext.User.FindFirst(ClaimsIssuer);
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                var name = httpContext.User.FindFirst(ClaimsName);
                return _identityResolver.ResolveUserId(providerId, userId, name);
            }
        }
        
        public string UserName
        {
            get
            {
                var httpContext = httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    _logger.LogError("No http context");
                    return string.Empty;
                }
                
                var providerId = httpContext.User.FindFirst(ClaimsIssuer);
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                var name = httpContext.User.FindFirst(ClaimsName);
                return _identityResolver.ResolveUserName(providerId, userId, name);
            }
        }
    }
}
