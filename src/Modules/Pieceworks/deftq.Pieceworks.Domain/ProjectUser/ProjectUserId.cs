using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectUser
{
    public sealed class ProjectUserId : ValueObject
    {
        public Guid Value { get; private set; }

        private ProjectUserId()
        {

        }

        private ProjectUserId(Guid value)
        {
            Value = value;
        }

        public static ProjectUserId Create(Guid pieceworkUserId)
        {
            if (pieceworkUserId == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(pieceworkUserId));
            }

            return new ProjectUserId(pieceworkUserId);
        }

        internal static ProjectUserId Unknown()
        {
            return new ProjectUserId(Guid.Empty);
        }
    }
}
