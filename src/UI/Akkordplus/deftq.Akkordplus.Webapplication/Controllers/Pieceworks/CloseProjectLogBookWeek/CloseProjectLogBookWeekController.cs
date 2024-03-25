using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.CloseProjectLogBookWeek;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CloseProjectLogBookWeek
{
    [Authorize]
    public class CloseProjectLogBookWeekController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        
        public CloseProjectLogBookWeekController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/logbook/weeks/close")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Log book"},
            Summary = "Close week in the log book")]
        public async Task<ActionResult> CloseProjectLogBookWeek(Guid projectId, CloseProjectLogBookWeekRequest request)
        {
            var command = CloseProjectLogBookWeekCommand.Create(projectId, request.UserId, request.Year, request.Week);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
