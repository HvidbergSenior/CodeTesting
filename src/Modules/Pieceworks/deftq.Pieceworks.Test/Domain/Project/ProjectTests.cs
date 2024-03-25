using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Serialization;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.project.Company;
using deftq.Pieceworks.Domain.projectCompensation;
using FluentAssertions;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.project
{
    public class ProjectTests
    {
        [Fact]
        public void CanSerializeAndDeserialize()
        {
            var serializer = new JsonSerializer<Project>();
            var obj = Any.Project();
            var json = serializer.Serialize(obj);
            var result = serializer.Deserialize(json);
            Assert.Equal(obj.ProjectId.Value, result?.ProjectId.Value);
        }

        [Fact]
        public void When_Created_ShouldHaveTitle()
        {
            var projectId = Any.ProjectId();
            var projectName = Any.ProjectName();
            var projectNumber = Any.ProjectNumber();
            var projectOwner = Any.ProjectOwner();
            var projectDescription = Any.ProjectDescription();
            var projectLumpSumPaymentDkr = ProjectLumpSumPayment.Empty();
            var projectStartDate = ProjectStartDate.Empty();
            var projectEndDate = ProjectEndDate.Empty();
            var projectPieceworkNumber = ProjectPieceWorkNumber.Empty();
            var projectOrderNumber = ProjectOrderNumber.Empty();
            var projectCreator = ProjectCreatedBy.Empty();
            var projectCreationTime = ProjectCreatedTime.Empty();
            var projectCompany = ProjectCompany.Empty();
            var project = Project.Create(projectId, projectName, projectNumber, projectPieceworkNumber, projectOrderNumber, projectDescription,
                projectOwner, ProjectPieceworkType.TwelveOneA, projectLumpSumPaymentDkr, projectStartDate, projectEndDate, projectCreator,
                projectCreationTime, projectCompany);

            Assert.Equal(projectName, project.ProjectName);
        }

        [Fact]
        public void When_Created_ProjectOwner_Is_Set_As_The_User_That_Created()
        {
            var projectId = Any.ProjectId();
            var name = Any.ProjectName();
            var projectNumber = Any.ProjectNumber();
            var projectOwner = Any.ProjectOwner();
            var projectDescription = Any.ProjectDescription();
            var projectLumpSumPaymentDkr = ProjectLumpSumPayment.Empty();
            var projectStartDate = ProjectStartDate.Empty();
            var projectEndDate = ProjectEndDate.Empty();
            var projectCreatedBy = ProjectCreatedBy.Empty();
            var projectCreatedTime = ProjectCreatedTime.Empty();
            var projectCompany = ProjectCompany.Empty();
            var projectPieceworkNumber = ProjectPieceWorkNumber.Empty();
            var projectOrderNumber = ProjectOrderNumber.Empty();
            var project = Project.Create(projectId, name, projectNumber, projectPieceworkNumber, projectOrderNumber, projectDescription, projectOwner,
                ProjectPieceworkType.TwelveOneB, projectLumpSumPaymentDkr, projectStartDate, projectEndDate, projectCreatedBy, projectCreatedTime,
                projectCompany);


            Assert.Equal(projectOwner.Name, project.ProjectOwner.Name);
        }

        [Fact]
        public void WhenUpdatingPieceworkTypeToTwelveOneC_LumpSumPaymentShouldBeZero()
        {
            var projectId = Any.ProjectId();
            var name = Any.ProjectName();
            var projectNumber = Any.ProjectNumber();
            var projectOwner = Any.ProjectOwner();
            var projectDescription = Any.ProjectDescription();
            var projectLumpSumPaymentDkr = ProjectLumpSumPayment.Create(100);
            var projectStartDate = ProjectStartDate.Empty();
            var projectEndDate = ProjectEndDate.Empty();
            var projectCreatedBy = ProjectCreatedBy.Empty();
            var projectCreatedTime = ProjectCreatedTime.Empty();
            var projectPieceworkNumber = ProjectPieceWorkNumber.Empty();
            var projectOrderNumber = ProjectOrderNumber.Empty();
            var projectCompany = ProjectCompany.Empty();
            var project = Project.Create(projectId, name, projectNumber, projectPieceworkNumber, projectOrderNumber, projectDescription, projectOwner,
                ProjectPieceworkType.TwelveTwo, projectLumpSumPaymentDkr, projectStartDate, projectEndDate, projectCreatedBy, projectCreatedTime,
                projectCompany);

            project.UpdateProjectType(ProjectPieceworkType.TwelveOneC, ProjectLumpSumPayment.Create(100));

            Assert.Equal(0, project.ProjectLumpSumPaymentDkr.Value);
        }

        [Fact]
        public void WhenUpdatingPieceworkTypeToTwelveTwo_ShouldReturnLumpSumPayment()
        {
            var projectId = Any.ProjectId();
            var name = Any.ProjectName();
            var projectNumber = Any.ProjectNumber();
            var projectOwner = Any.ProjectOwner();
            var projectDescription = Any.ProjectDescription();
            var projectLumpSumPaymentDkr = ProjectLumpSumPayment.Empty();
            var projectStartDate = ProjectStartDate.Empty();
            var projectEndDate = ProjectEndDate.Empty();
            var projectPieceworkNumber = ProjectPieceWorkNumber.Empty();
            var projectOrderNumber = ProjectOrderNumber.Empty();
            var projectCreator = ProjectCreatedBy.Empty();
            var projectCreationTime = ProjectCreatedTime.Empty();
            var projectCompany = ProjectCompany.Empty();
            var project = Project.Create(projectId, name, projectNumber, projectPieceworkNumber, projectOrderNumber, projectDescription, projectOwner,
                ProjectPieceworkType.TwelveOneB, projectLumpSumPaymentDkr, projectStartDate, projectEndDate, projectCreator, projectCreationTime,
                projectCompany);

            project.UpdateProjectType(ProjectPieceworkType.TwelveTwo, ProjectLumpSumPayment.Create(100));

            Assert.Equal(100, project.ProjectLumpSumPaymentDkr.Value);
        }

        [Fact]
        public void When_Created_Should_Raise_Event()
        {
            var project = Any.Project();
            Assert.Single(project.DomainEvents);
        }

        [Fact]
        public void When_Created_Should_Raise_ProjectCreatedEvent()
        {
            var project = Any.Project();
            Assert.IsType<ProjectCreatedDomainEvent>(project.PublishedEvent<ProjectCreatedDomainEvent>());
        }

        [Fact]
        public void When_Removed_Should_Raise_ProjectRemovedEvent()
        {
            var project = Any.Project();
            project.RemoveProject();
            Assert.IsType<ProjectRemovedDomainEvent>(project.PublishedEvent<ProjectRemovedDomainEvent>());
        }

        [Fact]
        public void GivenProject_WhenAddingProjectManager_ProjectManagerIsInList()
        {
            // Given project
            var project = Any.Project();

            // When adding manager
            var projectManager = Any.ProjectManager();
            project.AddProjectManager(projectManager);

            // Then manager is in list
            project.ProjectManagers.Should().ContainSingle().Which.Should().Be(projectManager);
        }

        [Fact]
        public void GivenProject_WhenAddingParticipant_ParticipantIsInList()
        {
            // Given project
            var project = Any.Project();

            // When adding manager
            var participant = Any.ProjectParticipant();
            project.AddProjectParticipant(participant);

            // Then manager is in list
            project.ProjectParticipants.Should().ContainSingle().Which.Should().Be(participant);
        }
    }
}
