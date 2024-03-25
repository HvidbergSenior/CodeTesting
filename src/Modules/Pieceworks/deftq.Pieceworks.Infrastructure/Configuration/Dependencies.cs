using Baseline;
using deftq.BuildingBlocks.Configuration;
using deftq.BuildingBlocks.DataAccess.InitialData;
using deftq.Pieceworks.Application;
using deftq.Pieceworks.Application.CreateProject;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.InvitationFlow;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Domain.projectFolder;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Domain.projectSpecificOperation;
using deftq.Pieceworks.Domain.projectUser;
using deftq.Pieceworks.Infrastructure.DemoData;
using deftq.Pieceworks.Infrastructure.projectDocument;
using FluentValidation;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace deftq.Pieceworks.Infrastructure.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection UsePieceworks(this IServiceCollection services, IHostEnvironment environment, IConfiguration config)
        {
            //services.AddMediatorHandlersFromAssembly(typeof(ApplicationTarget).Assembly);
            services.AddMediatorHandlersFromAssembly(typeof(DomainTarget).Assembly);
            services.TryAddScoped<IProjectRepository, ProjectRepository>();
            services.TryAddScoped<IProjectUserRepository, ProjectUserRepository>();
            services.AddScoped<IProjectFolderRootRepository, ProjectFolderRootRepository>();
            services.AddScoped<IProjectExtraWorkAgreementListRepository, ProjectExtraWorkAgreementListRepository>();
            services.AddScoped<IProjectFolderWorkRepository, ProjectFolderWorkRepository>();
            services.AddScoped<IProjectLogBookRepository, ProjectLogBookRepository>();
            services.AddScoped<IProjectDocumentRepository, ProjectDocumentRepository>();
            services.AddScoped<IProjectSpecificOperationListRepository, ProjectSpecificOperationListRepository>();
            services.AddScoped<IBaseRateAndSupplementRepository, BaseRateAndSupplementRepository>();
            services.AddScoped<IProjectCatalogFavoriteListRepository, ProjectCatalogFavoriteListRepository>();
            services.AddScoped<IProjectCompensationListRepository, ProjectCompensationListRepository>();
           // services.AddValidatorsFromAssemblyContaining<ApplicationTarget>(includeInternalTypes: true);
            //services.AddAuthorizersFromAssembly(typeof(ApplicationTarget).Assembly);
            services.AddSingleton<IDemoDataProvider>(new DemoProjects());
            services.AddSingleton<IConfigureMarten>(new ConfigureMarten());
            services.AddTransient<IProjectNumberGenerator, SequenceProjectNumberGenerator>();
            services.AddScoped<IInvitationRepository, InvitationRepository>();

            var docStorageConnectionString = config.GetSection("DocumentStorage")?["StorageAccountConnectionString"] ?? "";
            if (!environment.IsDevelopment() && docStorageConnectionString.IsNotEmpty())
            {
                services.AddScoped<IFileStorage>(sp => new BlobFileStorage(docStorageConnectionString));
            }
            else
            {
                services.AddScoped<IFileStorage, FilesystemFileStorage>();
            }

            return services;
        }
    }

    public class ConfigureMarten : IConfigureMarten
    {
        public void Configure(IServiceProvider services, StoreOptions options)
        {
            options.Schema.For<ProjectFolderRoot>().UniqueIndex("unique_index_folderroot_project", x => x.ProjectId.Value);
            options.Schema.For<ProjectExtraWorkAgreement>().UniqueIndex("unique_index_extraworkagreement_project", x => x.ProjectId.Value);
            options.Schema.For<ProjectFolderWork>()
                .UniqueIndex("unique_index_folderwork_folder", x => x.ProjectId.Value, x => x.ProjectFolderId.Value);
            options.Schema.For<ProjectLogBook>().UniqueIndex("unique_index_logbook_project", x => x.ProjectId.Value);
            options.Schema.For<ProjectCatalogFavoriteList>().UniqueIndex("unique_index_projectcatalogfavoritelist_project", x => x.ProjectId.Value);
            options.Schema.For<ProjectCompensationList>().UniqueIndex("unique_index_projectcompensationlist_project", x => x.ProjectId.Value);
            options.Schema.For<ProjectSpecificOperationList>()
                .UniqueIndex("unique_index_projectspecificoperationlist_project", x => x.ProjectId.Value);
        }
    }
}
