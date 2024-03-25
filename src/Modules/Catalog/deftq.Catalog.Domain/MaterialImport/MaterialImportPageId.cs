using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.MaterialImport
{
    public sealed class MaterialImportPageId : ValueObject
    {
        public Guid Value { get; private set; }

        private MaterialImportPageId()
        {
            Value = Guid.Empty;
        }

        private MaterialImportPageId(Guid value)
        {
            Value = value;
        }

        public static MaterialImportPageId Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(id));
            }

            return new MaterialImportPageId(id);
        }

        public static MaterialImportPageId Empty()
        {
            return new MaterialImportPageId();
        }
    }
}
