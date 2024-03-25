namespace deftq.Pieceworks.Domain.Calculation.Expression
{
    public sealed class DecimalNumber : IExpression
    {
        public decimal Value { get; private set; }
        public DecimalNumberUnit Unit { get; private set; }

        private DecimalNumber(decimal value, DecimalNumberUnit numberUnit)
        {
            Value = value;
            Unit = numberUnit;
        }

        public static DecimalNumber Number(decimal value)
        {
            return new DecimalNumber(value, DecimalNumberUnit.None);
        }

        public static IExpression Number(decimal value, DecimalNumberUnit numberUnit)
        {
            return new DecimalNumber(value, numberUnit);
        }
        
        public DecimalNumber Evaluate()
        {
            return this;
        }

        public bool IsCompound()
        {
            return false;
        }

        public string AsString()
        {
            if (Unit == DecimalNumberUnit.None)
            {
                return Value.ToString(IExpression.CultureInfo);    
            }
            return Value.ToString(IExpression.CultureInfo) + "" + Unit.UnitString;
        }
    }
}
