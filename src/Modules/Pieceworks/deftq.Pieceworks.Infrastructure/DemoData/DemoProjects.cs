using deftq.BuildingBlocks.DataAccess.InitialData;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.project.Company;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Domain.projectSpecificOperation;
using Marten;
using Marten.Services;

namespace deftq.Pieceworks.Infrastructure.DemoData
{
    public class DemoProjects : IDemoDataProvider
    {
        public async Task Populate(IDocumentStore documentStore, CancellationToken cancellation)
        {
            await ImportDemoProjectWithParticipants(documentStore, cancellation);
            await TheGreenWaveProject.ImportProject(documentStore, cancellation);
        }

        /// <summary>
        /// Import demo project owned by demo1@akkordplus.dk and with demo2@akkordplus.dk as participant
        /// Demo user 1: 24b8594e-2dbc-4fcb-8e38-481ff4d78b76
        /// Demo user 2: fb1b58b9-ae34-4f55-adf0-3814a6d3114c
        /// </summary>
#pragma warning disable MA0051
        private static async Task ImportDemoProjectWithParticipants(IDocumentStore documentStore, CancellationToken cancellation)
#pragma warning restore MA0051
        {
            await using var session = await documentStore.OpenSessionAsync(new SessionOptions(), cancellation);

            // Project
            var projectId = ProjectId.Create(Guid.Parse("335C5F5B-EC62-49C2-B745-5D3CC8733DD4"));
            var projectName = ProjectName.Create("Demo project");
            var projectDescription =
                ProjectDescription.Create("Projekt ejet af demo1@akkordplus.dk med demo2@akkordplus.dk og demo3@akkordplus.dk som deltagere");
            var projectOwner = ProjectOwner.Create("Demo user 1 (akkordholder)", Guid.Parse("24b8594e-2dbc-4fcb-8e38-481ff4d78b76"));
            var projectLumpSumPaymentDkr = ProjectLumpSumPayment.Empty();
            var projectStartDate = ProjectStartDate.Empty();
            var projectEndDate = ProjectEndDate.Empty();
            var projectPieceWorkNumber = ProjectPieceWorkNumber.Empty();
            var projectOrderNumber = ProjectOrderNumber.Empty();
            var projectNumber = ProjectNumber.Create(1);
            var projectCreator = ProjectCreatedBy.Create(projectOwner.Name, projectOwner.Id);
            var projectCreationTime = ProjectCreatedTime.Create(DateTimeOffset.Now);
            var projectCompany = ProjectCompany.Empty();
            var project = Project.Create(projectId, projectName, projectNumber, projectPieceWorkNumber, projectOrderNumber, projectDescription,
                projectOwner, ProjectPieceworkType.TwelveOneA,
                projectLumpSumPaymentDkr, projectStartDate, projectEndDate, projectCreator, projectCreationTime, projectCompany);
            project.AddProjectParticipant(ProjectParticipant.Create(ProjectParticipantId.Create(Guid.Parse("fb1b58b9-ae34-4f55-adf0-3814a6d3114c")),
                ProjectParticipantName.Create("Demo user 2"), ProjectEmail.Create("demo2@akkordplus.dk"),
                ProjectParticipantAddress.Create("Finlandsgde 10"), ProjectParticipantPhoneNumber.Create("89878685")));
            project.AddProjectParticipant(ProjectParticipant.Create(ProjectParticipantId.Create(Guid.Parse("b3d97f4b-7abc-4d76-b05e-ffb9bc229acf")), ProjectParticipantName.Create("Demo user 3"),
                ProjectEmail.Create("demo3@akkordplus.dk"), ProjectParticipantAddress.Create("Århusvej 222"), ProjectParticipantPhoneNumber.Create("23232424")));
            project.AddProjectManager(ProjectManager.Create(Guid.Parse("be0e85ff-ddbb-4284-8a4b-9e6cec85229e"), "Demo user 4 (projektleder)",
                ProjectEmail.Create("demo4@akkordplus.dk"), "Århusvej 222", "23232424"));
            await UpsertEntity(session, project);

            // Project folder root and folders
            var projectFolderRootId = ProjectFolderRootId.Create(Guid.Parse("11CB1E1F-9A49-4A57-99CE-827D15F3CE86"));
            var projectFolderRoot = ProjectFolderRoot.Create(projectId, projectName, projectFolderRootId, GetFolderRateAndSupplement());
            var folder1 = await CreateFolder(session, projectFolderRoot, "0F2CDDCA-F6F6-4C1F-ABBB-E291F2F94B71",
                "4FE97777-DD30-48B8-BE3A-ED9EA8C71DB6", "Bygning A", ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Normal(), null);
            var folder11 = await CreateFolder(session, projectFolderRoot, "549BA15C-048A-42D0-985A-3DDABE472E4E",
                "F643A6B3-29C2-4D33-AD88-905BBD0B1CCE", "1. Etage", ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Normal(),
                folder1.ProjectFolderId);
            var folder12 = await CreateFolder(session, projectFolderRoot, "7CF1CEE4-50B7-4D5D-805B-22C5ACE7D486",
                "CBEA994F-511B-450D-8A12-583D4DDBACCF", "2. Etage", ProjectFolderLock.Locked(), ProjectFolderExtraWork.Normal(),
                folder1.ProjectFolderId);
            var folder2 = await CreateFolder(session, projectFolderRoot, "3A355C13-2570-4016-B67A-C13C6F379CAB",
                "EE1C3386-12D3-4CC9-B362-8C1D77C78EAD", "Bygning B", ProjectFolderLock.Locked(), ProjectFolderExtraWork.Normal(), null);
            var folder21 = await CreateFolder(session, projectFolderRoot, "330E1FBA-A6D6-4304-8268-569E2898A65A",
                "CE35896F-189C-495D-80C8-E628C9DA0D67", "1. Etage", ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Normal(),
                folder2.ProjectFolderId);
            var folder22 = await CreateFolder(session, projectFolderRoot, "C894ABD3-8517-42C0-9983-6CA302B26F93",
                "86350592-B712-49BF-A5F4-3A4DE1BD56B5", "2. Etage", ProjectFolderLock.Locked(), ProjectFolderExtraWork.Normal(),
                folder2.ProjectFolderId);
            var folder3 = await CreateFolder(session, projectFolderRoot, "3DF3A4F2-5539-4B93-AC05-7E2CD4AAC469",
                "4C7C5B18-5299-45E4-8432-E2F9CF1EC9E1", "Bygning C", ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.ExtraWork(), null);
            var folder31 = await CreateFolder(session, projectFolderRoot, "98CD5ACD-4DBE-4B36-8946-C7941A75FCC2",
                "8AD61C1F-7BE9-4747-8ABF-385A0E9A8006", "1. Etage, ", ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Normal(),
                folder3.ProjectFolderId);
            var folder32 = await CreateFolder(session, projectFolderRoot, "97CD9B83-AD26-4EB1-BB89-ACA5BF8D239D",
                "7D505DF1-9BC9-4548-9B1A-DCFCED59B14D", "2. etage", ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Normal(),
                folder3.ProjectFolderId);
            await UpsertEntity(session, projectFolderRoot);

            // Log book
            var logBook = ProjectLogBook.Create(projectId, ProjectLogBookId.Create(Guid.Parse("649A5A21-88D9-4CAC-B54B-E757D4841C48")));
            await UpsertEntity(session, logBook);

            // Folder work
            var projectFolderWorkId = ProjectFolderWorkId.Create(Guid.Parse("E4513633-1D25-4FEF-BA03-E7D1A4F629D6"));
            var folderWork = ProjectFolderWork.Create(projectFolderWorkId, projectId, projectFolderRoot.RootFolder.ProjectFolderId);
            await UpsertEntity(session, folderWork);

            // Favorite list
            var favoriteListId = ProjectCatalogFavoriteListId.Create(Guid.Parse("3CB1584F-525C-4056-9404-A62F53BF406A"));
            var favoriteList = ProjectCatalogFavoriteList.Create(favoriteListId, projectId);
            favoriteList.AddFavorite(CatalogFavorite.Create(CatalogFavoriteId.Create(Guid.Parse("1F250D71-0008-4A6F-AE71-E7B600C2D7C0")),
                CatalogItemType.Material, CatalogItemId.Create(Guid.Parse("C6941EDA-001A-4740-A16B-BA739E576969")),
                CatalogItemNumber.Create("5703302153492"), CatalogItemText.Create("Fuga lampeudtag 4-l+j HV 509D6015"),
                CatalogItemUnit.Create("stk")));
            favoriteList.AddFavorite(CatalogFavorite.Create(CatalogFavoriteId.Create(Guid.Parse("FD66B75D-7CCF-419E-9D80-A11DF5FE9F20")),
                CatalogItemType.Operation, CatalogItemId.Create(Guid.Parse("E27F7E57-E374-4EAF-8D16-B73CA56EDF9B")),
                CatalogItemNumber.Create("050502000019"), CatalogItemText.Create("Firkanthul i gips o/15-30mm, o/300 t.o.m. 400cm2"),
                CatalogItemUnit.Create("stk")));
            await UpsertEntity(session, favoriteList);

            // Extra work agreement list
            var extraWorkAgreementListId = ProjectExtraWorkAgreementListId.Create(Guid.Parse("B2C8EC0C-4B43-45E3-BD3F-A0F398DCB94D"));
            var extraWorkAgreementList = ProjectExtraWorkAgreementList.Create(projectId, extraWorkAgreementListId);
            extraWorkAgreementList.SetCustomerRate(ProjectExtraWorkCustomerHourRate.Create(125.00m));
            extraWorkAgreementList.SetCompanyRate(ProjectExtraWorkAgreementCompanyHourRate.Create(214.95m));
            await UpsertEntity(session, extraWorkAgreementList);

            // Compensation list
            var compensationListGuid = ProjectCompensationListId.Create(Guid.Parse("42B80692-A8F9-4F1E-8ED7-29571A7C6A2D"));
            var compensationList = ProjectCompensationList.Create(projectId, compensationListGuid);
            await UpsertEntity(session, compensationList);
            
            // Project Specific Operations list
            var projectSpecificOperationsListId = ProjectSpecificOperationListId.Create(Guid.Parse("6372D5F3-80CB-475E-8167-B65F07469F2F"));
            var projectSpecificOperationsList = ProjectSpecificOperationList.Create(projectId, projectSpecificOperationsListId);
            await UpsertEntity(session, projectSpecificOperationsList);

            await session.SaveChangesAsync(cancellation);
        }

        private static async Task<ProjectFolder> CreateFolder(IDocumentSession session, ProjectFolderRoot projectFolderRoot, string folderGuid,
            string folderWorkGuid, string folderName, ProjectFolderLock locked, ProjectFolderExtraWork extraWork, ProjectFolderId? parentFolderId)
        {
            var folderId = ProjectFolderId.Create(Guid.Parse(folderGuid));
            var folder = ProjectFolder.Create(folderId, ProjectFolderName.Create(folderName), ProjectFolderDescription.Empty(),
                ProjectFolderCreatedBy.Empty(), FolderRateAndSupplement.InheritAll(), locked, extraWork);

            if (parentFolderId is not null)
            {
                projectFolderRoot.AddFolder(folder, parentFolderId);
            }
            else
            {
                projectFolderRoot.AddFolder(folder);
            }

            var projectFolderWorkId = ProjectFolderWorkId.Create(Guid.Parse(folderWorkGuid));
            var folderWork = ProjectFolderWork.Create(projectFolderWorkId, projectFolderRoot.ProjectId, folderId);
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(GetBaseRateAndSupplement(), folder);
            folderWork.AddWorkItem(
                WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), CatalogMaterialId.Create(Guid.Parse("C6941EDA-001A-4740-A16B-BA739E576969")),
                    WorkItemDate.Today(), WorkItemUser.Empty(),
                    WorkItemText.Create("Fuga lampeudtag 4-l+j HV 509D6015"), WorkItemEanNumber.Create("4511111111111"),
                    WorkItemMountingCode.FromCode(3),
                    WorkItemDuration.Create(45000), WorkItemAmount.Create(5), WorkItemUnit.Piece, new List<SupplementOperation>(),
                    new List<Supplement>()), baseRateAndSupplementProxy);
            folderWork.AddWorkItem(
                WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), CatalogOperationId.Create(Guid.Parse("064FEDF4-B6C0-47C2-A783-ADBFBCCBF716")),
                    WorkItemOperationNumber.Create("050401000001"),
                    WorkItemDate.Today(), WorkItemUser.Empty(),
                    WorkItemText.Create("Hul i beton <=10cm, Ø -10 mm"),
                    WorkItemDuration.Create(34800), WorkItemAmount.Create(11),
                    new List<Supplement>
                    {
                        Supplement.Create(SupplementId.Create(Guid.NewGuid()),
                            CatalogSupplementId.Create(Guid.Parse("CBBF2708-68F3-4B80-ABD7-0ACC3E910EC5")), SupplementNumber.Create("ht40"),
                            SupplementText.Create("Højdetillæg 7mtr. indtil 10mtr. 40%"), SupplementPercentage.Create(40))
                    }), baseRateAndSupplementProxy);
            await UpsertEntity(session, folderWork);

            return folder;
        }

        private static BaseRateAndSupplement GetBaseRateAndSupplement()
        {
            var personalTimeSupplementIntervals = new List<PersonalTimeSupplementInterval>()
            {
                PersonalTimeSupplementInterval.Create(PersonalTimeSupplement.Create(6), DateInterval.Always())
            };
            var baseRateIntervals = new List<BaseRateInterval>() { BaseRateInterval.Create(BaseRate.Create(214.74m), DateInterval.Always()) };
            var baseRateAndSupplement = BaseRateAndSupplement.Create(IndirectTimeSupplement.Create(64), SiteSpecificTimeSupplement.Create(2),
                personalTimeSupplementIntervals, baseRateIntervals, BaseRateRegulation.Create(0));
            return baseRateAndSupplement;
        }

        internal static FolderRateAndSupplement GetFolderRateAndSupplement()
        {
            return FolderRateAndSupplement.OverwriteAll(GetBaseRateAndSupplement());
        }

        private static Task UpsertEntity<T>(IDocumentSession session, T entity) where T : Entity
        {
            session.Delete(entity);
            session.Store(entity);
            return Task.CompletedTask;
        }
    }
}
