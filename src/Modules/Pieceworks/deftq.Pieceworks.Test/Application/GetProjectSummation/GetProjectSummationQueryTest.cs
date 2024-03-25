using deftq.Pieceworks.Application.GetProjectSummation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Infrastructure;
using deftq.Pieceworks.Test.Domain.Calculation;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectSummation
{
    public class GetProjectSummationQueryTest
    {
        private readonly ProjectInMemoryRepository _projectInMemoryRepository;
        private readonly ProjectFolderRootInMemoryRepository _projectFolderRootInMemoryRepository;
        private readonly ProjectLogBookInMemoryRepository _projectLogBookInMemoryRepository;
        private readonly ProjectFolderWorkInMemoryRepository _projectFolderWorkInMemoryRepository;
        private ProjectExtraWorkAgreementListInMemoryRepository _projectExtraWorkAgreementListInMemoryRepository;

        public GetProjectSummationQueryTest()
        {
            _projectInMemoryRepository = new ProjectInMemoryRepository();
            _projectFolderRootInMemoryRepository = new ProjectFolderRootInMemoryRepository();
            _projectLogBookInMemoryRepository = new ProjectLogBookInMemoryRepository();
            _projectFolderWorkInMemoryRepository = new ProjectFolderWorkInMemoryRepository();
            _projectExtraWorkAgreementListInMemoryRepository = new ProjectExtraWorkAgreementListInMemoryRepository();
        }

        [Fact]
        public async Task GivenEmptyProject_WhenGettingSummation_AllSumsAreZero()
        {
            var project = Any.Project();
            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, Any.ProjectName(), Any.ProjectFolderRootId(),
                FolderRateAndSupplement.InheritAll());
            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), project.ProjectId, ProjectFolderRoot.RootFolderId);
            var logBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var extraWorkAgreements = ProjectExtraWorkAgreementList.Create(project.ProjectId, Any.ProjectExtraWorkAgreementListId());
            
            await _projectInMemoryRepository.Add(project);
            await _projectFolderRootInMemoryRepository.Add(folderRoot);
            await _projectFolderWorkInMemoryRepository.Add(folderWork);
            await _projectLogBookInMemoryRepository.Add(logBook);
            await _projectExtraWorkAgreementListInMemoryRepository.Add(extraWorkAgreements);

            var query = GetProjectSummationQuery.Create(project.ProjectId);
            
            var handler = new GetProjectSummationQueryHandler(_projectInMemoryRepository, _projectFolderRootInMemoryRepository,
                _projectFolderWorkInMemoryRepository, _projectLogBookInMemoryRepository, _projectExtraWorkAgreementListInMemoryRepository);

            var projectSummationQueryResponse = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(0, projectSummationQueryResponse.TotalPaymentDkr);
            Assert.Equal(0, projectSummationQueryResponse.TotalCalculationSumDkr);
            Assert.Equal(0, projectSummationQueryResponse.TotalLogBookHours);
            Assert.Equal(0, projectSummationQueryResponse.TotalLumpSumDkr);
            Assert.Equal(0, projectSummationQueryResponse.TotalExtraWorkAgreementDkr);
            Assert.Equal(0, projectSummationQueryResponse.TotalWorkItemPaymentDkr);
            Assert.Equal(0, projectSummationQueryResponse.TotalWorkItemExtraWorkPaymentDkr);
        }

        [Fact]
        public async Task GivenProjectWithLogBookHours_WhenGettingSummation_ClosedHoursAreSummed()
        {
            var project = Any.Project();
            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, Any.ProjectName(), Any.ProjectFolderRootId(),
                FolderRateAndSupplement.InheritAll());
            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), project.ProjectId, ProjectFolderRoot.RootFolderId);
            var logBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var days = new List<ProjectLogBookDay>
            {
                ProjectLogBookDay.Create(LogBookDate.Create(new DateOnly(2023, 2, 15)),
                    LogBookTime.Create(LogBookHours.Create(7), LogBookMinutes.Create(30)))
            };
            var extraWorkAgreements = ProjectExtraWorkAgreementList.Create(project.ProjectId, Any.ProjectExtraWorkAgreementListId());

            // 1st user
            var logBookUser1 = Any.ProjectLogBookUser();
            logBook.RegisterWeek(logBookUser1, LogBookYear.Create(2023), LogBookWeek.Create(7), LogBookNote.Empty(), days);
            logBook.CloseWeek(logBookUser1, LogBookYear.Create(2023), LogBookWeek.Create(7));
            logBook.SumClosedHours(logBookUser1);

            // 2nd user
            var logBookUser2 = Any.ProjectLogBookUser();
            logBook.RegisterWeek(logBookUser2, LogBookYear.Create(2023), LogBookWeek.Create(7), LogBookNote.Empty(), days);
            logBook.CloseWeek(logBookUser2, LogBookYear.Create(2023), LogBookWeek.Create(7));
            logBook.SumClosedHours(logBookUser2);

            // 3rd user, not closing the week
            var logBookUser3 = Any.ProjectLogBookUser();
            logBook.RegisterWeek(logBookUser3, LogBookYear.Create(2023), LogBookWeek.Create(7), LogBookNote.Empty(), days);
            logBook.SumClosedHours(logBookUser3);

            await _projectInMemoryRepository.Add(project);
            await _projectFolderRootInMemoryRepository.Add(folderRoot);
            await _projectFolderWorkInMemoryRepository.Add(folderWork);
            await _projectLogBookInMemoryRepository.Add(logBook);
            await _projectExtraWorkAgreementListInMemoryRepository.Add(extraWorkAgreements);
            
            var query = GetProjectSummationQuery.Create(project.ProjectId);
            var handler = new GetProjectSummationQueryHandler(_projectInMemoryRepository, _projectFolderRootInMemoryRepository,
                _projectFolderWorkInMemoryRepository, _projectLogBookInMemoryRepository, _projectExtraWorkAgreementListInMemoryRepository);

            var projectSummationQueryResponse = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(15m, projectSummationQueryResponse.TotalLogBookHours);
        }

        [Fact]
        public async Task GivenProjectWithWorkItems_WhenGettingSummation_WorkItemsAreIncludedInPayment()
        {
            var project = Any.Project();
            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, Any.ProjectName(), Any.ProjectFolderRootId(),
                FolderRateAndSupplement.InheritAll());
            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), project.ProjectId, ProjectFolderRoot.RootFolderId);
            var logBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var extraWorkAgreements = ProjectExtraWorkAgreementList.Create(project.ProjectId, Any.ProjectExtraWorkAgreementListId());
            
            folderWork.AddWorkItem(
                WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), WorkItemText.Empty(),
                    WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(3), WorkItemDuration.Create(27000), WorkItemAmount.Create(7),
                    WorkItemUnit.Piece, new List<SupplementOperation>(), new List<Supplement>()),
                BaseRateAndSupplementTestUtil.GetDefaultBaseRateAndSupplementProxy());

            folderWork.AddWorkItem(
                WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), WorkItemText.Empty(),
                    WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(3), WorkItemDuration.Create(37000), WorkItemAmount.Create(9),
                    WorkItemUnit.Piece, new List<SupplementOperation>(), new List<Supplement>()),
                BaseRateAndSupplementTestUtil.GetDefaultBaseRateAndSupplementProxy());
            
            await _projectInMemoryRepository.Add(project);
            await _projectFolderRootInMemoryRepository.Add(folderRoot);
            await _projectFolderWorkInMemoryRepository.Add(folderWork);
            await _projectLogBookInMemoryRepository.Add(logBook);
            await _projectExtraWorkAgreementListInMemoryRepository.Add(extraWorkAgreements);

            var query = GetProjectSummationQuery.Create(project.ProjectId);
            var handler = new GetProjectSummationQueryHandler(_projectInMemoryRepository, _projectFolderRootInMemoryRepository,
                _projectFolderWorkInMemoryRepository, _projectLogBookInMemoryRepository, _projectExtraWorkAgreementListInMemoryRepository);

            var projectSummationQueryResponse = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(19.82m + 34.93m, projectSummationQueryResponse.TotalWorkItemPaymentDkr, 2);
            Assert.Equal(19.82m + 34.93m, projectSummationQueryResponse.TotalPaymentDkr, 2);
        }
        
        [Fact]
        public async Task GivenProjectWithExtraWork_WhenGettingSummation_PaymentIncludesExtraWork()
        {
            var project = Any.Project();
            var folderRoot = ProjectFolderRoot.Create(project.ProjectId, Any.ProjectName(), Any.ProjectFolderRootId(),
                FolderRateAndSupplement.InheritAll());
            var extraWorkFolder = Any.ProjectFolder();
            folderRoot.AddFolder(extraWorkFolder);
            folderRoot.MarkAsExtraWork(extraWorkFolder.ProjectFolderId);
            
            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), project.ProjectId, ProjectFolderRoot.RootFolderId);
            var extraWorkFolderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), project.ProjectId, extraWorkFolder.ProjectFolderId);
            var logBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var extraWorkAgreements = ProjectExtraWorkAgreementList.Create(project.ProjectId, Any.ProjectExtraWorkAgreementListId());
            
            folderWork.AddWorkItem(
                WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), WorkItemText.Empty(),
                    WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(3), WorkItemDuration.Create(27000), WorkItemAmount.Create(7),
                    WorkItemUnit.Piece, new List<SupplementOperation>(), new List<Supplement>()),
                BaseRateAndSupplementTestUtil.GetDefaultBaseRateAndSupplementProxy());

            extraWorkFolderWork.AddWorkItem(
                WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), WorkItemText.Empty(),
                    WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(3), WorkItemDuration.Create(37000), WorkItemAmount.Create(9),
                    WorkItemUnit.Piece, new List<SupplementOperation>(), new List<Supplement>()),
                BaseRateAndSupplementTestUtil.GetDefaultBaseRateAndSupplementProxy());
            
            await _projectInMemoryRepository.Add(project);
            await _projectFolderRootInMemoryRepository.Add(folderRoot);
            await _projectFolderWorkInMemoryRepository.Add(folderWork);
            await _projectFolderWorkInMemoryRepository.Add(extraWorkFolderWork);
            await _projectLogBookInMemoryRepository.Add(logBook);
            await _projectExtraWorkAgreementListInMemoryRepository.Add(extraWorkAgreements);

            var query = GetProjectSummationQuery.Create(project.ProjectId);
            var handler = new GetProjectSummationQueryHandler(_projectInMemoryRepository, _projectFolderRootInMemoryRepository,
                _projectFolderWorkInMemoryRepository, _projectLogBookInMemoryRepository, _projectExtraWorkAgreementListInMemoryRepository);

            var projectSummationQueryResponse = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(19.82m + 34.93m, projectSummationQueryResponse.TotalWorkItemPaymentDkr, 2);
            Assert.Equal(19.82m + 34.93m, projectSummationQueryResponse.TotalPaymentDkr, 2);
            Assert.Equal(34.93m, projectSummationQueryResponse.TotalWorkItemExtraWorkPaymentDkr, 2);
        }
    }
}
