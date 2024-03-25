namespace deftq.Pieceworks.Domain.Calculation.Expression
{
    public sealed class SumExpression : IExpression
    {
        public IExpression A { get; private set; }
        public IExpression B { get; private set; }

        private SumExpression(IExpression a, IExpression b)
        {
            A = a;
            B = b;
        }

        public static IExpression Sum(IExpression a, IExpression b)
        {
            return new SumExpression(a, b);
        }

        public static IExpression Sum(IExpression a, IExpression b, IExpression c)
        {
            return new SumExpression(new SumExpression(a, b), c);
        }
        
        public static IExpression Sum(IExpression a, IExpression b, IExpression c, IExpression d)
        {
            return new SumExpression(a, new SumExpression(b, new SumExpression(c, d)));
        }
        
        public DecimalNumber Evaluate()
        {
            return DecimalNumber.Number(A.Evaluate().Value + B.Evaluate().Value);
        }

        public bool IsCompound()
        {
            return true;
        }

        public string AsString()
        {
            var aStr = A.IsCompound() ? "(" + A.AsString() + ")" : A.AsString();
            var bStr = B.IsCompound() ? "(" + B.AsString() + ")" : B.AsString();
            return aStr + " + " + bStr;
        }
    }

}
