namespace deftq.Pieceworks.Application.CreateProject
{
    public interface IProjectNumberGenerator
    {
        Task<long> GetNextProjectNumber(CancellationToken cancellationToken);
    }
}
