using System.Globalization;
using Baseline.ImTools;
using deftq.Pieceworks.Application.CreateProject;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using DocumentFormat.OpenXml.Spreadsheet;

namespace deftq.Pieceworks.Application.GetStatusReportAsSpreadSheet
{
    public class DataSheetRowBuilder
    {
        public Cell[] BuildDataRow(Project project, ProjectExtraWorkAgreementList projectExtraWorkAgreementList,
            TotalFolderWorkTimeCalculationResult projectSummation)
        {
            var projectNumberCell = new Cell { CellValue = new CellValue((int)project.ProjectNumber.Value), DataType = CellValues.Number};
            var pieceWorkNumberCell = new Cell { CellValue = new CellValue(project.ProjectPieceWorkNumber.Value), DataType = CellValues.String };
            
            var projectNameCell = new Cell { CellValue = new CellValue(project.ProjectName.Value), DataType = CellValues.String };
            var projectCurrentDateCell = new Cell { CellValue = new CellValue(DateTime.Today), DataType = CellValues.String };
            var projectStartDateCell = new Cell
            {
                CellValue = new CellValue(project.ProjectStartDate.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                DataType = CellValues.String
            };
            
            var projectEndDateCell = new Cell
            {
                CellValue = new CellValue(project.ProjectEndDate.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                DataType = CellValues.String
            };
            
            var pieceworkTypeCell = new Cell { CellValue = new CellValue(project.ProjectPieceworkType.ToHumanReadable()), DataType = CellValues.String };

            var projectLumpSumCell = new Cell { CellValue = new CellValue(project.ProjectLumpSumPaymentDkr.Value), DataType = CellValues.Number };
            var projectSummationCell = new Cell
            {
                CellValue = new CellValue(projectSummation.TotalPaymentExpression.Evaluate().Value), DataType = CellValues.Number
            };
            var extraWorkAgreementTotalPaymentCell = new Cell
            {
                CellValue = new CellValue(projectExtraWorkAgreementList.ProjectExtraWorkAgreementTotalPaymentDkr.Value),
                DataType = CellValues.Number
            };
            
            IList<Cell> cells = new List<Cell>()
            {
                projectNumberCell,
                pieceWorkNumberCell,
                projectNameCell,
                projectCurrentDateCell,
                projectStartDateCell,
                projectEndDateCell,
                pieceworkTypeCell,
                projectLumpSumCell,
                projectSummationCell,
                extraWorkAgreementTotalPaymentCell
            };
            
            return cells.ToArray();
        }
    }
}
