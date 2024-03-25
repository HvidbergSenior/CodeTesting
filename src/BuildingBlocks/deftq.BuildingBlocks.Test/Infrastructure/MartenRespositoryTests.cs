using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Domain;
using deftq.BuildingBlocks.Fakes;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Marten;
using Marten.Services;
using Newtonsoft.Json;
using Weasel.Core;
using Xunit;

namespace deftq.BuildingBlocks.Test.Infrastructure
{
    public sealed class CarId : ValueObject
    {
        public Guid Value { get; }

        private CarId(Guid value)
        {
            Value = value;
        }

        public static CarId Create(Guid value)
        {
            return new CarId(value);
        }
    }

    public sealed class Car : Entity
    {
        public CarId CarId { get; }
        public Owner Owner { get; }

        private Car()
        {
            CarId = CarId.Create(Guid.Empty);
            Owner = Owner.Create();
        }

        private Car(CarId carId, Owner owner)
        {
            CarId = carId;
            Id = CarId.Value;
            Owner = owner;
        }
        
        public static Car Create()
        {
            var owner = Owner.Create();
            return new Car(CarId.Create(Guid.NewGuid()), owner);
        }
    }

    public sealed class OwnerId : ValueObject
    {
        public Guid Value { get; }

        private OwnerId(Guid value)
        {
            Value = value;
        }

        public static OwnerId Create(Guid value)
        {
            return new OwnerId(value);
        }
    }

    public sealed class Owner : Entity
    {
        public OwnerId OwnerId { get; }
        
        private Owner(OwnerId ownerId)
        {
            OwnerId = ownerId;
            Id = ownerId.Value;
        }

        public static Owner Create()
        {
            return new Owner(OwnerId.Create(Guid.NewGuid()));
        }
    }

    public class MartenRespositoryTests : IAsyncLifetime
    {
        private readonly TestcontainerDatabase testcontainers = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "db",
            Username = "postgres",
            Password = "postgres",
        })
        .Build();

        [Fact]
        public async Task Can_Save_AggregatesAsync()
        {
            var eventPublisher = new FakeEntityEventsPublisher();
            var options = new StoreOptions();
            var serializer = new JsonNetSerializer
            {
                NonPublicMembersStorage = NonPublicMembersStorage.NonPublicSetters,
                EnumStorage = EnumStorage.AsString,
            };

            serializer.Customize(_ =>
            {
                _.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
                _.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            });
            options.Serializer(serializer);
            options.Connection(testcontainers.ConnectionString);
            var store = new DocumentStore(options);

            using (var session = store.OpenSession())
            {
                var repository = new MartenDocumentRepository<Car>(session, eventPublisher);
                var car = Car.Create();
                await repository.Add(car);
                await repository.SaveChanges();
                await session.SaveChangesAsync();
            }
            store.Dispose();
            Assert.True(eventPublisher.HasBeenCalled);
        }

        [Fact]
        public async Task Can_Load_AggregatesAsync()
        {
            var car = Car.Create();
            var options = new StoreOptions();
            var serializer = new JsonNetSerializer
            {
                NonPublicMembersStorage = NonPublicMembersStorage.NonPublicSetters,
                EnumStorage = EnumStorage.AsString,
            };

            serializer.Customize(_ =>
            {
                _.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
                _.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            });
            options.Serializer(serializer);
            options.Connection(testcontainers.ConnectionString);
            var store = new DocumentStore(options);

            using (var writeSession = store.OpenSession())
            {
                var repository = new MartenDocumentRepository<Car>(writeSession, new FakeEntityEventsPublisher());
                await repository.Add(car);
                await writeSession.SaveChangesAsync();
            }
            Car carFromDatabase;
            using (var readSession = store.OpenSession())
            {
                var readRepository = new MartenDocumentRepository<Car>(readSession, new FakeEntityEventsPublisher());
                carFromDatabase = await readRepository.GetById(car.Id);
                await readSession.DisposeAsync();
            }
            store.Dispose();

            Assert.NotNull(carFromDatabase);
            Assert.Equal(car.Id, carFromDatabase.Id);
            Assert.Equal(car.Owner.Id, carFromDatabase.Owner.Id);
        }

        public Task DisposeAsync()
        {
            return testcontainers.DisposeAsync().AsTask();
        }

        public Task InitializeAsync()
        {
            return testcontainers.StartAsync();
        }
    }
}
