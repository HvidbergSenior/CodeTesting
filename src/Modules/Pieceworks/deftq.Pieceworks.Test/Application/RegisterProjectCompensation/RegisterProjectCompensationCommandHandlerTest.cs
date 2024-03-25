using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterProjectCompensation;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RegisterProjectCompensation
{
    public class RegisterProjectCompensationCommandHandlerTest
    {
        [Fact]
        public async Task WhenRegisteringCompensation_CompensationIsInCompensationList()
        {
            var fakeUnitOfWork = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var compensationRepository = new ProjectCompensationListInMemoryRepository();
            var project = Any.Project().WithParticipant(Guid.NewGuid());
            await projectRepository.Add(project , CancellationToken.None);
            await compensationRepository.Add(ProjectCompensationList.Create(project.ProjectId, Any.ProjectCompensationListId()));

            var compensationId = Any.Guid();
            var compensationPayment = 394;
            var compensationStartDate = ProjectCompensationDate.Create(DateOnly.MinValue);
            var compensationEndDate = ProjectCompensationDate.Create(DateOnly.MaxValue);

            var handler = new RegisterProjectCompensationCommandHandler(projectRepository, compensationRepository, fakeUnitOfWork);
            var cmd = RegisterProjectCompensationCommand.Create(project.ProjectId.Value, compensationId,
                new List<Guid>() { project.ProjectParticipants[0].ParticipantId.Value }, compensationPayment, compensationStartDate,
                compensationEndDate);
            await handler.Handle(cmd, CancellationToken.None);

            var compensationList = await compensationRepository.GetByProjectId(project.ProjectId.Value);

            Assert.Single(compensationRepository.Entities);
            Assert.True(compensationRepository.SaveChangesCalled);
            Assert.True(fakeUnitOfWork.IsCommitted);

            Assert.Equal(compensationId, compensationList.Compensations[0].ProjectCompensationId.Id);
            Assert.Equal(compensationPayment, compensationList.Compensations[0].ProjectCompensationPayment.Value);
            Assert.Equal(compensationStartDate, compensationList.Compensations[0].ProjectCompensationPeriod.StartDate);
            Assert.Equal(compensationEndDate, compensationList.Compensations[0].ProjectCompensationPeriod.EndDate);
        }
        
        [Fact]
        public async Task WhenRegisteringCompensationWithOwner_CompensationIsInCompensationList()
        {
            var fakeUnitOfWork = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var compensationRepository = new ProjectCompensationListInMemoryRepository();
            var project = Any.Project();
            await projectRepository.Add(project , CancellationToken.None);
            await compensationRepository.Add(ProjectCompensationList.Create(project.ProjectId, Any.ProjectCompensationListId()));

            var compensationId = Any.Guid();
            var compensationPayment = 394;
            var compensationStartDate = ProjectCompensationDate.Create(DateOnly.MinValue);
            var compensationEndDate = ProjectCompensationDate.Create(DateOnly.MaxValue);
            var projectOwner = project.ProjectOwner;

            var handler = new RegisterProjectCompensationCommandHandler(projectRepository, compensationRepository, fakeUnitOfWork);
            var cmd = RegisterProjectCompensationCommand.Create(project.ProjectId.Value, compensationId,
                new List<Guid>() { projectOwner.Id }, compensationPayment, compensationStartDate,
                compensationEndDate);
            await handler.Handle(cmd, CancellationToken.None);

            var compensationList = await compensationRepository.GetByProjectId(project.ProjectId.Value);

            Assert.Single(compensationRepository.Entities);
            Assert.True(compensationRepository.SaveChangesCalled);
            Assert.True(fakeUnitOfWork.IsCommitted);

            Assert.Equal(compensationId, compensationList.Compensations[0].ProjectCompensationId.Id);
            Assert.Equal(compensationPayment, compensationList.Compensations[0].ProjectCompensationPayment.Value);
            Assert.Equal(compensationStartDate, compensationList.Compensations[0].ProjectCompensationPeriod.StartDate);
            Assert.Equal(compensationEndDate, compensationList.Compensations[0].ProjectCompensationPeriod.EndDate);
        }
        
        [Fact]
        public async Task WhenRegisteringCompensationWithoutParticipants_ShouldThrow()
        {
            var fakeUnitOfWork = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var compensationRepository = new ProjectCompensationListInMemoryRepository();
            var project = Any.Project();
            await projectRepository.Add(project , CancellationToken.None);
            await compensationRepository.Add(ProjectCompensationList.Create(project.ProjectId, Any.ProjectCompensationListId()));

            var compensationId = Any.Guid();
            var compensationPayment = 394;
            var compensationStartDate = ProjectCompensationDate.Create(DateOnly.MinValue);
            var compensationEndDate = ProjectCompensationDate.Create(DateOnly.MaxValue);

            var handler = new RegisterProjectCompensationCommandHandler(projectRepository, compensationRepository, fakeUnitOfWork);
            var cmd = RegisterProjectCompensationCommand.Create(project.ProjectId.Value, compensationId,
                new List<Guid>(), compensationPayment, compensationStartDate,
                compensationEndDate);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }
        
        [Fact]
        public async Task WhenRegisteringCompensationWithProjectManager_ShouldThrow()
        {
            var fakeUnitOfWork = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var compensationRepository = new ProjectCompensationListInMemoryRepository();
            var project = Any.Project().WithProjectManager(Guid.NewGuid());
            await projectRepository.Add(project , CancellationToken.None);
            await compensationRepository.Add(ProjectCompensationList.Create(project.ProjectId, Any.ProjectCompensationListId()));

            var compensationId = Any.Guid();
            var compensationPayment = 394;
            var compensationStartDate = ProjectCompensationDate.Create(DateOnly.MinValue);
            var compensationEndDate = ProjectCompensationDate.Create(DateOnly.MaxValue);

            var handler = new RegisterProjectCompensationCommandHandler(projectRepository, compensationRepository, fakeUnitOfWork);
            var cmd = RegisterProjectCompensationCommand.Create(project.ProjectId.Value, compensationId,
                new List<Guid>() { project.ProjectManagers[0].Id}, compensationPayment, compensationStartDate,
                compensationEndDate);
            
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }
    }
}
