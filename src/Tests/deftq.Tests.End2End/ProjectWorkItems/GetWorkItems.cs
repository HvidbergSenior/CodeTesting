using deftq.Pieceworks.Application.GetWorkItems;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.project.Company;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using Marten;
using Xunit;

namespace deftq.Tests.End2End.ProjectWorkItems
{
    [Collection("End2End")]
    public class GetWorkItemsTest
    {
        private readonly WebAppFixture fixture;
        private readonly Api _api;

        public GetWorkItemsTest(WebAppFixture webAppFixture)
        {
            fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task Should_Find_WorkItems_In_Folder()
        {
            var projectId = Guid.NewGuid();
            var folderId = Guid.NewGuid();
            await CreateProjectWithWorkItems(fixture, projectId, fixture.UserId, folderId, CancellationToken.None);

            // Get work item
            var response = await _api.GetWorkItems(projectId, folderId);
            Assert.Equal(1, response.WorkItems.Count);
        }

        [Fact]
        public async Task WorkItem_Should_Include_SupplementOperations()
        {
            var projectId = Guid.NewGuid();
            var folderId = Guid.NewGuid();
            await CreateProjectWithWorkItems(fixture, projectId, fixture.UserId, folderId, CancellationToken.None);

            // Get work item
            var response = await _api.GetWorkItems(projectId, folderId);

            // Assert work item contains supplement operations
            Assert.Equal(2, response.WorkItems[0].WorkItemMaterial?.SupplementOperations.Count);
            var operation1 = response.WorkItems[0].WorkItemMaterial?.SupplementOperations
                .First(op => op.Text.Equals("Sweep the floor", StringComparison.Ordinal));
            var operation2 = response.WorkItems[0].WorkItemMaterial?.SupplementOperations
                .First(op => op.Text.Equals("Dig a hole", StringComparison.Ordinal));
            Assert.Equal(1, operation1?.Amount);
            Assert.Equal(10000, operation1?.OperationTimeMilliseconds);
            Assert.Equal(WorkItemSupplementOperationResponse.WorkItemSupplementOperationType.AmountRelated, operation1?.OperationType);
            Assert.Equal(5, operation2?.Amount);
            Assert.Equal(12000, operation2?.OperationTimeMilliseconds);
            Assert.Equal(WorkItemSupplementOperationResponse.WorkItemSupplementOperationType.UnitRelated, operation2?.OperationType);
        }

        private static async Task CreateProjectWithWorkItems(WebAppFixture fixture, Guid projectId, Guid ownerId, Guid folderId,
            CancellationToken cancellationToken)
        {
            var project = Project.Create(ProjectId.Create(projectId), ProjectName.Empty(), ProjectNumber.Empty(), ProjectPieceWorkNumber.Empty(),
                ProjectOrderNumber.Empty(), ProjectDescription.Empty(), ProjectOwner.Create("", ownerId), ProjectPieceworkType.TwelveTwo,
                ProjectLumpSumPayment.Empty(), ProjectStartDate.Empty(), ProjectEndDate.Empty(), ProjectCreatedBy.Empty(),
                ProjectCreatedTime.Empty(), ProjectCompany.Empty());

            var projectFolderRoot =
                ProjectFolderRoot.Create(ProjectId.Create(projectId), project.ProjectName, ProjectFolderRootId.Create(Guid.NewGuid()),
                    FolderRateAndSupplement.InheritAll());

            projectFolderRoot.AddFolder(ProjectFolder.Create(ProjectFolderId.Create(folderId), ProjectFolderName.Empty(),
                ProjectFolderDescription.Empty(), ProjectFolderCreatedBy.Empty(), FolderRateAndSupplement.InheritAll(), ProjectFolderLock.Unlocked(),
                ProjectFolderExtraWork.Normal()));

            var folderWork =
                ProjectFolderWork.Create(ProjectFolderWorkId.Create(Guid.NewGuid()), project.ProjectId, ProjectFolderId.Create(folderId));

            var supplementOperation1 = SupplementOperation.Create(SupplementOperationId.Create(Guid.NewGuid()),
                CatalogSupplementOperationId.Create(Guid.NewGuid()),
                SupplementOperationText.Create("Sweep the floor"), SupplementOperationType.AmountRelated(), SupplementOperationTime.Create(10000),
                SupplementOperationAmount.Create(1));
            var supplementOperation2 = SupplementOperation.Create(SupplementOperationId.Create(Guid.NewGuid()),
                CatalogSupplementOperationId.Create(Guid.NewGuid()),
                SupplementOperationText.Create("Dig a hole"), SupplementOperationType.UnitRelated(), SupplementOperationTime.Create(12000),
                SupplementOperationAmount.Create(5));

            folderWork.AddWorkItem(WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), CatalogMaterialId.Create(Guid.NewGuid()),
                    WorkItemDate.Create(DateOnly.FromDateTime(DateTime.Now)), WorkItemUser.Empty(),
                    WorkItemText.Empty(), WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(3), WorkItemDuration.Empty(),
                    WorkItemAmount.Empty(), WorkItemUnit.Meter, new List<SupplementOperation> { supplementOperation1, supplementOperation2 },
                    new List<Supplement>()),
                new BaseRateAndSupplementProxy(BaseRateAndSupplement(), projectFolderRoot.RootFolder));

            var sessionFactory = (ISessionFactory?)fixture.AppFactory.Services.GetService(typeof(ISessionFactory))!;

            await using var documentSession = sessionFactory.OpenSession();
            documentSession.Store(project);
            documentSession.Store(projectFolderRoot);
            documentSession.Store(folderWork);

            await documentSession.SaveChangesAsync(cancellationToken);
        }

        private static BaseRateAndSupplement BaseRateAndSupplement()
        {
            var personalTimeSupplementIntervals = new List<PersonalTimeSupplementInterval>()
            {
                PersonalTimeSupplementInterval.Create(PersonalTimeSupplement.Create(6), DateInterval.Always())
            };
            var baseRateIntervals = new List<BaseRateInterval>() { BaseRateInterval.Create(BaseRate.Create(214.74m), DateInterval.Always()) };
            return Pieceworks.Domain.BaseRateAndSupplement.Create(IndirectTimeSupplement.Create(64), SiteSpecificTimeSupplement.Create(2),
                personalTimeSupplementIntervals, baseRateIntervals, BaseRateRegulation.Create(0));
        }
    }
}
