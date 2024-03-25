using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using ProjectCreatedEventHandler = deftq.Pieceworks.Domain.projectExtraWorkAgreement.ProjectCreatedEventHandler;

namespace deftq.Pieceworks.Test.Domain.ExtraWorkAgreement
{
    public class ProjectCreatedEventHandlerTest
    {
        [Fact]
        public async Task Should_Create_ExtraWorkAgreementList_When_Project_Is_Created()
        {
            var projectId = Any.ProjectId();
            var extraWorkAgreementListRepository = new ProjectExtraWorkAgreementListInMemoryRepository();
            var evt = ProjectCreatedDomainEvent.Create(projectId, Any.ProjectName(), Any.ProjectDescription(), Any.ProjectOwner(),
                ProjectPieceworkType.TwelveTwo);

            var handler = new ProjectCreatedEventHandler(extraWorkAgreementListRepository);
            await handler.Handle(evt, CancellationToken.None);

            var extraWorkAgreementListFromRepository = await extraWorkAgreementListRepository.GetByProjectId(projectId.Value, CancellationToken.None);

            Assert.Equal(projectId, extraWorkAgreementListFromRepository.ProjectId);
        }
    }
}
