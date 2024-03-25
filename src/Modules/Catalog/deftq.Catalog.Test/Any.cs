using AutoFixture;
using deftq.Catalog.Domain.MaterialCatalog;

namespace deftq.Catalog.Test
{
    internal static class Any
    {
        private static readonly Fixture any = new();

        public static T Instance<T>()
        {
            return any.Create<T>();
        }

        public static Material Material()
        {
            return Domain.MaterialCatalog.Material.Create(MaterialId.Create(Guid.NewGuid()), EanNumber.Create("0043168256186"), MaterialName.Empty(),
                MaterialUnit.Meter(), new List<Mounting>(), MaterialReference.Empty(), MaterialReference.Empty(),
                MaterialPublished.Create(DateTimeOffset.Now));
        }

        public static Material WithEanNumber(this Material material, EanNumber eanNumber)
        {
            return Domain.MaterialCatalog.Material.Create(material.MaterialId, eanNumber, material.Name, material.Unit, material.Mountings,
                material.MasterMaterial, material.ReplacementMaterial, material.Published);
        }
    }
}
