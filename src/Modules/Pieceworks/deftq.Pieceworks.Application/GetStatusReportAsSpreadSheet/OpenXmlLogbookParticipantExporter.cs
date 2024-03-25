using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Domain.projectLogBook;
using DocumentFormat.OpenXml.Spreadsheet;

namespace deftq.Pieceworks.Application.GetStatusReportAsSpreadSheet
{
    public class OpenXmlLogbookParticipantExporter : AbstractStatusReportExporter
    {
        private readonly SheetExporter _exporter;
        private ProjectCompensationList _compensationList;

        public OpenXmlLogbookParticipantExporter(SheetData sheet)
        {
            _exporter = new SheetExporter(sheet, true);
            _compensationList = ProjectCompensationList.Empty();
        }

        internal override void VisitProject(Project project)
        {
        }

        public override void VisitLogBookDay(ProjectLogBookUser logBookUser, ProjectLogBookWeek logBookWeek, ProjectLogBookDay logBookDay)
        {
            if (!logBookUser.IsApprentice(logBookWeek))
            {
                _exporter.AddRow(OpenXmlLogbookExporterUtil.VisitLogBookDay(_compensationList, logBookUser, logBookWeek, logBookDay));
            }
        }

        public override void VisitExtraWorkAgreement(ProjectExtraWorkAgreementList extraWorkAgreementList)
        {
            // Ignore
        }

        public override void VisitProjectSummation(TotalFolderWorkTimeCalculationResult projectSummation)
        {
            // Ignore
        }

        internal override void VisitCompensation(ProjectCompensationList compensationList)
        {
            _compensationList = compensationList;
        }
    }
}
