using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectId : ValueObject
    {
        public Guid Value { get; private set; }

        private ProjectId()
        {
            Value = Guid.Empty;
        }

        private ProjectId(Guid value)
        {
            Value = value;
        }

        public static ProjectId Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(id));
            }

            return new ProjectId(id);
        }

        public static ProjectId Empty()
        {
            return new ProjectId(Guid.Empty);
        }
    }
}
