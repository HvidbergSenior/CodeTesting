namespace deftq.Pieceworks.Domain
{
    public sealed class PersonalTimeSupplement
    {
        public Decimal Value { get; private set; }

        private PersonalTimeSupplement()
        {
            Value = Decimal.MinValue;
        }

        private PersonalTimeSupplement(Decimal value)
        {
            Value = value;
        }

        public static PersonalTimeSupplement Create(Decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Personal time supplement must be greater than or equal to 0");
            }
            return new PersonalTimeSupplement(value);
        }

        public static PersonalTimeSupplement Empty()
        {
            return new PersonalTimeSupplement(Decimal.Zero);
        }
    }
}
