using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Catalog.Domain.MaterialCatalog;

namespace deftq.Catalog.Application.SearchMaterial
{
    public sealed class SearchMaterialQuery : IQuery<SearchMaterialResponse>
    {
        private const uint MaxHitsLimit = 1000;
        public string QueryString { get; private set; }
        public uint MaxHits { get; private set; }

        private SearchMaterialQuery(string queryString, uint maxHits)
        {
            QueryString = queryString;
            MaxHits = Math.Min(MaxHitsLimit, maxHits);
        }

        public static SearchMaterialQuery Create(string queryString, uint maxHits)
        {
            return new SearchMaterialQuery(queryString, maxHits);
        }
    }

    public sealed class SearchMaterialResponse
    {
        public IList<FoundMaterial> FoundMaterials { get; private set; }

        public SearchMaterialResponse(IList<FoundMaterial> foundMaterials)
        {
            FoundMaterials = foundMaterials;
        }
    }
    
    public sealed class FoundMaterial
    {
        public Guid Id { get; private set; }
        public string EanNumber { get; private set; }
        public string Name { get; private set; }
        public string Unit { get; private set; }
        
        public FoundMaterial(Guid id, string eanNumber, string name, string unit)
        {
            Id = id;
            EanNumber = eanNumber;
            Name = name;
            Unit = unit;
        }
    }

    internal class SearchMaterialQueryHandler : IQueryHandler<SearchMaterialQuery, SearchMaterialResponse>
    {
        private readonly IMaterialRepository _materialRepository;

        public SearchMaterialQueryHandler(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<SearchMaterialResponse> Handle(SearchMaterialQuery request, CancellationToken cancellationToken)
        {
            var result = await _materialRepository.Search(request.QueryString, request.MaxHits, cancellationToken);
            
            var foundMaterials = new List<FoundMaterial>();
            foreach (var foundMaterial in result)
            {
                foundMaterials.Add(new FoundMaterial(foundMaterial.MaterialId.Value, foundMaterial.EanNumber.Value, foundMaterial.Name.Value, foundMaterial.Unit.Value));
                if (request.MaxHits > 0 && foundMaterials.Count >= request.MaxHits)
                {
                    break;
                }
            }
            return new SearchMaterialResponse(foundMaterials);
        }
    }

    internal class SearchMaterialQueryAuthorizer : IAuthorizer<SearchMaterialQuery>
    {
        public Task<AuthorizationResult> Authorize(SearchMaterialQuery instance, CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}
