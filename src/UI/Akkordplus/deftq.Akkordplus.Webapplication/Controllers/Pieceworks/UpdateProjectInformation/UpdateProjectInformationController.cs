using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.UpdateProjectInformation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectNameAndOrderNumber
{
    [Authorize]
    public class UpdateProjectInformationController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<UpdateProjectInformationController> _logger;

        public UpdateProjectInformationController(ICommandBus commandBus, ILogger<UpdateProjectInformationController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/projectinformation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Projects" },
            Summary = "Update piecework name, description and order nr.")]
        [SwaggerResponse(200, "The piecework name was updated")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "Project not found")]
        public async Task<ActionResult> UpdateProjectInformation(Guid projectId, UpdateProjectInformationRequest request)
        {
            var command = UpdateProjectInformationCommand.Create(projectId, request.Name, request.Description ?? "", request.PieceworkNumber ?? "",
                request.OrderNumber ?? "");
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
