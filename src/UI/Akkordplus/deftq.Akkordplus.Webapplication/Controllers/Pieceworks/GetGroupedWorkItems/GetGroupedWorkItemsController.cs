using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetGroupedWorkItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetGroupedWorkItems
{
    
    [Authorize]
    public class GetGroupedWorkItemsController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;
        private readonly ILogger<GetGroupedWorkItemsController> _logger;

        public GetGroupedWorkItemsController(IQueryBus queryBus, ILogger<GetGroupedWorkItemsController> logger)
        {
            _queryBus = queryBus;
            _logger = logger;
        }
    
        [HttpGet]
        [Route("/api/projects/{projectId}/folders/{folderId}/workitems/grouped")]
        [ProducesResponseTypeAttribute(typeof(GetGroupedWorkItemsQueryResponse), 200)]
        [SwaggerOperation(
            Tags = new string[] {"Work items"},
            Summary = "Get all work items in current and sub folders grouped together by id")]
        [SwaggerResponse(404, "Project, source folder, sub folders or work items not found")]
        public async Task<ActionResult> GetGroupedWorkItems(Guid projectId, Guid folderId, uint maxHits)
        {
            var query = GetGroupedWorkItemsQuery.Create(projectId, folderId, maxHits);
            var resp = await _queryBus.Send<GetGroupedWorkItemsQuery, GetGroupedWorkItemsQueryResponse>(query);
            return base.Ok(resp);
        }
    }
}
