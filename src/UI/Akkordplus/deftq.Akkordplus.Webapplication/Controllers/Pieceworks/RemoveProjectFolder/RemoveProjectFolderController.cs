using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.RemoveProjectFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveProjectFolder
{
    [Authorize]
    public class RemoveProjectFolderController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<RemoveProjectFolderController> _logger;

        public RemoveProjectFolderController(ICommandBus commandBus, ILogger<RemoveProjectFolderController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }
        
        [HttpDelete]
        [Route("/api/projects/{projectId}/folders/{folderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Folders"},
            Summary = "Remove folder from the project")]
        [SwaggerResponse(200, "The folder was removed")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "Project or folder not found")]
        public async Task<ActionResult> CreateProjectFolder(Guid projectId, Guid folderId)
        {
            var command = RemoveProjectFolderCommand.Create(projectId, folderId);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
