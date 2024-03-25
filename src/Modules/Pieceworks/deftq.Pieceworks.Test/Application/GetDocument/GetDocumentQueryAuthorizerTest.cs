using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetDocument;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetDocument
{
    public class GetDocumentQueryAuthorizerTest
    {
        [Fact]
        public async Task GetDocumentAsOwner_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var documentRepository = new ProjectDocumentInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var document = ProjectDocument.Create(project.ProjectId, null, Any.ProjectDocumentId(),
                Any.ProjectDocumentName(), Any.ProjectDocumentUploadedTimestamp(), IFileReference.Empty());
            await documentRepository.Add(document);

            var authorizer =
                new GetDocumentQueryAuthorizer(projectRepository, documentRepository, uow, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetDocumentQuery.Create(document.ProjectDocumentId),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetDocumentAsNonOwner_ShouldNotBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var documentRepository = new ProjectDocumentInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project();
            await projectRepository.Add(project);

            var document = ProjectDocument.Create(project.ProjectId, null, Any.ProjectDocumentId(),
                Any.ProjectDocumentName(), Any.ProjectDocumentUploadedTimestamp(), IFileReference.Empty());
            await documentRepository.Add(document);

            var authorizer =
                new GetDocumentQueryAuthorizer(projectRepository, documentRepository, uow, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetDocumentQuery.Create(document.ProjectDocumentId),
                CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
