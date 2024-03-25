using deftq.Pieceworks.Application.GetWorkItemsAsSpreadSheet;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using deftq.Pieceworks.Domain.projectLogBook;
using Xunit;
using Xunit.Abstractions;

namespace deftq.Pieceworks.Test.Application.GetLogBookAsSpreadSheet
{
    public class OpenXmlExporterTest
    {
        private readonly ITestOutputHelper _output;

        public OpenXmlExporterTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(Skip = "manual")]
        //[Fact]
        public void ExportTest()
        {
            var outputFile = "C:/mjolner-code/projects/deftq/output.xlsx";
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            var project = Any.Project();
            var logBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var logBookUser = ProjectLogBookUser.Create(LogBookName.Create("test name"), Guid.NewGuid());
            var logBookUser2 = ProjectLogBookUser.Create(LogBookName.Create("another name"), Guid.NewGuid());
            // Week 1
            logBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(1), LogBookNote.Create("Uge 1, 2023"), new List<ProjectLogBookDay>
                {
                    ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2023, 1, 2)),
                        LogBookTime.Create(LogBookHours.Create(5), LogBookMinutes.Create(45))),
                    ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2023, 1, 3)),
                        LogBookTime.Create(LogBookHours.Create(3), LogBookMinutes.Create(30))),
                    ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2023, 1, 4)),
                        LogBookTime.Create(LogBookHours.Create(8), LogBookMinutes.Create(15))),
                });
            logBookUser.UpdateSalaryAdvance(LogBookYear.Create(2023), LogBookWeek.Create(1), LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(90));
            // Week 2
            logBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(2), LogBookNote.Create("Uge 2, 2023"), new List<ProjectLogBookDay>
                {
                    ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2023, 1, 10)),
                        LogBookTime.Create(LogBookHours.Create(12), LogBookMinutes.Create(15))),
                    ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2023, 1, 11)),
                        LogBookTime.Create(LogBookHours.Create(2), LogBookMinutes.Create(30))),
                    ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2023, 1, 12)),
                        LogBookTime.Create(LogBookHours.Create(12), LogBookMinutes.Create(45)))
                });
            logBookUser.UpdateSalaryAdvance(LogBookYear.Create(2023), LogBookWeek.Create(2), LogBookSalaryAdvanceRole.Participant, LogBookSalaryAdvanceAmount.Create(150));
            // Week 3
            logBookUser.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(3), LogBookNote.Create("Uge 3, 2023"), new List<ProjectLogBookDay>
                {
                    ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2023, 1, 16)),
                        LogBookTime.Create(LogBookHours.Create(13), LogBookMinutes.Create(15))),
                    ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2023, 1, 17)),
                        LogBookTime.Create(LogBookHours.Create(3), LogBookMinutes.Create(30))),
                    ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2023, 1, 18)),
                        LogBookTime.Create(LogBookHours.Create(13), LogBookMinutes.Create(45)))
                });

            logBook.ProjectLogBookUsers.Add(logBookUser);
            logBook.ProjectLogBookUsers.Add(logBookUser2);
            logBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(1));
            logBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(2));
            logBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(3));

            using var excelExport = new deftq.Pieceworks.Application.GetLogBookAsSpreadSheet.OpenXmlExporter();
            excelExport.ExportLogBook(project, logBook);
            var export = excelExport.GetExport();

            using var fileStream = File.OpenWrite(outputFile);
            fileStream.Write(export.Data);
            fileStream.Flush();
        }
    }
}
