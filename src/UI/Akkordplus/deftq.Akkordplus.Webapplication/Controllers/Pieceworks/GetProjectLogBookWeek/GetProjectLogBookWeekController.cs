using System.Globalization;
using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetProject;
using deftq.Pieceworks.Application.GetProjectLogBookWeek;
using deftq.Pieceworks.Domain.project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjectLogBookWeek
{
    [Authorize]
    public class GetProjectLogBookWeekController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;
        
        public GetProjectLogBookWeekController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet]
        [Route("/api/projects/{projectId}/logbook/{userId}/weeks/{year}/{week}")]
        [ProducesResponseType(typeof(GetProjectLogBookWeekQueryResponse),StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Log book"},
            Summary = "Get a specific week from the project log book")]
        public async Task<ActionResult> GetProjectLogBookWeek(Guid projectId, Guid userId, int year, int week)
        {
            return await ExecuteQuery(projectId, userId, year, week);
        }
        
        [HttpGet]
        [Route("/api/projects/{projectId}/logbook/{userId}/weeks/{year}/{month}/{day}")]
        [ProducesResponseType(typeof(GetProjectLogBookWeekQueryResponse),StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Log book"},
            Summary = "Get a specific week from the project log book")]
        public async Task<ActionResult> GetProjectLogBookWeek(Guid projectId, Guid userId, int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            return await ExecuteQuery(projectId, userId, ISOWeek.GetYear(date), ISOWeek.GetWeekOfYear(date));
        }
        
        private async Task<ActionResult> ExecuteQuery(Guid projectId, Guid userId, int year, int week)
        {
            var logBookQuery = GetProjectLogBookWeekQuery.Create(ProjectId.Create(projectId), userId, year, week);
            var weekResponse = await _queryBus.Send<GetProjectLogBookWeekQuery, GetProjectLogBookWeekQueryResponse>(logBookQuery);
            return base.Ok(weekResponse);
        }
    }
}
