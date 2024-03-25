using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace deftq.BuildingBlocks.Configuration
{
    public static class MediatorRegistrator
    {
        public static IServiceCollection AddMediatorHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            services.AddMediatR(assembly);
            return services;
        }
    }
}
