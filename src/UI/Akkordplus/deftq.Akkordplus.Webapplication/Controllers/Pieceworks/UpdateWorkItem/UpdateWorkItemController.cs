using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.UpdateWorkItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateWorkItem
{
    [Authorize]
    public class UpdateWorkItemController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<UpdateWorkItemController> _logger;

        public UpdateWorkItemController(ICommandBus commandBus, ILogger<UpdateWorkItemController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }

        [HttpPut]
        [Route("/api/projects/{projectId}/folders/{folderId}/workItems/{workItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Work items" },
            Summary = "Update work item")]
        [SwaggerResponse(200, "The work item was updated")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "Project, folder og work item not found")]
        public async Task<ActionResult> UpdateWorkItem(Guid projectId, Guid folderId, Guid workItemId, UpdateWorkItemRequest request)
        {
            var command = UpdateWorkItemCommand.Create(projectId, folderId, workItemId, request.WorkItemAmount);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
