using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetExtraWorkAgreements;
using deftq.Pieceworks.Domain.project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjectExtraworkAgreements
{
    [Authorize]
    public class GetExtraWorkAgreementsController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetExtraWorkAgreementsController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet]
        [Route("/api/projects/{projectId}/extraworkgreements")]
        [ProducesResponseType(typeof(ExtraWorkAgreementsResponse), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Extra work agreements" },
            Summary = "Get the list of extra work agreements for the project")]
        public async Task<ActionResult> GetProjectFavorites(Guid projectId)
        {
            var query = GetExtraWorkAgreementsQuery.Create(ProjectId.Create(projectId));
            var resp = await _queryBus.Send<GetExtraWorkAgreementsQuery, GetExtraWorkAgreementsQueryResponse>(query);
            return base.Ok(resp);
        }
    }
}
