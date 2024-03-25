using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.RemoveWorkItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveWorkItem
{
    [Authorize]
    public class RemoveWorkItemController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<RemoveWorkItemController> _logger;
        
        public RemoveWorkItemController(ICommandBus commandBus, ILogger<RemoveWorkItemController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }

        [HttpDelete]
        [Route("/api/projects/{projectId}/folders/{folderId}/workitems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Work items" },
            Summary = "Remove work items from folder")]
        [SwaggerResponse(200, "The work item was removed")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "Project, folder or work item not found")]
        public async Task<ActionResult> RemoveWorkItems(Guid projectId, Guid folderId, RemoveWorkItemRequest request)
        {
            var command = RemoveWorkItemCommand.Create(projectId, folderId, request.WorkItemIds);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
