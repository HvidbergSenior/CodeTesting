using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Generators;
using deftq.Pieceworks.Application.RegisterProjectSpecificOperation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectSpecificOperation
{
    [Authorize]
    public class RegisterProjectSpecificOperationController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IIdGenerator<Guid> _idGenerator;
        private readonly ILogger<RegisterProjectSpecificOperationController> _logger;

        public RegisterProjectSpecificOperationController(ICommandBus commandBus, IIdGenerator<Guid> idGenerator,
            ILogger<RegisterProjectSpecificOperationController> logger)
        {
            _commandBus = commandBus;
            _idGenerator = idGenerator;
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/projectspecificoperation")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [SwaggerOperation(
            Tags = new string[] { "Project Specific Operations" },
            Summary = "Create a new projects specific operation")]
        public async Task<ActionResult> CreateProjectSpecificOperation(Guid projectId, RegisterProjectSpecificOperationRequest request)
        {
            var projectSpecificOperationId = _idGenerator.Generate();

            var command = RegisterProjectSpecificOperationCommand.Create(projectId, projectSpecificOperationId, request.ExtraWorkAgreementNumber,
                request.Name, request.Description, request.OperationTimeMs, request.WorkingTimeMs);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
