using deftq.BuildingBlocks.Domain;
using deftq.Catalog.Domain.Time;

namespace deftq.Catalog.Domain.OperationCatalog
{
    public sealed class Operation : Entity
    {
        public OperationId OperationId { get; private set; }
        public OperationNumber OperationNumber { get; private set; }
        public OperationText OperationText { get; private set; }
        public OperationTime OperationTime { get; private set; }
        
        private Operation()
        {
            OperationId = OperationId.Empty();
            Id = OperationId.Value;
            OperationNumber = OperationNumber.Empty();
            OperationText = OperationText.Empty();
            OperationTime = OperationTime.Empty();
        }
        
        private Operation(OperationId operationId, OperationNumber operationNumber, OperationText operationText, OperationTime operationTime)
        {
            OperationId = operationId;
            Id = OperationId.Value;
            OperationNumber = operationNumber;
            OperationText = operationText;
            OperationTime = operationTime;
        }

        public static Operation Create(OperationId operationId, OperationNumber operationNumber, OperationText operationText, OperationTime operationTime)
        {
            return new Operation(operationId, operationNumber, operationText, operationTime);
        }
    }
}
