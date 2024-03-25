using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetProjects;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjects
{
    public class GetProjectsQueryTests
    {
        [Fact]
        public async Task NoProjects_ShouldReturnEmptyList()
        {
            var query = GetProjectsQuery.Create();
            var repository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var handler = new GetProjectsQueryHandler(repository, executionContext);

            var resp = await handler.Handle(query, CancellationToken.None);
            Assert.Empty(resp.Projects);
        }
        
        [Fact]
        public async Task NotProjectOwner_ShouldNotIncludeProject()
        {
            var query = GetProjectsQuery.Create();
            var repository = new ProjectInMemoryRepository();
            await repository.Add(Any.Project());
            var executionContext = new FakeExecutionContext();
            var handler = new GetProjectsQueryHandler(repository, executionContext);

            var resp = await handler.Handle(query, CancellationToken.None);
            Assert.Empty(resp.Projects);
        }
        
        [Fact]
        public async Task ProjectOwner_ShouldIncludeProject()
        {
            var query = GetProjectsQuery.Create();
            var repository = new ProjectInMemoryRepository();
            var project = Any.Project();
            await repository.Add(project);
            var executionContext = new FakeExecutionContext(project.ProjectOwner.Id);
            var handler = new GetProjectsQueryHandler(repository, executionContext);

            var resp = await handler.Handle(query, CancellationToken.None);
            Assert.Single(resp.Projects);
        }
        
        [Fact]
        public async Task ProjectParticipant_ShouldIncludeProject()
        {
            var executionContext = new FakeExecutionContext();
            var query = GetProjectsQuery.Create();
            var repository = new ProjectInMemoryRepository();
            var project = Any.Project().WithParticipant(executionContext.UserId);
            await repository.Add(project);
            
            var handler = new GetProjectsQueryHandler(repository, executionContext);
        
            var resp = await handler.Handle(query, CancellationToken.None);
            Assert.Single(resp.Projects);
        }
    }
}
