using deftq.BuildingBlocks.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace deftq.BuildingBlocks.Configuration
{
    public static class AuthorizerRegistrator
    {
        public static void AddAuthorizersFromAssembly(
                    this IServiceCollection services,
                    Assembly assembly,
                    ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var authorizerType = typeof(IAuthorizer<>);
            assembly.GetTypesAssignableTo(authorizerType)?.ForEach((type) =>
            {
                foreach (var implementedInterface in type.ImplementedInterfaces)
                {
                    switch (lifetime)
                    {
                        case ServiceLifetime.Scoped:
                            services.AddScoped(implementedInterface, type);
                            break;
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(implementedInterface, type);
                            break;
                        case ServiceLifetime.Transient:
                            services.AddTransient(implementedInterface, type);
                            break;
                    }
                }
            });
        }

        private static List<TypeInfo>? GetTypesAssignableTo(this Assembly assembly, Type compareType)
        {
            return assembly.DefinedTypes.Where(x => x.IsClass
                                && !x.IsAbstract
                                && x != compareType
                                && x.GetInterfaces()
                                        .Any(i => i.IsGenericType
                                                && i.GetGenericTypeDefinition() == compareType))?.ToList();
        }
    }
}
