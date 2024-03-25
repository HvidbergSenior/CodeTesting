using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetProjectLogBook;
using deftq.Pieceworks.Domain.project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjectLogBook
{
    [Authorize]
    public class GetProjectLogBookController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;
        
        public GetProjectLogBookController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet]
        [Route("/api/projects/{projectId}/logbook")]
        [ProducesResponseType(typeof(GetProjectLogBookQueryResponse), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Log book"},
            Summary = "Get the log book for a project")]
        public async Task<ActionResult> GetProjectLogBook(Guid projectId)
        {
            var query = GetProjectLogBookQuery.Create(ProjectId.Create(projectId));
            var resp = await _queryBus.Send<GetProjectLogBookQuery, GetProjectLogBookQueryResponse>(query);
            return base.Ok(resp);
        }
    }
}
