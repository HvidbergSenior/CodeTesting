using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.RemoveFromProjectFavorite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveProjectFavorites
{
    [Authorize]
    public class RemoveProjectFavoritesController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<RemoveProjectFavoritesController> _logger;

        public RemoveProjectFavoritesController(ICommandBus commandBus, ILogger<RemoveProjectFavoritesController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }

        [HttpDelete]
        [Route("/api/projects/{projectId}/favorites")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Favorites" },
            Summary = "Remove favorites from project")]
        [SwaggerResponse(200, "The favorite was removed")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "Favorite not found")]
        public async Task<ActionResult> RemoveFavorites(Guid projectId, RemoveProjectFavoritesRequest request)
        {
            var command = RemoveProjectFavoriteCommand.Create(projectId, request.FavoriteIds);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
