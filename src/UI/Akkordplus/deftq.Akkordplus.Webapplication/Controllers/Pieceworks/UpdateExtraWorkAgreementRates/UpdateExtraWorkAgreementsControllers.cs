using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.UpdateExtraWorkAgreementRates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateExtraWorkAgreementRates
{
    [Authorize]
    public class UpdateExtraWorkAgreementRatesController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<UpdateExtraWorkAgreementRatesController> _logger;
            
        public UpdateExtraWorkAgreementRatesController(ICommandBus commandBus, ILogger<UpdateExtraWorkAgreementRatesController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }

        [HttpPut]
        [Route("/api/projects/{projectId}/extraworkagreements/rates")]
        [SwaggerOperation(
            Tags = new string[] { "Extra work agreements" },
            Summary = "Update extra work agreement rates")]
        [SwaggerResponse(200, "The extra work agreement rates have been updated")]
        [SwaggerResponse(403, "User is not project owner")]
        [SwaggerResponse(404, "Project not found")]
        public async Task<ActionResult> UpdateExtraWorkAgreementRates(Guid projectId, [FromBody] UpdateExtraWorkAgreementRatesRequest request)
        {
            var command = UpdateExtraWorkAgreementRatesCommand.Create(projectId, request.CustomerRatePrHour, request.CompanyRatePrHour);
            await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
