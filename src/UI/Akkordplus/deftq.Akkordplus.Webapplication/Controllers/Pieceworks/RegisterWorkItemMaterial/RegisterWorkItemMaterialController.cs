using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Generators;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Time;
using deftq.Catalog.Application.GetMaterial;
using deftq.Catalog.Application.GetSupplements;
using deftq.Pieceworks.Application.RegisterWorkItemMaterial;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemMaterial
{
    [Authorize]
    public class RegisterWorkItemMaterialController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IIdGenerator<Guid> _idGenerator;
        private readonly ISystemTime _systemTime;

        public RegisterWorkItemMaterialController(ICommandBus commandBus, IQueryBus queryBus, IIdGenerator<Guid> idGenerator, ISystemTime systemTime)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
            _idGenerator = idGenerator;
            _systemTime = systemTime;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/folders/{folderId}/workitems/material")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Tags = new string[] { "Work items" },
            Summary = "Register a new material work item in a folder")]
        public async Task<ActionResult> RegisterWorkItemMaterial(Guid projectId, Guid folderId, RegisterWorkItemMaterialRequest materialRequest)
        {
            var workItemId = _idGenerator.Generate();
            var getMaterialQuery = GetMaterialQuery.Create(materialRequest.MaterialId);
            var material = await _queryBus.Send<GetMaterialQuery, GetMaterialResponse>(getMaterialQuery);
            var materialAssemblyResponse = material.Mountings.FirstOrDefault(m => m.MountingCode == materialRequest.WorkItemMountingCode);
            var operationTime = materialAssemblyResponse?.OperationTimeMilliseconds ?? 0;
            var today = _systemTime.Today();
            var supplementOperations = GetSupplementOperations(materialRequest, materialAssemblyResponse);
            var supplements = await GetSupplements(materialRequest);

            var command = RegisterWorkItemMaterialCommand.Create(projectId, folderId, materialRequest.MaterialId, workItemId, material.Name,
                today, operationTime,
                materialRequest.WorkItemMountingCode, materialRequest.WorkItemAmount,
                material.EanNumber, material.Unit, supplementOperations, supplements);

            await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }

        private async Task<IList<Supplement>> GetSupplements(RegisterWorkItemMaterialRequest materialRequest)
        {
            if (!materialRequest.Supplements.Any())
            {
                return new List<Supplement>();
            }
            
            var availableSupplements = await _queryBus.Send<GetSupplementsQuery, GetSupplementsResponse>(GetSupplementsQuery.Create());
            IList<Supplement> results = new List<Supplement>();
            foreach (var requestSupplement in materialRequest.Supplements)
            {
                var foundSupplement = availableSupplements.Supplements.FirstOrDefault(s => s.SupplementId == requestSupplement.SupplementId);
                if (foundSupplement is null)
                {
                    throw new NotFoundException($"Unable to find supplement with id {requestSupplement.SupplementId}"); 
                }
                results.Add(Supplement.Create(Guid.NewGuid(),foundSupplement.SupplementId, foundSupplement.SupplementNumber, foundSupplement.SupplementText, foundSupplement.SupplementPercentage));
            }

            return results;
        }

        private static List<SupplementOperation> GetSupplementOperations(RegisterWorkItemMaterialRequest materialRequest,  MaterialMountingResponse? materialAssemblyResponse)
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

                supplementOperations.Add(SupplementOperation.Create(Guid.NewGuid(), foundSupplementOperation.SupplementOperationId, foundSupplementOperation.Text,
                    operationType,
                    foundSupplementOperation.OperationTimeMilliseconds, operationRequest.Amount));
            }

            return supplementOperations;
        }
    }
}
