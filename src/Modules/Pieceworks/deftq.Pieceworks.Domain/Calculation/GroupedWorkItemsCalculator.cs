using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Domain.Calculation
{
    public class GroupedWorkItemsCalculator
    {
        public IList<GroupedWorkItemsResult> CalculateGroupedWorkItems(ProjectFolderRoot folderRoot, ProjectFolderId folderId,
            IList<ProjectFolderWork> folderWorkList)
        {
            var folder = folderRoot.RootFolder.Find(folderId);

            if (folder is null)
            {
                throw new ProjectFolderNotFoundException($"Unknown project folder id {folderId.Value}");
            }

            var folderWork = GetFolderWork(folder.ProjectFolderId, folderWorkList);

            var workItemGroup = folderWork.WorkItems.GroupBy(workItem => SelectKey(workItem));

            var workItemGroupSum = workItemGroup.Select(workItemGroup =>
                {
                    var text = workItemGroup.Select(workItem => workItem.Text.Value).FirstOrDefault() ?? String.Empty;
                    var summedAmount = workItemGroup.Select(workItem => workItem.Amount.Value).Sum();
                    var summedPaymentDkr = workItemGroup.Select(workItem => workItem.TotalPayment.Value).Sum();
                    return new GroupedWorkItemsResult(GetKeyAsString(workItemGroup.Key), text, summedAmount, summedPaymentDkr);
                })
                .ToList();

            foreach (var subFolder in folder.SubFolders)
            {
                var subFolderResult = CalculateGroupedWorkItems(folderRoot, subFolder.ProjectFolderId, folderWorkList);
                foreach (var result in subFolderResult)
                {
                    var groupedWorkItemsResult = workItemGroupSum.FirstOrDefault(group => string.Equals(group.Id, result.Id, StringComparison.Ordinal));
                    if (groupedWorkItemsResult is not null)
                    {
                        groupedWorkItemsResult.Add(result.Amount, result.PaymentDkr);
                    }
                    else
                    {
                        workItemGroupSum.Add(result);
                    }
                }
            }

            return workItemGroupSum;
        }

        private object SelectKey(WorkItem workItem)
        {
            if (workItem.IsMaterial())
            {
                return workItem.WorkItemMaterial!.EanNumber;
            }

            return workItem.WorkItemOperation!.WorkItemOperationNumber;
        }

        private string GetKeyAsString(object key)
        {
            if (key is WorkItemEanNumber eanNumber)
            {
                return eanNumber.Value;
            }
            else if (key is WorkItemOperationNumber operationNumber)
            {
                return operationNumber.Value;
            }

            throw new InvalidOperationException("Invalid key type should never happen");
        }

        private ProjectFolderWork GetFolderWork(ProjectFolderId folderId, IList<ProjectFolderWork> folderWorkList)
        {
            var folderWork = folderWorkList.FirstOrDefault(folderWork => folderWork.ProjectFolderId == folderId);
            if (folderWork is not null)
            {
                return folderWork;
            }

            throw new ProjectFolderWorkNotFoundException($"no folderWorks matching folder id {folderId.Value}");
        }
    }

    public class GroupedWorkItemsResult
    {
        public string Id { get;}
        public string Text { get; }
        public decimal Amount { get; private set; }
        public decimal PaymentDkr { get; private set; }

        public GroupedWorkItemsResult(string id, string text, decimal amount, decimal paymentDkr)
        {
            Id = id;
            Text = text;
            Amount = amount;
            PaymentDkr = paymentDkr;
        }

        public void Add(decimal resultAmount, decimal resultPaymentDkr)
        {
            Amount = Amount + resultAmount;
            PaymentDkr = PaymentDkr + resultPaymentDkr;
        }
    }
}
