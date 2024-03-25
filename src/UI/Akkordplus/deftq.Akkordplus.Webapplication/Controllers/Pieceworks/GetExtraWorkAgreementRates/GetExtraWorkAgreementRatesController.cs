using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetExtraWorkAgreementRates;
using deftq.Pieceworks.Domain.project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetExtraWorkAgreementRates
{

    [Authorize]
    public class GetExtraWorkAgreementRatesController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;
        private readonly ILogger<GetExtraWorkAgreementRatesController> _logger;

        public GetExtraWorkAgreementRatesController(IQueryBus queryBus, ILogger<GetExtraWorkAgreementRatesController> logger)
        {
            _queryBus = queryBus;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/projects/{projectId}/extraworkagreements/rates")]
        [ProducesResponseType(typeof(GetExtraWorkAgreementRatesQueryResponse), 200)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Tags = new string[] {"Extra work agreements"},
            Summary = "Gets extra work agreement rates")]
        public async Task<ActionResult> GetExtraWorkAgreementRates(Guid projectId)
        {
            var query = GetExtraWorkAgreementRatesQuery.Create(ProjectId.Create(projectId));
            var resp = await _queryBus.Send<GetExtraWorkAgreementRatesQuery, GetExtraWorkAgreementRatesQueryResponse>(query);
            return base.Ok(resp);
        }
    }
}
