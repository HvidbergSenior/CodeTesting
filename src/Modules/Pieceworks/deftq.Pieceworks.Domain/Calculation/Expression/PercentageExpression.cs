using static deftq.Pieceworks.Domain.Calculation.Expression.DecimalNumber;

namespace deftq.Pieceworks.Domain.Calculation.Expression
{
    public sealed class PercentageExpression : IExpression
    {
        public IExpression Value { get; private set; }
        private readonly bool _compound;
        private const decimal OneHundred = 100;

        private PercentageExpression(IExpression value, bool compound)
        {
            Value = value;
            _compound = compound;
        }
        
        public static PercentageExpression Percentage(IExpression value)
        {
            return new PercentageExpression(value, value.IsCompound());
        }
        
        public static PercentageExpression Percentage(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Percentage must be greater than 0", nameof(value));
            }
            return new PercentageExpression(DecimalNumber.Number(value), false);
        }

        public DecimalNumber Evaluate()
        {
            return Number(this.Value.Evaluate().Value / OneHundred);
        }

        public bool IsCompound()
        {
            return _compound;
        }

        public string AsString()
        {
            if (IsCompound())
            {
                return "(" + Value.AsString() + ")%";
            }
            return Value.AsString() + "%";
        }
    }

}
