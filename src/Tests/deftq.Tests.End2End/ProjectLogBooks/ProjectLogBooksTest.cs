using Xunit;

namespace deftq.Tests.End2End.ProjectLogBooks
{
    [Collection("End2End")]
    public class ProjectLogBooksTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public ProjectLogBooksTest(WebAppFixture fixture)
        {
            _fixture = fixture;
            _api = new Api(fixture);
        }

        [Fact]
        public async Task Should_Find_Created_Logbook()
        {
            // Create a project
            var projectId = await _api.CreateProject();

            // Assert log book was created
            var getLogBookResponse = await _api.GetLogBook(projectId);
            Assert.Equal(projectId, getLogBookResponse.ProjectId);
            Assert.Single(getLogBookResponse.Users);
        }
    }
}
