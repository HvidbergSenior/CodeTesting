using deftq.Pieceworks.Domain.projectFolderRoot;
using Xunit.Abstractions;

namespace deftq.Pieceworks.Test.Domain.projectFolderRoot
{
    public static class ProjectFolderRootExt
    {
        private static int indentDiff = 2;
        
        public static void PrettyPrint(this ProjectFolderRoot folderRoot, ITestOutputHelper testOutputHelper)
        {
            testOutputHelper.WriteLine("Folders:");
            PrintFolder(folderRoot.RootFolder, 0, testOutputHelper);
        }

        private static void PrintFolder(ProjectFolder folder, int indent, ITestOutputHelper testOutputHelper)
        {
            testOutputHelper.WriteLine("".PadLeft(indent) + folder.Name.Value);
            foreach (var subFolder in folder.SubFolders)
            {
                PrintFolder(subFolder, indent + indentDiff, testOutputHelper);
            }
        }
    }
}
