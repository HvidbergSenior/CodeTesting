using System.Reflection;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Domain.projectLogBook;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace deftq.Pieceworks.Application.GetStatusReportAsSpreadSheet
{
    public abstract class AbstractStatusReportExporter
    {
        internal abstract void VisitProject(Project project);

        internal abstract void VisitCompensation(ProjectCompensationList compensationList);

        public abstract void VisitLogBookDay(ProjectLogBookUser logBookUser, ProjectLogBookWeek logBookWeek, ProjectLogBookDay logBookDay);

        public abstract void VisitExtraWorkAgreement(ProjectExtraWorkAgreementList extraWorkAgreementList);

        public abstract void VisitProjectSummation(TotalFolderWorkTimeCalculationResult totalFolderWorkTimeCalculationResult);
        
    }

    public sealed class StatusReportExporter : IDisposable
    {
        private readonly List<AbstractStatusReportExporter> _exporters;

        private string reportTemplate =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException(),
                "reporttemplate.xlsx");

        private readonly MemoryStream _memoryStream;
        private readonly SpreadsheetDocument _spreadsheetDocument;

        public StatusReportExporter()
        {
            using (var fileStream = File.Open(reportTemplate, FileMode.Open))
            {
                _memoryStream = new MemoryStream();
                fileStream.CopyTo(_memoryStream);
            }

            _spreadsheetDocument = SpreadsheetDocument.Open(_memoryStream, true);
            _exporters = new List<AbstractStatusReportExporter>
            {
                new OpenXmlLogbookApprenticeExporter(GetSheetDataByName("skurbog lærlinge")),
                new OpenXmlLogbookParticipantExporter(GetSheetDataByName("skurbog svende")),
                new DataSheetExporter(GetSheetDataByName("data"))
            };
        }

        private SheetData GetSheetDataByName(string sheetName)
        {
            if (_spreadsheetDocument.WorkbookPart?.Workbook.Sheets?.Elements<Sheet>() is null)
            {
                throw new ArgumentNullException($"Cannot find list of sheets in spreadsheet, looking for sheet {sheetName}");
            }

            var sheets = _spreadsheetDocument.WorkbookPart.Workbook.Sheets.Elements<Sheet>();
            var refId = sheets.FirstOrDefault(s => string.Equals(s.Name, sheetName, StringComparison.OrdinalIgnoreCase))?.Id?.Value;

            if (refId is null)
            {
                throw new ArgumentNullException($"Cannot find ref id for sheet {sheetName}");
            }

            var worksheetPart = (WorksheetPart)_spreadsheetDocument.WorkbookPart.GetPartById(refId);
            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            if (sheetData is null)
            {
                throw new ArgumentNullException($"Cannot find sheet data for sheet {sheetName}");
            }

            return sheetData;
        }

        public byte[] GetData()
        {
                if (_spreadsheetDocument.WorkbookPart?.Workbook.CalculationProperties is not null)
                {
                    _spreadsheetDocument.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
                }
           
                if (_spreadsheetDocument.WorkbookPart?.Workbook.CalculationProperties != null)
                {
                    _spreadsheetDocument.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
                }

                _spreadsheetDocument.Dispose();
                return _memoryStream.ToArray();
        }

        public void Dispose()
        {
            _spreadsheetDocument.Dispose();
        }

        public StatusReportExport GetExport()
        {
            return new StatusReportExport("statusreport.xlsx", GetData());
        }

        public void ExportStatusReport(Project project, ProjectCompensationList compensationList, ProjectLogBook projectLogBook,
            ProjectExtraWorkAgreementList extraWorkAgreementList, TotalFolderWorkTimeCalculationResult projectSummation)
        {
            _exporters.ForEach(exporter => exporter.VisitProject(project));
            _exporters.ForEach(exporter => exporter.VisitCompensation(compensationList));
            _exporters.ForEach(exporter => exporter.VisitExtraWorkAgreement(extraWorkAgreementList));
            _exporters.ForEach(exporter => exporter.VisitProjectSummation(projectSummation));

            foreach (var logBookUser in projectLogBook.ProjectLogBookUsers)
            {
                ExportLogBookWeeks(logBookUser);
            }
        }

        private void ExportLogBookWeeks(ProjectLogBookUser logBookUser)
        {
            foreach (var logBookWeek in logBookUser.ProjectLogBookWeeks)
            {
                ExportLogBookDays(logBookWeek, logBookUser);
            }
        }

        private void ExportLogBookDays(ProjectLogBookWeek logBookWeek, ProjectLogBookUser logBookUser)
        {
            foreach (var logBookDay in logBookWeek.LogBookDays)
            {
                _exporters.ForEach(exporter => exporter.VisitLogBookDay(logBookUser, logBookWeek, logBookDay));
            }
        }
    }

    public class StatusReportExport
    {
        public string FileName { get; private set; }
        public byte[] Data { get; private set; }

        public StatusReportExport(string fileName, byte[] data)
        {
            FileName = fileName;
            Data = data;
        }
    }
}
