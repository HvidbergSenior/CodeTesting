using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork.Supplements
{
    public sealed class Supplement : Entity
    {
        public SupplementId SupplementId { get; private set; }
        public CatalogSupplementId CatalogSupplementId { get; private set; }
        public SupplementNumber SupplementNumber { get; private set; }
        public SupplementText SupplementText { get; private set; }
        public SupplementPercentage SupplementPercentage { get; private set; }
        
        private Supplement()
        {
            SupplementId = SupplementId.Empty();
            Id = SupplementId.Value;
            CatalogSupplementId = CatalogSupplementId.Empty();
            SupplementNumber = SupplementNumber.Empty();
            SupplementText = SupplementText.Empty();
            SupplementPercentage = SupplementPercentage.Empty();
        }

        private Supplement(SupplementId supplementId, CatalogSupplementId catalogSupplementId, SupplementNumber supplementNumber, SupplementText supplementText, SupplementPercentage supplementPercentage)
        {
            SupplementId = supplementId;
            Id = SupplementId.Value;
            CatalogSupplementId = catalogSupplementId;
            SupplementNumber = supplementNumber;
            SupplementText = supplementText;
            SupplementPercentage = supplementPercentage;
        }

        public static Supplement Create(SupplementId supplementId, CatalogSupplementId catalogSupplementId, SupplementNumber supplementNumber, SupplementText supplementText, SupplementPercentage supplementPercentage)
        {
            return new Supplement(supplementId, catalogSupplementId, supplementNumber, supplementText, supplementPercentage);
        }
    }
}
