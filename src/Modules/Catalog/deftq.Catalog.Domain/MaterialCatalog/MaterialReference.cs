using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.MaterialCatalog
{
    public sealed class MaterialReference : ValueObject
    {
        public MaterialId ReferenceMaterialId { get; private set; }
        public EanNumber ReferenceMaterialEan { get; private set; }

        private MaterialReference()
        {
            ReferenceMaterialId = MaterialId.Empty();
            ReferenceMaterialEan = EanNumber.Empty();
        }

        private MaterialReference(MaterialId referenceMaterialId, EanNumber referenceMaterialEan)
        {
            ReferenceMaterialId = referenceMaterialId;
            ReferenceMaterialEan = referenceMaterialEan;
        }

        public static MaterialReference Create(MaterialId referenceMaterialId, EanNumber referenceMaterialEan)
        {
            return new MaterialReference(referenceMaterialId, referenceMaterialEan);
        }
        
        public static MaterialReference Empty()
        {
            return new MaterialReference(MaterialId.Empty(), EanNumber.Empty());
        }
    }
}
