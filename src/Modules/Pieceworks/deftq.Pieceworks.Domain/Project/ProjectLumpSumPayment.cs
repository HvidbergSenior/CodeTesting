using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectLumpSumPayment : ValueObject
    {
        public decimal Value { get; private set; }

        private ProjectLumpSumPayment()
        {
            Value = Decimal.Zero;
        }

        private ProjectLumpSumPayment(decimal value)
        {
            Value = value;
        }
        
        public static ProjectLumpSumPayment Create(decimal value)
        {
            return new ProjectLumpSumPayment(value);
        }

        public static ProjectLumpSumPayment Empty()
        {
            return new ProjectLumpSumPayment(decimal.Zero);
        }
        
        public bool IsEmpty()
        {
            return this == Empty();
        }
    }
}
