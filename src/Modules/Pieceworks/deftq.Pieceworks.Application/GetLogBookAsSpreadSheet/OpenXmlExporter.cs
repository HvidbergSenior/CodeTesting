using System.Globalization;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace deftq.Pieceworks.Application.GetLogBookAsSpreadSheet
{
    public class OpenXmlExporter : AbstractLogBookExporter, IDisposable
    {
        private readonly Exporter _exporter;
        private readonly IList<Cell> _headers;

        public OpenXmlExporter()
        {
            _exporter = new Exporter();

            _headers = new List<Cell>
            {
                new() { CellValue = new CellValue("Navn"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Dato"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Timer"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Lærling"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Låst j/n"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Uge"), DataType = CellValues.String },
                new() { CellValue = new CellValue("År"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Note"), DataType = CellValues.String },
            };
        }

        protected override void VisitProject(Project project)
        {
            _exporter.AddSheet(project.ProjectName.Value);
        }

        protected override void VisitLogBookDay(ProjectLogBookUser projectLogBookUser, ProjectLogBookWeek projectLogBookWeek,
            ProjectLogBookDay projectLogBookDay)
        {
            var nameCell = new Cell { CellValue = new CellValue(projectLogBookUser.Name.Value), DataType = CellValues.String };
            var dateCell = new Cell
            {
                CellValue = new CellValue(projectLogBookDay.Date.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                DataType = CellValues.String
            };
            var hoursCell = new Cell
            {
                CellValue = new CellValue(projectLogBookDay.Time.GetHours().Value + (projectLogBookDay.Time.GetMinutes().Value / 60.0m)),
                DataType = CellValues.Number
            };
            var salaryAdvance = projectLogBookUser.FindFromAndToSalaryAdvance(projectLogBookWeek.Year, projectLogBookWeek.Week);
            var isApprentice = salaryAdvance.start is not null && salaryAdvance.start.SalaryAdvance.Role is not null &&
                             salaryAdvance.start.SalaryAdvance.Role == LogBookSalaryAdvanceRole.Apprentice;
            var apprenticeCell = new Cell { CellValue = new CellValue(isApprentice ? "j" : "n"), DataType = CellValues.String };
            var closedCell = projectLogBookWeek.Closed
                ? new Cell { CellValue = new CellValue("j"), DataType = CellValues.String }
                : new Cell() { CellValue = new CellValue("n") };
            var weekCell = new Cell { CellValue = new CellValue(projectLogBookWeek.Week.Value), DataType = CellValues.Number };
            var yearCell = new Cell { CellValue = new CellValue(projectLogBookWeek.Year.Value), DataType = CellValues.Number };
            var noteCell = new Cell { CellValue = new CellValue(projectLogBookWeek.Note.Value), DataType = CellValues.String };

            var cells = new List<Cell>
            {
                nameCell,
                dateCell,
                hoursCell,
                apprenticeCell,
                closedCell,
                weekCell,
                yearCell,
                noteCell,
            };

            _exporter.AddRow(cells.ToArray());
        }


        public override LogBookExport GetExport()
        {
            _exporter.AddHeaderRow(_headers.ToArray());
            return new LogBookExport("skurbog.xlsx", _exporter.GetData());
        }

        public void Dispose()
        {
            _exporter.Dispose();
        }

        private class Exporter : IDisposable
        {
            private readonly MemoryStream _memoryStream;
            private readonly SpreadsheetDocument _spreadsheetDocument;
            private SheetData? _sheetData;
            private uint _rowIndex = 2;

            public Exporter()
            {
                _memoryStream = new MemoryStream();
                _spreadsheetDocument = SpreadsheetDocument.Create(_memoryStream, SpreadsheetDocumentType.Workbook);

                var workBookPart = _spreadsheetDocument.AddWorkbookPart();
                workBookPart.Workbook = new Workbook();
            }

            public void AddSheet(string sheetName)
            {
                if (_spreadsheetDocument.WorkbookPart is null)
                {
                    throw new InvalidOperationException("spreadsheetDocument.WorkbookPart is null");
                }

                var sheets = _spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());

                var worksheetPart = _spreadsheetDocument.WorkbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                var relationshipIdPart = _spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart);
                var sheet = new Sheet() { Id = relationshipIdPart, SheetId = 1, Name = sheetName };

                sheets.Append(sheet);

                _sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            }

            public void AddRow(params Cell[] cells)
            {
                if (_sheetData is null)
                {
                    throw new InvalidOperationException("sheetData is null");
                }

                var sheetRow = new Row { RowIndex = new UInt32Value(_rowIndex) };
                _sheetData.Append(sheetRow);

                for (int i = 0; i < cells.Length; i++)
                {
                    sheetRow.InsertAt(cells[i], i);
                }

                _rowIndex++;
            }

            public byte[] GetData()
            {
                _spreadsheetDocument.Dispose();
                return _memoryStream.ToArray();
            }

            public void Dispose()
            {
                _spreadsheetDocument.Dispose();
                _memoryStream.Dispose();
            }

            public void AddHeaderRow(params Cell[] cells)
            {
                if (_sheetData is null)
                {
                    throw new InvalidOperationException("sheetData is null");
                }

                var sheetRow = new Row { RowIndex = new UInt32Value((uint)1) };
                for (int i = 0; i < cells.Length; i++)
                {
                    sheetRow.InsertAt(cells[i], i);
                }

                _sheetData.PrependChild(sheetRow);
            }
        }
    }
}
