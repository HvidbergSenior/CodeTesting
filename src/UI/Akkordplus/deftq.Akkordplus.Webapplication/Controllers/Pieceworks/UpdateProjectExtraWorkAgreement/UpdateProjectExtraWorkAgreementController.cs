using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.UpdateExtraWorkAgreement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectExtraWorkAgreement
{
    [Authorize]
    public class UpdateProjectExtraWorkAgreementController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<UpdateProjectExtraWorkAgreementController> _logger;

        public UpdateProjectExtraWorkAgreementController(ICommandBus commandBus, ILogger<UpdateProjectExtraWorkAgreementController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }

        [HttpPut]
        [Route("/api/projects/{projectId}/extraworkagreements/{extraWorkAgreementId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [SwaggerOperation(
            Tags = new string[] {"Extra work agreements"},
            Summary = "Update a specific extra work agreement")]
        public async Task<ActionResult> UpdateExtraWorkAgreement(Guid projectId, Guid extraWorkAgreementId,
            UpdateProjectExtraWorkAgreementsRequest request)
        {
            var requestExtraWorkAgreementType = request.ExtraWorkAgreementType switch
            { 
                UpdateProjectExtraWorkAgreementsRequest.UpdateExtraWorkAgreementType.Other => UpdateExtraWorkAgreementType.Other,
                UpdateProjectExtraWorkAgreementsRequest.UpdateExtraWorkAgreementType.AgreedPayment => UpdateExtraWorkAgreementType.AgreedPayment,
                UpdateProjectExtraWorkAgreementsRequest.UpdateExtraWorkAgreementType.CompanyHours => UpdateExtraWorkAgreementType.CompanyHours,
                UpdateProjectExtraWorkAgreementsRequest.UpdateExtraWorkAgreementType.CustomerHours => UpdateExtraWorkAgreementType.CustomerHours,
                _ => throw new ArgumentException("Invalid ExtraWorkAgreementType", nameof(request))
            };
            
            var command = UpdateExtraWorkAgreementCommand.Create(projectId, extraWorkAgreementId, request.ExtraWorkAgreementNumber,
                request.Name, request.Description ?? "", requestExtraWorkAgreementType, request.PaymentDkr, request.WorkTime?.Hours, request.WorkTime?.Minutes);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
