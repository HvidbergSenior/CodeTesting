using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetStatusReportAsSpreadSheet;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetStatusReportAsSpreadSheet
{
    public class GetStatusReportAsSpreadSheetQueryAuthorizerTest
    {
        [Fact]
        public async Task GetStatusReportAsSpreadSheetAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer =
                new GetStatusReportAsSpreadSheetQueryHandler.GetStatusReportAsSpreadSheetQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetStatusReportAsSpreadSheetQuery.Create(project.ProjectId.Value),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetStatusReportAsSpreadSheetAsParticipants_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer =
                new GetStatusReportAsSpreadSheetQueryHandler.GetStatusReportAsSpreadSheetQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetStatusReportAsSpreadSheetQuery.Create(project.ProjectId.Value),
                CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetStatusReportAsSpreadSheetAsProjectManager_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer =
                new GetStatusReportAsSpreadSheetQueryHandler.GetStatusReportAsSpreadSheetQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetStatusReportAsSpreadSheetQuery.Create(project.ProjectId.Value),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
    }
}
