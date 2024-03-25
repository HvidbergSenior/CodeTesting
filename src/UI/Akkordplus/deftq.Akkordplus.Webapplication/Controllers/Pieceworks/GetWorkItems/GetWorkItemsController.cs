using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetWorkItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetWorkItems
{
    [Authorize]
    public class GetWorkItemsController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetWorkItemsController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet]
        [Route("/api/projects/{projectId}/folders/{folderId}/workitems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Work items"},
            Summary = "Get all work items registered in folder")]
        [ProducesResponseTypeAttribute(typeof(GetWorkItemsQueryResponse), 200)]
        public async Task<ActionResult> GetWorkItems(Guid projectId, Guid folderId)
        {
            var query = GetWorkItemsQuery.Create(projectId, folderId);
            var resp = await _queryBus.Send<GetWorkItemsQuery, GetWorkItemsQueryResponse>(query);
            return base.Ok(resp);
        }
    }
}
