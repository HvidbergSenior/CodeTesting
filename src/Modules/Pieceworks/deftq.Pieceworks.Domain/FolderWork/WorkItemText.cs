using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemText : ValueObject
    {
        public string Value { get; private set; }

        private WorkItemText()
        {
            Value = string.Empty;
        }

        private WorkItemText(string value)
        {
            Value = value;
        }

        public static WorkItemText Create(string value)
        {
            return new WorkItemText(value);
        }
        
        public static WorkItemText Empty()
        {
            return new WorkItemText(string.Empty);
        } 
    }
}
