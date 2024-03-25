using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.CopyProjectFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CopyProjectFolder
{
    [Authorize]
    public class CopyProjectFolderController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<CopyProjectFolderController> _logger;

        public CopyProjectFolderController(ICommandBus commandBus, ILogger<CopyProjectFolderController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }
    
        [HttpPut]
        [Route("/api/projects/{projectId}/folders/{sourceFolderId}/copy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Folders"},
            Summary = "Copy folder to a new location in the project")]
        [SwaggerResponse(200, "The folder was copied")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "Project, source folder or destination folder not found")]
        public async Task<ActionResult> CopyProjectFolder(Guid projectId, Guid sourceFolderId, CopyProjectFolderRequest request)
        {
            var command = CopyProjectFolderCommand.Create(projectId, sourceFolderId, request.DestinationFolderId);
            await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
