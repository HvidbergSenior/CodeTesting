using deftq.Pieceworks.Application.GetWorkItemsAsSpreadSheet;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using deftq.Pieceworks.Domain.projectLogBook;
using Xunit;
using Xunit.Abstractions;

namespace deftq.Pieceworks.Test.Application.GetWorkItemsAsSpreadSheet
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
            var outputFile = "C:/mjolner-code/projects/deftq/akkord+/output.xlsx";
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            var project = Any.Project();
            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, Any.ProjectName(), Any.ProjectFolderRootId(),
                FolderRateAndSupplement.OverwriteAll(Any.BaseRateAndSupplement()));
            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), project.ProjectId, folderRoot.RootFolder.ProjectFolderId);

            using var excelExport = new OpenXmlExporter(Any.BaseRateAndSupplement());
            excelExport.ExportWorkItems(project, folderRoot, new List<ProjectFolderWork> { folderWork });
            var export = excelExport.GetExport();

            using var fileStream = File.OpenWrite(outputFile);
            fileStream.Write(export.Data);
            fileStream.Flush();
        }
    }
}
