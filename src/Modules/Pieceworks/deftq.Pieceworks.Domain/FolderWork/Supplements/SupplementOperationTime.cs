using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork.Supplements
{
    public sealed class SupplementOperationTime : ValueObject
    {
        /// <summary>
        /// Operation time in milliseconds
        /// </summary>
        public decimal Value { get; private set; }

        private SupplementOperationTime()
        {
            Value = Decimal.Zero;
        }

        private SupplementOperationTime(decimal value)
        {
            Value = value;
        }

        /// <summary>
        /// Create a new operation time value object. 
        /// </summary>
        /// <param name="value">Operation time in milliseconds</param>
        public static SupplementOperationTime Create(decimal value)
        {
            return new SupplementOperationTime(value);
        }

        /// <summary>
        /// An operation time of 0 milliseconds
        /// </summary>
        public static SupplementOperationTime Empty()
        {
            return new SupplementOperationTime(decimal.Zero);
        }

        public TimeSpan AsTimeSpan()
        {
            return TimeSpan.FromMilliseconds(decimal.ToInt64(Value));
        }
    }
}
