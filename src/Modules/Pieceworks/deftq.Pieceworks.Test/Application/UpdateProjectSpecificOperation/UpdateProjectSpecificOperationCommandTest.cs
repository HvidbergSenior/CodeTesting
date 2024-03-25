using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Application.RegisterProjectSpecificOperation;
using deftq.Pieceworks.Application.UpdateProjectSpecificOperation;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectSpecificOperation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateProjectSpecificOperation
{
    public class UpdateProjectSpecificOperationCommandTest
    {
        private FakeUnitOfWork fakeUnitOfWork;
        private IProjectRepository projectRepository;
        private IExecutionContext executionContext;
        private IProjectFolderRootRepository projectFolderRootRepository;
        private ProjectSpecificOperationListInMemoryRepository projectSpecificOperationRepository;
        private IBaseRateAndSupplementRepository baseRateAndSupplementRepository;

        public UpdateProjectSpecificOperationCommandTest()
        {
            fakeUnitOfWork = new FakeUnitOfWork();
            projectRepository = new ProjectInMemoryRepository();
            executionContext = new FakeExecutionContext();
            projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            projectSpecificOperationRepository = new ProjectSpecificOperationListInMemoryRepository();
            baseRateAndSupplementRepository = new BaseRateAndSupplementInMemoryRepository();
        }
        
        [Fact]
        public async Task UpdateProjectSpecificOperationWithOnlyOperationTime()
        {
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            await projectSpecificOperationRepository.Add(ProjectSpecificOperationList.Create(project.ProjectId, Any.ProjectSpecificOperationListId()));

            var registerHandler = new RegisterProjectSpecificOperationCommandHandler(projectFolderRootRepository, baseRateAndSupplementRepository,
                projectSpecificOperationRepository,
                fakeUnitOfWork, new SystemTime());
            var registerCmd = RegisterProjectSpecificOperationCommand.Create(project.ProjectId.Value, Any.Guid(), "12345", "Grav et hul",
                String.Empty,
                2520000, 0);
            await registerHandler.Handle(registerCmd, CancellationToken.None);
            var projectSpecificOperationList = await projectSpecificOperationRepository.GetByProjectId(project.ProjectId.Value, CancellationToken.None);
            var list = projectSpecificOperationList.ProjectSpecificOperations;
            var operationId = list.First().ProjectSpecificOperationId;

            var updatehandler = new UpdateProjectSpecificOperationCommandHandler(projectFolderRootRepository, baseRateAndSupplementRepository,
                projectSpecificOperationRepository, fakeUnitOfWork, new SystemTime());
            var updateCmd = UpdateProjectSpecificOperationCommand.Create(project.ProjectId.Value, operationId.Value, "54321", "fyld hullet",
                "Forkert sted", 420000, 0);
            await updatehandler.Handle(updateCmd, CancellationToken.None);
            projectSpecificOperationList = await projectSpecificOperationRepository.GetByProjectId(project.ProjectId.Value, CancellationToken.None);
            list = projectSpecificOperationList.ProjectSpecificOperations;
            var updatedOperation = list.First();
            
            Assert.Equal(operationId, updatedOperation.ProjectSpecificOperationId);
            Assert.Equal("54321", updatedOperation.ProjectSpecificOperationExtraWorkAgreementNumber.Value);
            Assert.Equal("fyld hullet", updatedOperation.ProjectSpecificOperationName.Value);
            Assert.Equal("Forkert sted", updatedOperation.ProjectSpecificOperationDescription.Value);
            Assert.Equal(420000, updatedOperation.OperationTime.Value);
            Assert.Equal(0, updatedOperation.WorkingTime.Value);
        }
        
        [Fact]
        public async Task UpdateProjectSpecificOperationWithOnlyWorkingTime()
        {
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            var projectFolderRoot = Any.ProjectFolderRoot(project.ProjectId);
            await projectFolderRootRepository.Add(projectFolderRoot);

            await projectSpecificOperationRepository.Add(ProjectSpecificOperationList.Create(project.ProjectId, Any.ProjectSpecificOperationListId()));

            var registerHandler = new RegisterProjectSpecificOperationCommandHandler(projectFolderRootRepository, baseRateAndSupplementRepository,
                projectSpecificOperationRepository,
                fakeUnitOfWork, new SystemTime());
            var registerCmd = RegisterProjectSpecificOperationCommand.Create(project.ProjectId.Value, Any.Guid(), "12345", "Grav et hul",
                String.Empty,
                0, 2520000);
            await registerHandler.Handle(registerCmd, CancellationToken.None);
            var projectSpecificOperationList = await projectSpecificOperationRepository.GetByProjectId(project.ProjectId.Value, CancellationToken.None);
            var list = projectSpecificOperationList.ProjectSpecificOperations;
            var operationId = list.First().ProjectSpecificOperationId;

            var updatehandler = new UpdateProjectSpecificOperationCommandHandler(projectFolderRootRepository, baseRateAndSupplementRepository,
                projectSpecificOperationRepository, fakeUnitOfWork, new SystemTime());
            var updateCmd = UpdateProjectSpecificOperationCommand.Create(project.ProjectId.Value, operationId.Value, "54321", "fyld hullet",
                "Forkert sted", 0, 420000);
            await updatehandler.Handle(updateCmd, CancellationToken.None);
            projectSpecificOperationList = await projectSpecificOperationRepository.GetByProjectId(project.ProjectId.Value, CancellationToken.None);
            list = projectSpecificOperationList.ProjectSpecificOperations;
            var updatedOperation = list.First();
            
            Assert.Equal(operationId, updatedOperation.ProjectSpecificOperationId);
            Assert.Equal("54321", updatedOperation.ProjectSpecificOperationExtraWorkAgreementNumber.Value);
            Assert.Equal("fyld hullet", updatedOperation.ProjectSpecificOperationName.Value);
            Assert.Equal("Forkert sted", updatedOperation.ProjectSpecificOperationDescription.Value);
            Assert.Equal(238853.50m, updatedOperation.OperationTime.Value, 2);
            Assert.Equal(420000, updatedOperation.WorkingTime.Value);
        }
        
        [Fact]
        public async Task UpdateProjectSpecificOperationWithNoTimesExpectException()
        {
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            var projectFolderRoot = Any.ProjectFolderRoot(project.ProjectId);
            await projectFolderRootRepository.Add(projectFolderRoot);

            await projectSpecificOperationRepository.Add(ProjectSpecificOperationList.Create(project.ProjectId, Any.ProjectSpecificOperationListId()));

            var registerHandler = new RegisterProjectSpecificOperationCommandHandler(projectFolderRootRepository, baseRateAndSupplementRepository,
                projectSpecificOperationRepository,
                fakeUnitOfWork, new SystemTime());
            var registerCmd = RegisterProjectSpecificOperationCommand.Create(project.ProjectId.Value, Any.Guid(), "12345", "Grav et hul",
                String.Empty,
                0, 2520000);
            await registerHandler.Handle(registerCmd, CancellationToken.None);
            var projectSpecificOperationList = await projectSpecificOperationRepository.GetByProjectId(project.ProjectId.Value, CancellationToken.None);
            var list = projectSpecificOperationList.ProjectSpecificOperations;
            var operationId = list.First().ProjectSpecificOperationId;

            var updatehandler = new UpdateProjectSpecificOperationCommandHandler(projectFolderRootRepository, baseRateAndSupplementRepository,
                projectSpecificOperationRepository, fakeUnitOfWork, new SystemTime());
            var updateCmd = UpdateProjectSpecificOperationCommand.Create(project.ProjectId.Value, operationId.Value, "12345", "Grav et hul",
                String.Empty, 0, 0);
            await Assert.ThrowsAsync<ArgumentException>(() => updatehandler.Handle(updateCmd, CancellationToken.None));
            
        }
    }
}
