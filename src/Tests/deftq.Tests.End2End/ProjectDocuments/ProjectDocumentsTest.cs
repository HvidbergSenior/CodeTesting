using System.Net;
using FluentAssertions;
using Xunit;

namespace deftq.Tests.End2End.ProjectDocuments
{
    [Collection("End2End")]
    public class ProjectFoldersTest
    {
        private readonly WebAppFixture fixture;
        private readonly Api _api;

        public ProjectFoldersTest(WebAppFixture webAppFixture)
        {
            fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }
        
        [Fact]
        public async Task GivenProject_UploadingDocument_FoldersShouldIncludeDocument()
        {
            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");
            
            // Upload document
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                using (var byteArrayContent = new ByteArrayContent(new byte[] { 1, 2, 3, 4 }))
                {
                    multipartFormContent.Add(byteArrayContent, "file", "doc.pdf");
                    var uploadResponse = await fixture.Client.PostAsync($"/api/projects/{projectId}/folders/{folderId}/documents", multipartFormContent);
                    Assert.Equal(HttpStatusCode.Created, uploadResponse.StatusCode);
                }
            }
         
            // Get folder structure with document
            var foldersResponse = await _api.GetFolderRoot(projectId);
            Assert.Single(foldersResponse.RootFolder.SubFolders[0].Documents);
            Assert.Equal("doc.pdf", foldersResponse.RootFolder.SubFolders[0].Documents[0].Name);
            
            // Get file
            var bytes = await fixture.Client.GetByteArrayAsync($"/api/documents/{foldersResponse.RootFolder.SubFolders[0].Documents[0].DocumentId}");
            Assert.Equal(new byte[] { 1, 2, 3, 4 }, bytes);
        }
        
        [Fact]
        public async Task GivenProject_UploadingTooLargeDocument_ShouldReturnError()
        {
            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");
            
            // Upload document
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                var fileSizeInBytes = 11*1024*1024;
                using (var byteArrayContent = new ByteArrayContent(new byte[fileSizeInBytes]))
                {
                    multipartFormContent.Add(byteArrayContent, "file", "doc.pdf");
                    var uploadResponse = await fixture.Client.PostAsync($"/api/projects/{projectId}/folders/{folderId}/documents", multipartFormContent);
                    Assert.Equal(HttpStatusCode.BadRequest, uploadResponse.StatusCode);
                }
            }
        }
        
        [Fact]
        public async Task GivenProjectWithDocument_DeletingDocument_RemovesDocument()
        {
            // GIVEN
            // Create a project
            var projectId = await _api.CreateProject();

            // Create a folder
            var folderId = await _api.CreateFolder(projectId, "myFolder", "description");
            
            // Upload document
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                using (var byteArrayContent = new ByteArrayContent(new byte[] { 1, 2, 3, 4 }))
                {
                    multipartFormContent.Add(byteArrayContent, "file", "doc.pdf");
                    await fixture.Client.PostAsync($"/api/projects/{projectId}/folders/{folderId}/documents", multipartFormContent);
                }
            }

            var foldersResponse = await _api.GetFolderRoot(projectId);
            var documentId = foldersResponse.RootFolder.SubFolders[0].Documents[0].DocumentId;

            // WHEN
            // Delete document
            var httpResponseMessage = await fixture.Client.DeleteAsync($"/api/projects/{projectId}/folders/{folderId}/documents/{documentId}");

            // THEN
            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            // Containing folder structure must be empty 
            foldersResponse = await _api.GetFolderRoot(projectId);
            Assert.Empty(foldersResponse.RootFolder.SubFolders[0].Documents);
            
            // File is deleted
            var response = await fixture.Client.GetAsync($"/api/documents/{documentId}");
            response.Should().Be404NotFound();
        }
    }
}
