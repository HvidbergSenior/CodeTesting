using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetLogBookAsSpreadSheet;
using deftq.Pieceworks.Application.GetStatusReportAsSpreadSheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetStatusReportAsSpreadSheet
{
    [Authorize]
    public class GetStatusReportAsSpreadSheetController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetStatusReportAsSpreadSheetController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet]
        [Route("api/projects/{projectId}/reports/statusreportspreatsheet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Reports" },
            Summary = "Get status report in a spreadsheet")]
        public async Task<ActionResult> GetStatusReportAsSpreadSheet(Guid projectId)
        {
            var query = GetStatusReportAsSpreadSheetQuery.Create(projectId);
            var resp = await _queryBus.Send<GetStatusReportAsSpreadSheetQuery, GetStatusReportAsSpreadSheetQueryResponse>(query);
            return base.File(resp.GetBuffer(), "application/octet-stream", resp.FileName);
        }
    }
}
