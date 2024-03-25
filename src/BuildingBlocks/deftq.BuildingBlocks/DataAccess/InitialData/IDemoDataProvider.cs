using Marten;

namespace deftq.BuildingBlocks.DataAccess.InitialData
{
    public interface IDemoDataProvider
    {
        Task Populate(IDocumentStore documentStore, CancellationToken cancellation);
    }
}
