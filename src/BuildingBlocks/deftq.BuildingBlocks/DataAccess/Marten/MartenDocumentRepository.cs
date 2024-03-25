using deftq.BuildingBlocks.Domain;
using deftq.BuildingBlocks.Events;
using deftq.BuildingBlocks.Exceptions;
using Marten;

namespace deftq.BuildingBlocks.DataAccess.Marten
{
    public class MartenDocumentRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly IDocumentSession documentSession;
        private readonly IEntityEventsPublisher aggregateEventsPublisher;

        public MartenDocumentRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher)
        {
            this.documentSession = documentSession;
            this.aggregateEventsPublisher = aggregateEventsPublisher;
        }
        
        public Task<TEntity> Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Updated = DateTime.UtcNow;
            documentSession.Insert(entity);
            aggregateEventsPublisher.TryEnqueueEventsFrom(entity, out _);
            return Task.FromResult(entity);
        }

        public Task Delete(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            documentSession.Delete(entity);
            aggregateEventsPublisher.TryEnqueueEventsFrom(entity, out _);

            return Task.CompletedTask;
        }

        public Task<bool> DeleteById(Guid id, CancellationToken cancellationToken = default)
        {
            documentSession.Delete<TEntity>(id);
            return Task.FromResult(true);
        }

        public Task<TEntity?> FindById(object id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Id needs to have value");
            }

            return id switch
            {
                Guid guid => documentSession.LoadAsync<TEntity>(guid, cancellationToken),
                long l => documentSession.LoadAsync<TEntity>(l, cancellationToken),
                int i => documentSession.LoadAsync<TEntity>(i, cancellationToken),
                string s => documentSession.LoadAsync<TEntity>(s, cancellationToken),
                _ => throw new ArgumentException("Id was of a type not supported.", nameof(id))
            };
        }

        public async Task<TEntity> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await FindById(id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException($"Entity {id} not found.");
            }
            return entity;
        }

        public IQueryable<TEntity> Query()
        {
            return documentSession.Query<TEntity>();
        }

        public Task SaveChanges(CancellationToken cancellationToken = default)
        {
            return aggregateEventsPublisher.Publish(cancellationToken);
        }

        public Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Updated = DateTime.UtcNow;
            documentSession.Store(entity);
            aggregateEventsPublisher.TryEnqueueEventsFrom(entity, out _);

            return Task.FromResult(entity);
        }
    }
}
