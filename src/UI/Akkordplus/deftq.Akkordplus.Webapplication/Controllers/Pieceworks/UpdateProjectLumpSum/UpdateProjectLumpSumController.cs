using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.UpdateProjectLumpSum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectLumpSum
{
    [Authorize]
    public class UpdateProjectLumpSumController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<UpdateProjectLumpSumController> _logger;

        public UpdateProjectLumpSumController(ICommandBus commandBus, ILogger<UpdateProjectLumpSumController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }

        [HttpPut]
        [Route("/api/projects/{projectId}/projectlumpsum")]
        [SwaggerOperation(
            Tags = new[] { "Projects" },
            Summary = "Edit project lump sum")]
        [SwaggerResponse(200, "The project lump sum has been updated")]
        [SwaggerResponse(403, "User is not project owner")]
        [SwaggerResponse(401, "User is not authorized")]
        public async Task<ActionResult> UpdateExtraWorkAgreementRates(Guid projectId, UpdateProjectLumpSumRequest request)
        {
            var command = UpdateProjectLumpSumCommand.Create(projectId, request.LumpSumDkr);
            await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
