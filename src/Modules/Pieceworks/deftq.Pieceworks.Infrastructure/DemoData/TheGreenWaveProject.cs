using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.project.Company;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Domain.projectSpecificOperation;
using Marten;
using Marten.Services;

namespace deftq.Pieceworks.Infrastructure.DemoData
{
    public static class TheGreenWaveProject
    {
#pragma warning disable MA0051
        internal static async Task ImportProject(IDocumentStore documentStore, CancellationToken cancellation)
#pragma warning restore MA0051
        {
            await using var session = await documentStore.OpenSessionAsync(new SessionOptions(), cancellation);
            
            // Project
            var projectId = ProjectId.Create(Guid.Parse("B80D6476-C180-4EC9-8E8F-3C8518AAA078"));
            var projectName = ProjectName.Create("4 lejligheder");
            var projectDescription =
                ProjectDescription.Create("Opførelse af studie- og ældreboliger");
            var projectOwner = ProjectOwner.Create("Demo user 1 (akkordholder)", Guid.Parse("24b8594e-2dbc-4fcb-8e38-481ff4d78b76"));
            var projectLumpSumPaymentDkr = ProjectLumpSumPayment.Empty();
            var projectStartDate = ProjectStartDate.Create(new DateOnly(2023, 7, 31));
            var projectEndDate = ProjectEndDate.Create(new DateOnly(2023, 10, 31));
            var projectPieceWorkNumber = ProjectPieceWorkNumber.Create("31072023");
            var projectOrderNumber = ProjectOrderNumber.Create("1012023");
            var projectNumber = ProjectNumber.Create(1);
            var projectCreator = ProjectCreatedBy.Create(projectOwner.Name, projectOwner.Id);
            var projectCreationTime = ProjectCreatedTime.Create(DateTimeOffset.Now);
            var projectCompany = ProjectCompany.Create(ProjectCompanyName.Create("Den Grønne Bølge"), ProjectAddress.Create("Den Grønne Mark 1"), ProjectCompanyCvrNo.Create("12345678"), ProjectCompanyPNo.Empty());
            var project = Project.Create(projectId, projectName, projectNumber, projectPieceWorkNumber, projectOrderNumber, projectDescription, projectOwner, ProjectPieceworkType.TwelveOneB,
                projectLumpSumPaymentDkr, projectStartDate, projectEndDate, projectCreator, projectCreationTime, projectCompany);
            project.AddProjectParticipant(ProjectParticipant.Create(ProjectParticipantId.Create(Guid.Parse("fb1b58b9-ae34-4f55-adf0-3814a6d3114c")),
                ProjectParticipantName.Create("Demo user 2"), ProjectEmail.Create("demo2@akkordplus.dk"),
                ProjectParticipantAddress.Create("Finlandsgde 10"), ProjectParticipantPhoneNumber.Create("89878685")));
            project.AddProjectParticipant(ProjectParticipant.Create(ProjectParticipantId.Create(Guid.Parse("b3d97f4b-7abc-4d76-b05e-ffb9bc229acf")), ProjectParticipantName.Create("Demo user 3"),
                ProjectEmail.Create("demo3@akkordplus.dk"), ProjectParticipantAddress.Create("Århusvej 222"), ProjectParticipantPhoneNumber.Create("23232424")));
            project.AddProjectManager(ProjectManager.Create(Guid.Parse("be0e85ff-ddbb-4284-8a4b-9e6cec85229e"), "Demo user 4 (projektleder)",
                ProjectEmail.Create("demo4@akkordplus.dk"), "Århusvej 222", "23232424"));
            await InsertIfNotExist(session, project);

            // Project folder root and folders
            var projectFolderRootId = ProjectFolderRootId.Create(Guid.Parse("0C455FD2-C865-4D0A-B4C0-D16DEF73B238"));
            var projectFolderRoot = ProjectFolderRoot.Create(projectId, projectName, projectFolderRootId, DemoProjects.GetFolderRateAndSupplement());
            await InsertIfNotExist(session, projectFolderRoot);

            // Log book
            var logBook = ProjectLogBook.Create(projectId, ProjectLogBookId.Create(Guid.Parse("D424CF1B-146E-4690-8374-8881BAAB3F05")));
            await InsertIfNotExist(session, logBook);

            // Folder work
            var projectFolderWorkId = ProjectFolderWorkId.Create(Guid.Parse("FCB636BA-323F-4E19-B939-518C980639B9"));
            var folderWork = ProjectFolderWork.Create(projectFolderWorkId, projectId, projectFolderRoot.RootFolder.ProjectFolderId);
            await InsertIfNotExist(session, folderWork);

            // Favorite list
            var favoriteListId = ProjectCatalogFavoriteListId.Create(Guid.Parse("3C0BD172-2063-44B8-9677-863C747F416F"));
            var favoriteList = ProjectCatalogFavoriteList.Create(favoriteListId, projectId);
            await InsertIfNotExist(session, favoriteList);

            // Extra work agreement list
            var extraWorkAgreementListId = ProjectExtraWorkAgreementListId.Create(Guid.Parse("21CD8C5B-9FE5-4627-9F11-F30EACEC5592"));
            var extraWorkAgreementList = ProjectExtraWorkAgreementList.Create(projectId, extraWorkAgreementListId);
            extraWorkAgreementList.SetCustomerRate(ProjectExtraWorkCustomerHourRate.Create(250.00m));
            extraWorkAgreementList.SetCompanyRate(ProjectExtraWorkAgreementCompanyHourRate.Create(250.00m));
            await InsertIfNotExist(session, extraWorkAgreementList);

            // Compensation list
            var compensationListGuid = ProjectCompensationListId.Create(Guid.Parse("4916C085-DC17-4923-99A8-937FBC8CCA15"));
            var compensationList = ProjectCompensationList.Create(projectId, compensationListGuid);
            await InsertIfNotExist(session, compensationList);
            
            // Project Specific Operations list
            var projectSpecificOperationsListId = ProjectSpecificOperationListId.Create(Guid.Parse("C8DA4490-5E47-4FE7-BCF5-87924CA0DC69"));
            var projectSpecificOperationsList = ProjectSpecificOperationList.Create(projectId, projectSpecificOperationsListId);
            await InsertIfNotExist(session, projectSpecificOperationsList);

            await session.SaveChangesAsync(cancellation);
        }

        private static Task InsertIfNotExist<T>(IDocumentSession session, T entity) where T : Entity
        {
            var existingEntity = session.Load<T>(entity.Id);
            if (existingEntity is not null)
            {
                session.Delete(entity);
            }
            session.Store(entity);
            return Task.CompletedTask;
        }
    }
}
