using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.MoveProjectFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.MoveProjectFolder
{
    [Authorize]
    public class MoveProjectFolderController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<MoveProjectFolderController> _logger;

        public MoveProjectFolderController(ICommandBus commandBus, ILogger<MoveProjectFolderController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }
        
        [HttpPut]
        [Route("/api/projects/{projectId}/folders/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Folders"},
            Summary = "Move folder to a new location in the project")]
        [SwaggerResponse(200, "The folder was moved")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "Project, source folder or destination folder not found")]
        public async Task<ActionResult> MoveProjectFolder(Guid projectId, MoveProjectFolderRequest request)
        {
            var command = MoveProjectFolderCommand.Create(projectId, request.FolderId, request.DestinationFolderId);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
