namespace deftq.Catalog.Domain.MaterialImport
{
    public interface IImportRepository
    {
        /// <summary>
        /// Switch from the existing material catalog to the catalog published on <paramref name="published"/>.
        /// All pages from the new catalog must be imported prior to calling this, if not the catalog is not
        /// switched.
        /// </summary>
        Task SwitchMaterialCatalog(DateTimeOffset published, CancellationToken cancellationToken);
    }
}
