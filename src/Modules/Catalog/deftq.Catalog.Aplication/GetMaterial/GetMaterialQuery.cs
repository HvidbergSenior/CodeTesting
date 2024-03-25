using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Exceptions;
using deftq.Catalog.Domain.MaterialCatalog;
using deftq.Catalog.Domain.OperationCatalog;
using Marten;

namespace deftq.Catalog.Application.GetMaterial
{
    public sealed class GetMaterialQuery : IQuery<GetMaterialResponse>
    {
        public MaterialId MaterialId { get; private set; }

        private GetMaterialQuery(MaterialId materialId)
        {
            MaterialId = materialId;
        }

        public static GetMaterialQuery Create(Guid materialId)
        {
            return new GetMaterialQuery(MaterialId.Create(materialId));
        }
    }

    public sealed class GetMaterialResponse
    {
        public Guid Id { get; private set; }
        public string EanNumber { get; private set; }
        public string Name { get; private set; }
        public string Unit { get; private set; }
        public IList<MaterialMountingResponse> Mountings { get; private set; }

        public GetMaterialResponse(Guid id, string eanNumber, string name, string unit, IList<MaterialMountingResponse> mountings)
        {
            Id = id;
            EanNumber = eanNumber;
            Name = name;
            Unit = unit;
            Mountings = mountings;
        }
    }

    public sealed class MaterialMountingResponse
    {
        public int MountingCode { get; private set; }
        public string Text { get; private set; }
        public decimal OperationTimeMilliseconds { get; private set; }
        public IList<SupplementOperationResponse> SupplementOperations { get; private set; }

        public MaterialMountingResponse(int mountingCode, string text, decimal operationTimeMilliseconds,
            IList<SupplementOperationResponse> supplementOperations)
        {
            MountingCode = mountingCode;
            Text = text;
            OperationTimeMilliseconds = operationTimeMilliseconds;
            SupplementOperations = supplementOperations;
        }
    }

    public sealed class SupplementOperationResponse
    {
        public enum SupplementOperationType { AmountRelated, UnitRelated }
        
        public Guid SupplementOperationId { get; private set; }
        public string Text { get; private set; }
        public SupplementOperationType Type { get; private set; }
        public decimal OperationTimeMilliseconds { get; private set; }

        public SupplementOperationResponse(Guid supplementOperationId, string text, SupplementOperationType type, decimal operationTimeMilliseconds)
        {
            SupplementOperationId = supplementOperationId;
            Text = text;
            Type = type;
            OperationTimeMilliseconds = operationTimeMilliseconds;
        }
    }

    internal class GetMaterialQueryHandler : IQueryHandler<GetMaterialQuery, GetMaterialResponse>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IOperationRepository _operationRepository;
        private readonly IMartenConfig _martenConfig;

        public GetMaterialQueryHandler(IMaterialRepository materialRepository, IOperationRepository operationRepository, IMartenConfig martenConfig)
        {
            _materialRepository = materialRepository;
            _operationRepository = operationRepository;
            _martenConfig = martenConfig;
        }
        
        public async Task<GetMaterialResponse> Handle(GetMaterialQuery request, CancellationToken cancellationToken)
        {
            IDictionary<Guid, Material> masters = new Dictionary<Guid, Material>();
            IDictionary<Guid, Material> replacements = new Dictionary<Guid, Material>();

            // Fetch material from catalog
            var material = await FetchMaterialFromCatalog(request.MaterialId.Value, masters, replacements, cancellationToken);
            
            // Get material mountings while considering master materials
            var mountings = GetMountings(material, masters);
            
            // Get supplement operations, can be embedded or fetched from operations catalog 
            var supplementOperations = await GetSupplementOperationsFromCatalog(mountings, cancellationToken);
            
            // Create response
            var mountingsResponse = CreateMountingsResponse(mountings, supplementOperations);
            return new GetMaterialResponse(material.Id, material.EanNumber.Value, material.Name.Value, material.Unit.Value, mountingsResponse);
        }
        
        private async Task<Material> FetchMaterialFromCatalog(Guid materialId, IDictionary<Guid, Material> masters, IDictionary<Guid, Material> replacements,
            CancellationToken cancellationToken)
        {
            var query = _materialRepository.Query()
                .Include(m => m.MasterMaterial.ReferenceMaterialId.Value, masters)
                .Include(m => m.ReplacementMaterial.ReferenceMaterialId.Value, replacements)
                .Where(m => m.MaterialId.Value == materialId);

            var result = await query.FirstOrDefaultAsync(cancellationToken);
            if (result is null)
            {
                throw new NotFoundException($"Material with id {materialId} not found");
            }

            return result;
        }

        private static IList<Mounting> GetMountings(Material material, IDictionary<Guid, Material> masters)
        {
            var mountings = material.Mountings;
            
            // If master material, then use mountings from master
            if (material.HasMasterMaterial())
            {
                var masterMaterialId = material.MasterMaterial.ReferenceMaterialId.Value;
                if (masters.ContainsKey(masterMaterialId))
                {
                    mountings = masters[masterMaterialId].Mountings;
                }
                else
                {
                    throw new NotFoundException($"Master material with id {masterMaterialId} not found");
                }
            }

            return mountings;
        }

        private async Task<IDictionary<Guid, Operation>> GetSupplementOperationsFromCatalog(IList<Mounting> mountings, CancellationToken cancellationToken)
        {
            IDictionary<Guid, Operation> supplementOperations = new Dictionary<Guid, Operation>();
            
            // Get referenced operations from operations catalog
            var referencedOperations = mountings.SelectMany(m => m.SupplementOperations).Where(m => !m.IsEmbedded());
            IList<Guid> referencedOperationIds = referencedOperations.Select(o => o.OperationId.Value).ToList();
            var supplementOperationsQuery = _operationRepository.Query().Where(o => o.OperationId.Value.In(referencedOperationIds));
            var returnedOperations = await supplementOperationsQuery.ToListAsync(cancellationToken);
            foreach (var operation in returnedOperations)
            {
                supplementOperations.Add(operation.OperationId.Value, operation);
            }
            return supplementOperations;
        }

        private static List<MaterialMountingResponse> CreateMountingsResponse(IList<Mounting> mountings, IDictionary<Guid, Operation> supplementOperations)
        {
            var mountingsResponse = new List<MaterialMountingResponse>();
            foreach (var mounting in mountings)
            {
                var mountingCode = mounting.MountingCode.Code;
                var text = mounting.MountingCode.Text;
                var operationTimeMilliseconds = mounting.OperationTime.Milliseconds;
                var supplementOperationsResult = CreateSupplementOperations(supplementOperations, mounting);
                mountingsResponse.Add(new MaterialMountingResponse(mountingCode, text, operationTimeMilliseconds, supplementOperationsResult));
            }
            return mountingsResponse;
        }

        private static List<SupplementOperationResponse> CreateSupplementOperations(IDictionary<Guid, Operation> supplementOperations, Mounting mounting)
        {
            var supplementOperationsResult = new List<SupplementOperationResponse>();
            foreach (var operation in mounting.SupplementOperations)
            {
                var operationType = operation.IsAmountRelated()
                    ? SupplementOperationResponse.SupplementOperationType.AmountRelated
                    : SupplementOperationResponse.SupplementOperationType.UnitRelated;

                if (operation.IsEmbedded())
                {
                    supplementOperationsResult.Add(new SupplementOperationResponse(operation.SupplementOperationId.Value,
                        operation.OperationText.Value, operationType,
                        operation.OperationTime.Milliseconds));
                }
                else
                {
                    if (!supplementOperations.ContainsKey(operation.OperationId.Value))
                    {
                        throw new NotFoundException($"Operation with id {operation.OperationId.Value} not found");
                    }

                    var supplementOperation = supplementOperations[operation.OperationId.Value];
                    supplementOperationsResult.Add(new SupplementOperationResponse(operation.SupplementOperationId.Value,
                        supplementOperation.OperationText.Value, operationType,
                        supplementOperation.OperationTime.Milliseconds));
                }
            }
            return supplementOperationsResult;
        }
    }

    internal class GetMaterialQueryAuthorizer : IAuthorizer<GetMaterialQuery>
    {
        public Task<AuthorizationResult> Authorize(GetMaterialQuery instance, CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}
