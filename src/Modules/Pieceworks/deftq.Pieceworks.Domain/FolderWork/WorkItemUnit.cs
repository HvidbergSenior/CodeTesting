using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemUnit : ValueObject
    {
        public string Value { get; private set; }

        public static WorkItemUnit Meter { get; } = Create("meter");
        public static WorkItemUnit Piece { get; } = Create("stk");

        private WorkItemUnit()
        {
            Value = String.Empty;
        }
        
        private WorkItemUnit(string value)
        {
            Value = value;
        }

        public static WorkItemUnit Create(string value)
        {
            return new WorkItemUnit(value);
        }

        public static WorkItemUnit Empty()
        {
            return new WorkItemUnit("");
        }
    }
}
