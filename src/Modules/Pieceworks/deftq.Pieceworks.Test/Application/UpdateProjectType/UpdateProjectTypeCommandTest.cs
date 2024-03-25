using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectType;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateProjectType
{
    public class UpdateProjectTypeCommandTest
    {
        [Fact]
        public async Task UpdateProjectType_ShouldUpdate()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectInMemoryRepository();

            var project = Any.Project();
            await repository.Add(project);

            var handler = new UpdateProjectTypeCommandHandler(repository, uow);
            var pieceWorkTypeValue = PieceworkType.TwelveTwo;
            var startDateValue = ProjectStartDate.Create(DateOnly.FromDayNumber(39));
            var endDateValue = ProjectEndDate.Create(DateOnly.FromDayNumber(50));
            var lumpSumPaymentValue = ProjectLumpSumPayment.Create(100);

            var cmd = UpdateProjectTypeCommand.Create(project.ProjectId.Value, pieceWorkTypeValue, startDateValue, endDateValue, lumpSumPaymentValue);
            await handler.Handle(cmd, CancellationToken.None);
            
            project = await repository.GetById(project.Id);
            
            Assert.Equal(ProjectPieceworkType.TwelveTwo, project.ProjectPieceworkType);
            Assert.Equal(100, project.ProjectLumpSumPaymentDkr.Value);
            
            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
        }
    }
}
