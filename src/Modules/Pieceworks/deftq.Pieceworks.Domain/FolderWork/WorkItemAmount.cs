using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemAmount : ValueObject
    {
        public decimal Value { get; private set; }

        private WorkItemAmount()
        {
            Value = decimal.Zero;
        }

        private WorkItemAmount(decimal value)
        {
            Value = value;
        }

        public static WorkItemAmount Create(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Work item amount must be greater than or equal to 0");
            }
            return new WorkItemAmount(value);
        }

        public static WorkItemAmount Empty()
        {
            return new WorkItemAmount(Decimal.Zero);
        }
    }
}
