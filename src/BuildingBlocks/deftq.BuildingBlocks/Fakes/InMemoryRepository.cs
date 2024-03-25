using System.Text;
using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Domain;
using deftq.BuildingBlocks.Exceptions;
using Marten.Services;

namespace deftq.BuildingBlocks.Fakes
{
    public class InMemoryRepository<T> : IRepository<T>, IReadonlyRepository<T> where T : Entity
    {
        public IDictionary<Guid, string> Entities { get; }
        public bool SaveChangesCalled { get; private set; }
        private JsonNetSerializer JsonSerializer { get; } 
        
        public InMemoryRepository()
        {
            Entities = new Dictionary<Guid, string>();
            SaveChangesCalled = false;
            JsonSerializer = Registration.GetJsonNetSerializer();
        }

        public Task<T> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            Entities.TryGetValue(id, out var entity);
            if (entity == null)
                throw new NotFoundException($"Entity {id} not found.");
            
            return Task.FromResult(Deserialize(entity));
        }

        
        
        public Task<T?> FindById(object id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Id needs to have value");
            }

            if (id is Guid guid)
            { 
                Entities.TryGetValue(guid, out var entity);
                return DeserializeNullable(entity);
            }
            return Task.FromResult((T?)null);
        }

        public Task<T> Add(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Add(entity.Id, JsonSerializer.ToJson(entity));
            return Task.FromResult(entity);
        }

        public Task<T> Update(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Remove(entity.Id);
            Entities.Add(entity.Id, JsonSerializer.ToJson(entity));
            return Task.FromResult(entity);
        }

        public Task Delete(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Remove(entity.Id);
            return Task.CompletedTask;
        }

        public Task<bool> DeleteById(Guid id, CancellationToken cancellationToken = default)
        {
            Entities.Remove(id);
            return Task.FromResult(true);
        }

        public Task SaveChanges(CancellationToken cancellationToken = default)
        {
            SaveChangesCalled = true;
            return Task.CompletedTask;
        }

        public IQueryable<T> Query()
        {
            return Entities.Values.Select(Deserialize).AsQueryable<T>();
        }
        
        private Task<T?> DeserializeNullable(string? json)
        {
            if (json is null)
            {
                return Task.FromResult(default(T?));
            }
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return Task.FromResult(JsonSerializer.FromJson<T?>(ms));
        }
        
        protected T Deserialize(string json)
        {
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return JsonSerializer.FromJson<T>(ms);
        }
    }
}
