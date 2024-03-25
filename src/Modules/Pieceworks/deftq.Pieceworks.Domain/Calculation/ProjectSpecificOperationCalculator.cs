using deftq.Pieceworks.Domain.Calculation.Expression;
using static deftq.Pieceworks.Domain.Calculation.Expression.PercentageExpression;
using static deftq.Pieceworks.Domain.Calculation.Expression.SumExpression;
using static deftq.Pieceworks.Domain.Calculation.Expression.MultiplyExpression;
using static deftq.Pieceworks.Domain.Calculation.Expression.DecimalNumber;
using static deftq.Pieceworks.Domain.Calculation.Expression.DivideExpression;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Domain.projectSpecificOperation;

namespace deftq.Pieceworks.Domain.Calculation
{
    public class ProjectSpecificOperationCalculator
    {
        private readonly BaseRateAndSupplementProxy _baseRateAndSupplementProxy;
        private readonly ISystemTime _systemTime;

        public ProjectSpecificOperationCalculator(BaseRateAndSupplementProxy baseRateAndSupplementProxy, ISystemTime systemTime)
        {
            _baseRateAndSupplementProxy = baseRateAndSupplementProxy;
            _systemTime = systemTime;
        }

        public ProjectSpecificOperationTime CalculateOperationTime(ProjectSpecificOperationTime workingTime)
        {
            var workTime = Number(workingTime.Value, DecimalNumberUnit.Ms);
            
            // Base rate and supplement
            var combinedSupplement = CalculateCombinedSupplementPercentage();
            var combinedSupplementMultiplication = Sum(Number(1), Number(combinedSupplement.Evaluate().Value));
            var totalOperationTimeMilliseconds = Divide( workTime, combinedSupplementMultiplication);
            return ProjectSpecificOperationTime.Create(totalOperationTimeMilliseconds.Evaluate().Value);
        }
        
        public IExpression CalculateCombinedSupplementPercentage()
        {
            var personalPercentage = Percentage(_baseRateAndSupplementProxy.GetPersonalTimeSupplement(_systemTime.Today()));
            var indirectPercentage = Percentage(_baseRateAndSupplementProxy.IndirectTimeSupplement);
            var siteSpecificPercentage = Percentage(_baseRateAndSupplementProxy.SiteSpecificTimeSupplement);

            var combinedSupplement = Sum(indirectPercentage, personalPercentage, Multiply(indirectPercentage, personalPercentage),
                siteSpecificPercentage);
            return combinedSupplement;
        }

        public CalculateOperationPaymentResult CalculateOperationPayment(decimal operationTimeMs)
        {
            if (operationTimeMs == 0)
            {
                return new CalculateOperationPaymentResult(Number(0, DecimalNumberUnit.Dkr));
            }
            
            var personalPercentage = Percentage(_baseRateAndSupplementProxy.GetPersonalTimeSupplement(_systemTime.Today()));
            var indirectPercentage = Percentage(_baseRateAndSupplementProxy.IndirectTimeSupplement);
            var siteSpecificPercentage = Percentage(_baseRateAndSupplementProxy.SiteSpecificTimeSupplement);

            var combinedSupplement = Sum(indirectPercentage, personalPercentage, Multiply(indirectPercentage, personalPercentage),
                siteSpecificPercentage);
            
            var combinedSupplementMultiplication = Sum(Number(1), Number(combinedSupplement.Evaluate().Value));
            
            var baseRate = Number(_baseRateAndSupplementProxy.GetBaseRate(_systemTime.Today()), DecimalNumberUnit.Dkr);
            var baseRateRegulation = Percentage(_baseRateAndSupplementProxy.BaseRateRegulation);
            var numberUnit = DecimalNumberUnit.Create("ms/hour");
            var totalWorkTimeMilliseconds = Multiply(Number(operationTimeMs, DecimalNumberUnit.Ms), combinedSupplementMultiplication);
            var hours = Divide(totalWorkTimeMilliseconds, Number(3600000, numberUnit));
            return new CalculateOperationPaymentResult(Multiply(hours, baseRate, Sum(Number(1), baseRateRegulation)));
        }
    }

    public class CalculateOperationPaymentResult
    {
        public IExpression Payment { get; private set; }

        public CalculateOperationPaymentResult(IExpression payment)
        {
            Payment = payment;
        }
    }
}
