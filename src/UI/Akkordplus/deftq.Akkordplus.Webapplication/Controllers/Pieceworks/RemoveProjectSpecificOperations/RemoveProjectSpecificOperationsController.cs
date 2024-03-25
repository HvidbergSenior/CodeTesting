using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.RemoveProjectSpecificOperations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveProjectSpecificOperations
{
    [Authorize]
    public class RemoveProjectSpecificOperationsController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<RemoveProjectSpecificOperationsController> _logger;

        public RemoveProjectSpecificOperationsController(ICommandBus commandBus, ILogger<RemoveProjectSpecificOperationsController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }

        [HttpDelete]
        [Route("/api/projects/{projectId}/projectspecificoperation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Project Specific Operations" },
            Summary = "Remove projects specific operations from project")]
        [SwaggerResponse(200, "The project specific operations was removed")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "project specific operation not found")]
        public async Task<ActionResult> RemoveProjectSpecificOperations(Guid projectId, RemoveProjectSpecificOperationsRequest request)
        {
            var command = RemoveProjectSpecificOperationsCommand.Create(projectId, request.ProjectSpecificOperationIds);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
