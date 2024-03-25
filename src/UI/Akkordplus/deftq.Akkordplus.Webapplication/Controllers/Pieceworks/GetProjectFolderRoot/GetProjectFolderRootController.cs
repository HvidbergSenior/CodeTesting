using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetProjectFolderRoot;
using deftq.Pieceworks.Domain.project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjectFolderRoot
{
    [Authorize]
    public class GetProjectFolderRootController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetProjectFolderRootController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet]
        [Route("/api/projects/{projectId}/folders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Folders"},
            Summary = "Get all folders for a project")]
        [ProducesResponseTypeAttribute(typeof(GetProjectFolderRootQueryResponse), 200)]
        public async Task<ActionResult> GetProjectFolderRoot(Guid projectId)
        {
            var query = GetProjectFolderRootQuery.Create(ProjectId.Create(projectId));
            var resp = await _queryBus.Send<GetProjectFolderRootQuery, GetProjectFolderRootQueryResponse>(query);
            return base.Ok(resp);
        }
    }
}
