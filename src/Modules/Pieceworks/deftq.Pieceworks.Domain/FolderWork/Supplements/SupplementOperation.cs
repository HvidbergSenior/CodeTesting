using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork.Supplements
{
    public sealed class SupplementOperation : Entity
    {
        /// <summary>
        /// Unique id that identifies this specific registering of an supplement operation 
        /// </summary>
        public SupplementOperationId SupplementOperationId { get; private set; }
        
        /// <summary>
        /// Id that identifies the supplement operation in the material catalog
        /// </summary>
        public CatalogSupplementOperationId CatalogSupplementOperationId { get; private set; }
        
        /// <summary>
        /// Text description
        /// </summary>
        public SupplementOperationText Text { get; private set; }
        
        /// <summary>
        /// Type of supplement operation, either related to each unit of material or the total material amount
        /// </summary>
        public SupplementOperationType OperationType { get; private set; }
        
        /// <summary>
        /// Operation time for a single unit
        /// </summary>
        public SupplementOperationTime OperationTime { get; private set; }
        
        /// <summary>
        /// Number of units mounted
        /// </summary>
        public SupplementOperationAmount OperationAmount { get; private set; }
        
        private SupplementOperation()
        {
            SupplementOperationId = SupplementOperationId.Empty();
            Id = SupplementOperationId.Value;
            CatalogSupplementOperationId = CatalogSupplementOperationId.Empty();
            Text = SupplementOperationText.Empty();
            OperationType = SupplementOperationType.Empty();
            OperationTime = SupplementOperationTime.Empty();
            OperationAmount = SupplementOperationAmount.Empty();
        }

        private SupplementOperation(SupplementOperationId supplementOperationId, CatalogSupplementOperationId catalogSupplementOperationId, SupplementOperationText text,
            SupplementOperationType supplementOperationType,
            SupplementOperationTime operationTime, SupplementOperationAmount operationAmount)
        {
            SupplementOperationId = supplementOperationId;
            CatalogSupplementOperationId = catalogSupplementOperationId;
            Id = SupplementOperationId.Value;
            Text = text;
            OperationType = supplementOperationType;
            OperationTime = operationTime;
            OperationAmount = operationAmount;
        }

        public static SupplementOperation Create(SupplementOperationId supplementOperationId, CatalogSupplementOperationId catalogSupplementOperationId, SupplementOperationText supplementOperationText,
            SupplementOperationType supplementOperationType,
            SupplementOperationTime supplementOperationTime, SupplementOperationAmount supplementOperationAmount)
        {
            return new SupplementOperation(supplementOperationId, catalogSupplementOperationId, supplementOperationText, supplementOperationType, supplementOperationTime, supplementOperationAmount);
        }
        
        public bool IsAmountRelated()
        {
            return OperationType == SupplementOperationType.AmountRelated();
        }
        
        public bool IsUnitRelated()
        {
            return OperationType == SupplementOperationType.UnitRelated();
        }
    }
}
