using deftq.Pieceworks.Domain.projectFolderRoot;
using Microsoft.VisualBasic;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.projectFolderRoot
{
    public class ProjectFolderNameTest
    {
        [Fact]
        public void WhenCreatingFolder_FolderNameIsValidated()
        {
            ProjectFolderName.Create("");
            ProjectFolderName.Create("     ");
            ProjectFolderName.Create(Any.String(500));
            
            Assert.Throws<ArgumentException>(() => ProjectFolderName.Create(Any.String(501)));
        }
    }
}
