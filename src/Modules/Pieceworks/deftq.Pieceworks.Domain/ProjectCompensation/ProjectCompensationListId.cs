using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectCompensation
{
    public sealed class ProjectCompensationListId : ValueObject
    {
        public Guid Value { get; private set; }

        private ProjectCompensationListId()
        {
            Value = Guid.Empty;
        }

        private ProjectCompensationListId(Guid value)
        {
            Value = value;
        }

        public static ProjectCompensationListId Create(Guid value)
        {
            return new ProjectCompensationListId(value);
        }

        public static ProjectCompensationListId Empty()
        {
            return new ProjectCompensationListId();
        }
    }
}
