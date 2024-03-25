using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Generators;
using deftq.Pieceworks.Application.RegisterProjectCompensation;
using deftq.Pieceworks.Domain.projectCompensation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectCompensation
{
    [Authorize]
    public class RegisterCompensationController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IIdGenerator<Guid> _idGenerator;
        private readonly ILogger<RegisterCompensationController> _logger;

        public RegisterCompensationController(ICommandBus commandBus, IIdGenerator<Guid> idGenerator, ILogger<RegisterCompensationController> logger)
        {
            _commandBus = commandBus;
            _idGenerator = idGenerator;
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/compensations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Tags = new string[] { "Compensations" },
            Summary = "Register a compensation on a project")]
        public async Task<ActionResult> RegisterProjectCompensation(Guid projectId, RegisterCompensationRequest request)
        {
            var compensationId = _idGenerator.Generate();
            var startDate = ProjectCompensationDate.Create(request.CompensationStartDate);
            var endDate = ProjectCompensationDate.Create(request.CompensationEndDate);

            var command = RegisterProjectCompensationCommand.Create(projectId, compensationId, request.CompensationParticipantIds,  request.CompensationPayment,
                startDate, endDate);
            await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
