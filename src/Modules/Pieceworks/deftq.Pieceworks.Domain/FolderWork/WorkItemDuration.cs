using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemDuration : ValueObject
    {
        /// <summary>
        /// Operation time in milliseconds
        /// </summary>
        public decimal Value { get; private set; }

        private WorkItemDuration()
        {
            Value = Decimal.Zero;
        }

        private WorkItemDuration(decimal value)
        {
            Value = value;
        }

        /// <summary>
        /// Create a new operation time value object. 
        /// </summary>
        /// <param name="value">Operation time in milliseconds</param>
        public static WorkItemDuration Create(decimal value)
        {
            return new WorkItemDuration(value);
        }

        /// <summary>
        /// An operation time of 0 milliseconds
        /// </summary>
        public static WorkItemDuration Empty()
        {
            return new WorkItemDuration(decimal.Zero);
        }

        public TimeSpan AsTimeSpan()
        {
            return TimeSpan.FromMilliseconds(decimal.ToInt64(Value));
        }
    }
}
