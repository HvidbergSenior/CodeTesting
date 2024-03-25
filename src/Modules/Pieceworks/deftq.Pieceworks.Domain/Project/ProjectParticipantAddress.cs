using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectParticipantAddress : ValueObject
    {
        public string Value { get; private set; }

        private ProjectParticipantAddress()
        {
            Value = string.Empty;
        }

        private ProjectParticipantAddress(string value)
        {
            Value = value;
        }

        public static ProjectParticipantAddress Create(string value)
        {
            return new ProjectParticipantAddress(value);
        }

        public static ProjectParticipantAddress Empty()
        {
            return new ProjectParticipantAddress(String.Empty);
        }
    }
}
