using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetProjectSummation;
using deftq.Pieceworks.Application.GetProjectUsers;
using deftq.Pieceworks.Domain.project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjectUsers
{
    [Authorize]
    public class GetProjectUsersController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetProjectUsersController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }
        
        [HttpGet]
        [Route("/api/projects/{projectId}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Users"},
            Summary = "Get users for project")]
        [ProducesResponseTypeAttribute(typeof(GetProjectUsersQueryResponse), 200)]
        public async Task<ActionResult> GetProjectUsers(Guid projectId)
        {
            var query = GetProjectUsersQuery.Create(ProjectId.Create(projectId));
            var resp = await _queryBus.Send<GetProjectUsersQuery, GetProjectUsersQueryResponse>(query);
            return base.Ok(resp);
        }
    }
}
