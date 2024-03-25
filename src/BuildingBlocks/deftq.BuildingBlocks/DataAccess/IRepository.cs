namespace deftq.BuildingBlocks.DataAccess
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// This method is used to get a document by a know id.
        /// If document is not found a NotFoundException will be thrown.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Task<TEntity> GetById(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Used to find a document by id. Will return null if document is not found.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity?> FindById(object id, CancellationToken cancellationToken = default);

        Task<TEntity> Add(TEntity entity, CancellationToken cancellationToken = default);

        Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = default);

        Task Delete(TEntity entity, CancellationToken cancellationToken = default);

        Task<bool> DeleteById(Guid id, CancellationToken cancellationToken = default);

        Task SaveChanges(CancellationToken cancellationToken = default);
    }
}
