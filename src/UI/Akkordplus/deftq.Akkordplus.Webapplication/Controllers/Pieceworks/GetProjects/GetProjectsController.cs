using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetProjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjects
{
    [Authorize]
    public class GetProjectsController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetProjectsController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet]
        [Route("/api/projects")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Projects"},
            Summary = "Get all projects")]
        [ProducesResponseTypeAttribute(typeof(GetProjectsResponse), 200)]
        public async Task<ActionResult> GetProjects()
        {
            var query = GetProjectsQuery.Create();
            var resp = await _queryBus.Send<GetProjectsQuery, GetProjectsResponse>(query);
            return base.Ok(resp);
        }
    }
}
