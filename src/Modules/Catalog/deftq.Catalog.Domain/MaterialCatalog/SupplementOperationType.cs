using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.MaterialCatalog
{
    public sealed class SupplementOperationType : ValueObject
    {
        private const string AmountRelatedValue = "amount";
        private const string UnitRelatedValue = "unit";
        
        public string Value { get; private set; }

        private SupplementOperationType()
        {
            Value = string.Empty;
        }

        private SupplementOperationType(string value)
        {
            Value = value;
        }
        
        public static SupplementOperationType Empty()
        {
            return new SupplementOperationType(string.Empty);
        }

        /// <summary>
        /// A supplement operation type where the operation amount covers a single material unit (eg. when calculating,
        /// the operation amount is multiplied by the number of material units).
        /// </summary>
        public static SupplementOperationType UnitRelated()
        {
            return new SupplementOperationType(UnitRelatedValue);
        }
        
        /// <summary>
        /// A supplement operation type where the operation amount covers all material units.
        /// </summary>
        public static SupplementOperationType AmountRelated()
        {
            return new SupplementOperationType(AmountRelatedValue);
        }
        
        public static SupplementOperationType FromString(string type)
        {
            return type.ToLowerInvariant() switch
            {
                UnitRelatedValue => UnitRelated(),
                AmountRelatedValue => AmountRelated(),
                _ => Empty()
            };
        }
    }
}
