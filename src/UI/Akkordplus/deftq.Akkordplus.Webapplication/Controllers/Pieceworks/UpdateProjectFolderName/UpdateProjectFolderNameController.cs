using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.UpdateProjectFolderName;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderName
{
    [Authorize]
    public class UpdateProjectFolderNameController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<UpdateProjectFolderNameController> _logger;

        public UpdateProjectFolderNameController(ICommandBus commandBus, ILogger<UpdateProjectFolderNameController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }

        [HttpPut]
        [Route("/api/projects/{projectId}/folders/{folderId}/name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Folders" },
            Summary = "Edit folder name")]
        [SwaggerResponse(200, "The folder name was edited")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "Project or folder not found")]
        public async Task<ActionResult> UpdateFolderName(Guid projectId, Guid folderId, UpdateProjectFolderNameRequest request)
        {
            var command = UpdateProjectFolderNameCommand.Create(projectId, folderId, request.ProjectFolderName);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
