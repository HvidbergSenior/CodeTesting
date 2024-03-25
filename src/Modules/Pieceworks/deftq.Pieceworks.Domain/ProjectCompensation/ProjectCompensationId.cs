using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectCompensation
{
    public sealed class ProjectCompensationId : ValueObject
    {
        public Guid Id { get; private set; }

        private ProjectCompensationId()
        {
            Id = Guid.Empty;
        }

        private ProjectCompensationId(Guid id)
        {
            Id = id;
        }

        public static ProjectCompensationId Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(id));
            }

            return new ProjectCompensationId(id);
        }
    }
}
