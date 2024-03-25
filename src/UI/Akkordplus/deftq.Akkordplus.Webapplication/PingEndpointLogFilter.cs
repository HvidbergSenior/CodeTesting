using System.Reflection;
using deftq.Akkordplus.WebApplication.Controllers.Ping;
using Microsoft.AspNetCore.Mvc;
using Serilog.Core;
using Serilog.Events;

namespace deftq.Akkordplus.WebApplication
{
    public class PingEndpointLogFilter : ILogEventFilter
    {
        private string _pingRoutePath = "";

        public PingEndpointLogFilter()
        {
            var pingControllerMethod = typeof(PingController).GetMethod(nameof(PingController.Ping));
            if (pingControllerMethod is not null)
            {
                var attribute = pingControllerMethod.GetCustomAttribute(typeof(RouteAttribute));
                _pingRoutePath = (attribute as RouteAttribute)?.Template ?? "";
            }
        }

        public bool IsEnabled(LogEvent logEvent)
        {
            if (logEvent is not null && logEvent.Properties.TryGetValue("RequestPath", out var pathProperty))
            {
                return !_pingRoutePath.Equals((pathProperty as ScalarValue)?.Value);
            }

            return true;
        }
    }
}
