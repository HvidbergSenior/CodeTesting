using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.Time
{
    public sealed class OperationTime : ValueObject
    {
        public decimal Milliseconds { get; private set; }
        private static decimal MillisecondsPerMinute = 60000;

        private OperationTime()
        {
            Milliseconds = Decimal.Zero;
        }

        private OperationTime(decimal milliseconds)
        {
            Milliseconds = milliseconds;
        }

        public static OperationTime Create(decimal milliseconds)
        {
            return new OperationTime(milliseconds);
        }

        public static OperationTime FromOneThousandthMinutes(decimal oneThousandthMinutes)
        {
            return new OperationTime((oneThousandthMinutes / 1000) * MillisecondsPerMinute);
        }
        
        public static OperationTime Empty()
        {
            return new OperationTime(0);
        }
        
        public TimeSpan AsTimeSpan()
        {
            return new TimeSpan(0,0,0,0,  Decimal.ToInt32(Milliseconds));
        }
    }
}
