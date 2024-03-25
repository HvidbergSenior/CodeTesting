using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.RemoveCompensationPayments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveCompensationPayments
{
    [Authorize]
    public class RemoveCompensationPaymentsController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<RemoveCompensationPaymentsController> _logger;
        
        public RemoveCompensationPaymentsController(ICommandBus commandBus, ILogger<RemoveCompensationPaymentsController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }

        [HttpDelete]
        [Route("/api/projects/{projectId}/compensations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Compensations" },
            Summary = "Register one or more compensations from a project")]
        [SwaggerResponse(200, "The compensations were removed")]
        [SwaggerResponse(403, "User not authorized to remove compensations")]
        [SwaggerResponse(404, "Compensation not found")]
        public async Task<ActionResult> RemoveCompensations(Guid projectId, RemoveCompensationPaymentsRequest request)
        {
            var command = RemoveCompensationPaymentsCommand.Create(projectId, request.CompensationPaymentIds);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
