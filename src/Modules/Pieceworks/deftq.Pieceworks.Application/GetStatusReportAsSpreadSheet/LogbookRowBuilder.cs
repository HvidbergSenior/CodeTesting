using System.Globalization;
using deftq.Pieceworks.Domain.projectLogBook;
using DocumentFormat.OpenXml.Spreadsheet;

namespace deftq.Pieceworks.Application.GetStatusReportAsSpreadSheet
{
    public static class LogbookRowBuilder
    {
        public static Cell[] BuildRow(ProjectLogBookUser logBookUser, ProjectLogBookWeek logBookWeek, ProjectLogBookDay logBookDay,
            decimal hourlySalaryAdvanceDkr, decimal totalSalaryAdvanceDkr, decimal hourlyCompensationPaymentDkr, decimal totalCompensationPaymentDkr)
        {
            var cells = BuildCommonRowCells(logBookUser, logBookWeek, logBookDay, hourlySalaryAdvanceDkr, totalSalaryAdvanceDkr, hourlyCompensationPaymentDkr, totalCompensationPaymentDkr);

            return cells.ToArray();
        }
        

        private static IList<Cell> BuildCommonRowCells(ProjectLogBookUser logBookUser, ProjectLogBookWeek logBookWeek, ProjectLogBookDay logBookDay,
            decimal hourlySalaryAdvanceDkr, decimal totalSalaryAdvanceDkr, decimal hourlyCompensationPaymentDkr, decimal totalCompensationPaymentDkr)
        {
            var userIdCell = new Cell { CellValue = new CellValue(logBookUser.UserId.ToString()), DataType = CellValues.String };
            var nameCell = new Cell { CellValue = new CellValue(logBookUser.Name.Value), DataType = CellValues.String };
            var dateCell = new Cell
            {
                CellValue = new CellValue(logBookDay.Date.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                DataType = CellValues.String
            };
            var hoursValue = logBookDay.Time.GetHours().Value + (logBookDay.Time.GetMinutes().Value / 60.0m);
            var hoursCell = new Cell { CellValue = new CellValue(hoursValue), DataType = CellValues.Number };

            var closedCell = logBookWeek.Closed
                ? new Cell { CellValue = new CellValue("j"), DataType = CellValues.String }
                : new Cell() { CellValue = new CellValue("n"), DataType = CellValues.String };
            var weekCell = new Cell { CellValue = new CellValue(logBookWeek.Week.Value), DataType = CellValues.Number };
            var yearCell = new Cell { CellValue = new CellValue(logBookWeek.Year.Value), DataType = CellValues.Number };
            var noteCell = new Cell { CellValue = new CellValue(logBookWeek.Note.Value), DataType = CellValues.String };

            var hourlyCompensationPaymentCell = new Cell { CellValue = new CellValue(hourlyCompensationPaymentDkr), DataType = CellValues.Number };
            
            var salaryAdvanceCell = new Cell { CellValue = new CellValue(hourlySalaryAdvanceDkr), DataType = CellValues.Number };
            
            var compensationPaymentSumCell = new Cell { CellValue = new CellValue(totalCompensationPaymentDkr), DataType = CellValues.Number };
            
            var salaryAdvanceSumCell = new Cell { CellValue = new CellValue(totalSalaryAdvanceDkr), DataType = CellValues.Number };

            var cells = new List<Cell>
            {
                userIdCell,
                nameCell,
                dateCell,
                hoursCell,
                closedCell,
                weekCell,
                yearCell,
                noteCell,
                hourlyCompensationPaymentCell,
                salaryAdvanceCell,
                compensationPaymentSumCell,
                salaryAdvanceSumCell
            };

            return cells;
        }
    }
}
