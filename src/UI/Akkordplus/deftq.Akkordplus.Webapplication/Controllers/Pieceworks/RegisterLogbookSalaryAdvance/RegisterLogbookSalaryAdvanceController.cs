using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.RegisterLogBookSalaryAdvance;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterLogbookSalaryAdvance
{
    [Authorize]
    public class RegisterLogbookSalaryAdvanceController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;

        public RegisterLogbookSalaryAdvanceController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/logbook/salaryadvance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Tags = new string[] { "Log book" },
            Summary = "Register salary advance for user")]
        public async Task<ActionResult> RegisterLogBookSalaryAdvance(Guid projectId, RegisterLogbookSalaryAdvanceRequest request)
        {
            var command = RegisterLogBookSalaryAdvanceCommand.Create(ProjectId.Create(projectId), request.UserId, request.Year, request.Week,
                MapRole(request.Type), request.Amount);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }

        private LogBookSalaryAdvanceRole MapRole(LogBookSalaryAdvanceRoleRequest role)
        {
            switch (role)
            {
                case LogBookSalaryAdvanceRoleRequest.Apprentice:
                    return LogBookSalaryAdvanceRole.Apprentice;
                case LogBookSalaryAdvanceRoleRequest.Participant:
                default:
                    return LogBookSalaryAdvanceRole.Participant;
            }
        }
    }
}
