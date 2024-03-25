using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Exceptions;
using deftq.Catalog.Domain.OperationCatalog;

namespace deftq.Catalog.Application.GetOperation
{
    public sealed class GetOperationQuery : IQuery<GetOperationResponse>
    {
        public OperationId OperationId { get; private set; }

        private GetOperationQuery(OperationId operationId)
        {
            OperationId = operationId;
        }

        public static GetOperationQuery Create(Guid operationId)
        {
            return new GetOperationQuery(OperationId.Create(operationId));
        }
    }

    public sealed class GetOperationResponse
    {
        public Guid OperationId { get; private set; }
        public string OperationNumber { get; private set; }
        public string OperationText { get; private set; }
        public decimal OperationTimeMilliseconds { get; private set; }

        public GetOperationResponse(Guid operationId, string operationNumber, string operationText, decimal operationTimeMilliseconds)
        {
            OperationId = operationId;
            OperationNumber = operationNumber;
            OperationText = operationText;
            OperationTimeMilliseconds = operationTimeMilliseconds;
        }
    }

    internal class GetOperationQueryHandler : IQueryHandler<GetOperationQuery, GetOperationResponse>
    {
        private readonly IOperationRepository _operationRepository;
        private readonly IMartenConfig _martenConfig;

        public GetOperationQueryHandler(IOperationRepository operationRepository, IMartenConfig martenConfig)
        {
            _operationRepository = operationRepository;
            _martenConfig = martenConfig;
        }

        public async Task<GetOperationResponse> Handle(GetOperationQuery request, CancellationToken cancellationToken)
        {
            var operation = await _operationRepository.FindById(request.OperationId.Value, cancellationToken);

            if (operation is not null)
            {
                return new GetOperationResponse(operation.OperationId.Value, operation.OperationNumber.Value, operation.OperationText.Value,
                    operation.OperationTime.Milliseconds);
            }

            throw new NotFoundException($"Operation with id {request.OperationId.Value} not found");
        }
    }

    internal class GetOperationQueryAuthorizer : IAuthorizer<GetOperationQuery>
    {
        public Task<AuthorizationResult> Authorize(GetOperationQuery instance, CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}
