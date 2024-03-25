using Xunit;

namespace deftq.Tests.End2End.ProjectSpecificOperation
{
    [Collection("End2End")]
    public class ProjectSpecificOperationTest
    {
        private readonly Api _api;

        public ProjectSpecificOperationTest(WebAppFixture webAppFixture)
        {
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task GivenProject_WhenRegisterProjectSpecificOperationWithOperationTime_ThenGetOperations()
        {
            var projectId = await _api.CreateProject();
            await _api.RegisterProjectSpecificOperation(projectId, "12345", "Grav et hul", "Bare et eller andet sted", 2580000, 0);
            var res = await _api.GetProjectSpecificOperationsList(projectId);
            
            Assert.NotNull(res);
            Assert.Single(res.ProjectSpecificOperations);
            var operation = res.ProjectSpecificOperations.ToList().First();
            Assert.Equal(2580000, operation.OperationTimeMs);
            Assert.Equal(0, operation.WorkingTimeMs);
        }
        
        [Fact]
        public async Task GivenProject_WhenRegisterProjectSpecificOperationWithWorkingTime_ThenGetOperationsAndTimeIsCalculated()
        {
            var projectId = await _api.CreateProject();
            await _api.RegisterProjectSpecificOperation(projectId, "12345", "Grav et hul", "Bare et eller andet sted", 0, 258000);
            var res = await _api.GetProjectSpecificOperationsList(projectId);
            
            Assert.NotNull(res);
            Assert.Single(res.ProjectSpecificOperations);
            var operation = res.ProjectSpecificOperations.ToList().First();
            Assert.Equal( 146724.29m, operation.OperationTimeMs, 2);
            Assert.Equal(258000, operation.WorkingTimeMs);
        }

        [Fact]
        public async Task GivenProject_WhenNoProjectSpecificOperationRegistred_TheGetOperationsIsEmpty()
        {
            var projectId = await _api.CreateProject();
            var res = await _api.GetProjectSpecificOperationsList(projectId);
            
            Assert.NotNull(res);
            Assert.Empty(res.ProjectSpecificOperations);
        }
        
        [Fact]
        public async Task GivenProject_WhenRegisterMultiProjectSpecificOperations_ThenGetOperationsList()
        {
            var projectId = await _api.CreateProject();
            await _api.RegisterProjectSpecificOperation(projectId, "12345", "Grav et hul", "Bare et eller andet sted", 2580000, 0);
            await _api.RegisterProjectSpecificOperation(projectId, "34567", "Grav 2 huller", "Bare et andet sted", 3580000, 0);
            await _api.RegisterProjectSpecificOperation(projectId, "56789", "Grav 5 huller", "Bare et nyt sted", 5580000, 0);
            var res = await _api.GetProjectSpecificOperationsList(projectId);
            
            Assert.NotNull(res);
            Assert.Equal(3, res.ProjectSpecificOperations.Count());
        }
        
        [Fact]
        public async Task GivenProject_WhenUpdateProjectSpecificOperation_ThenGetOperationsList()
        {
            var projectId = await _api.CreateProject();
            await _api.RegisterProjectSpecificOperation(projectId, "12345", "Grav et hul", "Bare et eller andet sted", 2580000, 0);
            var res = await _api.GetProjectSpecificOperationsList(projectId);
            Assert.Single(res.ProjectSpecificOperations);
            var projectSpecificOperationId = res.ProjectSpecificOperations.First().ProjectSpecificOperationId;

            await _api.UpdateProjectSpecificOperation(projectId, projectSpecificOperationId, "54321", "fyld hullet", String.Empty, 0, 42000);
            res = await _api.GetProjectSpecificOperationsList(projectId);
            Assert.Single(res.ProjectSpecificOperations);
            var operation = res.ProjectSpecificOperations.First();
            Assert.Equal("54321", operation.ExtraWorkAgreementNumber);
            Assert.Equal("fyld hullet", operation.Name);
            Assert.Empty(operation.Description);
            Assert.Equal(23885.35m, operation.OperationTimeMs, 2);
            Assert.Equal(42000, operation.WorkingTimeMs);
        }

        [Fact]
        public async Task GivenProject_WhenRemoveProjectSpecificOperation_ThenGetOperationsList()
        {
            var projectId = await _api.CreateProject();
            await _api.RegisterProjectSpecificOperation(projectId, "12345", "Grav et hul", "Bare et eller andet sted", 0, 25000);
            await _api.RegisterProjectSpecificOperation(projectId, "54321", "Grav et andet hul", String.Empty, 2580000, 0);
            var res = await _api.GetProjectSpecificOperationsList(projectId);
            Assert.Equal(2, res.ProjectSpecificOperations.Count());
            res = await _api.GetProjectSpecificOperationsList(projectId);
            var opreation = res.ProjectSpecificOperations.First();

            await _api.DeleteProjectSpecificOperation(projectId, opreation.ProjectSpecificOperationId);
            res = await _api.GetProjectSpecificOperationsList(projectId);
            Assert.Single(res.ProjectSpecificOperations);
        }
    }
}
