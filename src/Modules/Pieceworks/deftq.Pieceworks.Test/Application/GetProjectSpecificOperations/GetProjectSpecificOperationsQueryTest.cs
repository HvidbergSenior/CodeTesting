using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Application.GetProjectSpecificOperations;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectSpecificOperation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectSpecificOperations
{
    public class GetProjectSpecificOperationsQueryTest
    {
        private IProjectRepository _projectRepository;
        private IExecutionContext _executionContext;
        private IProjectFolderRootRepository _projectFolderRootRepository;
        private ProjectSpecificOperationListInMemoryRepository _projectSpecificOperationListInMemoryRepository;
        private IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private ISystemTime _systemTime;

        public GetProjectSpecificOperationsQueryTest()
        {
            _projectRepository = new ProjectInMemoryRepository();
            _executionContext = new FakeExecutionContext();
            _projectFolderRootRepository = new ProjectFolderRootInMemoryRepository();
            _projectSpecificOperationListInMemoryRepository = new ProjectSpecificOperationListInMemoryRepository();
            _baseRateAndSupplementRepository = new BaseRateAndSupplementInMemoryRepository();
            _systemTime = new SystemTime();
        }

        [Fact]
        public async Task Register3ProjectSpecificOperationsThenGetList()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectRepository.Add(project);
            await _projectFolderRootRepository.Add(Any.ProjectFolderRoot(project.ProjectId));

            var projectSpecificOperationList = ProjectSpecificOperationList.Create(project.ProjectId, Any.ProjectSpecificOperationListId());
            projectSpecificOperationList.AddProjectSpecificOperation(Any.ProjectSpecificOperation("12345", "Grav et hul", "Ud med jorden", 35000, 25000));
            projectSpecificOperationList.AddProjectSpecificOperation(Any.ProjectSpecificOperation("34567", "Fyld et gravet hul", "Ind med jorden",
                53000, 25000));
            projectSpecificOperationList.AddProjectSpecificOperation(Any.ProjectSpecificOperation("98765", "Ryd op", "", 10000, 25000));
            await _projectSpecificOperationListInMemoryRepository.Add(projectSpecificOperationList);

            var handler = new GetProjectSpecificOperationsQueryHandler(_projectFolderRootRepository, _baseRateAndSupplementRepository,
                _projectSpecificOperationListInMemoryRepository, _executionContext, _systemTime);
            var cmd = GetProjectSpecificOperationsQuery.Create(project.ProjectId);
            var res = await handler.Handle(cmd, CancellationToken.None);

            Assert.Equal(3, res.ProjectSpecificOperations.Count());

            var operation12345 = res.ProjectSpecificOperations.FirstOrDefault(o => o.ExtraWorkAgreementNumber.Equals("12345"));
            var operation34567 = res.ProjectSpecificOperations.FirstOrDefault(o => o.ExtraWorkAgreementNumber.Equals("34567"));
            var operation98765 = res.ProjectSpecificOperations.FirstOrDefault(o => o.ExtraWorkAgreementNumber.Equals("98765"));

            Assert.NotNull(operation12345);
            Assert.NotNull(operation34567);
            Assert.NotNull(operation98765);

            Assert.Equal("Grav et hul", operation12345?.Name);
            Assert.Equal("Fyld et gravet hul", operation34567?.Name);
            Assert.Equal("Ryd op", operation98765?.Name);

            Assert.Equal("Ud med jorden", operation12345?.Description);
            Assert.Equal("Ind med jorden", operation34567?.Description);
            Assert.Equal("", operation98765?.Description);

            Assert.Equal(35000, operation12345?.OperationTimeMs);
            Assert.Equal(53000, operation34567?.OperationTimeMs);
            Assert.Equal(10000, operation98765?.OperationTimeMs);

            Assert.Equal(25000, operation12345?.WorkingTimeMs);
            Assert.Equal(25000, operation34567?.WorkingTimeMs);
            Assert.Equal(25000, operation98765?.WorkingTimeMs);

            Assert.Equal(3.67m, operation12345!.Payment, 2);
            Assert.Equal(5.56m, operation34567!.Payment, 2);
            Assert.Equal(1.05m, operation98765!.Payment, 2);
        }

        [Fact]
        public async Task RegisterNoProjectSpecificOperationGetEmptyList()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectRepository.Add(project);
            await _projectFolderRootRepository.Add(Any.ProjectFolderRoot(project.ProjectId));

            var projectSpecificOperationList = ProjectSpecificOperationList.Create(project.ProjectId, Any.ProjectSpecificOperationListId());
            await _projectSpecificOperationListInMemoryRepository.Add(projectSpecificOperationList);

            var handler = new GetProjectSpecificOperationsQueryHandler(_projectFolderRootRepository, _baseRateAndSupplementRepository,
                _projectSpecificOperationListInMemoryRepository, _executionContext, _systemTime);
            var cmd = GetProjectSpecificOperationsQuery.Create(project.ProjectId);
            var res = await handler.Handle(cmd, CancellationToken.None);

            Assert.Empty(res.ProjectSpecificOperations);
        }
    }
}
