using deftq.BuildingBlocks.Fakes;
using Xunit;

namespace deftq.BuildingBlocks.Test.Infrastructure
{
    public class InMemoryRepositoryTests
    {
        [Fact]
        public async Task Can_Save_EntityAsync()
        {
            var car = Car.Create();
            var repository = new InMemoryRepository<Car>();
            
            await repository.Add(car);

            Assert.Single(repository.Entities);
        }

        [Fact]
        public async Task Cant_Add_Duplicate()
        {
            var car = Car.Create();
            var repository = new InMemoryRepository<Car>();
            await repository.Add(car);

            await Assert.ThrowsAsync<ArgumentException>(async () => await repository.Add(car));
        }

        [Fact]
        public async Task Can_Find_EntityAsync()
        {
            var car = Car.Create();
            var carId = car.Id;
            var repository = new InMemoryRepository<Car>();

            await repository.Add(car);

            var foundCar = await repository.FindById(carId);
            Assert.Equal(carId, foundCar?.Id);
        }
    }
}
