using System.Globalization;

namespace deftq.Pieceworks.Domain.Calculation.Expression
{
    public interface IExpression
    {
        protected static readonly CultureInfo CultureInfo = CultureInfo.GetCultureInfo("da-DK");
    
        DecimalNumber Evaluate();
    
        bool IsCompound();
    
        string AsString();
    }
}
