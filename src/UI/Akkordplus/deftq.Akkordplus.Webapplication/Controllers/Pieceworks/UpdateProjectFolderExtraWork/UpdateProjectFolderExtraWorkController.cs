using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.UpdateProjectFolderExtraWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderExtraWork
{
    [Authorize]
    public class UpdateProjectFolderExtraWorkController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;

        public UpdateProjectFolderExtraWorkController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }
        
        [HttpPost]
        [Route("/api/projects/{projectId}/folders/{folderId}/extrawork")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Folders" },
            Summary = "Mark a folder as extra work or normal work")]
        [SwaggerResponse(200, "Folder extra work is updated")]
        [SwaggerResponse(403, "User not project owner or project manager")]
        [SwaggerResponse(404, "Folder not found")]
        public async Task<ActionResult> UpdateExtraWorkProjectFolder(Guid projectId, Guid folderId, UpdateProjectFolderExtraWorkRequest request)
        {
            var extraWork = request.FolderExtraWorkUpdate == UpdateProjectFolderExtraWorkRequest.ExtraWorkUpdate.ExtraWork
                ? UpdateProjectFolderExtraWorkCommand.ExtraWork.ExtraWork
                : UpdateProjectFolderExtraWorkCommand.ExtraWork.NormalWork;

            var command = UpdateProjectFolderExtraWorkCommand.Create(projectId, folderId, extraWork);
            await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
