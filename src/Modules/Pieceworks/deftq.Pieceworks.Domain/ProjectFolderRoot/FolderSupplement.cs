using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.FolderWork.Supplements;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class FolderSupplement : Entity
    {
        public SupplementId SupplementId { get; private set; }
        public CatalogSupplementId CatalogSupplementId { get; private set; }
        public SupplementNumber SupplementNumber { get; private set; }
        public SupplementText SupplementText { get; private set; }
        public SupplementPercentage SupplementPercentage { get; private set; }

        private FolderSupplement(SupplementId supplementId, CatalogSupplementId catalogSupplementId, SupplementNumber supplementNumber,
            SupplementText supplementText, SupplementPercentage supplementPercentage)
        {
            SupplementId = supplementId;
            CatalogSupplementId = catalogSupplementId;
            SupplementNumber = supplementNumber;
            SupplementText = supplementText;
            SupplementPercentage = supplementPercentage;
        }

        private FolderSupplement()
        {
            SupplementId = SupplementId.Empty();
            CatalogSupplementId = CatalogSupplementId.Empty();
            SupplementNumber = SupplementNumber.Empty();
            SupplementText = SupplementText.Empty();
            SupplementPercentage = SupplementPercentage.Empty();
        }

        public static FolderSupplement Create(SupplementId supplementId, CatalogSupplementId catalogSupplementId, SupplementNumber supplementNumber,
            SupplementText supplementText, SupplementPercentage supplementPercentage)
        {
            return new FolderSupplement(supplementId, catalogSupplementId, supplementNumber, supplementText, supplementPercentage);
        }

        /// <summary>
        /// make a copy of a FolderSupplement, with all the values and add new supplement id
        /// </summary>
        /// <param name="toCopy">The FolderSupplement to copy from</param>
        /// <returns>The new copied FolderSupplement with a new SupplementId</returns>
        public static FolderSupplement Copy(FolderSupplement toCopy)
        {
            return Create(SupplementId.Create(Guid.NewGuid()), toCopy.CatalogSupplementId, toCopy.SupplementNumber,
                toCopy.SupplementText, toCopy.SupplementPercentage);
        }
    }
}
