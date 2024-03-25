using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetCompensationPaymentParticipantsInPeriod;
using deftq.Pieceworks.Domain.project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetCompensationPaymentUsers
{
    [Authorize]
    public class GetCompensationPaymentParticipantsInPeriodController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetCompensationPaymentParticipantsInPeriodController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }
        
        [HttpPost]
        [Route("/api/projects/{projectId}/compensations/participants")]
        [ProducesResponseType(typeof(GetCompensationPaymentResponse),StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Compensations"},
            Summary = "Get the participants on the project, with there hours and compensation payment in a period")]
        public async Task<ActionResult> GetCompensationPaymentUsers(Guid projectId, GetCompensationPaymentParticipantsInPeriodRequest request)
        {
            var query = GetCompensationPaymentParticipantsInPeriodQuery.Create(ProjectId.Create(projectId), request.StartDate, request.EndDate, request.Amount);
            var response = await _queryBus.Send<GetCompensationPaymentParticipantsInPeriodQuery, GetCompensationPaymentResponse>(query);
            return base.Ok(response);
        }
    }
}
