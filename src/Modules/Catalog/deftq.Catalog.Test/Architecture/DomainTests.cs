using System.Reflection;
using deftq.BuildingBlocks.Domain;
using NetArchTest.Rules;
using Newtonsoft.Json;
using Xunit;

namespace deftq.Catalog.Test.Architecture
{
    public class DomainTests : TestBase
    {
        [Fact]
        public void ValueObject_Should_Be_Immutable()
        {
            var types = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(ValueObject))
                .GetTypes();

            AssertAreImmutable(types);
        }

        [Fact]
        public void ValueObject_Should_Be_Sealed()
        {
            var types = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(ValueObject))
                .GetTypes();

            AssertAreSealed(types);
        }

        [Fact]
        public void ValueObject_Should_Have_Private_Constructor_With_Parameters_For_Its_State()
        {
            var valueObjects = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(ValueObject)).GetTypes();

            var failingTypes = new List<Type>();
            foreach (var entityType in valueObjects)
            {
                var hasExpectedConstructor = false;

                const BindingFlags bindingFlags = BindingFlags.DeclaredOnly |
                                                  BindingFlags.Public |
                                                  BindingFlags.Instance;
                var names = entityType.GetFields(bindingFlags).Select(x => x.Name.ToUpperInvariant()).ToList();
                var propertyNames = entityType.GetProperties(bindingFlags).Select(x => x.Name.ToUpperInvariant()).ToList();
                names.AddRange(propertyNames);
                var constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var constructorInfo in constructors)
                {
                    var parameters = constructorInfo.GetParameters().Select(x => x.Name?.ToUpperInvariant()).ToList();

                    if (names.Intersect(parameters, StringComparer.Ordinal).Count() == names.Count)
                    {
                        hasExpectedConstructor = true;
                        break;
                    }
                }

                if (!hasExpectedConstructor)
                {
                    failingTypes.Add(entityType);
                }
            }

            AssertFailingTypes(failingTypes);
        }

        [Fact]
        public void ValueObjects_Should_Have_Parameterless_Private_Constructor()
        {
            var valueObjects = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(ValueObject)).GetTypes();

            var failingTypes = new List<Type>();
            foreach (var entityType in valueObjects)
            {
                var hasPrivateParameterlessConstructor = false;
                var constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var constructorInfo in constructors)
                {
                    if (constructorInfo.IsPrivate && constructorInfo.GetParameters().Length == 0)
                    {
                        hasPrivateParameterlessConstructor = true;
                    }
                }

                if (!hasPrivateParameterlessConstructor)
                {
                    failingTypes.Add(entityType);
                }
            }

            AssertFailingTypes(failingTypes);
        }

        [Fact]
        public void Aggregates_Should_Have_Parameterless_Private_Constructor()
        {
            var entityTypes = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(Entity))
                .GetTypes();

            var failingTypes = new List<Type>();
            foreach (var entityType in entityTypes)
            {
                var hasPrivateParameterlessConstructor = false;
                var constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var constructorInfo in constructors)
                {
                    if (constructorInfo.IsPrivate && constructorInfo.GetParameters().Length == 0)
                    {
                        hasPrivateParameterlessConstructor = true;
                    }
                }

                if (!hasPrivateParameterlessConstructor)
                {
                    failingTypes.Add(entityType);
                }
            }

            AssertFailingTypes(failingTypes);
        }

        [Fact]
        public void Aggregates_Should_Only_Have_Public_Properties()
        {
            var types = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(Entity))
                .GetTypes();

            const BindingFlags bindingFlags = BindingFlags.DeclaredOnly |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Instance |
                                              BindingFlags.Static;

            var failingTypes = new List<Type>();
            foreach (var type in types)
            {
                var publicProperties = type.GetProperties(bindingFlags);

                if (publicProperties.Length > 0)
                {
                    failingTypes.Add(type);
                }
            }

            AssertFailingTypes(failingTypes);
        }

        [Fact]
        public void Domain_Object_Should_Have_Only_Private_Constructors()
        {
            var domainObjectTypes = Types.InAssembly(DomainAssembly)
                .That()
                    .Inherit(typeof(Entity))
                .Or()
                        .Inherit(typeof(ValueObject))
                .GetTypes();

            var failingTypes = new List<Type>();
            foreach (var domainObjectType in domainObjectTypes)
            {
                var constructors = domainObjectType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                foreach (var constructorInfo in constructors)
                {
                    if (!constructorInfo.IsPrivate)
                    {
                        failingTypes.Add(domainObjectType);
                    }
                }
            }

            AssertFailingTypes(failingTypes);
        }

        [Fact]
        public void Domain_Object_Should_Have_Private_Setter_Propties()
        {
            var domainObjectTypes = Types.InAssembly(DomainAssembly)
               .That()
                   .Inherit(typeof(Entity))
               .Or()
                    .Inherit(typeof(ValueObject))

               .GetTypes();

            var failingTypes = new List<Type>();
            foreach (var item in domainObjectTypes)
            {
                var properties = item.GetProperties();
                foreach (var property in properties)
                {
                    if (property.GetGetMethod()!.IsStatic)
                    {
                        continue;
                    }
                    
                    var jsonIgnore = property.GetCustomAttribute(typeof(JsonIgnoreAttribute));
                    if (jsonIgnore == null)
                    {
                        var setter = property.GetSetMethod(nonPublic: true);
                        if (setter is null)
                        {
                            failingTypes.Add(item);
                        }
                    }
                }
            }
            AssertFailingTypes(failingTypes);
        }

        [Fact]
        public void DomainEvent_Should_Be_Immutable()
        {
            var types = Types.InAssembly(DomainAssembly)
                .That()
                    .Inherit(typeof(DomainEvent))
                        .Or()
                    .ImplementInterface(typeof(IDomainEvent))
                .GetTypes();

            AssertAreImmutable(types);
        }

        [Fact]
        public void DomainEvent_Should_Have_DomainEventPostfix()
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                    .Inherit(typeof(DomainEvent))
                .Or()
                    .ImplementInterface(typeof(IDomainEvent))
                .Should().HaveNameEndingWith("DomainEvent", StringComparison.Ordinal)
                .GetResult();

            AssertArchTestResult(result);
        }

        [Fact]
        public void DomainEvent_Should_Be_Sealed()
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                    .Inherit(typeof(DomainEvent))
                .Or()
                    .ImplementInterface(typeof(IDomainEvent))
                .Should().HaveNameEndingWith("DomainEvent", StringComparison.Ordinal)
                .GetTypes();

            AssertAreSealed(result);
        }
    }
}