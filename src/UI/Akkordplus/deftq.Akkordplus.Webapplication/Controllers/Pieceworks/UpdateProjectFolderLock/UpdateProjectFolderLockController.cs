using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.UpdateProjectFolderLock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderLock
{
    [Authorize]
    public class UpdateLockProjectFolderController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;

        public UpdateLockProjectFolderController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/folders/{folderId}/lock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Folders" },
            Summary = "Lock or unlock folder")]
        public async Task<ActionResult> UpdateLockProjectFolder(Guid projectId, Guid folderId, UpdateLockProjectFolderRequest request)
        {
            var folderLock = request.FolderLock == UpdateLockProjectFolderRequest.Lock.Locked
                ? UpdateProjectFolderLockCommand.Lock.Locked
                : UpdateProjectFolderLockCommand.Lock.Unlocked;
            var command = UpdateProjectFolderLockCommand.Create(projectId, folderId, folderLock, request.Recursive);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
