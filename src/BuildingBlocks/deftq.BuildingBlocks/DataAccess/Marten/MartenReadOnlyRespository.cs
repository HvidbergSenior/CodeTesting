using deftq.BuildingBlocks.Domain;
using Marten;

namespace deftq.BuildingBlocks.DataAccess.Marten
{
    public class MartenReadOnlyRespository<T> : IReadonlyRepository<T> where T : Entity
    {
        private readonly IQuerySession querySession;

        public MartenReadOnlyRespository(IQuerySession querySession)
        {
            this.querySession = querySession;
        }

        public IQueryable<T> Query()
        {
            return querySession.Query<T>();
        }
    }
}
