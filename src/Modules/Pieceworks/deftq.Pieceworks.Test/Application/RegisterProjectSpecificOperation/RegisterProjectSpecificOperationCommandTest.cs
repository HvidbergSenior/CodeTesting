using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Application.RegisterProjectSpecificOperation;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectSpecificOperation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RegisterProjectSpecificOperation
{
    public class RegisterProjectSpecificOperationCommandTest
    {
        private FakeUnitOfWork fakeUnitOfWork;
        private IProjectRepository projectRepository;
        private IExecutionContext executionContext;
        private IProjectFolderRootRepository projectFolderRootRepository;
        private ProjectSpecificOperationListInMemoryRepository projectSpecificOperationRepository;
        private IBaseRateAndSupplementRepository baseRateAndSupplementRepository;

        public RegisterProjectSpecificOperationCommandTest()
        {
            fakeUnitOfWork = new FakeUnitOfWork();
            projectRepository = new ProjectInMemoryRepository();
            executionContext = new FakeExecutionContext();
            projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            projectSpecificOperationRepository = new ProjectSpecificOperationListInMemoryRepository();
            baseRateAndSupplementRepository = new BaseRateAndSupplementInMemoryRepository();
        }

        [Fact]
        public async Task RegisterProjectSpecificOperationWithOperationTimeTest()
        {
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            await projectSpecificOperationRepository.Add(ProjectSpecificOperationList.Create(project.ProjectId,
                Any.ProjectSpecificOperationListId()));

            var handler = new RegisterProjectSpecificOperationCommandHandler(projectFolderRootRepository, baseRateAndSupplementRepository,
                projectSpecificOperationRepository,
                fakeUnitOfWork, new SystemTime());
            var cmd = RegisterProjectSpecificOperationCommand.Create(project.ProjectId.Value, Any.Guid(), "12345", "Grav et hul", String.Empty,
                2520000, 0);
            await handler.Handle(cmd, CancellationToken.None);
            var projectSpecificOperationList =
                await projectSpecificOperationRepository.GetByProjectId(project.ProjectId.Value, CancellationToken.None);
            var list = projectSpecificOperationList.ProjectSpecificOperations;

            Assert.Single(projectSpecificOperationRepository.Entities);
            Assert.True(projectSpecificOperationRepository.SaveChangesCalled);
            Assert.True(fakeUnitOfWork.IsCommitted);

            Assert.Single(list);
            var item = list[0];
            Assert.Equal("12345", item.ProjectSpecificOperationExtraWorkAgreementNumber.Value);
            Assert.Equal("Grav et hul", item.ProjectSpecificOperationName.Value);
            Assert.Equal(string.Empty, item.ProjectSpecificOperationDescription.Value);
            Assert.False(item.OperationTime.IsEmpty());
            Assert.Equal(2520000, item.OperationTime.Value);
            Assert.Equal(0, item.WorkingTime.Value);
        }

        [Fact]
        public async Task RegisterProjectSpecificOperationWithWorkingTimeTest()
        {
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            var projectFolderRoot = Any.ProjectFolderRoot(project.ProjectId);
            await projectFolderRootRepository.Add(projectFolderRoot);

            await projectSpecificOperationRepository.Add(ProjectSpecificOperationList.Create(project.ProjectId,
                Any.ProjectSpecificOperationListId()));
            await projectFolderRootRepository.Add(Any.ProjectFolderRoot(project.ProjectId));

            var handler = new RegisterProjectSpecificOperationCommandHandler(projectFolderRootRepository, baseRateAndSupplementRepository,
                projectSpecificOperationRepository,
                fakeUnitOfWork, new SystemTime());
            var cmd = RegisterProjectSpecificOperationCommand.Create(project.ProjectId.Value, Any.Guid(), "12345", "Grav et hul", String.Empty, 0,
                2520000);
            await handler.Handle(cmd, CancellationToken.None);
            var projectSpecificOperationList =
                await projectSpecificOperationRepository.GetByProjectId(project.ProjectId.Value, CancellationToken.None);
            var list = projectSpecificOperationList.ProjectSpecificOperations;

            Assert.Single(list);
            var item = list[0];
            Assert.False(item.OperationTime.IsEmpty());
            Assert.Equal(1433121.02m, item.OperationTime.Value, 2);
            Assert.False(item.WorkingTime.IsEmpty());
            Assert.Equal(2520000, item.WorkingTime.Value);
        }
        
        [Fact]
        public async Task RegisterProjectSpecificOperationWithWorkingAndOperationTimeOnOperationIsSetTest()
        {
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            var projectFolderRoot = Any.ProjectFolderRoot(project.ProjectId);
            await projectFolderRootRepository.Add(projectFolderRoot);

            await projectSpecificOperationRepository.Add(ProjectSpecificOperationList.Create(project.ProjectId,
                Any.ProjectSpecificOperationListId()));
            await projectFolderRootRepository.Add(Any.ProjectFolderRoot(project.ProjectId));

            var handler = new RegisterProjectSpecificOperationCommandHandler(projectFolderRootRepository, baseRateAndSupplementRepository,
                projectSpecificOperationRepository,
                fakeUnitOfWork, new SystemTime());
            var cmd = RegisterProjectSpecificOperationCommand.Create(project.ProjectId.Value, Any.Guid(), "12345", "Grav et hul", String.Empty, 25000,
                25000);
            await handler.Handle(cmd, CancellationToken.None);
            var projectSpecificOperationList =
                await projectSpecificOperationRepository.GetByProjectId(project.ProjectId.Value, CancellationToken.None);
            var list = projectSpecificOperationList.ProjectSpecificOperations;

            Assert.Single(list);
            var item = list[0];
            Assert.False(item.OperationTime.IsEmpty());
            Assert.Equal(25000, item.OperationTime.Value, 2);
            Assert.True(item.WorkingTime.IsEmpty());
            Assert.Equal(0, item.WorkingTime.Value);
        }
        
        [Fact]
        public async Task RegisterProjectSpecificOperationWithNoTimesTest()
        {
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            var projectFolderRoot = Any.ProjectFolderRoot(project.ProjectId);
            await projectFolderRootRepository.Add(projectFolderRoot);

            await projectSpecificOperationRepository.Add(ProjectSpecificOperationList.Create(project.ProjectId,
                Any.ProjectSpecificOperationListId()));
            await projectFolderRootRepository.Add(Any.ProjectFolderRoot(project.ProjectId));

            var handler = new RegisterProjectSpecificOperationCommandHandler(projectFolderRootRepository, baseRateAndSupplementRepository,
                projectSpecificOperationRepository,
                fakeUnitOfWork, new SystemTime());
            var cmd = RegisterProjectSpecificOperationCommand.Create(project.ProjectId.Value, Any.Guid(), "12345", "Grav et hul", String.Empty, 0,
                0);
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(cmd, CancellationToken.None));
           
        }
    }
}
