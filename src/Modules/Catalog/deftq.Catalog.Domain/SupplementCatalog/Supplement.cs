using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.SupplementCatalog
{
    public sealed class Supplement : Entity
    {
        public SupplementId SupplementId { get; private set; }
        public SupplementNumber SupplementNumber { get; set; }
        public SupplementText SupplementText { get; set; }
        public SupplementValue SupplementValue { get; private set; }
        
        private Supplement()
        {
            SupplementId = SupplementId.Empty();
            Id = SupplementId.Value;
            SupplementNumber = SupplementNumber.Empty();
            SupplementText = SupplementText.Empty();
            SupplementValue = SupplementValue.Empty();
        }

        private Supplement(SupplementId supplementId, SupplementNumber supplementNumber, SupplementText supplementText, SupplementValue supplementValue)
        {
            SupplementId = supplementId;
            Id = SupplementId.Value;
            SupplementNumber = supplementNumber;
            SupplementText = supplementText;
            SupplementValue = supplementValue;
        }

        public static Supplement Create(SupplementId supplementId, SupplementNumber supplementNumber, SupplementText supplementText, SupplementValue supplementValue)
        {
            return new Supplement(supplementId, supplementNumber, supplementText, supplementValue);
        }
    }
}
