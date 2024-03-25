using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetLogBookAsSpreadSheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetLogBookAsSpreadSheet
{
    [Authorize]
    public class GetLogBookAsSpreadSheetController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetLogBookAsSpreadSheetController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet]
        [Route("api/projects/{projectId}/reports/logbookspreadsheet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Reports" },
            Summary = "Get logbook in a spreadsheet")]
        public async Task<ActionResult> GetLogBookAsSpreadSheet(Guid projectId)
        {
            var query = GetLogBookAsSpreadSheetQuery.Create(projectId);
            var resp = await _queryBus.Send<GetLogBookAsSpreadSheetQuery, GetLogBookAsSpreadSheetQueryResponse>(query);
            return base.File(resp.GetBuffer(), "application/octet-stream", resp.FileName);
        }
    }
}
