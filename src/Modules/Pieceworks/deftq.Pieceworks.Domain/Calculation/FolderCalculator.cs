using deftq.Pieceworks.Domain.Calculation.Expression;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.projectFolderRoot;
using static deftq.Pieceworks.Domain.Calculation.Expression.DecimalNumber;
using static deftq.Pieceworks.Domain.Calculation.Expression.DecimalNumberUnit;
using static deftq.Pieceworks.Domain.Calculation.Expression.SumExpression;

namespace deftq.Pieceworks.Domain.Calculation
{
    public class FolderCalculator
    {
        public TotalFolderWorkTimeCalculationResult CalculateTotalOperationTime(ProjectFolderRoot projectFolderRoot, ProjectFolderId projectFolderId,
            IList<ProjectFolderWork> projectFolderWorks)
        {
            var folder = projectFolderRoot.GetFolder(projectFolderId);
            var folderWork = GetFolderWork(projectFolderWorks, projectFolderId);
            var folderSum = SumWorkItems(folderWork.WorkItems, folder.IsExtraWork());

            foreach (var subFolder in folder.SubFolders)
            {
                var subFolderResult = CalculateTotalOperationTime(projectFolderRoot, subFolder.ProjectFolderId, projectFolderWorks);
                var totalWorkTime = Sum(folderSum.TotalWorkTimeExpression, subFolderResult.TotalWorkTimeExpression);
                var totalPayment = Sum(folderSum.TotalPaymentExpression, subFolderResult.TotalPaymentExpression);
                var totalExtraWorkTime = Sum(folderSum.TotalExtraWorkTimeExpression, subFolderResult.TotalExtraWorkTimeExpression);
                var totalExtraWorkPayment = Sum(folderSum.TotalExtraWorkPaymentExpression, subFolderResult.TotalExtraWorkPaymentExpression);
                folderSum = new TotalFolderWorkTimeCalculationResult(totalWorkTime, totalPayment, totalExtraWorkTime, totalExtraWorkPayment);
            }

            return folderSum;
        }

        private static ProjectFolderWork GetFolderWork(IList<ProjectFolderWork> projectFolderWorks, ProjectFolderId folderId)
        {
            var folderWork = projectFolderWorks.FirstOrDefault(w => w.ProjectFolderId == folderId);
            if (folderWork is null)
            {
                throw new InvalidOperationException($"Unable to find work items for folder with id {folderId.Value}");
            }

            return folderWork;
        }

        private static TotalFolderWorkTimeCalculationResult SumWorkItems(IList<WorkItem> workItems, bool isExtraWork)
        {
            if (workItems.Count > 0)
            {
                var first = workItems.First();
                var remaining = workItems.Skip(1);

                IExpression totalPayment = Number(first.TotalPayment.Value, Dkr);
                IExpression totalWorkTime = Number(first.TotalWorkTime.Value, Ms);

                foreach (var workItem in remaining)
                {
                    totalPayment = Sum(totalPayment, Number(workItem.TotalPayment.Value));
                    totalWorkTime = Sum(totalWorkTime, Number(workItem.TotalWorkTime.Value));
                }

                if (isExtraWork)
                {
                    return new TotalFolderWorkTimeCalculationResult(totalWorkTime, totalPayment, totalWorkTime, totalPayment);
                }

                return new TotalFolderWorkTimeCalculationResult(totalWorkTime, totalPayment, Number(0), Number(0));
            }

            return new TotalFolderWorkTimeCalculationResult(Number(0), Number(0), Number(0), Number(0));
        }
    }

    public class TotalFolderWorkTimeCalculationResult
    {
        public IExpression TotalWorkTimeExpression { get; private set; }
        public IExpression TotalPaymentExpression { get; private set; }

        public IExpression TotalExtraWorkTimeExpression { get; private set; }
        public IExpression TotalExtraWorkPaymentExpression { get; private set; }

        public TotalFolderWorkTimeCalculationResult(IExpression totalWorkTimeExpression, IExpression totalPaymentExpression,
            IExpression totalExtraWorkTimeExpression, IExpression totalExtraWorkPaymentExpression)
        {
            TotalWorkTimeExpression = totalWorkTimeExpression;
            TotalPaymentExpression = totalPaymentExpression;
            TotalExtraWorkTimeExpression = totalExtraWorkTimeExpression;
            TotalExtraWorkPaymentExpression = totalExtraWorkPaymentExpression;
        }
    }
}
