using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectInformation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateProjectName
{
    public class UpdateProjectInformationCommandTest
    {
        [Fact]
        public async Task UpdateProjectName_ShouldUpdate()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectInMemoryRepository();
            var handler = new UpdateProjectInformationCommandHandler(repository, uow);

            var project = Any.Project();
            await repository.Add(project, CancellationToken.None);

            var nameValue = Any.String(30);

            var cmd = UpdateProjectInformationCommand.Create(project.ProjectId.Value, nameValue, "409238", "230948", "23409");
            await handler.Handle(cmd, CancellationToken.None);

            project = await repository.GetById(project.Id);
            Assert.Equal(nameValue, project.ProjectName.Value);
        }

        [Fact]
        public async Task UpdateProjectDescription_ShouldUpdate()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectInMemoryRepository();
            var handler = new UpdateProjectInformationCommandHandler(repository, uow);

            var project = Any.Project();
            await repository.Add(project, CancellationToken.None);

            var descriptionValue = Any.String(30);

            var cmd = UpdateProjectInformationCommand.Create(project.ProjectId.Value, "name", descriptionValue, "230948", "23409");
            await handler.Handle(cmd, CancellationToken.None);

            project = await repository.GetById(project.Id);
            Assert.Equal(descriptionValue, project.ProjectDescription.Value);
        }

        [Fact]
        public async Task UpdateProjectPieceWorkNumberAndOrderNumber_ShouldUpdate()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectInMemoryRepository();
            var handler = new UpdateProjectInformationCommandHandler(repository, uow);

            var project = Any.Project();
            await repository.Add(project, CancellationToken.None);

            var pieceworkNumberValue = Any.String(8);
            var orderNumberValue = Any.String(10);

            var cmd = UpdateProjectInformationCommand.Create(project.ProjectId.Value, "name", "description", pieceworkNumberValue, orderNumberValue);
            await handler.Handle(cmd, CancellationToken.None);

            project = await repository.GetById(project.Id);
            Assert.Equal(pieceworkNumberValue, project.ProjectPieceWorkNumber.Value);
            Assert.Equal(orderNumberValue, project.ProjectOrderNumber.Value);
        }

        [Fact]
        public async Task UpdateProjectInformation_ShouldSaveAndCommitChanges()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectInMemoryRepository();
            var handler = new UpdateProjectInformationCommandHandler(repository, uow);

            var project = Any.Project();
            await repository.Add(project, CancellationToken.None);

            var nameValue = Any.String(30);
            var cmd = UpdateProjectInformationCommand.Create(project.ProjectId.Value, nameValue, "409238", "230948", "23409");
            await handler.Handle(cmd, CancellationToken.None);

            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
        }
    }
}
