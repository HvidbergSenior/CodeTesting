using System.Text.RegularExpressions;
using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemEanNumber : ValueObject
    {
        public string Value { get; private set; }

        private readonly string regex = "^[0-9]{13}$";

        private const string EmptyEan = "0000000000000";

        private WorkItemEanNumber()
        {
            Value = EmptyEan;
        }
        

        private WorkItemEanNumber(string value)
        {
            if (!Regex.IsMatch(value, regex, RegexOptions.None, TimeSpan.FromMilliseconds(100)))
            {
                throw new ArgumentException("Invalid EAN number", nameof(value));
            }

            Value = value;
        }
        
        public static WorkItemEanNumber Create(string value)
        {
            return new WorkItemEanNumber(value);
        }

        public static WorkItemEanNumber Empty()
        {
            return new WorkItemEanNumber(EmptyEan);
        }
    }
}
