using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterLogBookSalaryAdvance;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RegisterLogBookSalaryAdvance
{
    public class RegisterLogBookSalaryAdvanceCommandAuthorizerTest
    {
        [Fact]
        public async Task GivenLogBook_WhenRegisteringSalaryAdvanceAsProjectOwner_ShouldRegister()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RegisterLogBookSalaryAdvanceCommandHandler.RegisterLogBookSalaryAdvanceCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(RegisterLogBookSalaryAdvanceCommand.Create(project.ProjectId, Any.ProjectParticipant().Id, 2023, 1, LogBookSalaryAdvanceRole.Participant, 42),
                    CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GivenLogBook_WhenRegisteringSalaryAdvanceAsProjectManager_ShouldRegister()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RegisterLogBookSalaryAdvanceCommandHandler.RegisterLogBookSalaryAdvanceCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(RegisterLogBookSalaryAdvanceCommand.Create(project.ProjectId, Any.ProjectParticipant().Id, 2023, 1, LogBookSalaryAdvanceRole.Participant, 42),
                    CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GivenLogBook_WhenRegisteringSalaryAdvanceAsParticipant_ShouldRegister()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RegisterLogBookSalaryAdvanceCommandHandler.RegisterLogBookSalaryAdvanceCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(RegisterLogBookSalaryAdvanceCommand.Create(project.ProjectId, Any.ProjectParticipant().Id, 2023, 1, LogBookSalaryAdvanceRole.Participant, 42),
                    CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
