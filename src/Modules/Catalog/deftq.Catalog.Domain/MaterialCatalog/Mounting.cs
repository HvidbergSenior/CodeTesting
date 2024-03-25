using deftq.BuildingBlocks.Domain;
using deftq.Catalog.Domain.Time;

namespace deftq.Catalog.Domain.MaterialCatalog
{
    public sealed class Mounting : ValueObject
    {
        public MountingCode MountingCode { get; private set; }
        public OperationTime OperationTime { get; private set; }
        public IList<SupplementOperation> SupplementOperations { get; private set; }

        private Mounting()
        {
            MountingCode = MountingCode.Empty();
            OperationTime = OperationTime.Empty();
            SupplementOperations = new List<SupplementOperation>();
        }

        private Mounting(MountingCode mountingCode, OperationTime operationTime, IList<SupplementOperation> supplementOperations)
        {
            MountingCode = mountingCode;
            OperationTime = operationTime;
            SupplementOperations = supplementOperations;
        }

        public static Mounting Create(MountingCode mountingCode, OperationTime operationTime, IList<SupplementOperation> supplementOperations)
        {
            return new Mounting(mountingCode, operationTime, supplementOperations);
        }
        
        public static Mounting Empty()
        {
            return new Mounting();
        }
    }
}
