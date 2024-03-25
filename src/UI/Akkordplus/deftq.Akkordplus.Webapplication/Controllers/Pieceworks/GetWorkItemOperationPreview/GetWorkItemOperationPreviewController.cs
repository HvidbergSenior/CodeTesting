using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Time;
using deftq.Catalog.Application.GetOperation;
using deftq.Catalog.Application.GetSupplements;
using deftq.Pieceworks.Application.GetWorkItemOperationPreview;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetWorkItemOperationPreview
{
    [Authorize]
    public class GetWorkItemOperationPreviewController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;
        private readonly ISystemTime _systemTime;

        public GetWorkItemOperationPreviewController(IQueryBus queryBus, ISystemTime systemTime)
        {
            _queryBus = queryBus;
            _systemTime = systemTime;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/folders/{folderId}/workitems/operation/preview")]
        [ProducesResponseType(typeof(GetWorkItemOperationPreviewQueryResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Tags = new [] { "Work items" },
            Summary = "Preview expected work time for an operation work item in a folder")]
        public async Task<ActionResult> RegisterWorkItemMaterial(Guid projectId, Guid folderId,
            GetWorkItemOperationPreviewRequest operationPreviewRequest)
        {
            var getOperationQuery = GetOperationQuery.Create(operationPreviewRequest.OperationId);
            var operation = await _queryBus.Send<GetOperationQuery, GetOperationResponse>(getOperationQuery);
            var operationTime = operation?.OperationTimeMilliseconds ?? 0;
            var today = _systemTime.Today();
            var supplements = await GetSupplements(operationPreviewRequest);

            var query = GetWorkItemOperationPreviewQuery.Create(projectId, folderId, today, operationTime, operationPreviewRequest.WorkItemAmount,
                supplements);
            
            var resp = await _queryBus.Send<GetWorkItemOperationPreviewQuery, GetWorkItemOperationPreviewQueryResponse>(query);
            return base.Ok(resp);
        }

        private async Task<IList<Supplement>> GetSupplements(GetWorkItemOperationPreviewRequest materialPreviewRequest)
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
    }
}
