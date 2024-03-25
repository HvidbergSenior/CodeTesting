using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using deftq.Akkordplus.WebApplication;
using NetArchTest.Rules;
using Xunit;

namespace deftq.Tests.ArchTests
{
    public abstract class TestBase
    {
        protected static Assembly AkkordPlus => typeof(ApplicationTarget).Assembly;

        public const string WebapplicationNamespace = "deftq.Akkordplus.WebApplication";

        public const string PieceworksNamespace = "deftq.Pieceworks";
        
        public const string CatalogNamespace = "deftq.Catalog";

        public const string UserAccessNamespace = "deftq.Modules.UserAccess";

        protected static void AssertAreImmutable(IEnumerable<Type> types)
        {
            IList<Type> failingTypes = new List<Type>();
            foreach (var type in types)
            {
                if (type.GetFields().Any(x => !x.IsInitOnly) || type.GetProperties().Any(x => x.CanWrite))
                {
                    failingTypes.Add(type);
                    break;
                }
            }

            AssertFailingTypes(failingTypes);
        }

        protected static void AssertFailingTypes(IEnumerable<Type> types)
        {
            if (types == null)
            {
                Assert.True(1 == 1);
            }
            else
            {
                Assert.Empty(types);
            }
        }

        protected static void AssertArchTestResult(TestResult result)
        {
            AssertFailingTypes(result.FailingTypes);
        }
    }
}
