using System.Text.RegularExpressions;
using Baseline;
using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project.Company
{
    public sealed class ProjectCompanyCvrNo : ValueObject
    {
        private readonly string regex = "^[0-9]{8}$";
        
        private const string EmptyCvrNo = "00000000";
        
        public string Value { get; private set; }

        private ProjectCompanyCvrNo()
        {
            Value = EmptyCvrNo;
        }

        private ProjectCompanyCvrNo(string value)
        {
            if (!value.IsEmpty() && !Regex.IsMatch(value, regex, RegexOptions.None, TimeSpan.FromMilliseconds(100)))
            {
                throw new ArgumentException("Invalid CVR number", nameof(value));
            }
            Value = value;
        }

        public static ProjectCompanyCvrNo Create(string value)
        {
            return new ProjectCompanyCvrNo(value);
        }

        public static ProjectCompanyCvrNo Empty()
        {
            return new ProjectCompanyCvrNo();
        }
    }
}
