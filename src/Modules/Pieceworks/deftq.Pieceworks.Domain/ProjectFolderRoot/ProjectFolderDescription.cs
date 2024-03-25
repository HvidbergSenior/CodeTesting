using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderDescription : ValueObject
    {
        public string Value { get; private set; }

        private ProjectFolderDescription()
        {
            Value = string.Empty;
        }

        private ProjectFolderDescription(string value)
        {
            Value = value;
        }

        public static ProjectFolderDescription Create(string value)
        {
            return new ProjectFolderDescription(value);
        }

        public static ProjectFolderDescription Empty()
        {
            return new ProjectFolderDescription(string.Empty);
        }
    }
}
