using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectPieceWorkNumber : ValueObject
    {
        public String Value { get; private set; }

        private ProjectPieceWorkNumber()
        {
            Value = string.Empty;
        }

        private ProjectPieceWorkNumber(string value)
        {
            Value = value;
        }

        public static ProjectPieceWorkNumber Create(string value)
        {
            if (value.Length > 15)
            {
                throw new ArgumentException("Value cant be longer than 15 chars", nameof(value));
            }

            return new ProjectPieceWorkNumber(value);
        }

        public static ProjectPieceWorkNumber Empty()
        {
            return new ProjectPieceWorkNumber(string.Empty);
        }
    }
}
