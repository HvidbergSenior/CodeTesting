using System.Diagnostics.CodeAnalysis;
using deftq.BuildingBlocks.Domain;
using Xunit;

namespace deftq.BuildingBlocks.Test.Domain
{
    public class ValueObjectTests
    {
        internal sealed class Name : ValueObject
        {
            public string Value { get; init; }

            public Name(string value)
            {
                Value = value;
            }
        }

        internal sealed class Age : ValueObject
        {
            public int Value { get; init; }

            public Age(int value)
            {
                Value = value;
            }
        }

        internal sealed class Person : ValueObject
        {
            [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
            private Name Name { get; init; }

            [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
            private Age Age { get; init; }

            public Person(Name name, Age age)
            {
                Name = name;
                Age = age;
            }
        }

        [Fact]
        public void Will_Be_Equal_If_All_Values_Are_Equal()
        {
            var name = new Name(Guid.NewGuid().ToString());
            var age = new Age(1);
            var first = new Person(name, age);
            var second = new Person(name, age);

            Assert.Equal(first, second);
            Assert.False(first != second);
        }

        [Fact]
        public void Will_Be_Equal_If_Value_Is_Equal()
        {
            var nameInput = Guid.NewGuid().ToString();
            var first = new Name(nameInput);
            var second = new Name(nameInput);

            Assert.Equal(first.Value, second.Value);
            Assert.True(first.Equals(second));
            Assert.True(first == second);
            Assert.False(first != second);
        }

        [Fact]
        public void Will_Not_Be_Equal_If_Value_Is_Not_Equal()
        {
            var nameInput = Guid.NewGuid().ToString();
            var first = new Name(nameInput);
            var second = new Name(nameInput.ToUpperInvariant());
            Name? third = null;

            Assert.NotEqual(first.Value, second.Value);
            Assert.NotEqual(first, third);
            Assert.False(first.Equals(second));
            Assert.False(first == second);
            Assert.True(first != second);
        }
    }
}
