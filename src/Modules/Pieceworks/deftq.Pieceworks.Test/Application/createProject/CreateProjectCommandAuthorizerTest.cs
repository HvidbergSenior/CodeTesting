using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetProjects;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.CreateProject
{
    public class CreateProjectCommandAuthorizerTest
    {
        private readonly ProjectInMemoryRepository _projectRepository;
        private readonly FakeExecutionContext _executionContext;

        public CreateProjectCommandAuthorizerTest()
        {
            _projectRepository = new ProjectInMemoryRepository();
            _executionContext = new FakeExecutionContext();
        }

        [Fact]
        public async Task CreateProject_ShouldOnlyRequireLogin()
        {
            var project = Any.Project();
            await _projectRepository.Add(project);

            var authorizer = new GetProjectsQueryAuthorizer(_executionContext);

            var authorizationResult = await authorizer.Authorize(GetProjectsQuery.Create(), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }
    }
}
