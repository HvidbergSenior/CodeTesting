using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Application.CreateProject;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.application.CreateProject;

public class CreateProjectCommandTest
{
    [Fact]
    public async Task CreateProjectCommand_ShouldCreateSuccessfully()
    {
        var uow = new FakeUnitOfWork();
        var repository = new ProjectInMemoryRepository();
        var executionContext = new FakeExecutionContext();
        var projectNumberGenerator = new RandomProjectNumberGenerator();
        var handler = new CreateProjectCommandHandler(repository, uow, executionContext, new SystemTime(), projectNumberGenerator);

        var cmd = CreateProjectCommand.Create(Any.Instance<Guid>(), Any.Instance<string>(), Any.Instance<string>(), PieceworkType.TwelveTwo,
            Any.Instance<decimal>());

        await handler.Handle(cmd, CancellationToken.None);

        Assert.Single(repository.Entities);
        Assert.True(repository.SaveChangesCalled);
        Assert.True(uow.IsCommitted);
    }

    [Fact]
    public async Task CreateProjectCommand_ShouldHaveCreatedByAndCreatedTime()
    {
        var uow = new FakeUnitOfWork();
        var repository = new ProjectInMemoryRepository();
        var executionContext = new FakeExecutionContext();
        var projectNumberGenerator = new RandomProjectNumberGenerator();
        var handler = new CreateProjectCommandHandler(repository, uow, executionContext, new SystemTime(), projectNumberGenerator);

        var projectId = Any.Guid();
        var cmd = CreateProjectCommand.Create(projectId, Any.Instance<string>(), Any.Instance<string>(), PieceworkType.TwelveTwo,
            Any.Instance<decimal>());

        await handler.Handle(cmd, CancellationToken.None);

        var project = await repository.GetById(projectId, CancellationToken.None);

        Assert.True(project.ProjectCreatedTime.Value > DateTimeOffset.Now.AddMinutes(-3));
        Assert.True(project.ProjectCreatedTime.Value < DateTimeOffset.Now.AddMinutes(3));
        Assert.Equal(executionContext.UserName, project.ProjectCreatedBy.Name);
        Assert.Equal(executionContext.UserId, project.ProjectCreatedBy.Id);
    }
}
