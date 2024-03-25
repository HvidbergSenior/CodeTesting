using deftq.Pieceworks.Application.GetStatusReportAsSpreadSheet;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.project.Company;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Domain.projectLogBook;
using Xunit;
using static deftq.Pieceworks.Domain.Calculation.Expression.DecimalNumber;

namespace deftq.Pieceworks.Test.Application.GetStatusReportAsSpreadSheet
{
    public class OpenXMLExporterTest
    {
        [Fact(Skip = "manual")]
        //[Fact]
        public void ExportStatusReport_ShouldExportFile()
        {
            //Create project
            var project = Project.Create(Any.ProjectId(), ProjectName.Create("Test Project"), Any.ProjectNumber(), Any.ProjectPieceWorkNumber(),
                Any.ProjectOrderNumber(), Any.ProjectDescription(), Any.ProjectOwner(), ProjectPieceworkType.TwelveTwo,
                ProjectLumpSumPayment.Create(1000), ProjectStartDate.Create(new DateOnly(2023, 4, 9)),
                ProjectEndDate.Create(new DateOnly(2023, 10, 10)), Any.ProjectCreator(), ProjectCreatedTime.Empty(),
                ProjectCompany.Create(ProjectCompanyName.Create("test company"), ProjectAddress.Create("test adresse"),
                    ProjectCompanyCvrNo.Create("00000000"), ProjectCompanyPNo.Create("2222222222")));
            var projectParticipant = Any.ProjectParticipant();
            project.AddProjectParticipant(projectParticipant);

            //Create extra work agreement
            var extraworkAgreementList = ProjectExtraWorkAgreementList.Create(project.ProjectId, Any.ProjectExtraWorkAgreementListId());
            var extraworkAgreement = ProjectExtraWorkAgreement.Create(project.ProjectId, Any.ProjectExtraWorkAgreementId(),
                Any.ProjectExtraWorkAgreementName(), Any.ProjectExtraWorkAgreementDescription(), ProjectExtraWorkAgreementNumber.Create("2348923+4"),
                ProjectExtraWorkAgreementPaymentDkr.Create(1000.50m));
            extraworkAgreementList.AddExtraWorkAgreement(extraworkAgreement);

            //Create project compensation
            var compensation = ProjectCompensation.Create(Any.ProjectCompensationId(), ProjectCompensationPayment.Create(2000),
                ProjectCompensationPeriod.Create(ProjectCompensationDate.Create(DateOnly.FromDateTime(new DateTime(2023, 1, 2))),
                    ProjectCompensationDate.Create(DateOnly.FromDateTime(new DateTime(2023, 6, 13)))),
                new List<ProjectParticipantId> { ProjectParticipantId.Create(Guid.NewGuid()) });

            var compensation2 = ProjectCompensation.Create(Any.ProjectCompensationId(), ProjectCompensationPayment.Create(3000),
                ProjectCompensationPeriod.Create(ProjectCompensationDate.Create(DateOnly.FromDateTime(new DateTime(2026, 6, 6))),
                    ProjectCompensationDate.Create(DateOnly.FromDateTime(new DateTime(2026, 6, 13)))),
                new List<ProjectParticipantId> { ProjectParticipantId.Create(Any.Guid()) });

            var compensation3 = ProjectCompensation.Create(Any.ProjectCompensationId(), ProjectCompensationPayment.Create(100),
                ProjectCompensationPeriod.Create(ProjectCompensationDate.Create(DateOnly.FromDateTime(new DateTime(2010, 6, 6))),
                    ProjectCompensationDate.Create(DateOnly.FromDateTime(new DateTime(2026, 6, 13)))),
                new List<ProjectParticipantId> { ProjectParticipantId.Create(Any.Guid()) });
            var compensationList = ProjectCompensationList.Create(project.ProjectId, Any.ProjectCompensationListId());
            compensationList.AddCompensation(compensation);
            compensationList.AddCompensation(compensation2);
            compensationList.AddCompensation(compensation3);

            //Export to excel filer
            var exporter = new StatusReportExporter();
            var totalFolderWorkTimeCalculationResult = new TotalFolderWorkTimeCalculationResult(Number(348), Number(100), Number(200), Number(200));
            exporter.ExportStatusReport(project, compensationList, GenerateLogBook(project), extraworkAgreementList,
                totalFolderWorkTimeCalculationResult);
            var statusReportExport = exporter.GetExport();
            File.Delete(statusReportExport.FileName);
            File.WriteAllBytes(statusReportExport.FileName, statusReportExport.Data);
        }

        private ProjectLogBook GenerateLogBook(Project project)
        {
            var logBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var logBookUser1 = ProjectLogBookUser.Create(LogBookName.Create("Lærling først"), Guid.NewGuid());
            var logBookUser2 = ProjectLogBookUser.Create(LogBookName.Create("Altid Svend"), Guid.NewGuid());

            for (int week = 1; week <= 10; week++)
            {
                GenerateLogBookWeek(logBookUser1, 2023, week);
                GenerateLogBookWeek(logBookUser2, 2023, week);
            }

            logBookUser1.UpdateSalaryAdvance(LogBookYear.Create(2023), LogBookWeek.Create(1), LogBookSalaryAdvanceRole.Apprentice,
                LogBookSalaryAdvanceAmount.Create(90));
            logBookUser2.UpdateSalaryAdvance(LogBookYear.Create(2023), LogBookWeek.Create(1), LogBookSalaryAdvanceRole.Participant,
                LogBookSalaryAdvanceAmount.Create(150));
            logBookUser1.UpdateSalaryAdvance(LogBookYear.Create(2023), LogBookWeek.Create(5), LogBookSalaryAdvanceRole.Participant,
                LogBookSalaryAdvanceAmount.Create(150));

            logBook.ProjectLogBookUsers.Add(logBookUser1);
            logBook.ProjectLogBookUsers.Add(logBookUser2);

            return logBook;
        }

        private void GenerateLogBookWeek(ProjectLogBookUser logBookUser, int year, int week)
        {
            var days = ProjectLogBookWeek.FindWeekDays(LogBookYear.Create(year), LogBookWeek.Create(week));

            logBookUser.RegisterWeek(LogBookYear.Create(year), LogBookWeek.Create(week), LogBookNote.Create($"Uge {week}, {year}"),
                new List<ProjectLogBookDay>
                {
                    GenerateLogBookDay(days[0]),
                    GenerateLogBookDay(days[1]),
                    GenerateLogBookDay(days[2]),
                    GenerateLogBookDay(days[3]),
                    GenerateLogBookDay(days[4]),
                });

            logBookUser.CloseWeek(LogBookYear.Create(2023), LogBookWeek.Create(week));
        }

        private ProjectLogBookDay GenerateLogBookDay(DateOnly date)
        {
            return ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(date.Year, date.Month, date.Day)),
                LogBookTime.Create(LogBookHours.Create(7), LogBookMinutes.Create(30)));
        }

        [Fact(Skip = "manual")]
        //[Fact]
        public void GenerateExcelWithCompensation()
        {
            //Create project
            var project = Project.Create(Any.ProjectId(), ProjectName.Create("Test Project"), Any.ProjectNumber(), Any.ProjectPieceWorkNumber(),
                Any.ProjectOrderNumber(), Any.ProjectDescription(), Any.ProjectOwner(), ProjectPieceworkType.TwelveTwo,
                ProjectLumpSumPayment.Create(1000), ProjectStartDate.Create(new DateOnly(2023, 4, 9)),
                ProjectEndDate.Create(new DateOnly(2023, 10, 10)), Any.ProjectCreator(), ProjectCreatedTime.Empty(),
                ProjectCompany.Create(ProjectCompanyName.Create("test company"), ProjectAddress.Create("test adresse"),
                    ProjectCompanyCvrNo.Create("00000000"), ProjectCompanyPNo.Create("2222222222")));
            var projectParticipant = Any.ProjectParticipant();
            project.AddProjectParticipant(projectParticipant);

            var logBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var logBookUser1 = ProjectLogBookUser.Create(LogBookName.Create("Lærling først"), projectParticipant.ParticipantId.Value);
            var logBookUser2 = ProjectLogBookUser.Create(LogBookName.Create("Altid Svend"), Any.Guid());
            logBook.ProjectLogBookUsers.Add(logBookUser1);
            logBook.ProjectLogBookUsers.Add(logBookUser2);
            logBookUser1.RegisterWeek(LogBookYear.Create(2023), LogBookWeek.Create(6), Any.LogBookNote(),
                new List<ProjectLogBookDay>
                {
                    ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2023, 2, 11)),
                        LogBookTime.Create(LogBookHours.Create(7), LogBookMinutes.Create(30)))
                });

            //Create project compensation
            var compensation = ProjectCompensation.Create(Any.ProjectCompensationId(), ProjectCompensationPayment.Create(2000),
                ProjectCompensationPeriod.Create(ProjectCompensationDate.Create(DateOnly.FromDateTime(new DateTime(2023, 1, 2))),
                    ProjectCompensationDate.Create(DateOnly.FromDateTime(new DateTime(2023, 6, 13)))),
                new List<ProjectParticipantId> { projectParticipant.ParticipantId});

            var compensation2 = ProjectCompensation.Create(Any.ProjectCompensationId(), ProjectCompensationPayment.Create(3000),
                ProjectCompensationPeriod.Create(ProjectCompensationDate.Create(DateOnly.FromDateTime(new DateTime(2026, 6, 6))),
                    ProjectCompensationDate.Create(DateOnly.FromDateTime(new DateTime(2026, 6, 13)))),
                new List<ProjectParticipantId> {projectParticipant.ParticipantId});

            var compensation3 = ProjectCompensation.Create(Any.ProjectCompensationId(), ProjectCompensationPayment.Create(100),
                ProjectCompensationPeriod.Create(ProjectCompensationDate.Create(DateOnly.FromDateTime(new DateTime(2010, 6, 6))),
                    ProjectCompensationDate.Create(DateOnly.FromDateTime(new DateTime(2026, 6, 13)))),
                new List<ProjectParticipantId> { ProjectParticipantId.Create(Any.Guid()) });
            
            var compensation4 = ProjectCompensation.Create(Any.ProjectCompensationId(), ProjectCompensationPayment.Create(50),
                ProjectCompensationPeriod.Create(ProjectCompensationDate.Create(DateOnly.FromDateTime(new DateTime(2023, 1, 4))),
                    ProjectCompensationDate.Create(DateOnly.FromDateTime(new DateTime(2023, 6, 13)))),
                new List<ProjectParticipantId> { projectParticipant.ParticipantId});
            
            var compensationList = ProjectCompensationList.Create(project.ProjectId, Any.ProjectCompensationListId());
            compensationList.AddCompensation(compensation);
            compensationList.AddCompensation(compensation2);
            compensationList.AddCompensation(compensation3);
            compensationList.AddCompensation(compensation4);

            //Export to excel filer
            var exporter = new StatusReportExporter();
            var totalFolderWorkTimeCalculationResult = new TotalFolderWorkTimeCalculationResult(Number(348), Number(100), Number(200), Number(200));
            exporter.ExportStatusReport(project, compensationList, logBook, Any.ProjectExtraWorkAgreementList(),
                totalFolderWorkTimeCalculationResult);
            var statusReportExport = exporter.GetExport();
            File.Delete(statusReportExport.FileName);
            File.WriteAllBytes(statusReportExport.FileName, statusReportExport.Data);
        }
    }
}
