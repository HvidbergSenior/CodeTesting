namespace deftq.Pieceworks.Domain.Calculation.Expression
{
    public sealed class DivideExpression : IExpression
    {
        public IExpression A { get; private set; }
        public IExpression B { get; private set; }

        private DivideExpression(IExpression a, IExpression b)
        {
            A = a;
            B = b;
        }

        public static DivideExpression Divide(IExpression a, IExpression b)
        {
            return new DivideExpression(a, b);
        }

        public DecimalNumber Evaluate()
        {
            return DecimalNumber.Number(A.Evaluate().Value / B.Evaluate().Value);
        }

        public bool IsCompound()
        {
            return true;
        }

        public string AsString()
        {
            var aStr = A.IsCompound() ? "(" + A.AsString() + ")" : A.AsString();
            var bStr = B.IsCompound() ? "(" + B.AsString() + ")" : B.AsString();
            return aStr + " / " + bStr;
        }
    }

}
