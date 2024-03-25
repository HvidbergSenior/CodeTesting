using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemPayment : ValueObject
    {
        /// <summary>
        /// Payment in DKR
        /// </summary>
        public decimal Value { get; private set; }

        private WorkItemPayment()
        {
            Value = Decimal.Zero;
        }

        private WorkItemPayment(decimal value)
        {
            Value = value;
        }

        /// <summary>
        /// Create a new operation payment value object. 
        /// </summary>
        /// <param name="value">Operation payment in DKR</param>
        /// <returns></returns>
        public static WorkItemPayment Create(decimal value)
        {
            return new WorkItemPayment(value);
        }

        public static WorkItemPayment Empty()
        {
            return new WorkItemPayment(decimal.Zero);
        }
    }
}
