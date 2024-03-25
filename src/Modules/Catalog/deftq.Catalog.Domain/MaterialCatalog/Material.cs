using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.MaterialCatalog
{
    public sealed class Material : Entity
    {
        public MaterialId MaterialId { get; private set; }

        public EanNumber EanNumber { get; private set; }
        public MaterialName Name { get; private set; }
        public MaterialUnit Unit { get; private set; }

        public IList<Mounting> Mountings { get; private set; }

        public MaterialReference MasterMaterial { get; private set; }
        public MaterialReference ReplacementMaterial { get; private set; }

        public MaterialPublished Published { get; private set; }
        
        private Material()
        {
            MaterialId = MaterialId.Empty();
            Id = MaterialId.Value;
            EanNumber = EanNumber.Empty();
            Name = MaterialName.Empty();
            Unit = MaterialUnit.Empty();
            Mountings = new List<Mounting>();
            MasterMaterial = MaterialReference.Empty();
            ReplacementMaterial = MaterialReference.Empty();
            Published = MaterialPublished.Empty();
        }

        private Material(MaterialId materialId, EanNumber eanNumber, MaterialName name, MaterialUnit unit, IList<Mounting> mountings,
            MaterialReference masterMaterial, MaterialReference replacementMaterial, MaterialPublished published)
        {
            MaterialId = materialId;
            Id = MaterialId.Value;
            EanNumber = eanNumber;
            Name = name;
            Unit = unit;
            Mountings = mountings;
            MasterMaterial = masterMaterial;
            ReplacementMaterial = replacementMaterial;
            Published = published;
        }

        public static Material Create(MaterialId materialId, EanNumber eanNumber, MaterialName name, MaterialUnit unit, IList<Mounting> mountings,
            MaterialReference masterMaterial, MaterialReference replacementMaterial, MaterialPublished published)
        {
            return new Material(materialId, eanNumber, name, unit, mountings, masterMaterial, replacementMaterial, published);
        }

        public bool HasReplacementMaterial()
        {
            return ReplacementMaterial != MaterialReference.Empty();
        }

        public bool HasMasterMaterial()
        {
            return MasterMaterial != MaterialReference.Empty();
        }
    }
}
