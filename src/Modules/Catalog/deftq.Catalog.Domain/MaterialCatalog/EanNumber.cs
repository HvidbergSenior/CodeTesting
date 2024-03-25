using System.Text.RegularExpressions;
using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.MaterialCatalog
{
    public sealed class EanNumber : ValueObject
    {
        public string Value { get; private set; }
        
        private readonly string regex = "^[0-9]{13}$";
        
        private const string EmptyEan = "0000000000000";

        private EanNumber()
        {
            Value = EmptyEan;
        }

        private EanNumber(string value)
        {
            if (!Regex.IsMatch(value, regex, RegexOptions.None, TimeSpan.FromMilliseconds(100)))
            {
                throw new ArgumentException("Invalid EAN number", nameof(value));
            }
            Value = value;
        }

        public static EanNumber Create(string value)
        {
            return new EanNumber(value);
        }
        
        public static EanNumber Empty()
        {
            return new EanNumber(EmptyEan);
        }
    }
}
