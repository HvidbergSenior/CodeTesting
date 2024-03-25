using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectCreatedBy : ValueObject
    {
        public string Name { get; private set; }
        public Guid Id { get; private set; }

        private ProjectCreatedBy()
        {
            Name = String.Empty;
            Id = Guid.Empty;
        }

        private ProjectCreatedBy(string name, Guid id)
        {
            Name = name;
            Id = id;
        }

        public static ProjectCreatedBy Create(string name, Guid userId)
        {
            return new ProjectCreatedBy(name, userId);
        }

        public static ProjectCreatedBy Empty()
        {
            return new ProjectCreatedBy(String.Empty, Guid.Empty);
        }
    }
}