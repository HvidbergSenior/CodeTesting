using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.InvitationFlow
{
    public sealed class InvitationId : ValueObject
    {
        public Guid Value { get; private set; }

        private InvitationId()
        {
            Value = Guid.Empty;
        }

        private InvitationId(Guid value)
        {
            Value = value;
        }

        public static InvitationId Create(Guid value)
        {
            return new InvitationId(value);
        }

        public static InvitationId Empty()
        {
            return new InvitationId(Guid.Empty);
        }
    }
}
