using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateProjectFolder;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Generators;
using deftq.Pieceworks.Application.CopyWorkItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CopyWorkItems
{
    [Authorize]
    public class CopyWorkItemsController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IIdGenerator<Guid> _idGenerator;
        private readonly ILogger<CreateProjectFolderController> _logger;

        public CopyWorkItemsController(ICommandBus commandBus, IIdGenerator<Guid> idGenerator, ILogger<CreateProjectFolderController> logger)
        {
            _commandBus = commandBus;
            _idGenerator = idGenerator;
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/folders/{sourceFolderId}/workitems/copy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Tags = new string[] { "Work items" },
            Summary = "Copy work items from source folder to destination folder")]
        public async Task<ActionResult> CopyWorkItems(Guid projectId, Guid sourceFolderId, CopyWorkItemsRequest copyRequest)
        {
            var command = CopyWorkItemsCommand.Create(projectId, sourceFolderId, copyRequest.DestinationFolderId, copyRequest.WorkItemIds);
            await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
