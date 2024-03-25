using deftq.BuildingBlocks.Domain;
using deftq.Catalog.Domain.OperationCatalog;
using deftq.Catalog.Domain.Time;

namespace deftq.Catalog.Domain.MaterialCatalog
{
    public sealed class SupplementOperation : Entity
    {
        public SupplementOperationId SupplementOperationId { get; private set; }

        /// <summary>
        /// Id of operation in the operations catalog
        /// </summary>
        public OperationId OperationId { get; private set; }

        /// <summary>
        /// Alternative id
        /// </summary>
        public OperationNumber OperationNumber { get; private set; }

        /// <summary>
        /// Text description of operation
        /// </summary>
        public OperationText OperationText { get; private set; }

        /// <summary>
        /// Operation time
        /// </summary>
        public OperationTime OperationTime { get; private set; }

        /// <summary>
        /// Type of supplement operation, determines if the operation amount is related to a single unit of material or the entire amount.
        /// </summary>
        public SupplementOperationType SupplementOperationType { get; private set; }

        private SupplementOperation()
        {
            SupplementOperationId = SupplementOperationId.Empty();
            Id = SupplementOperationId.Value;
            OperationId = OperationId.Empty();
            OperationNumber = OperationNumber.Empty();
            OperationText = OperationText.Empty();
            OperationTime = OperationTime.Empty();
            SupplementOperationType = SupplementOperationType.Empty();
        }

        private SupplementOperation(SupplementOperationId supplementOperationId, OperationId operationId, OperationNumber operationNumber,
            OperationText operationText, OperationTime operationTime, SupplementOperationType supplementOperationType)
        {
            SupplementOperationId = supplementOperationId;
            Id = SupplementOperationId.Value;
            OperationId = operationId;
            OperationNumber = operationNumber;
            OperationText = operationText;
            OperationTime = operationTime;
            SupplementOperationType = supplementOperationType;
        }

        /// <summary>
        /// Create a supplement operation that references a general operation from the operation catalog.
        /// </summary>
        public static SupplementOperation Create(SupplementOperationId supplementOperationId, OperationId operationId, SupplementOperationType supplementOperationType)
        {
            return new SupplementOperation(supplementOperationId, operationId, OperationNumber.Empty(), OperationText.Empty(), OperationTime.Empty(), supplementOperationType);
        }

        /// <summary>
        /// Create a self contained supplement operation.
        /// </summary>
        public static SupplementOperation Create(SupplementOperationId supplementOperationId, OperationNumber operationNumber,
            OperationText operationText, OperationTime operationTime, SupplementOperationType supplementOperationType)
        {
            return new SupplementOperation(supplementOperationId, OperationId.Empty(), operationNumber, operationText, operationTime, supplementOperationType);
        }

        public static SupplementOperation Empty()
        {
            return new SupplementOperation();
        }

        public bool IsEmbedded()
        {
            return OperationId == OperationId.Empty();
        }
        
        public bool IsAmountRelated()
        {
            return SupplementOperationType == SupplementOperationType.AmountRelated();
        }
        
        public bool IsUnitRelated()
        {
            return SupplementOperationType == SupplementOperationType.UnitRelated();
        }
    }
}
