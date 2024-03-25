using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.RemoveDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveDocument
{
    [Authorize]
    public class RemoveDocumentController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        
        public RemoveDocumentController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        [HttpDelete]
        [Route("/api/projects/{projectId}/folders/{folderId}/documents/{documentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Tags = new string[] {"Documents"},
            Summary = "Remove document")]
        public async Task<ActionResult> RemoveDocument(Guid projectId, Guid folderId, Guid documentId)
        {
            var command = RemoveProjectDocumentCommand.Create(projectId, folderId, documentId);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
