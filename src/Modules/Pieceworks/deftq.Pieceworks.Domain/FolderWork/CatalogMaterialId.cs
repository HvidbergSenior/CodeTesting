using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class CatalogMaterialId : ValueObject
    {
        public Guid Value { get; private set; }

        private CatalogMaterialId()
        {
            Value = Guid.Empty;
        }

        private CatalogMaterialId(Guid value)
        {
            Value = value;
        }

        public static CatalogMaterialId Create(Guid value)
        {
            return new CatalogMaterialId(value);
        }

        public static CatalogMaterialId Empty()
        {
            return new CatalogMaterialId(Guid.Empty);
        }
    }
}
