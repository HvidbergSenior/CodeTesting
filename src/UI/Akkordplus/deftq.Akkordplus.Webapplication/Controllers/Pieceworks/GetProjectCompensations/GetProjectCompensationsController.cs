using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetProjectCompensation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjectCompensations
{
    [Authorize]
    public class GetProjectCompensationsController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetProjectCompensationsController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet]
        [Route("/api/projects/{projectId}/compensations")]
        [ProducesResponseType(typeof(GetProjectCompensationListQueryResponse), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Favorites" },
            Summary = "Get the list of favorites on the project")]
        public async Task<ActionResult> GetProjectCompensations(Guid projectId)
        {
            var query = GetProjectCompensationListQuery.Create(projectId);
            var resp = await _queryBus.Send<GetProjectCompensationListQuery, GetProjectCompensationListQueryResponse>(query);
            return base.Ok(resp);
        }
    }
}
