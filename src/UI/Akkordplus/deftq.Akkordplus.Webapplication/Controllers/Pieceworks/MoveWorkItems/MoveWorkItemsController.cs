using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.MoveWorkItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.MoveWorkItems
{
    [Authorize]
    public class MoveWorkItemsController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<MoveWorkItemsController> _logger;

        public MoveWorkItemsController(ICommandBus commandBus, ILogger<MoveWorkItemsController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }
    
        [HttpPut]
        [Route("/api/projects/{projectId}/folders/{folderId}/workitems/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Work items"},
            Summary = "Work items that are moved to a new location in the project")]
        [SwaggerResponse(200, "The work items are moved")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "Project, work items, source folder or destination folder not found")]
        public async Task<ActionResult> MoveWorkItems(Guid projectId, Guid folderId, MoveWorkItemsRequest request)
        {
            var command = MoveWorkItemsCommand.Create(projectId, folderId, request.DestinationFolderId, request.WorkItemIds);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
