using System.Reflection;
using deftq.Catalog.Application;
using deftq.Catalog.Domain;
using deftq.Catalog.Infrastructure;
using NetArchTest.Rules;
using Xunit;

namespace deftq.Catalog.Test.Architecture
{
    public class TestBase
    {
        protected TestBase()
        {
        }

        protected static Assembly ApplicationAssembly => typeof(ApplicationTarget).Assembly;

        protected static Assembly DomainAssembly => typeof(DomainTarget).Assembly;

        protected static Assembly InfrastructureAssembly => typeof(InfrastructureTarget).Assembly;

        protected static void AssertAreImmutable(IEnumerable<Type> types)
        {
            IList<Type> failingTypes = new List<Type>();
            foreach (var type in types)
            {
                if (type.GetFields().Any(x => !x.IsInitOnly) || type.GetProperties().Any(x => x.CanWrite && x.PropertyType.IsNotPublic))
                {
                    failingTypes.Add(type);
                    break;
                }
            }

            AssertFailingTypes(failingTypes);
        }

        protected static void AssertFailingTypes(IEnumerable<Type> types)
        {
            if (types != null)
            {
                Assert.Empty(types);
            }
        }

        protected static void AssertAreSealed(IEnumerable<Type> types)
        {
            if (types is null)
            {
                throw new ArgumentNullException(nameof(types));
            }
            var failingTypes = new List<Type>();
            foreach (var item in types)
            {
                if (!item.IsSealed)
                {
                    failingTypes.Add(item);
                }
            }
            AssertFailingTypes(failingTypes);
        }

        protected static void AssertArchTestResult(TestResult result)
        {
            AssertFailingTypes(result.FailingTypes);
        }
    }
}
