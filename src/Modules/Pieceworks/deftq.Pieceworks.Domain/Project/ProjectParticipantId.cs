using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectParticipantId : ValueObject
    {
        public Guid Value { get; private set; }

        private ProjectParticipantId()
        {
            Value = Guid.Empty;
        }

        private ProjectParticipantId(Guid value)
        {
            Value = value;
        }

        public static ProjectParticipantId Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(id));
            }

            return new ProjectParticipantId(id);
        }

        public static ProjectParticipantId Empty()
        {
            return new ProjectParticipantId(Guid.Empty);
        }
    }
}
