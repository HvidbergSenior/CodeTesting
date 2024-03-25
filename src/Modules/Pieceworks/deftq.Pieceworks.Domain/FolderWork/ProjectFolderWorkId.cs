using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class ProjectFolderWorkId : ValueObject
    {
        public Guid Value { get; private set; }

        private ProjectFolderWorkId()
        {
            Value = Guid.Empty;
        }

        private ProjectFolderWorkId(Guid value)
        {
            Value = value;
        }

        public static ProjectFolderWorkId Create(Guid value)
        {
            return new ProjectFolderWorkId(value);
        }

        public static ProjectFolderWorkId Empty()
        {
            return new ProjectFolderWorkId(Guid.Empty);
        }
    }
}
