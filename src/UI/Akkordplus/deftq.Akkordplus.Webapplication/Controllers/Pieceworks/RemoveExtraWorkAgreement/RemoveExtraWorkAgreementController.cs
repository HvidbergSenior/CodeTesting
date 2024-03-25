using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.RemoveExtraWorkAgreement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveExtraWorkAgreement
{
    [Authorize]
    public class RemoveExtraWorkAgreementController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<RemoveExtraWorkAgreementController> _logger;

        public RemoveExtraWorkAgreementController(ICommandBus commandBus, ILogger<RemoveExtraWorkAgreementController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }

        [HttpDelete]
        [Route("/api/projects/{projectId}/extraworkagreements")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Extra work agreements"},
            Summary = "Remove extra work agreements from project")]
        [SwaggerResponse(200, "The extra work agreement was removed")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "Project or extra work agreement not found")]
        public async Task<ActionResult> RemoveWorkItems(Guid projectId, RemoveExtraWorkAgreementRequest request)
        {
            var command = RemoveExtraWorkAgreementCommand.Create(projectId, request.ExtraWorkAgreementIds);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
