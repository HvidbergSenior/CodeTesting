using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectParticipantName : ValueObject
    {
        public string Value { get; private set; }

        private ProjectParticipantName()
        {
            Value = String.Empty;
        }

        private ProjectParticipantName(string value)
        {
            Value = value;
        }

        public static ProjectParticipantName Create(string value)
        {
            return new ProjectParticipantName(value);
        }

        public static ProjectParticipantName Empty()
        {
            return new ProjectParticipantName(String.Empty);
        }
    }
}
