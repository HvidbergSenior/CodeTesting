using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.UpdateProjectFolderDescription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderDescription
{
    [Authorize]
    public class UpdateProjectFolderDescriptionController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<UpdateProjectFolderDescriptionController> _logger;

        public UpdateProjectFolderDescriptionController(ICommandBus commandBus, ILogger<UpdateProjectFolderDescriptionController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }
        
        [HttpPut]
        [Route("/api/projects/{projectId}/folders/{folderId}/description")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Folders" },
            Summary = "Edit folder description")]
        [SwaggerResponse(200, "The folder description was edited")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "Project or folder not found")]
        public async Task<ActionResult> UpdateFolderDescription(Guid projectId, Guid folderId, UpdateProjectFolderDescriptionRequest request)
        {
            var command = UpdateProjectFolderDescriptionCommand.Create(projectId, folderId, request.ProjectFolderDescription);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
