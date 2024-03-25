using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderCreatedBy : ValueObject
    {
        public string Name { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }

        private ProjectFolderCreatedBy()
        {
            Name = String.Empty;
            Timestamp = DateTimeOffset.MinValue;
        }

        private ProjectFolderCreatedBy(string name, DateTimeOffset timestamp)
        {
            Name = name;
            Timestamp = timestamp;
        }

        public static ProjectFolderCreatedBy Create(string name, DateTimeOffset timestamp)
        {
            return new ProjectFolderCreatedBy(name, timestamp);
        }

        public static ProjectFolderCreatedBy Empty()
        {
            return new ProjectFolderCreatedBy(String.Empty, DateTimeOffset.MinValue);
        }
    }
}