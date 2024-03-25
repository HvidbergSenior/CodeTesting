using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Generators;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Time;
using deftq.Catalog.Application.GetOperation;
using deftq.Catalog.Application.GetSupplements;
using deftq.Pieceworks.Application.RegisterWorkItemOperation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Supplement = deftq.Pieceworks.Application.RegisterWorkItemOperation.Supplement;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemOperation
{
    [Authorize]
    public class RegisterWorkItemOperationController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IIdGenerator<Guid> _idGenerator;
        private readonly ISystemTime _systemTime;

        public RegisterWorkItemOperationController(ICommandBus commandBus, IQueryBus queryBus, IIdGenerator<Guid> idGenerator, ISystemTime systemTime)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
            _idGenerator = idGenerator;
            _systemTime = systemTime;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/folders/{folderId}/workitems/operation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Tags = new string[] { "Work items" },
            Summary = "Register a new operation work item in a folder")]
        public async Task<OkResult> RegisterWorkItemOperation(Guid projectId, Guid folderId, RegisterWorkItemOperationRequest operationRequest)
        {
            var workItemId = _idGenerator.Generate();
            var getOperationQuery = GetOperationQuery.Create(operationRequest.OperationId);
            var operation = await _queryBus.Send<GetOperationQuery, GetOperationResponse>(getOperationQuery);
            var operationTime = operation.OperationTimeMilliseconds;
            var today = _systemTime.Today();
            var supplements = await GetSupplements(operationRequest);

            var command = RegisterWorkItemOperationCommand.Create(projectId, folderId, operationRequest.OperationId,
                operation.OperationNumber, workItemId, operation.OperationText, today, operationTime,
                operationRequest.WorkItemAmount, supplements);

            await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }

        private async Task<IList<Supplement>> GetSupplements(RegisterWorkItemOperationRequest materialRequest)
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

                results.Add(Supplement.Create(Guid.NewGuid(), foundSupplement.SupplementId, foundSupplement.SupplementNumber,
                    foundSupplement.SupplementText, foundSupplement.SupplementPercentage));
            }

            return results;
        }
    }
}
