using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Application.GetDocument;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Infrastructure;
using deftq.Pieceworks.Infrastructure.projectDocument;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetDocument
{
    public class GetDocumentQueryTests
    {
        [Fact]
        public Task Should_Throw_NotFoundException()
        {
            var repository = new ProjectDocumentInMemoryRepository();
            var fileStorage = new InMemoryFileStorage();
            var query = GetDocumentQuery.Create(Any.ProjectDocumentId());
            var handler = new GetDocumentQueryHandler(repository, fileStorage);

            return Assert.ThrowsAsync<NotFoundException>(
                async () => await handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Should_Return_File()
        {
            var repository = new ProjectDocumentInMemoryRepository();
            var fileStorage = new InMemoryFileStorage();
            IFileReference reference;
            using (var stream = new MemoryStream(new byte[] {1,2,3,4,5}))
            {
                reference = await fileStorage.StoreFileAsync(Any.ProjectId(), Any.ProjectDocumentId(),
                    Any.ProjectDocumentName(), stream,
                    CancellationToken.None);
            }
            var document = Any.ProjectDocument(reference);
            await repository.Add(document);
            var query = GetDocumentQuery.Create(document.ProjectDocumentId);
            var handler = new GetDocumentQueryHandler(repository, fileStorage);

            var response = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(document.ProjectDocumentName.Value, response.Filename);
            Assert.Equal(new byte[] {1,2,3,4,5}, response.GetBuffer());
        }
    }
}
