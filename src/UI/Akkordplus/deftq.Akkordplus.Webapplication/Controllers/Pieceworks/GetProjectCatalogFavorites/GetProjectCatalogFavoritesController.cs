using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetFavoriteList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjectCatalogFavorites
{
    [Authorize]
    public class GetProjectCatalogFavoritesController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetProjectCatalogFavoritesController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet]
        [Route("/api/projects/{projectId}/favorites")]
        [ProducesResponseType(typeof(GetProjectFavoriteListQueryResponse), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Favorites" },
            Summary = "Get the list of favorites on the project")]
        public async Task<ActionResult> GetProjectFavorites(Guid projectId)
        {
            var query = GetProjectFavoriteListQuery.Create(projectId);
            var resp = await _queryBus.Send<GetProjectFavoriteListQuery, GetProjectFavoriteListQueryResponse>(query);
            return base.Ok(resp);
        }
    }
}
