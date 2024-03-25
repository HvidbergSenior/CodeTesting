using deftq.BuildingBlocks.Domain;

namespace deftq.BuildingBlocks.Fakes
{
    public static class EntityExtensions
    {
        public static T? PublishedEvent<T>(this Entity entity) where T : class, IDomainEvent
        {
            return entity.DomainEvents.LastOrDefault(e => e.GetType() == typeof(T)) as T;
        }
    }
}