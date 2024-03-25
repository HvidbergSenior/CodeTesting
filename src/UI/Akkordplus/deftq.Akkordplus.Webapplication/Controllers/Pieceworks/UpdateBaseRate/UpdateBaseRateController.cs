using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.UpdateBaseRateRegulation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateBaseRate
{
    [Authorize]
    public class UpdateBaseRateController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<UpdateBaseRateController> _logger;

        public UpdateBaseRateController(ICommandBus commandBus, ILogger<UpdateBaseRateController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }

        [HttpPut]
        [Route("/api/projects/{projectId}/folders/{folderId}/baserate")]
        [SwaggerOperation(
            Tags = new string[] { "Folders" },
            Summary = "Update base rate regulation for a folder")]
        [SwaggerResponse(200, "The folder base rate was updated")]
        [SwaggerResponse(403, "User is not project owner")]
        public async Task<ActionResult> UpdateRates(Guid projectId, Guid folderId, [FromBody] UpdateBaseRateRequest request)
        {
            var status = request.BaseRateRegulationPercentage.Status == BaseRateStatusUpdate.Inherit
                ? UpdateBaseRateRegulationCommand.UpdateBaseRateRegulationStatusEnum.Inherit
                : UpdateBaseRateRegulationCommand.UpdateBaseRateRegulationStatusEnum.Overwrite;
            
            var command = UpdateBaseRateRegulationCommand.Create(projectId, folderId, request.BaseRateRegulationPercentage.Value, status);
            await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
