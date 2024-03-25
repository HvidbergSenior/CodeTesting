using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectSpecificOperation
{
    public sealed class ProjectSpecificOperationListId : ValueObject
    {
        public Guid Value { get; private set; }

        private ProjectSpecificOperationListId()
        {
            Value = Guid.Empty;
        }

        private ProjectSpecificOperationListId(Guid value)
        {
            Value = value;
        }

        public static ProjectSpecificOperationListId Create(Guid value)
        {
            return new ProjectSpecificOperationListId(value);
        }

        public static ProjectSpecificOperationListId Empty()
        {
            return new ProjectSpecificOperationListId(Guid.Empty);
        }
    }
}
