using deftq.BuildingBlocks.Domain;
using Xunit;

namespace deftq.BuildingBlocks.Test.Domain
{
    internal sealed class CarDoorId : ValueObject
    {
        public Guid Value { get; }

        public CarDoorId(Guid value)
        {
            Value = value;
        }
    }

    internal sealed class CarDoor : Entity
    {
        public CarDoorId CarDoorId { get; }

        public CarDoor(CarDoorId doorId)
        {
            CarDoorId = doorId;
            Id = CarDoorId.Value;
        }
    }

    public class EntityTests
    {
        [Fact]
        public void Will_Be_Equal_If_Id_Is_Equal()
        {
            var doorId = new CarDoorId(Guid.NewGuid());

            var first = new CarDoor(doorId);
            var second = new CarDoor(doorId);

            Assert.Equal(first, second);
            Assert.True(first.Equals(second));
            Assert.True(Equals(first, second));
            Assert.True(first == second);
        }

        [Fact]
        public void Will_Not_Be_Equal_If_Id_Is_Equal()
        {
            var first = new CarDoor(new CarDoorId(Guid.NewGuid()));
            var second = new CarDoor(new CarDoorId(Guid.NewGuid()));
         
            Assert.NotEqual(first, second);
            Assert.False(first.Equals(second));
            Assert.False(Equals(first, second));
            Assert.False(first == second);
            Assert.True(first != second);
        }
    }
}
