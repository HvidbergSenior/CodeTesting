namespace deftq.BuildingBlocks.DataAccess
{
    public interface IReadonlyRepository<out TEntity> where TEntity : class
    {
        IQueryable<TEntity> Query();
    }
}
