using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectSpecificOperation;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Generators;
using deftq.Pieceworks.Application.UpdateProjectSpecificOperation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectSpecificOperation
{
    [Authorize]
    public class UpdateProjectSpecificOperationController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IIdGenerator<Guid> _idGenerator;
        private readonly ILogger<RegisterProjectSpecificOperationController> _logger;

        public UpdateProjectSpecificOperationController(ICommandBus commandBus, IIdGenerator<Guid> idGenerator,
            ILogger<RegisterProjectSpecificOperationController> logger)
        {
            _commandBus = commandBus;
            _idGenerator = idGenerator;
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/projectspecificoperation/{projectSpecificOperationId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [SwaggerOperation(
            Tags = new string[] { "Project Specific Operations" },
            Summary = "Create a new projects specific operation")]
        public async Task<ActionResult> UpdateProjectSpecificOperation(Guid projectId, Guid projectSpecificOperationId, UpdateProjectSpecificOperationRequest request)
        {
            var command = UpdateProjectSpecificOperationCommand.Create(projectId, projectSpecificOperationId, request.ExtraWorkAgreementNumber,
                request.Name, request.Description, request.OperationTimeMs, request.WorkingTimeMs);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
