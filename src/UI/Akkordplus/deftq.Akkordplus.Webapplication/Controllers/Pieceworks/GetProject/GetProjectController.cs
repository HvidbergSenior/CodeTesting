using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetProject;
using deftq.Pieceworks.Domain.project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProject
{
    [Authorize]
    public class GetProjectController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetProjectController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet]
        [Route("/api/projects/{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Projects"},
            Summary = "Get project by id")]
        [ProducesResponseTypeAttribute(typeof(GetProjectResponse), 200)]
        public async Task<ActionResult> GetProject(Guid projectId)
        {
            var query = GetProjectQuery.Create(ProjectId.Create(projectId));
            var resp = await _queryBus.Send<GetProjectQuery, GetProjectResponse>(query);
            return base.Ok(resp);
        }
    }
}
