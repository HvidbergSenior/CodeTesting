using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Catalog.Domain.OperationCatalog;

namespace deftq.Catalog.Application.SearchOperation
{
    public sealed class SearchOperationQuery : IQuery<SearchOperationResponse>
    {
        private const uint MaxHitsLimit = 1000;
        public string QueryString { get; private set; }
        public uint MaxHits { get; private set; }

        private SearchOperationQuery(string queryString, uint maxHits)
        {
            QueryString = queryString;
            MaxHits = Math.Min(MaxHitsLimit, maxHits);
        }

        public static SearchOperationQuery Create(string queryString, uint maxHits)
        {
            return new SearchOperationQuery(queryString, maxHits);
        }
    }

    public sealed class SearchOperationResponse
    {
        public IList<FoundOperation> FoundOperations { get; private set; }

        public SearchOperationResponse(IList<FoundOperation> foundOperations)
        {
            FoundOperations = foundOperations;
        }
    }
    
    public sealed class FoundOperation
    {
        public Guid OperationId { get; private set; }
        public string OperationNumber { get; private set; }
        public string OperationText { get; private set; }

        public FoundOperation(Guid operationId, string operationNumber, string operationText)
        {
            OperationId = operationId;
            OperationNumber = operationNumber;
            OperationText = operationText;
        }
    }

    internal class SearchOperationQueryHandler : IQueryHandler<SearchOperationQuery, SearchOperationResponse>
    {
        private readonly IOperationRepository _operationRepository;

        public SearchOperationQueryHandler(IOperationRepository operationRepository)
        {
            _operationRepository = operationRepository;
        }

        public async Task<SearchOperationResponse> Handle(SearchOperationQuery request, CancellationToken cancellationToken)
        {
            var result = await _operationRepository.Search(request.QueryString, request.MaxHits, cancellationToken);
            
            var foundOperations = new List<FoundOperation>();
            foreach (var foundOperation in result)
            {
                foundOperations.Add(new FoundOperation(foundOperation.Id, foundOperation.OperationNumber.Value, foundOperation.OperationText.Value));
                if (request.MaxHits > 0 && foundOperations.Count >= request.MaxHits)
                {
                    break;
                }
            }
            return new SearchOperationResponse(foundOperations);
        }
    }

    internal class SearchOperationQueryAuthorizer : IAuthorizer<SearchOperationQuery>
    {
        public Task<AuthorizationResult> Authorize(SearchOperationQuery instance, CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}
