using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Domain.projectLogBook;
using DocumentFormat.OpenXml.Spreadsheet;

namespace deftq.Pieceworks.Application.GetStatusReportAsSpreadSheet
{
    public class DataSheetExporter : AbstractStatusReportExporter
    {
        private readonly SheetExporter _exporter;
        private ProjectExtraWorkAgreementList? _extraWorkAgreementList;
        private Project? _project;
        private TotalFolderWorkTimeCalculationResult? _projectSummation;

        private DataSheetRowBuilder builder = new DataSheetRowBuilder();
        
        public DataSheetExporter(SheetData sheetData)
        {
            _exporter = new SheetExporter(sheetData, true);
        }
        internal override void VisitProject(Project project)
        {
            _project = project;
            ExportRowIfComplete();
        }

        internal override void VisitCompensation(ProjectCompensationList compensationList)
        {
            // Ignore
        }

        public override void VisitLogBookDay(ProjectLogBookUser logBookUser, ProjectLogBookWeek logBookWeek, ProjectLogBookDay logBookDay)
        {
            // Ignore
        }

        public override void VisitExtraWorkAgreement(ProjectExtraWorkAgreementList extraWorkAgreementList)
        {
            _extraWorkAgreementList = extraWorkAgreementList;
            ExportRowIfComplete();
        }

        public override void VisitProjectSummation(TotalFolderWorkTimeCalculationResult projectSummation)
        {
            _projectSummation = projectSummation;
            ExportRowIfComplete();
        }

        private void ExportRowIfComplete()
        {
            if (_project is not null && _extraWorkAgreementList is not null && _projectSummation is not null)
            {
                var buildDataRow = builder.BuildDataRow(_project, _extraWorkAgreementList, _projectSummation);
                _exporter.AddRow(buildDataRow);
            }
        }
    }
}
