using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Queries;
using deftq.Catalog.Application.GetMaterial;
using deftq.Catalog.Application.GetOperation;
using deftq.Pieceworks.Application.RegisterProjectCatalogFavorite;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectCatalogFavorite
{
    [Authorize]
    public class RegisterProjectCatalogFavoriteController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly ILogger<RegisterProjectCatalogFavoriteController> _logger;

        public RegisterProjectCatalogFavoriteController(ICommandBus commandBus, IQueryBus queryBus, ILogger<RegisterProjectCatalogFavoriteController> logger)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
            _logger = logger;
        }
        
        [HttpPost]
        [Route("/api/projects/{projectId}/favorites")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Tags = new string[] {"Favorites"},
            Summary = "Register a new material or operation af favorite on a project")]
        public async Task<ActionResult> RegisterProjectFavorite(Guid projectId, RegisterProjectCatalogFavoriteRequest request)
        {
            var catalogItemType = request.CatalogType == FavoriteCatalogType.Material ? CatalogItemType.Material : CatalogItemType.Operation;
            string catalogItemNumber, catalogItemText, catalogItemUnit;
            if (request.CatalogType == FavoriteCatalogType.Material)
            {
                var getMaterialQuery = GetMaterialQuery.Create(request.CatalogId);
                var material = await _queryBus.Send<GetMaterialQuery, GetMaterialResponse>(getMaterialQuery);
                catalogItemNumber = material.EanNumber;
                catalogItemText = material.Name;
                catalogItemUnit = material.Unit;
            }
            else
            {
                var getOperationQuery = GetOperationQuery.Create(request.CatalogId);
                var operation = await _queryBus.Send<GetOperationQuery, GetOperationResponse>(getOperationQuery);
                catalogItemNumber = operation.OperationNumber;
                catalogItemText = operation.OperationText;
                catalogItemUnit = String.Empty;
            }
            
            var command = RegisterProjectCatalogFavoriteCommand.Create(projectId, Guid.NewGuid(), catalogItemType, request.CatalogId, catalogItemNumber, catalogItemText, catalogItemUnit);
            await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
