using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectSpecificOperation
{
    public sealed class ProjectSpecificOperationId : ValueObject
    {
        public Guid Value { get; private set; }

        private ProjectSpecificOperationId()
        {
            Value = Guid.Empty;
        }

        private ProjectSpecificOperationId(Guid value)
        {
            Value = value;
        }

        public static ProjectSpecificOperationId Create(Guid value)
        {
            return new ProjectSpecificOperationId(value);
        }

        public static ProjectSpecificOperationId Empty()
        {
            return new ProjectSpecificOperationId(Guid.Empty);
        }
    }
}
