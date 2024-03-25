using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Generators;
using deftq.Pieceworks.Application.CreateProjectFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateProjectFolder
{
    [Authorize]
    public class CreateProjectFolderController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IIdGenerator<Guid> _idGenerator;
        private readonly ILogger<CreateProjectFolderController> _logger;

        public CreateProjectFolderController(ICommandBus commandBus, IIdGenerator<Guid> idGenerator,
            ILogger<CreateProjectFolderController> logger)
        {
            _commandBus = commandBus;
            _idGenerator = idGenerator;
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/folders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Folders"},
            Summary = "Create a new folder in a project")]
        public async Task<ActionResult> CreateProjectFolder(Guid projectId,
            [FromBody] CreateProjectFolderRequest request)
        {
            var folderId = _idGenerator.Generate();
            var command = CreateProjectFolderCommand.Create(projectId, folderId, request.FolderName, request.FolderDescription ?? "", request.ParentFolderId);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
