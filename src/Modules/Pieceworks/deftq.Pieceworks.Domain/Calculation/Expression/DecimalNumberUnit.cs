namespace deftq.Pieceworks.Domain.Calculation.Expression
{
    public sealed class DecimalNumberUnit
    {
        public string UnitString { get; }

        private DecimalNumberUnit(string unitString)
        {
            UnitString = unitString;
        }

        public static readonly DecimalNumberUnit Ms = new("ms");
        public static readonly DecimalNumberUnit Dkr = new("dkr");
        public static readonly DecimalNumberUnit Meter = new("m");
        public static readonly DecimalNumberUnit None = new(String.Empty);

        public static DecimalNumberUnit Create(string unitString)
        {
            return new DecimalNumberUnit(unitString);
        }
    }
}
