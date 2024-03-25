using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderName : ValueObject
    {
        private const int MaxLength = 500;
        
        public String Value { get; private set; }
        
        private ProjectFolderName()
        {
            Value = string.Empty;
        }

        private ProjectFolderName(string value)
        {
            Value = value;
        }

        public static ProjectFolderName Create(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Length > MaxLength)
            {
                throw new ArgumentException($"Max folder name is {MaxLength} characters", nameof(value));
            }

            return new ProjectFolderName(value);
        }

        public static ProjectFolderName Empty()
        {
            return new ProjectFolderName(string.Empty);
        }
    }
}
