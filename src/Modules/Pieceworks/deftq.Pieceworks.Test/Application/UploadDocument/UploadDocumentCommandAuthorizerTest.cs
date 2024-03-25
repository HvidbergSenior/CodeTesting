using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UploadDocument;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UploadDocument
{
    public class UploadDocumentCommandAuthorizerTest
    {
        [Fact]
        public async Task UploadDocumentAsOwner_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            byte[] data = new byte[] { 1, 2, 3, 4 };
            using (var ms = new MemoryStream(data))
            {
                var authorizer = new UploadDocumentCommandAuthorizer(projectRepository, uow, executionContext);
                var authorizationResult = await authorizer.Authorize(UploadDocumentCommand.Create(project.ProjectId.Value, null,  Any.ProjectDocumentId().Value, Any.String(10), ms), CancellationToken.None);    
                
                Assert.True(authorizationResult.IsAuthorized);
            }
        }
        
        [Fact]
        public async Task UploadDocumentAsNonOwner_ShouldNotBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);
            
            byte[] data = new byte[] { 1, 2, 3, 4 };
            using (var ms = new MemoryStream(data))
            {
                var authorizer = new UploadDocumentCommandAuthorizer(projectRepository, uow, executionContext);
                var authorizationResult = await authorizer.Authorize(UploadDocumentCommand.Create(project.ProjectId.Value, null,  Any.ProjectDocumentId().Value, Any.String(10), ms), CancellationToken.None);    
                
                Assert.False(authorizationResult.IsAuthorized);
            }
        }
    }
}
