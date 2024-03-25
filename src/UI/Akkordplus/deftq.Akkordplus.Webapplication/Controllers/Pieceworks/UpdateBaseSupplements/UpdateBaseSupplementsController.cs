using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.UpdateBaseSupplements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateBaseSupplements
{
    [Authorize]
    public class UpdateBaseSupplementsController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<UpdateBaseSupplementsController> _logger;

        public UpdateBaseSupplementsController(ICommandBus commandBus, ILogger<UpdateBaseSupplementsController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }

        [HttpPut]
        [Route("/api/projects/{projectId}/folders/{folderId}/basesupplements")]
        [SwaggerOperation(
            Tags = new string[] { "Folders" },
            Summary = "Update base supplements for a folder")]
        [SwaggerResponse(200, "The folder base supplements were updated")]
        [SwaggerResponse(403, "User not project owner")]
        public async Task<ActionResult> UpdateRates(Guid projectId, Guid folderId, [FromBody] UpdateBaseSupplementsRequest request)
        {
            var indirectTimeStatus = request.IndirectTimePercentage.Status == BaseSupplementStatusUpdate.Inherit
                ? UpdateBaseSupplementsCommand.UpdateBaseSupplementStatusEnum.Inherit
                : UpdateBaseSupplementsCommand.UpdateBaseSupplementStatusEnum.Overwrite;

            var siteSpecificStatus = request.SiteSpecificTimePercentage.Status == BaseSupplementStatusUpdate.Inherit
                ? UpdateBaseSupplementsCommand.UpdateBaseSupplementStatusEnum.Inherit
                : UpdateBaseSupplementsCommand.UpdateBaseSupplementStatusEnum.Overwrite;

            var command = UpdateBaseSupplementsCommand.Create(projectId, folderId, request.IndirectTimePercentage.Value, indirectTimeStatus,
                request.SiteSpecificTimePercentage.Value, siteSpecificStatus);
            await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
