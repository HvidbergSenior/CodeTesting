using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class ProjectFolderWork : Entity
    {
        public ProjectFolderWorkId ProjectFolderWorkId { get; private set; }
        public ProjectId ProjectId { get; private set; }
        public ProjectFolderId ProjectFolderId { get; private set; }
        public IList<WorkItem> WorkItems { get; private set; }

        private ProjectFolderWork()
        {
            ProjectFolderWorkId = ProjectFolderWorkId.Empty();
            Id = ProjectFolderWorkId.Value;
            ProjectId = ProjectId.Empty();
            ProjectFolderId = ProjectFolderId.Empty();
            WorkItems = new List<WorkItem>();
        }

        private ProjectFolderWork(ProjectFolderWorkId projectFolderWorkId, ProjectId projectId, ProjectFolderId projectFolderId)
        {
            ProjectFolderWorkId = projectFolderWorkId;
            Id = ProjectFolderWorkId.Value;
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            WorkItems = new List<WorkItem>();
        }

        public static ProjectFolderWork Create(ProjectFolderWorkId projectFolderWorkId, ProjectId projectId, ProjectFolderId projectFolderId)
        {
            return new ProjectFolderWork(projectFolderWorkId, projectId, projectFolderId);
        }

        public void AddWorkItem(WorkItem workItem, BaseRateAndSupplementProxy baseRateAndSupplementProxy)
        {
            var workItemCalculator = new WorkItemCalculator(baseRateAndSupplementProxy);
            var calculationResult = workItemCalculator.CalculateTotalOperationTime(workItem);
            workItem.UpdateTotalOperationTime(calculationResult);
            WorkItems.Add(workItem);
        }

        public void RemoveWorkItems(IList<WorkItemId> workItemIds)
        {
            foreach (var workItemId in workItemIds)
            {
                RemoveWorkItem(workItemId);
            }

            if (workItemIds.Count > 0)
            {
                AddDomainEvent(WorkItemRemovedDomainEvent.Create(ProjectId, ProjectFolderId, workItemIds));
            }
        }

        private void RemoveWorkItem(WorkItemId workItemId)
        {
            var foundWorkItem = WorkItems.FirstOrDefault(wi => wi.WorkItemId == workItemId);
            if (foundWorkItem is null)
            {
                throw new WorkItemNotFoundException(workItemId.Value);
            }

            WorkItems.Remove(foundWorkItem);
        }

        public void MoveWorkItems(ProjectFolderWork destinationFolderWork, IList<WorkItemId> workItemIds,
            BaseRateAndSupplementProxy baseRateAndSupplementProxy)
        {
            if (ProjectId != destinationFolderWork.ProjectId)
            {
                throw new NotAllowedToMoveWorkItemToOtherProjectException(ProjectId, destinationFolderWork.ProjectId);
            }

            foreach (var workItemId in workItemIds)
            {
                MoveWorkItem(destinationFolderWork, workItemId, baseRateAndSupplementProxy);
            }

            if (workItemIds.Count > 0)
            {
                AddDomainEvent(WorkItemMovedDomainEvent.Create(ProjectId, ProjectFolderId, destinationFolderWork.ProjectFolderId, workItemIds));
            }
        }

        private void MoveWorkItem(ProjectFolderWork destinationFolderWork, WorkItemId workItemId, BaseRateAndSupplementProxy destinationBaseRateAndSupplementProxy)
        {
            var workItemToMove = WorkItems.FirstOrDefault(wi => wi.WorkItemId == workItemId);
            if (workItemToMove is null)
            {
                throw new WorkItemNotFoundException(workItemId.Value);
            }

            RemoveWorkItem(workItemToMove.WorkItemId);
            destinationFolderWork.AddWorkItem(workItemToMove, destinationBaseRateAndSupplementProxy);
        }

        public void CopyWorkItems(ProjectFolderWork destinationFolderWork, BaseRateAndSupplementProxy destinationBaseRateAndSupplementProxy,
            IList<WorkItemId> workItemIds, IExecutionContext executionContext)
        {
            foreach (var workItemId in workItemIds)
            {
                var workItemToCopy = WorkItems.FirstOrDefault(workItem => workItem.WorkItemId == workItemId);
                if (workItemToCopy is null)
                {
                    throw new WorkItemNotFoundException(workItemId.Value);
                }
                CreateCopy(destinationFolderWork, destinationBaseRateAndSupplementProxy, workItemToCopy, executionContext);
            }

            if (workItemIds.Count > 0)
            {
                destinationFolderWork.AddDomainEvent(WorkItemsCopiedDomainEvent.Create(ProjectId, destinationFolderWork.ProjectFolderId, workItemIds));    
            }
        }

        private void CreateCopy(ProjectFolderWork destinationFolderWork, BaseRateAndSupplementProxy destinationBaseRateAndSupplementProxy, WorkItem workItem,
            IExecutionContext executionContext)
        {
            var workItemId = WorkItemId.Create(Guid.NewGuid());
            var workItemDate = WorkItemDate.Today();
            var workItemUser = WorkItemUser.Create(executionContext.UserId, executionContext.UserName);
            var workItemText = workItem.Text;
            var workItemOperationTime = workItem.OperationTime;
            var workItemAmount = workItem.Amount;
            var supplements = CreateCopy(workItem.Supplements);
            if (workItem.IsMaterial())
            {
                var catalogMaterialId = workItem.WorkItemMaterial!.CatalogMaterialId;
                var workItemEanNumber = workItem.WorkItemMaterial.EanNumber;
                var workItemMountingCode = workItem.WorkItemMaterial.MountingCode;
                var workItemUnit = workItem.WorkItemMaterial.Unit;
                var workItemSupplementOperations = CreateCopy(workItem.WorkItemMaterial.SupplementOperations);
                var copy = WorkItem.Create(workItemId, catalogMaterialId, workItemDate, workItemUser, workItemText, workItemEanNumber,
                    workItemMountingCode, workItemOperationTime, workItemAmount, workItemUnit, workItemSupplementOperations, supplements);
                destinationFolderWork.AddWorkItem(copy, destinationBaseRateAndSupplementProxy);
            }
            else if (workItem.IsOperation())
            {
                CatalogOperationId catalogOperationId = workItem.WorkItemOperation!.CatalogOperationId;
                WorkItemOperationNumber workItemOperationNumber = workItem.WorkItemOperation.WorkItemOperationNumber;
                var copy = WorkItem.Create(workItemId, catalogOperationId, workItemOperationNumber, workItemDate, workItemUser, workItemText, 
                    workItemOperationTime, workItemAmount, supplements);
                destinationFolderWork.AddWorkItem(copy, destinationBaseRateAndSupplementProxy);
            }
            else
            {
                throw new NotSupportedException("Unable to copy work item, not based on material or operation");
            }
        }

        private IList<Supplement> CreateCopy(IList<Supplement> supplements)
        {
            return supplements.Select(supplement =>
            {
                var supplementId = SupplementId.Create(Guid.NewGuid());
                return Supplement.Create(supplementId, supplement.CatalogSupplementId, supplement.SupplementNumber, supplement.SupplementText,
                    supplement.SupplementPercentage);
            }).ToList();
        }

        private IList<SupplementOperation> CreateCopy(IList<SupplementOperation> supplementOperations)
        {
            return supplementOperations.Select(operation =>
            {
                var supplementOperationId = SupplementOperationId.Create(Guid.NewGuid());
                return SupplementOperation.Create(supplementOperationId, operation.CatalogSupplementOperationId, operation.Text,
                    operation.OperationType, operation.OperationTime, operation.OperationAmount);
            }).ToList();
        }

        public void UpdateWorkItem(WorkItemId workItemId, WorkItemAmount workItemAmount, BaseRateAndSupplementProxy baseRateAndSupplementProxy)
        {
            var workItemToUpdate = WorkItems.FirstOrDefault(wi => wi.WorkItemId == workItemId);
            if (workItemToUpdate is null)
            {
                throw new WorkItemNotFoundException(workItemId.Value);
            }

            workItemToUpdate.UpdateAmount(workItemAmount, baseRateAndSupplementProxy);
        }
    }
}
