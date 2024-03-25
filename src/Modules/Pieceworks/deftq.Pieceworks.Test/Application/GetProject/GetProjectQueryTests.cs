using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetProject;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.project.Company;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Infrastructure;
using FluentAssertions;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProject
{
    public class GetProjectQueryTests
    {
        [Fact]
        public Task Should_Throw_NotFoundException()
        {
            var query = GetProjectQuery.Create(ProjectId.Create(Guid.NewGuid()));
            var repository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var handler = new GetProjectQueryHandler(repository, executionContext);

            return Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Should_Project_with_correct_user_role()
        {
            var repository = new ProjectInMemoryRepository();

            var projectOwner = ProjectOwner.Create("John Doe", Guid.NewGuid());
            var projectParticipant = Any.ProjectParticipant();
            var projectLumpSumPaymentDkr = ProjectLumpSumPayment.Empty();
            var projectStartDate = ProjectStartDate.Empty();
            var projectEndDate = ProjectEndDate.Empty();
            var projectCreatedBy = ProjectCreatedBy.Empty();
            var projectCreatedTime = ProjectCreatedTime.Empty();
            var projectCompany = ProjectCompany.Empty();
            var project = Project.Create(Any.ProjectId(), Any.ProjectName(), Any.ProjectNumber(), Any.ProjectPieceWorkNumber(),
                Any.ProjectOrderNumber(), Any.ProjectDescription(), projectOwner, Any.ProjectPieceworkType(), projectLumpSumPaymentDkr,
                projectStartDate, projectEndDate, projectCreatedBy, projectCreatedTime, projectCompany);
            project.AddProjectParticipant(projectParticipant);
            await repository.Add(project);

            // Check that the project owner gets the user role ProjectOwner 
            var executionContext = new FakeExecutionContext(projectOwner.Id);
            var query = GetProjectQuery.Create(project.ProjectId);
            var handler = new GetProjectQueryHandler(repository, executionContext);
            var dto = await handler.Handle(query, CancellationToken.None);
            dto.CurrentUserRole.Should().Be(ProjectRole.ProjectOwner);

            // Check that a project participant gets the user role ProjectParticipant 
            executionContext = new FakeExecutionContext(projectParticipant.Id);
            handler = new GetProjectQueryHandler(repository, executionContext);
            dto = await handler.Handle(query, CancellationToken.None);
            dto.CurrentUserRole.Should().Be(ProjectRole.ProjectParticipant);

            // Check that a unaffiliated user results in an exception
            executionContext = new FakeExecutionContext(Guid.NewGuid());
            handler = new GetProjectQueryHandler(repository, executionContext);
            await Assert.ThrowsAsync<UserNotAffiliatedWithProjectException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Should_Return_Project()
        {
            var projectOwner = ProjectOwner.Create("John Doe", Guid.NewGuid());
            var repository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext(projectOwner.Id);
            var project = Any.Project().OwnedBy(projectOwner);
            await repository.Add(project);
            var query = GetProjectQuery.Create(project.ProjectId);
            var handler = new GetProjectQueryHandler(repository, executionContext);
            var dto = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(query.ProjectId.Value, dto.Id);
        }
    }
}
