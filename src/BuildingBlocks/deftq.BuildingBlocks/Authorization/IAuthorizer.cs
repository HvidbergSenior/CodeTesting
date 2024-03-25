namespace deftq.BuildingBlocks.Authorization
{
    public interface IAuthorizer<T>
    {
        Task<AuthorizationResult> Authorize(T instance, CancellationToken cancellation);
    }
}
