using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.InvitationFlow
{
    public sealed class InvitationExpiration : ValueObject
    {
        // Configurable perhaps?
        private static readonly DateTime DefaultInvitationValidity = DateTime.UtcNow.AddDays(3);
        public DateTime Value { get; private set; }

        private InvitationExpiration()
        {
            Value = DefaultInvitationValidity;
        }

        private InvitationExpiration(DateTime value)
        {
            Value = value;
        }

        public static InvitationExpiration Create(DateTime value)
        {
            return new InvitationExpiration(value);
        }

        public static InvitationExpiration Default()
        {
            return new InvitationExpiration(DefaultInvitationValidity);
        }
    }
}
