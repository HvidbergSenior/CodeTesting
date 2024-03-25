using System.Text.RegularExpressions;
using Baseline;
using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project.Company
{
    public sealed class ProjectCompanyPNo : ValueObject
    {
        private readonly string regex = "^[0-9]{10}$";
        
        private const string EmptyPNo = "00000000";
        
        public string Value { get; private set; }

        private ProjectCompanyPNo()
        {
            Value = EmptyPNo;
        }

        private ProjectCompanyPNo(string value)
        {
            if (!value.IsEmpty() && !Regex.IsMatch(value, regex, RegexOptions.None, TimeSpan.FromMilliseconds(100)))
            {
                throw new ArgumentException("Invalid P number", nameof(value));
            }
            Value = value;
        }

        public static ProjectCompanyPNo Create(string value)
        {
            return new ProjectCompanyPNo(value);
        }

        public static ProjectCompanyPNo Empty()
        {
            return new ProjectCompanyPNo();
        }
    }
}
