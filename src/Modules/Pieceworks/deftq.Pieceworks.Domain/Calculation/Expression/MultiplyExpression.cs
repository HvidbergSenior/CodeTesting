namespace deftq.Pieceworks.Domain.Calculation.Expression
{
    public sealed class MultiplyExpression : IExpression
    {
        public IExpression A { get; private set; }
        public IExpression B { get; private set; }

        private MultiplyExpression(IExpression a, IExpression b)
        {
            A = a;
            B = b;
        }

        public static IExpression Multiply(IExpression a, IExpression b)
        {
            return new MultiplyExpression(a, b);
        }

        public static IExpression Multiply(IExpression a, IExpression b, IExpression c)
        {
            return new MultiplyExpression(new MultiplyExpression(a, b), c);
        }
        
        public static IExpression Multiply(IExpression a, IExpression b, IExpression c, IExpression d)
        {
            return new MultiplyExpression(new MultiplyExpression(a, b), new MultiplyExpression(c, d));
        }
        
        public DecimalNumber Evaluate()
        {
            return DecimalNumber.Number(A.Evaluate().Value * B.Evaluate().Value);
        }

        public bool IsCompound()
        {
            return true;
        }

        public string AsString()
        {
            var aStr = A.IsCompound() ? "(" + A.AsString() + ")" : A.AsString();
            var bStr = B.IsCompound() ? "(" + B.AsString() + ")" : B.AsString();
            return aStr + " * " + bStr;
        }
    }

}
