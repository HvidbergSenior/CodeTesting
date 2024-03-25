using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.MaterialCatalog
{
    public sealed class MaterialUnit : ValueObject
    {
        private const string MeterString = "meter";
        private const string UnitString = "stk";
        
        public string Value { get; private set; }

        private MaterialUnit()
        {
            Value = String.Empty;
        }

        private MaterialUnit(string value)
        {
            Value = value;
        }
        
        private static MaterialUnit Create(string value)
        {
            return new MaterialUnit(value);
        }

        public static MaterialUnit Empty()
        {
            return new MaterialUnit("");
        }
        
        public static MaterialUnit Meter()
        {
            return new MaterialUnit(MeterString);
        }
        
        public static MaterialUnit Piece()
        {
            return new MaterialUnit(UnitString);
        }

        public static MaterialUnit FromString(string unit)
        {
            if (unit is null)
            {
                return Empty();
            }
            
            return unit.ToLowerInvariant() switch
            {
                MeterString => Meter(),
                UnitString => Piece(),
                _ => Empty()
            };
        }
    }
}
