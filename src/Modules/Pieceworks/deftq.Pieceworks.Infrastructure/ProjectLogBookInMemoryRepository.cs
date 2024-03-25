using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.projectLogBook;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectLogBookInMemoryRepository : InMemoryRepository<ProjectLogBook>, IProjectLogBookRepository
        {
            public Task<ProjectLogBook> GetByProjectId(Guid id, CancellationToken cancellationToken = default)
            {
                var result = Query().FirstOrDefault(x => x.ProjectId.Value == id);
                if (result == null)
                {
                    throw new NotFoundException($"ProjectLogBook With id {id} not found");
                }
                return Task.FromResult(result);
            }
        }
}
