using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetProjectSpecificOperations;
using deftq.Pieceworks.Domain.project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjectSpecificOperations
{
    [Authorize]
    public class GetProjectSpecificOperationsController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetProjectSpecificOperationsController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet]
        [Route("/api/projects/{projectId}/projectspecificoperation")]
        [ProducesResponseType(typeof(GetProjectSpecificOperationsListResponse), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Project Specific Operations" },
            Summary = "Get the list of project specific operations on the project")]
        public async Task<ActionResult> GetProjectSpecificOperationsList(Guid projectId)
        {
            var query = GetProjectSpecificOperationsQuery.Create(ProjectId.Create(projectId));
            var resp = await _queryBus.Send<GetProjectSpecificOperationsQuery, GetProjectSpecificOperationsListResponse>(query);
            return base.Ok(resp);
        }
    }
}
