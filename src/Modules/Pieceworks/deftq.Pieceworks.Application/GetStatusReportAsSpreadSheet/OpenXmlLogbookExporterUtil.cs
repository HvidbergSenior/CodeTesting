using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Domain.projectLogBook;
using DocumentFormat.OpenXml.Spreadsheet;

namespace deftq.Pieceworks.Application.GetStatusReportAsSpreadSheet
{
    public static class OpenXmlLogbookExporterUtil
    {
        public static Cell[] VisitLogBookDay(ProjectCompensationList compensationList,ProjectLogBookUser logBookUser, ProjectLogBookWeek logBookWeek, ProjectLogBookDay logBookDay)
        {
            var salaryAdvance = logBookUser.FindFromAndToSalaryAdvance(logBookWeek.Year, logBookWeek.Week);
            
            var compensations = compensationList.Compensations;
            var compensationsOnLogBookDay = compensations.Where(compensation => compensation.IsDateIncluded(logBookDay.Date.Value)).ToList();
            var compensationOnParticipant = compensationsOnLogBookDay.Where(compensation => compensation.IsParticipantIncluded(logBookUser));

            var totalHours = new decimal(logBookDay.Time.Value.TotalHours);

            var hourlyCompensationPaymentDkr = 0.0m;
            foreach (var compensation in compensationOnParticipant)
            {
                var compensationPaymentDkr = compensation.ProjectCompensationPayment.Value;
                hourlyCompensationPaymentDkr += compensationPaymentDkr;
            }

            var totalCompensationPaymentDkr = hourlyCompensationPaymentDkr * totalHours;
            

            var hourlySalaryAdvanceDkr = salaryAdvance.start?.SalaryAdvance?.Amount?.Value ?? 0;
            var totalSalaryAdvance = hourlySalaryAdvanceDkr * totalHours;

            return LogbookRowBuilder.BuildRow(logBookUser, logBookWeek, logBookDay,
                hourlySalaryAdvanceDkr, totalSalaryAdvance, hourlyCompensationPaymentDkr, totalCompensationPaymentDkr);
        }
    }
}
