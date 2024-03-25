using System.Reflection;
using deftq.Akkordplus.WebApplication;
using deftq.Akkordplus.WebApplication.Controllers;
using Microsoft.AspNetCore.Authorization;
using NetArchTest.Rules;
using Xunit;

namespace deftq.Tests.ArchTests.Controllers
{
    public class ControllerTests : TestBase
    {
        [Fact]
        public void WebApplication_Controllers_ShouldRequireAuthentication()
        {
            List<Assembly> webApplicationAssemblies = new List<Assembly>
            {
                typeof(ApplicationTarget).Assembly
            };

            var result = Types.InAssemblies(webApplicationAssemblies)
                .That()
                .Inherit(typeof(ApiControllerBase))
                .Should()
                .HaveCustomAttribute(typeof(AuthorizeAttribute))
                .GetResult();

            AssertArchTestResult(result);
        }
    }
}
