using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetProjectFolderSummation;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjectFolderSummation
{
    [Authorize]
    public class GetProjectFolderSummationController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetProjectFolderSummationController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }
        
        [HttpGet]
        [Route("/api/projects/{projectId}/folders/{folderId}/summation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] {"Folders"},
            Summary = "Get summation of all work items in folder (including subfolders)")]
        [ProducesResponseTypeAttribute(typeof(GetProjectFolderSummationQueryResponse), 200)]
        public async Task<ActionResult> GetProjectFolderSummation(Guid projectId, Guid folderId)
        {
            var query = GetProjectFolderSummationQuery.Create(ProjectId.Create(projectId), ProjectFolderId.Create(folderId));
            var resp = await _queryBus.Send<GetProjectFolderSummationQuery, GetProjectFolderSummationQueryResponse>(query);
            return base.Ok(resp);
        }
    }
}
