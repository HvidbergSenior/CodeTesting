using deftq.BuildingBlocks.Configuration;
using deftq.BuildingBlocks.DataAccess.InitialData;
using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.Catalog.Application;
using deftq.Catalog.Domain;
using deftq.Catalog.Domain.MaterialCatalog;
using deftq.Catalog.Domain.MaterialImport;
using deftq.Catalog.Domain.OperationCatalog;
using deftq.Catalog.Domain.SupplementCatalog;
using deftq.Catalog.Infrastructure.DemoData;
using FluentValidation;
using Marten;
using Marten.Schema;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Postgresql.Tables;

namespace deftq.Catalog.Infrastructure.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection UseCatalog(this IServiceCollection services)
        {
            services.AddMediatorHandlersFromAssembly(typeof(ApplicationTarget).Assembly);
            services.AddMediatorHandlersFromAssembly(typeof(DomainTarget).Assembly);
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<IOperationRepository, OperationRepository>();
            services.AddScoped<ISupplementRepository, SupplementRepository>();
            services.AddScoped<IMaterialImportPageRepository, MaterialImportPageRepository>();
            services.AddScoped<IImportRepository, ImportRepository>();
            services.AddValidatorsFromAssemblyContaining<ApplicationTarget>(includeInternalTypes: true);
            services.AddAuthorizersFromAssembly(typeof(ApplicationTarget).Assembly);
            services.AddSingleton<IConfigureMarten>(new ConfigureMarten());
            services.AddSingleton<IDemoDataProvider>(new DemoMaterials());
            services.AddSingleton<IDemoDataProvider>(new DemoSupplements());
            services.AddSingleton<IDemoDataProvider>(new DemoOperations());
            return services;
        }
    }

    internal class ConfigureMarten : IConfigureMarten
    {
        private string _ginTrgmOpsMask = "? gin_trgm_ops";

        public void Configure(IServiceProvider services, StoreOptions options)
        {
            var martenConfig = services.GetService<IMartenConfig>()!;
            
            // Materials catalog
            options.Schema.For<Material>().FullTextIndex(martenConfig.FullTextSearchLanguage,m => m.Name.Value);
            options.Schema.For<Material>().UniqueIndex(UniqueIndexType.Computed, m => m.EanNumber.Value);
            options.Schema.For<Material>().Index(m => m.EanNumber.Value, c =>
            {
                c.Method = IndexMethod.gin;
                c.Mask = _ginTrgmOpsMask;
            });
            options.Schema.For<Material>().Index(m => m.Name.Value, c =>
            {
                c.Method = IndexMethod.gin;
                c.Mask = _ginTrgmOpsMask;
            });
            
            // Operations catalog
            options.Schema.For<Operation>().FullTextIndex(martenConfig.FullTextSearchLanguage,o => o.OperationText.Value);
            options.Schema.For<Operation>().Index(o => o.OperationText.Value, c =>
            {
                c.Method = IndexMethod.gin;
                c.Mask = _ginTrgmOpsMask;
            });
            options.Schema.For<Operation>().Index(m => m.OperationNumber.Value, c =>
            {
                c.Method = IndexMethod.gin;
                c.Mask = _ginTrgmOpsMask;
            });
            
            // Material import page
            options.Schema.For<MaterialImportPage>().UniqueIndex(m => m.Published, c => c.StartRow);
        }
    }
}
