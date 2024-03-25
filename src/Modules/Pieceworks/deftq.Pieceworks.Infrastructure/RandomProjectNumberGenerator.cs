using deftq.Pieceworks.Application.CreateProject;

namespace deftq.Pieceworks.Infrastructure
{
    public class RandomProjectNumberGenerator : IProjectNumberGenerator
    {
        private readonly Random _random = new Random();
        
        public Task<long> GetNextProjectNumber(CancellationToken cancellationToken)
        {
#pragma warning disable CA5394
            return Task.FromResult(_random.NextInt64());
#pragma warning restore CA5394
        }
    }
}
