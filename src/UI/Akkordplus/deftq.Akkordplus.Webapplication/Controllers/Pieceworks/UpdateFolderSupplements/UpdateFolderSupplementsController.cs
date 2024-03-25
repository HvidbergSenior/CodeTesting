using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Exceptions;
using deftq.Catalog.Application.GetSupplements;
using deftq.Pieceworks.Application.UpdateFolderSupplements;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.projectFolderRoot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateFolderSupplements
{
    [Authorize]
    public class UpdateFolderSupplementsController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;

        public UpdateFolderSupplementsController(ICommandBus commandBus, IQueryBus queryBus)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/folders/{folderId}/supplements")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Folders" },
            Summary = "update folder supplements")]
        [SwaggerResponse(200, "The folder supplements was updated")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "Project or folder not found")]
        public async Task<ActionResult> UpdateFolderSupplements(Guid projectId, Guid folderId, UpdateFolderSupplementsRequest request)
        {
            var supplements = await GetSupplements(request);
            var command = UpdateFolderSupplementsCommand.Create(projectId, folderId, supplements);
            var resp = await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }

        private async Task<IList<FolderSupplement>> GetSupplements(UpdateFolderSupplementsRequest request)
        {
            var supplementIds = request.FolderSupplements.ToHashSet();
            
            if (!supplementIds.Any())
            {
                return new List<FolderSupplement>();
            }

            var availableSupplements = await _queryBus.Send<GetSupplementsQuery, GetSupplementsResponse>(GetSupplementsQuery.Create());
            IList<FolderSupplement> results = new List<FolderSupplement>();
            foreach (var supplementId in supplementIds)
            {
                var foundSupplement = availableSupplements.Supplements.FirstOrDefault(s => s.SupplementId == supplementId);
                if (foundSupplement is null)
                {
                    throw new NotFoundException($"Unable to find supplement with id {supplementId}");
                }

                results.Add(FolderSupplement.Create( SupplementId.Create(Guid.NewGuid()), CatalogSupplementId.Create(foundSupplement.SupplementId), SupplementNumber.Create(foundSupplement.SupplementNumber),
                   SupplementText.Create(foundSupplement.SupplementText), SupplementPercentage.Create(foundSupplement.SupplementPercentage)));
            }

            return results;
        }
    }
}
