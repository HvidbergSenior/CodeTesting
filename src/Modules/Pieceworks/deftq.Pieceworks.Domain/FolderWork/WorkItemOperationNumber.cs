using System.Text.RegularExpressions;
using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemOperationNumber : ValueObject
    {
        public string Value { get; private set; }
        
        private WorkItemOperationNumber()
        {
            Value = String.Empty;
        }

        private WorkItemOperationNumber(string value)
        {
            Value = value;
        }
        
        public static WorkItemOperationNumber Create(string value)
        {
            return new WorkItemOperationNumber(value);
        }

        public static WorkItemOperationNumber Empty()
        {
            return new WorkItemOperationNumber(string.Empty);
        }
    }
}
