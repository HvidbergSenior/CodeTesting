using Marten.Metadata;
using Newtonsoft.Json;

namespace deftq.BuildingBlocks.Domain
{
    public abstract class Entity : IEquatable<Entity?>, IVersioned
    {
        public Guid Id { get; protected set; }
        public Guid Version { get; set; }
        public DateTime Created { get; internal set; }
        public DateTime? Updated { get; internal set; }

        private readonly List<IDomainEvent> _domainEvents = new();

        protected Entity()
        {
            Created = DateTime.UtcNow;
        }

        [JsonIgnore]
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        
        public static bool operator ==(Entity? first, Entity? second)
        {
            if (first is null && second is null)
            {
                return true;
            }
            
            return first is not null && first.Equals(second);
        }
        
        public static bool operator !=(Entity? first, Entity? second)
        {
            return !(first == second);
        }
        
        public bool Equals(Entity? other)
        {
            if (ReferenceEquals(other,null))
            {
                return false;
            }
            if (other.GetType() != GetType())
            {
                return false;
            }
            return string.Equals(Id.ToString(), other.Id.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Entity)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() * 53;
        }
    }
}
