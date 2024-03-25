using System.Security.Claims;

namespace deftq.BuildingBlocks.Application
{
    public interface IIdentityResolver
    { 
        Guid ResolveUserId(Claim? providerId, Claim? userId, Claim? name);
        
        string ResolveUserName(Claim? providerId, Claim? userId, Claim? name);
    }
}
