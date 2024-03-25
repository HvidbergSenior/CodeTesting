namespace deftq.BuildingBlocks.DataAccess
{
    public interface IUnitOfWork
    {
        Task Commit(CancellationToken cancellationToken);
    }
}
