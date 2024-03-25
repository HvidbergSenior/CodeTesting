using deftq.BuildingBlocks.DataAccess.InitialData;
using deftq.BuildingBlocks.Serialization;
using Marten;
using Marten.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Weasel.Core;
using Weasel.Postgresql;

namespace deftq.BuildingBlocks.DataAccess.Marten
{
    public static class Registration
    {
        public static IServiceCollection AddMarten(this IServiceCollection services, IConfiguration config, bool addFullTextExtension = false, bool initializeData = false)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var martenConfig = config.GetSection(MartenConfig.DefaultConfigKey).Get<MartenConfig>();
            return AddMarten(services, martenConfig, addFullTextExtension, initializeData);
        }

        public static IServiceCollection AddMarten(this IServiceCollection services, MartenConfig martenConfig, bool addFullTextExtension = false, bool initializeData = false)
        {
            services.AddSingleton<IMartenConfig>(martenConfig);
            
            var serializer = GetJsonNetSerializer();

            var martenConfiguration = services.AddMarten(ops =>
            {
                ops.Connection(martenConfig.ConnectionString);
                ops.DatabaseSchemaName = martenConfig.SchemaName;
                ops.Serializer(serializer);

                if (addFullTextExtension)
                {
                    ops.Storage.ExtendedSchemaObjects.Add(new Extension("pg_trgm"));    
                }
                
                //ops.Logger(new ConsoleMartenLogger());
            });
            
            if (initializeData)
            {
                martenConfiguration.InitializeWith<DataInitializer>();    
            }

            if (addFullTextExtension)
            {
                martenConfiguration.ApplyAllDatabaseChangesOnStartup();
            }
            
            return services;
        }

        public static JsonNetSerializer GetJsonNetSerializer()
        {
            var serializer = new JsonNetSerializer
            {
                NonPublicMembersStorage = NonPublicMembersStorage.NonPublicSetters, 
                EnumStorage = EnumStorage.AsString,
            };

            serializer.Customize(_ =>
            {
                _.FloatParseHandling = FloatParseHandling.Decimal;
                _.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
                _.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                _.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                _.Converters.Add(new NewtonsoftDateOnlyJsonConverter());
            });

            return serializer;
        }
    }
}
