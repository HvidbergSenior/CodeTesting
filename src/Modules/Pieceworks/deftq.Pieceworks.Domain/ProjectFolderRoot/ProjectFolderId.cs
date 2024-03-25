using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderId : ValueObject
    {
        public Guid Value { get; private set; }

        private ProjectFolderId()
        {
            Value = Guid.Empty;
        }

        private ProjectFolderId(Guid value)
        {
            Value = value;
        }

        public static ProjectFolderId Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(id));
            }

            return new ProjectFolderId(id);
        }

        public static ProjectFolderId Empty()
        {
            return new ProjectFolderId(Guid.Empty);
        }
    }
}
