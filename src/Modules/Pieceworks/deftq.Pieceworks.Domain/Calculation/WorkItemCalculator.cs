using deftq.Pieceworks.Domain.Calculation.Expression;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.projectFolderRoot;
using static deftq.Pieceworks.Domain.Calculation.Expression.PercentageExpression;
using static deftq.Pieceworks.Domain.Calculation.Expression.SumExpression;
using static deftq.Pieceworks.Domain.Calculation.Expression.MultiplyExpression;
using static deftq.Pieceworks.Domain.Calculation.Expression.DecimalNumber;
using static deftq.Pieceworks.Domain.Calculation.Expression.DivideExpression;

namespace deftq.Pieceworks.Domain.Calculation
{
    public class WorkItemCalculator
    {
        private readonly BaseRateAndSupplementProxy _baseRateAndSupplementProxy;

        public WorkItemCalculator(BaseRateAndSupplementProxy baseRateAndSupplementProxy)
        {
            _baseRateAndSupplementProxy = baseRateAndSupplementProxy;
        }

        public TotalWorkTimeCalculationResult CalculateTotalOperationTime(WorkItem workItem)
        {
            var date = workItem.Date.Value;
            var operationTime = workItem.OperationTime.Value;
            var amount = workItem.Amount.Value;

            // Base rate and supplement
            var combinedSupplement = CalculateCombinedSupplementPercentage(date);

            var operationTimeMilliseconds = Number(operationTime, DecimalNumberUnit.Ms);
            var amountNumber = workItem.IsMaterial() && workItem.HasUnit()
                ? Number(amount, DecimalNumberUnit.Create(workItem.WorkItemMaterial!.Unit.Value))
                : Number(amount);

            // Work time
            var combinedSupplementMultiplication = Sum(Number(1), Number(combinedSupplement.Evaluate().Value));
            var totalWorkTimeMilliseconds = Multiply(operationTimeMilliseconds, amountNumber, combinedSupplementMultiplication);

            if (workItem.IsMaterial())
            {
                // Include material supplement operations
                totalWorkTimeMilliseconds =
                    IncludeSupplementOperations(workItem, totalWorkTimeMilliseconds, amountNumber, combinedSupplementMultiplication);
            }

            var effectiveSupplements = GetEffectiveSupplements(_baseRateAndSupplementProxy.FolderSupplements, workItem.Supplements);

            // Supplements
            if (effectiveSupplements.Count > 0 && amount > 0)
            {
                IExpression SupplementCalc(Supplement supplement) =>
                    Multiply(totalWorkTimeMilliseconds, Percentage(supplement.SupplementPercentage.Value));

                var supplementsSum = SupplementCalc(effectiveSupplements.First());

                foreach (var supplement in effectiveSupplements.Skip(1))
                {
                    supplementsSum = Sum(supplementsSum, SupplementCalc(supplement));
                }

                totalWorkTimeMilliseconds = Sum(totalWorkTimeMilliseconds, supplementsSum);
            }

            // Payment
            var baseRate = Number(_baseRateAndSupplementProxy.GetBaseRate(date), DecimalNumberUnit.Dkr);
            var baseRateRegulation = Percentage(_baseRateAndSupplementProxy.BaseRateRegulation);
            var numberUnit = DecimalNumberUnit.Create("ms/hour");
            var hours = Divide(Number(totalWorkTimeMilliseconds.Evaluate().Value, DecimalNumberUnit.Ms), Number(3600000, numberUnit));
            var totalPayment = Multiply(hours, baseRate, Sum(Number(1), baseRateRegulation));

            return new TotalWorkTimeCalculationResult(combinedSupplement, totalWorkTimeMilliseconds, totalPayment);
        }

        private IList<Supplement> GetEffectiveSupplements(IReadOnlyList<FolderSupplement> folderSupplements, IList<Supplement> workItemSupplements)
        {
            var supplementsFromFolder = folderSupplements.Select(folderSupplement => Supplement.Create(SupplementId.Empty(),
                folderSupplement.CatalogSupplementId, folderSupplement.SupplementNumber, folderSupplement.SupplementText, folderSupplement.SupplementPercentage));

            var result = new List<Supplement>();
            result.AddRange(supplementsFromFolder);
            foreach (var workItemSupplement in workItemSupplements)
            {
                if (!result.Any(supplement => supplement.CatalogSupplementId.Equals(workItemSupplement.CatalogSupplementId)))
                {
                    result.Add(workItemSupplement);
                }
            }

            return result;
        }

        public IExpression CalculateCombinedSupplementPercentage(DateOnly date)
        {
            var personalPercentage = Percentage(_baseRateAndSupplementProxy.GetPersonalTimeSupplement(date));
            var indirectPercentage = Percentage(_baseRateAndSupplementProxy.IndirectTimeSupplement);
            var siteSpecificPercentage = Percentage(_baseRateAndSupplementProxy.SiteSpecificTimeSupplement);

            var combinedSupplement = Sum(indirectPercentage, personalPercentage, Multiply(indirectPercentage, personalPercentage),
                siteSpecificPercentage);
            return combinedSupplement;
        }

        private static IExpression IncludeSupplementOperations(WorkItem workItem, IExpression totalWorkTimeMilliseconds, IExpression amountNumber,
            IExpression combinedSupplementMultiplication)
        {
            var workItemMaterial = workItem.WorkItemMaterial!;

            // Unit related supplement operations
            var unitRelatedOperations = workItemMaterial.SupplementOperations.Where(op => op.IsUnitRelated()).ToList();
            if (unitRelatedOperations.Count > 0)
            {
                IExpression UnitRelatedOperationCalc(SupplementOperation op) => Multiply(Number(op.OperationTime.Value, DecimalNumberUnit.Ms),
                    Number(op.OperationAmount.Value), amountNumber, combinedSupplementMultiplication);

                var unitRelatedSupplementOperationsSum = UnitRelatedOperationCalc(unitRelatedOperations.First());
                foreach (var operation in unitRelatedOperations.Skip(1))
                {
                    unitRelatedSupplementOperationsSum = Sum(unitRelatedSupplementOperationsSum, UnitRelatedOperationCalc(operation));
                }

                totalWorkTimeMilliseconds = Sum(totalWorkTimeMilliseconds, unitRelatedSupplementOperationsSum);
            }

            // Amount related supplement operations
            var amountRelatedOperations = workItemMaterial.SupplementOperations.Where(op => op.IsAmountRelated()).ToList();
            if (amountRelatedOperations.Count > 0 && amountNumber.Evaluate().Value > 0)
            {
                IExpression AmountRelatedOperationCalc(SupplementOperation op) =>
                    Multiply(Number(op.OperationTime.Value, DecimalNumberUnit.Ms), Number(op.OperationAmount.Value),
                        combinedSupplementMultiplication);

                var amountRelatedSupplementOperationsSum = AmountRelatedOperationCalc(amountRelatedOperations.First());
                foreach (var operation in amountRelatedOperations.Skip(1))
                {
                    amountRelatedSupplementOperationsSum = Sum(amountRelatedSupplementOperationsSum, AmountRelatedOperationCalc(operation));
                }

                totalWorkTimeMilliseconds = Sum(totalWorkTimeMilliseconds, amountRelatedSupplementOperationsSum);
            }

            return totalWorkTimeMilliseconds;
        }
    }

    public class TotalWorkTimeCalculationResult
    {
        public IExpression CombinedSupplementExpression { get; private set; }
        public IExpression TotalWorkTimeExpression { get; private set; }
        public IExpression TotalPaymentExpression { get; private set; }

        public TotalWorkTimeCalculationResult(IExpression combinedSupplementExpression, IExpression totalWorkTimeExpression,
            IExpression totalPaymentExpression)
        {
            CombinedSupplementExpression = combinedSupplementExpression;
            TotalWorkTimeExpression = totalWorkTimeExpression;
            TotalPaymentExpression = totalPaymentExpression;
        }
    }
}
