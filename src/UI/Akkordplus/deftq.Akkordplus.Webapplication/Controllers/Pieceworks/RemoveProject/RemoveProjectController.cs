using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateProject;
using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.RemoveProject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveProject
{
    [Authorize]
    public class RemoveProjectController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<CreateProjectController> _logger;

        public RemoveProjectController(ICommandBus commandBus, ILogger<CreateProjectController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }

        [HttpDelete]
        [Route("/api/projects/{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Projects"},
            Summary = "Remove a project")]
        [SwaggerResponse(200, "The project was removed")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "Project not found")]
        public async Task<ActionResult> CreateProject(Guid projectId)
        {
            var command = RemoveProjectCommand.Create(projectId);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
