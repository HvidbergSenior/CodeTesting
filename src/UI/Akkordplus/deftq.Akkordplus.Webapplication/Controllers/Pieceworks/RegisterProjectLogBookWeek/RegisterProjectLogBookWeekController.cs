using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.RegisterProjectLogBookWeek;
using deftq.Pieceworks.Domain.projectLogBook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectLogBookWeek
{
    [Authorize]
    public class RegisterProjectLogBookWeekController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        
        public RegisterProjectLogBookWeekController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/logbook/weeks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Tags = new string[] {"Log book"},
            Summary = "Register a new log book week")]
        public async Task<ActionResult> RegisterProjectLogBookWeek(Guid projectId, RegisterProjectLogBookWeekRequest request)
        {
            var mappedDays = request.Days.Select(day =>
            {
                var date = ConvertToDate(day);
                var hours = LogBookHours.Create(day.Hours);
                var minutes = LogBookMinutes.Create(day.Minutes);
                return ProjectLogBookDay.Create(date, LogBookTime.Create(hours, minutes));
            }).ToList();
            
            var command = RegisterProjectLogBookWeekCommand.Create(projectId, request.UserId, request.Year, request.Week, request.Note, mappedDays);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }

        private static LogBookDate ConvertToDate(RegisterProjectLogBookDay day)
        {
            return LogBookDate.Create(DateOnly.FromDateTime(day.Date.ToLocalTime().DateTime));
        }
    }
}
