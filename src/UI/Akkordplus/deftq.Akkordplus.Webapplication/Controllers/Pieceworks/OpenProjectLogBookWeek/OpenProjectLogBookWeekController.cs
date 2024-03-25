using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.CloseProjectLogBookWeek;
using deftq.Pieceworks.Application.OpenProjectLogBookWeek;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.OpenProjectLogBookWeek
{
    [Authorize]
    public class OpenProjectLogBookWeekController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        
        public OpenProjectLogBookWeekController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/logbook/weeks/open")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Log book"},
            Summary = "Open previously closed week in the log book")]
        public async Task<ActionResult> OpenProjectLogBookWeek(Guid projectId, OpenProjectLogBookWeekRequest request)
        {
            var command = OpenProjectLogBookWeekCommand.Create(projectId, request.UserId, request.Year, request.Week);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
