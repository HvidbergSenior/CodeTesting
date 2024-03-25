
using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectSpecificOperation
{
    public sealed class ProjectSpecificOperationTime : ValueObject
    {
        /// <summary>
        /// Operation time in milliseconds
        /// </summary>
        public decimal Value { get; private set; }
        
        private ProjectSpecificOperationTime()
        {
            Value = Decimal.Zero;
        }

        private ProjectSpecificOperationTime(decimal value)
        {
            Value = value;
        }

        public static ProjectSpecificOperationTime Create(decimal value)
        {
            if (value <= 0)
            {
                throw new ArgumentException("value can not be negative or 0", nameof(value));
            }
            return new ProjectSpecificOperationTime(value);
        }

        public static ProjectSpecificOperationTime Empty()
        {
            return new ProjectSpecificOperationTime(decimal.Zero);
        }

        public bool IsEmpty()
        {
            return Value == 0;
        }
    }
}
