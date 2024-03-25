using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;

namespace deftq.Pieceworks.Application.GetLogBookAsSpreadSheet
{
    public abstract class AbstractLogBookExporter
    {
        protected abstract void VisitProject(Project project);
        protected abstract void VisitLogBookDay(ProjectLogBookUser projectLogBookUser, ProjectLogBookWeek projectLogBookWeek, ProjectLogBookDay projectLogBookDay);
        public abstract LogBookExport GetExport();

        public void ExportLogBook(Project project, ProjectLogBook projectLogBook)
        {
            VisitProject(project);

            foreach (var logBookUser in projectLogBook.ProjectLogBookUsers)
            {
                foreach (var logBookWeek in logBookUser.ProjectLogBookWeeks)
                {
                    foreach (var logBookDay in logBookWeek.LogBookDays)
                    {
                        VisitLogBookDay(logBookUser, logBookWeek, logBookDay);
                    }
                }
            }
        }
    }

    public class LogBookExport
    {
        public string FileName { get; private set; }
        public byte[] Data { get; private set; }

        public LogBookExport(string fileName, byte[] data)
        {
            FileName = fileName;
            Data = data;
        }
    }
}
