using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetWorkItemsAsSpreadSheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetWorkItemsAsSpreadSheet
{
    [Authorize]
    public class GetWorkItemsAsSpreadSheetController : ApiControllerBase
    {
        
        private readonly IQueryBus _queryBus;

        public GetWorkItemsAsSpreadSheetController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet]
        [Route("/api/projects/{projectId}/reports/workitemsspreadsheet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Reports"},
            Summary = "Get all work items in a spreadsheet")]
        public async Task<ActionResult> GetWorkItemsAsSpreadSheet(Guid projectId)
        {
            var query = GetWorkItemsAsSpreadSheetQuery.Create(projectId);
            var resp = await _queryBus.Send<GetWorkItemsAsSpreadSheetQuery, GetWorkItemsAsSpreadSheetQueryResponse>(query);
            return base.File(resp.GetBuffer(), "application/octet-stream", resp.Filename);
        }
    }
}
