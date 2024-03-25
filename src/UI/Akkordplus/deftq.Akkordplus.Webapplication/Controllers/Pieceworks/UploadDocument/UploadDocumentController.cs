using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateProject;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetDocument;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Generators;
using deftq.Pieceworks.Application.UploadDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UploadDocument
{
    [Authorize]
    public class UploadDocumentController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IIdGenerator<Guid> _idGenerator;
        private readonly ILogger<CreateProjectController> _logger;

        public UploadDocumentController(ICommandBus commandBus, IIdGenerator<Guid> idGenerator,
            ILogger<CreateProjectController> logger)
        {
            _commandBus = commandBus;
            _idGenerator = idGenerator;
            _logger = logger;
        }

        [Consumes("multipart/form-data")]
        [HttpPost]
        [Route("/api/projects/{projectId}/documents")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Tags = new string[] {"Documents"},
            Summary = "Upload document to project")]
        public async Task<ActionResult> UploadDocument(Guid projectId, [FromForm] IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var id = _idGenerator.Generate();
                var command = UploadDocumentCommand.Create(projectId, null, id, file.FileName, stream);
                await _commandBus.Send(command, HttpContext.RequestAborted);

                var routeValues = new
                {
                    action = nameof(GetDocumentController.GetDocument),
                    controller = "GetDocument",
                    projectId = id,
                };

                return CreatedAtRoute(routeValues, id);    
            }
        }

        [Consumes("multipart/form-data")]
        [HttpPost]
        [Route("/api/projects/{projectId}/folders/{folderId}/documents")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Tags = new string[] {"Documents"},
            Summary = "Upload document to project in specific folder")]
        public async Task<ActionResult> UploadDocumentToFolder(Guid projectId, Guid folderId, [FromForm] IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var id = _idGenerator.Generate();
                var command = UploadDocumentCommand.Create(projectId, folderId, id, file.FileName, stream);
                await _commandBus.Send(command, HttpContext.RequestAborted);

                var routeValues = new
                {
                    action = nameof(GetDocumentController.GetDocument),
                    controller = "GetDocument",
                    documentId = id,
                };

                return CreatedAtRoute(routeValues, id);    
            }
        }
    }
}
