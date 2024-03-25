using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Application.UploadDocument;
using deftq.Pieceworks.Infrastructure;
using deftq.Pieceworks.Infrastructure.projectDocument;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.UploadDocument
{
    public class UploadDocumentCommandTest
    {
        [Fact]
        public async Task UploadDocumentTest()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectDocumentInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var fileStorage = new InMemoryFileStorage();
            var handler = new UploadDocumentCommandHandler(repository, uow, executionContext, fileStorage, new SystemTime());
            
            var projectDocumentId = Any.ProjectDocumentId();
            
            byte[] data = new byte[] { 1, 2, 3, 4 };
            using (var ms = new MemoryStream(data))
            {
                var cmd = UploadDocumentCommand.Create(Any.ProjectId().Value, null, projectDocumentId.Value, "aftaleseddel.pdf", ms);
                await handler.Handle(cmd, CancellationToken.None);    
            }
            
            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
            
            var document = await repository.GetById(projectDocumentId.Value);
            Assert.NotNull(document);
        }
    }
}
