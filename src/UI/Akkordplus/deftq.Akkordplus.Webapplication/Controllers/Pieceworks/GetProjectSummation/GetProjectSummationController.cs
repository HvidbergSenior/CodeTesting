using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetProjectFolderSummation;
using deftq.Pieceworks.Application.GetProjectSummation;
using deftq.Pieceworks.Domain.project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjectSummation
{
    [Authorize]
    public class GetProjectSummationController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetProjectSummationController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }
        
        [HttpGet]
        [Route("/api/projects/{projectId}/summation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Projects"},
            Summary = "Get summation for project")]
        [ProducesResponseTypeAttribute(typeof(GetProjectSummationQueryResponse), 200)]
        public async Task<ActionResult> GetProjectSummation(Guid projectId)
        {
            var query = GetProjectSummationQuery.Create(ProjectId.Create(projectId));
            var resp = await _queryBus.Send<GetProjectSummationQuery, GetProjectSummationQueryResponse>(query);
            return base.Ok(resp);
        }
    }
}
