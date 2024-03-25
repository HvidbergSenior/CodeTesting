using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderLock : ValueObject
    {
        public string Value { get; private set;}
        
        private ProjectFolderLock()
        {
            Value = String.Empty;
        }

        private ProjectFolderLock(string value)
        {
            Value = value;
        }

        private static ProjectFolderLock Create(string value)
        {
            return new ProjectFolderLock(value);
        }

        public static ProjectFolderLock Empty()
        {
            return new ProjectFolderLock(String.Empty);
        }

        public static ProjectFolderLock Locked()
        {
            return Create("locked");
        }

        public static ProjectFolderLock Unlocked()
        {
            return Create("unlocked");
        }

        public bool IsLocked()
        {
            return this == Locked();
        }
    }
}
