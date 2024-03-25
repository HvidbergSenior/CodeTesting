using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Generators;
using deftq.Pieceworks.Application.CreateProject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateProject
{
    [Authorize]
    public class CreateProjectController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IIdGenerator<Guid> _idGenerator;
        private readonly ILogger<CreateProjectController> _logger;

        public CreateProjectController(ICommandBus commandBus, IIdGenerator<Guid> idGenerator,
            ILogger<CreateProjectController> logger)
        {
            _commandBus = commandBus;
            _idGenerator = idGenerator;
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/projects")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [SwaggerOperation(
            Tags = new string[] {"Projects"},
            Summary = "Create a new project")]
        public async Task<ActionResult> CreateProject(CreateProjectRequest request)
        {
            var id = _idGenerator.Generate();
            var pieceworkSum = request.PieceworkType != PieceworkType.TwelveTwo ? 0 : request.PieceworkSum ?? 0;
            var command = CreateProjectCommand.Create(id, request.Title, request.Description ?? "", request.PieceworkType, pieceworkSum);
            await _commandBus.Send(command, HttpContext.RequestAborted);

            var routeValues = new
            {
                action = nameof(GetProject.GetProjectController.GetProject),
                controller = "GetProject",
                projectId = id,
            };

            return CreatedAtRoute(routeValues, id);
        }
    }
}
