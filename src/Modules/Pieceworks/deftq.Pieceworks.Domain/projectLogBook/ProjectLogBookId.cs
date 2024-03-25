using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class ProjectLogBookId : ValueObject
    {
        public Guid Id { get; private set; }
        
        private ProjectLogBookId()
        {
            Id = Guid.Empty;
        }

        private ProjectLogBookId(Guid id)
        {
            Id = id;
        }
        
        public static ProjectLogBookId Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(id));
            }

            return new ProjectLogBookId(id);
        }
    }
}
