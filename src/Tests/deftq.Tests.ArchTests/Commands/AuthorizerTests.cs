using System.Reflection;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Catalog.Application.GetSupplements;
using deftq.Catalog.Application.Import.CreateMaterialImportPage;
using deftq.Catalog.Application.Import.GetMaterialImportStatus;
using deftq.Catalog.Application.Import.ImportMaterialCatalog;
using deftq.Pieceworks.Application;
using deftq.Pieceworks.Application.CreateProject;
using Mono.Cecil;
using NetArchTest.Rules;
using Xunit;

namespace deftq.Tests.ArchTests.Commands
{
    public class AuthorizerTests : TestBase
    {
        private static readonly IList<Assembly> Assemblies = new List<Assembly>
        {
            typeof(ApplicationTarget).Assembly,
            typeof(Catalog.Application.ApplicationTarget).Assembly,
            typeof(UserAccess.Application.ApplicationTarget).Assembly
        };

        private static readonly IList<Type> ExcludedClasses = new List<Type>
        {
            typeof(CreateProjectCommand),
            typeof(ImportMaterialCatalogCommand),
            typeof(CreateMaterialImportPageCommand),
            typeof(GetMaterialImportStatusQuery),
            typeof(GetSupplementsQuery)
        };

        [Fact]
        public void Commands_Should_Have_Authorizers()
        {
            var result = Types.InAssemblies(Assemblies)
                .That()
                .ImplementInterface(typeof(ICommand<>))
                .Should()
                .MeetCustomRule(new AuthorizerExistsRule())
                .GetResult();

            AssertArchTestResult(result);
        }

        [Fact]
        public void Queries_Should_Have_Authorizers()
        {
            var result = Types.InAssemblies(Assemblies)
                .That()
                .ImplementInterface(typeof(IQuery<>))
                .Should()
                .MeetCustomRule(new AuthorizerExistsRule())
                .GetResult();

            AssertArchTestResult(result);
        }

        private class AuthorizerExistsRule : ICustomRule
        {
            public bool MeetsRule(TypeDefinition type)
            {
                if (ExcludedClasses.Any(c => c.Name.Equals(type.Name, StringComparison.Ordinal)))
                {
                    return true;
                }

                var authorizers = Types.InAssemblies(Assemblies).That().ImplementInterface(typeof(IAuthorizer<>)).GetTypes().ToList();
                return authorizers.Any(t => t.Name.Equals(type.Name + "Authorizer", StringComparison.Ordinal));
            }
        }
    }
}
