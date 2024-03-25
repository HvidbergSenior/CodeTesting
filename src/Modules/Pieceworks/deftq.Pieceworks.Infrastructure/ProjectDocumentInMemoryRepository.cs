using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.projectDocument;

namespace deftq.Pieceworks.Infrastructure
{
    public class ProjectDocumentInMemoryRepository : InMemoryRepository<ProjectDocument>, IProjectDocumentRepository
    {
        
    }
}
