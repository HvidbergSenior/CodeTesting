using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderRootId : ValueObject
    {
        public Guid Id { get; private set; }

        private ProjectFolderRootId()
        {
            Id = Guid.Empty;
        }

        private ProjectFolderRootId(Guid id)
        {
            Id = id;
        }

        public static ProjectFolderRootId Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(id));
            }

            return new ProjectFolderRootId(id);
        }

        public static ProjectFolderRootId Empty()
        {
            return new ProjectFolderRootId(Guid.Empty);
        }

    }
}