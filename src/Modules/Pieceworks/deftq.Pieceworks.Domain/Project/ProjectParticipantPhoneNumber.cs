using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectParticipantPhoneNumber : ValueObject
    {
        public string Value { get; private set; }
        
        private ProjectParticipantPhoneNumber()
        {
            Value = string.Empty;
        }

        private ProjectParticipantPhoneNumber(string value)
        {
            Value = value;
        }

        public static ProjectParticipantPhoneNumber Create(string value)
        {
            return new ProjectParticipantPhoneNumber(value);
        }

        public static ProjectParticipantPhoneNumber Empty()
        {
            return new ProjectParticipantPhoneNumber(String.Empty);
        }
    }
}
