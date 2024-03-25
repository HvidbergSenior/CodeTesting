using deftq.BuildingBlocks.Application;
using deftq.UserAccess.Application.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace deftq.UserAccess.Infrastructure.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection UseUserAccess(this IServiceCollection services)
        {
            services.AddScoped<IIdentityResolver, IdentityResolver>();
            return services;
        }
    }
}
