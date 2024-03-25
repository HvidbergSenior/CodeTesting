using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Time;
using deftq.Catalog.Application.GetMaterial;
using deftq.Catalog.Application.GetSupplements;
using deftq.Pieceworks.Application.GetWorkItemMaterialPreview;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetWorkItemMaterialPreview
{
    [Authorize]
    public class GetWorkItemMaterialPreviewController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;
        private readonly ISystemTime _systemTime;

        public GetWorkItemMaterialPreviewController(IQueryBus queryBus, ISystemTime systemTime)
        {
            _queryBus = queryBus;
            _systemTime = systemTime;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/folders/{folderId}/workitems/material/preview")]
        [ProducesResponseType(typeof(GetWorkItemMaterialPreviewQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Tags = new [] { "Work items" },
            Summary = "Preview expected work time for a material work item in a folder")]
        public async Task<ActionResult> RegisterWorkItemMaterial(Guid projectId, Guid folderId,
            GetWorkItemMaterialPreviewRequest materialPreviewRequest)
        {
            var getMaterialQuery = GetMaterialQuery.Create(materialPreviewRequest.MaterialId);
            var material = await _queryBus.Send<GetMaterialQuery, GetMaterialResponse>(getMaterialQuery);
            var materialAssemblyResponse = material.Mountings.FirstOrDefault(m => m.MountingCode == materialPreviewRequest.WorkItemMountingCode);
            var operationTime = materialAssemblyResponse?.OperationTimeMilliseconds ?? 0;
            var today = _systemTime.Today();
            var supplementOperations = GetSupplementOperations(materialPreviewRequest, materialAssemblyResponse);
            var supplements = await GetSupplements(materialPreviewRequest);

            var query = GetWorkItemMaterialPreviewQuery.Create(projectId, folderId, today, operationTime, materialPreviewRequest.WorkItemAmount,
                supplementOperations, supplements);

            var resp = await _queryBus.Send<GetWorkItemMaterialPreviewQuery, GetWorkItemMaterialPreviewQueryResponse>(query);
            return base.Ok(resp);
        }

        private async Task<IList<Supplement>> GetSupplements(GetWorkItemMaterialPreviewRequest materialPreviewRequest)
        {
            if (!materialPreviewRequest.Supplements.Any())
            {
                return new List<Supplement>();
            }

            var availableSupplements = await _queryBus.Send<GetSupplementsQuery, GetSupplementsResponse>(GetSupplementsQuery.Create());
            IList<Supplement> results = new List<Supplement>();
            foreach (var requestSupplement in materialPreviewRequest.Supplements)
            {
                var foundSupplement = availableSupplements.Supplements.FirstOrDefault(s => s.SupplementId == requestSupplement.SupplementId);
                if (foundSupplement is null)
                {
                    throw new NotFoundException($"Unable to find supplement with id {requestSupplement.SupplementId}");
                }

                results.Add(Supplement.Create(foundSupplement.SupplementPercentage));
            }

            return results;
        }

        private static List<SupplementOperation> GetSupplementOperations(GetWorkItemMaterialPreviewRequest materialRequest,
            MaterialMountingResponse? materialAssemblyResponse)
        {
            if (materialRequest.SupplementOperations.Any() && materialAssemblyResponse is null)
            {
                throw new NotFoundException($"Unable to add supplement operations because mounting code was not found");
            }

            var supplementOperations = new List<SupplementOperation>();
            foreach (var operationRequest in materialRequest.SupplementOperations)
            {
                var foundSupplementOperation = materialAssemblyResponse?.SupplementOperations.FirstOrDefault(op =>
                    op.SupplementOperationId == operationRequest.SupplementOperationId);

                if (foundSupplementOperation is null)
                {
                    throw new NotFoundException("Supplement operation was not found");
                }

                var operationType =
                    foundSupplementOperation.Type == SupplementOperationResponse.SupplementOperationType.AmountRelated
                        ? SupplementOperation.SupplementOperationType.AmountRelated
                        : SupplementOperation.SupplementOperationType.UnitRelated;

                supplementOperations.Add(SupplementOperation.Create(operationType, foundSupplementOperation.OperationTimeMilliseconds,
                    operationRequest.Amount));
            }

            return supplementOperations;
        }
    }
}
