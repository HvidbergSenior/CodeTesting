using System.Reflection;
using deftq.BuildingBlocks.Integration;
using deftq.UserAccess.Application;
using deftq.UserAccess.Domain;
using deftq.UserAccess.Infrastructure;
using deftq.UserAccess.Integration;
using NetArchTest.Rules;
using Xunit;

namespace deftq.Tests.ArchTests.Modules
{
    public class ModuleTests : TestBase
    {
        [Fact]
        public void UserAccessModule_DoesNotHave_Dependency_On_Other_Modules()
        {
            var otherModules = new List<string>
            {
                PieceworksNamespace, CatalogNamespace, WebapplicationNamespace
            };
            List<Assembly> userAccessAssemblies = new List<Assembly>
            {
                typeof(ApplicationTarget).Assembly,
                typeof(DomainTarget).Assembly,
                typeof(InfrastructureTarget).Assembly,
                typeof(IntegrationTarget).Assembly
            };

            var result = Types.InAssemblies(userAccessAssemblies)
                .That()
                .DoNotImplementInterface(typeof(IIntegrationEventListener<>))
                .And().DoNotHaveNameEndingWith("IntegrationEventHandler", StringComparison.InvariantCulture)
                .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
                .GetResult();

            AssertArchTestResult(result);
        }
        
        [Fact]
        public void CatalogModule_DoesNotHave_Dependency_On_Other_Modules()
        {
            var otherModules = new List<string>
            {
                PieceworksNamespace, UserAccessNamespace, WebapplicationNamespace
            };
            List<Assembly> userAccessAssemblies = new List<Assembly>
            {
                typeof(Catalog.Application.ApplicationTarget).Assembly,
                typeof(Catalog.Domain.DomainTarget).Assembly,
                typeof(Catalog.Infrastructure.InfrastructureTarget).Assembly,
                typeof(Catalog.Integration.IntegrationTarget).Assembly
            };

            var result = Types.InAssemblies(userAccessAssemblies)
                .That()
                .DoNotImplementInterface(typeof(IIntegrationEventListener<>))
                .And().DoNotHaveNameEndingWith("IntegrationEventHandler", StringComparison.InvariantCulture)
                .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
                .GetResult();

            AssertArchTestResult(result);
        }
        
        [Fact]
        public void PieceworksModule_DoesNotHave_Dependency_On_Other_Modules()
        {
            var otherModules = new List<string>
            {
                CatalogNamespace, UserAccessNamespace, WebapplicationNamespace
            };
            List<Assembly> userAccessAssemblies = new List<Assembly>
            {
                //typeof(Pieceworks.Application.ApplicationTarget).Assembly,
                typeof(Pieceworks.Domain.DomainTarget).Assembly,
                typeof(Pieceworks.Infrastructure.InfrastructureTarget).Assembly,
                typeof(Pieceworks.Integration.IntegrationTarget).Assembly
            };

            var result = Types.InAssemblies(userAccessAssemblies)
                .That()
                .DoNotImplementInterface(typeof(IIntegrationEventListener<>))
                .And().DoNotHaveNameEndingWith("IntegrationEventHandler", StringComparison.InvariantCulture)
                .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
                .GetResult();

            AssertArchTestResult(result);
        }
    }
}
