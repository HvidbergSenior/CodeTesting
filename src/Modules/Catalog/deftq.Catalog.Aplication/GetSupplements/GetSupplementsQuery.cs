using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.DataAccess;
using deftq.Catalog.Domain.SupplementCatalog;

namespace deftq.Catalog.Application.GetSupplements
{
    public sealed class GetSupplementsQuery : IQuery<GetSupplementsResponse>
    {
        private GetSupplementsQuery()
        {
            
        }

        public static GetSupplementsQuery Create()
        {
            return new GetSupplementsQuery();
        }
    }

    public sealed class GetSupplementsResponse
    {
        public IList<SupplementResponse> Supplements { get; private set; }

        public GetSupplementsResponse(IList<SupplementResponse> supplements)
        {
            Supplements = supplements;
        }
    }

    public sealed class SupplementResponse
    {
        public Guid SupplementId { get; private set; }
        public string SupplementNumber { get; private set; }
        public string SupplementText { get; private set; }
        public decimal SupplementPercentage { get; private set; }
        
        public SupplementResponse(Guid supplementId, string supplementNumber, string supplementText, decimal supplementPercentage)
        {
            SupplementId = supplementId;
            SupplementNumber = supplementNumber;
            SupplementText = supplementText;
            SupplementPercentage = supplementPercentage;
        }
    }
    
    internal class GetSupplementsQueryHandler : IQueryHandler<GetSupplementsQuery, GetSupplementsResponse>
    {
        private readonly ISupplementRepository _supplementRepository;

        public GetSupplementsQueryHandler(ISupplementRepository supplementRepository)
        {
            _supplementRepository = supplementRepository;
        }

        public async Task<GetSupplementsResponse> Handle(GetSupplementsQuery request, CancellationToken cancellationToken)
        {
            var supplements = await _supplementRepository.GetAllAsync(cancellationToken);
            var resultSupplements = new List<SupplementResponse>();
            foreach (var supplement in supplements)
            {
                resultSupplements.Add(new SupplementResponse(supplement.SupplementId.Value, supplement.SupplementNumber.Value, supplement.SupplementText.Value, supplement.SupplementValue.Value));
            }
            
            return new GetSupplementsResponse(resultSupplements);
        }
    }
}
