using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectOwner : ValueObject
    {
        public string Name { get; private set; }
        public Guid Id { get; private set; }

        private ProjectOwner()
        {
            Name = String.Empty;
            Id = Guid.Empty;
        }

        private ProjectOwner(string name, Guid id)
        {
            Name = name;
            Id = id;
        }

        public static ProjectOwner Create(string name, Guid userId)
        {
            return new ProjectOwner(name, userId);
        }

        public static ProjectOwner Empty()
        {
            return new ProjectOwner(String.Empty, Guid.Empty);
        }
    }
}