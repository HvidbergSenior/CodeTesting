using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Generators;
using deftq.BuildingBlocks.Application.IntegrationEvents;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.BuildingBlocks.Integration;
using deftq.BuildingBlocks.PipelineBehaviour;
using deftq.BuildingBlocks.Time;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace deftq.BuildingBlocks.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection UseBuildingBlocks(this IServiceCollection services)
        {
            services.AddTransient<ICommandBus, CommandBus>();
            services.AddTransient<IQueryBus, QueryBus>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IExecutionContext, HttpExecutionContext>();
            services.AddScoped<IEntityEventsPublisher, EntityEventsPublisher>();
            services.AddScoped<IIntegrationEventBroker, IntegrationEventBroker>();
            services.AddScoped<IUnitOfWork, MartenUnitOfWork>();

            services.AddSingleton<IIdGenerator<Guid>, SequentialGuidIdGenerator>();

            services.AddSingleton<IEnvironment, Environment>();
            services.AddSingleton<ISystemTime, SystemTime>();
            
            // Order of the pipeline registration is important.
            // Pipeline will be executed in the registered order.

            // Validation of the command / query is the first step in our pipeline
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Loggingbehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Validationbehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Authorizationbehaviour<,>));
            
            services.TryAddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();
            
            return services;
        }
    }
}
