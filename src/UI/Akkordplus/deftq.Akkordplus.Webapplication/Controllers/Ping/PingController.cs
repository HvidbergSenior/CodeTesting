using Microsoft.AspNetCore.Mvc;

namespace deftq.Akkordplus.WebApplication.Controllers.Ping
{
    public class PingController : ControllerBase
    {
        [HttpGet]
        [Route("/api/ping")]
        public Task<ActionResult> Ping()
        {
            return Task.FromResult<ActionResult>(base.Ok("pong"));
        }
    }
}
