
using System.Security.Claims;
using System.Text;
using deftq.BuildingBlocks.Application;

namespace deftq.UserAccess.Application.Identity
{
    public class IdentityResolver : IIdentityResolver
    {
        public Guid ResolveUserId(Claim? providerId, Claim? userId, Claim? name)
        {
            if (userId != null)
            {
                try
                {
                    return new Guid(userId.Value);
                }
                catch (FormatException)
                {
                    // Fall-through
                }
                catch (OverflowException)
                {
                    // Fall-through
                }

                var userIdValue = userId.Value;
                var hashBytes = Encoding.ASCII.GetBytes(userIdValue);
                var guidBytes = new byte[16];
                Array.Copy(hashBytes, guidBytes, hashBytes.Length <= 16 ? hashBytes.Length : 16);
                return new Guid(guidBytes);
            }
            return Guid.Empty;
        }

        public string ResolveUserName(Claim? providerId, Claim? userId, Claim? name)
        {
            return name!.Value;
        }
    }
}
