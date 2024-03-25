using System.Security.Cryptography;
using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.InvitationFlow
{
    public sealed class InvitationRandomValue : ValueObject
    {
        public string Value { get; private set; }

        private InvitationRandomValue()
        {
            Value = string.Empty;
        }

        private InvitationRandomValue(string value)
        {
            Value = value;
        }

        public static InvitationRandomValue Create(string value)
        {
            return new InvitationRandomValue(value);
        }

        public static InvitationRandomValue NewValue()
        {
            return new InvitationRandomValue(CreateRandomStringValue(20));
        }

        private static string CreateRandomStringValue(int length = 20)
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, length);
        }
    }
}
