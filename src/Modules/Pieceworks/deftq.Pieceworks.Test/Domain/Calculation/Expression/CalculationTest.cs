using Xunit;
using Xunit.Abstractions;
using static deftq.Pieceworks.Domain.Calculation.Expression.MultiplyExpression;
using static deftq.Pieceworks.Domain.Calculation.Expression.SumExpression;
using static deftq.Pieceworks.Domain.Calculation.Expression.DecimalNumber;
using static deftq.Pieceworks.Domain.Calculation.Expression.PercentageExpression;
using static deftq.Pieceworks.Domain.Calculation.Expression.DecimalNumberUnit;

namespace deftq.Pieceworks.Test.Domain.Calculation.Expression
{
    public class CalculationTest
    {
        private readonly ITestOutputHelper _output;

        public CalculationTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Sum_Numbers()
        {
            var expr = Multiply(Sum(Number(1.5m, Meter), Number(2.5m, Ms)), Percentage(200.5m));
            var res = expr.Evaluate();

            _output.WriteLine(expr.AsString() + " = " + res.Value);

            Assert.Equal(8.02m, res.Value);
        }
        
        [Fact]
        public void Sum_Multiple_Numbers()
        {
            var expr = Sum(Number(1.5m, Meter), Number(2.6m, Ms));
            var res = expr.Evaluate();
            _output.WriteLine(expr.AsString() + " = " + res.Value);
            Assert.Equal(4.1m, res.Value);
            
            expr = Sum(Number(1.5m, Meter), Number(2.5m, Ms), Number(10.7m));
            res = expr.Evaluate();
            _output.WriteLine(expr.AsString() + " = " + res.Value);
            Assert.Equal(14.7m, res.Value);
            
            expr = Sum(Number(1.5m, Meter), Number(2.5m, Ms), Number(10.7m), Number(6.7m));
            res = expr.Evaluate();
            _output.WriteLine(expr.AsString() + " = " + res.Value);
            Assert.Equal(21.4m, res.Value);
        }
        
        [Fact]
        public void Multiply_Multiple_Numbers()
        {
            var expr = Multiply(Number(1.5m, Meter), Number(2.6m, Ms));
            var res = expr.Evaluate();
            _output.WriteLine(expr.AsString() + " = " + res.Value);
            Assert.Equal(3.9m, res.Value);
            
            expr = Multiply(Number(1.5m, Meter), Number(2.5m, Ms), Number(10.7m));
            res = expr.Evaluate();
            _output.WriteLine(expr.AsString() + " = " + res.Value);
            Assert.Equal(40.125m, res.Value);
            
            expr = Multiply(Number(1.5m, Meter), Number(2.5m, Ms), Number(10.7m), Number(6.7m));
            res = expr.Evaluate();
            _output.WriteLine(expr.AsString() + " = " + res.Value);
            Assert.Equal(268.8375m, res.Value);
        }
    }
}
