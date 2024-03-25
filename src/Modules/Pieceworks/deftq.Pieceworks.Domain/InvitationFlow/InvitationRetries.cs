using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.InvitationFlow
{
    public sealed class InvitationRetries : ValueObject
    {
        public int Value { get; private set; }

        private InvitationRetries()
        {
            Value = 0;
        }

        private InvitationRetries(int value)
        {
            Value = value;
        }

        public static InvitationRetries Create(int value)
        {
            return new InvitationRetries(value);
        }

        public static InvitationRetries Empty()
        {
            return new InvitationRetries(0);
        }
    }
}
