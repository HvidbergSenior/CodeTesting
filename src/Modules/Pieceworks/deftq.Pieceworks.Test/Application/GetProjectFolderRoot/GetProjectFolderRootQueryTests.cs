using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Application.GetProjectFolderRoot;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectFolderRoot
{
    public class GetProjectFolderRootQueryTests
    {
        [Fact]
        public Task Should_Throw_NotFoundException()
        {
            var query = GetProjectFolderRootQuery.Create(ProjectId.Create(Guid.NewGuid()));
            var repository = new ProjectFolderRootInMemoryRepository();
            var baseRateAndSupplementRepository = new BaseRateAndSupplementRepository();
            var systemTime = new SystemTime();
            var handler = new GetProjectFolderRootQueryHandler(repository, baseRateAndSupplementRepository, systemTime);

            return Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, CancellationToken.None));
        }
        
        [Fact]
        public async Task Should_Return_ProjectFolderRoot()
        {
            var repository = new ProjectFolderRootInMemoryRepository();
            var projectFolderRoot = Any.ProjectFolderRoot();
            await repository.Add(projectFolderRoot);
            var query = GetProjectFolderRootQuery.Create(projectFolderRoot.ProjectId);
            var baseRateAndSupplementRepository = new BaseRateAndSupplementRepository();
            var systemTime = new SystemTime();
            var handler = new GetProjectFolderRootQueryHandler(repository, baseRateAndSupplementRepository, systemTime);
            var dto = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(projectFolderRoot?.ProjectId.Value, dto.ProjectId);
        }
    }
}
