using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Generators;
using deftq.Pieceworks.Application.RegisterExtraWorkAgreement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateExtraWorkAgreement
{
    [Authorize]
    public class CreateExtraWorkAgreementController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IIdGenerator<Guid> _idGenerator;
        private readonly ILogger<CreateExtraWorkAgreementController> _logger;

        public CreateExtraWorkAgreementController(ICommandBus commandBus, IIdGenerator<Guid> idGenerator,
            ILogger<CreateExtraWorkAgreementController> logger)
        {
            _commandBus = commandBus;
            _idGenerator = idGenerator;
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/extraworkagreements")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [SwaggerOperation(
            Tags = new string[] { "Extra work agreements" },
            Summary = "Create a new extrawork agreement")]
        public async Task<ActionResult> CreateExtraWorkAgreement(Guid projectId, CreateExtraWorkAgreementRequest request)
        {
            var extraWorkAgreementId = _idGenerator.Generate();

            var requestExtraWorkAgreementType = request.ExtraWorkAgreementType switch
            {
                CreateExtraWorkAgreementRequest.ExtraWorkAgreementTypeRequest.Other => ExtraWorkAgreementType.Other,
                CreateExtraWorkAgreementRequest.ExtraWorkAgreementTypeRequest.AgreedPayment => ExtraWorkAgreementType.AgreedPayment,
                CreateExtraWorkAgreementRequest.ExtraWorkAgreementTypeRequest.CompanyHours => ExtraWorkAgreementType.CompanyHours,
                CreateExtraWorkAgreementRequest.ExtraWorkAgreementTypeRequest.CustomerHours => ExtraWorkAgreementType.CustomerHours,
                _ => throw new ArgumentException("Invalid ExtraWorkAgreementType", nameof(request))
            };
            
            var command = RegisterExtraWorkAgreementCommand.Create(projectId, extraWorkAgreementId, request.ExtraWorkAgreementNumber, request.Name,
                request.Description ?? "", requestExtraWorkAgreementType, request.PaymentDkr, request.WorkTime?.Hours, request.WorkTime?.Minutes);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
