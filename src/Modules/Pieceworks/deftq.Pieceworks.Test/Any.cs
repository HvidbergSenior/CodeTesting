using System.Security.Cryptography;
using AutoFixture;
using Baseline;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.InvitationFlow;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.project.Company;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Domain.projectSpecificOperation;
using deftq.Pieceworks.Domain.projectUser;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test
{
    internal static class Any
    {
        private static readonly Fixture any = new();

        public static T Instance<T>()
        {
            return any.Create<T>();
        }

        internal static Guid Guid()
        {
            return Instance<Guid>();
        }

        internal static Project Project()
        {
            var projectId = ProjectId();
            var name = ProjectName();
            var projectNumber = ProjectNumber();
            var description = ProjectDescription();
            var owner = ProjectOwner(Instance<Guid>());
            var pieceworkType = ProjectPieceworkType();
            var projectLumpSumPaymentDkr = ProjectLumpSumPayment.Empty();
            var projectStartDate = ProjectStartDate.Empty();
            var projectEndDate = ProjectEndDate.Empty();
            var projectOrderNumber = Pieceworks.Domain.project.ProjectOrderNumber.Empty();
            var projectPieceworkNumber = Pieceworks.Domain.project.ProjectPieceWorkNumber.Empty();
            var projectCreator = ProjectCreator();
            var projectCreationTime = ProjectCreatedTime.Create(DateTimeOffset.Now);
            var projectCompany = ProjectCompany.Empty();
            return Pieceworks.Domain.project.Project.Create(projectId, name, projectNumber, projectPieceworkNumber, projectOrderNumber, description,
                owner, pieceworkType, projectLumpSumPaymentDkr, projectStartDate, projectEndDate, projectCreator, projectCreationTime,
                projectCompany);
        }

        internal static ProjectFolderRoot ProjectFolderRoot()
        {
            var projectId = ProjectId();
            var projectName = ProjectName();
            var projectFolderRootId = ProjectFolderRootId();
            var defaultFolderRateAndSupplement = GetDefaultFolderRateAndSupplement();
            return Pieceworks.Domain.projectFolderRoot.ProjectFolderRoot.Create(projectId, projectName, projectFolderRootId,
                defaultFolderRateAndSupplement);
        }
        
        internal static ProjectFolderRoot ProjectFolderRoot(ProjectId projectId)
        {
            var projectName = ProjectName();
            var projectFolderRootId = ProjectFolderRootId();
            var defaultFolderRateAndSupplement = GetDefaultFolderRateAndSupplement();
            return Pieceworks.Domain.projectFolderRoot.ProjectFolderRoot.Create(projectId, projectName, projectFolderRootId,
                defaultFolderRateAndSupplement);
        }

        internal static ProjectFolderRootId ProjectFolderRootId()
        {
            return Pieceworks.Domain.projectFolderRoot.ProjectFolderRootId.Create(Instance<Guid>());
        }

        internal static ProjectLogBook ProjectLogBook()
        {
            var projectId = ProjectId();
            var projectLogBookId = ProjectLogBookId();

            return Pieceworks.Domain.projectLogBook.ProjectLogBook.Create(projectId, projectLogBookId);
        }

        internal static ProjectLogBookUser ProjectLogBookUser()
        {
            return Pieceworks.Domain.projectLogBook.ProjectLogBookUser.Create(LogBookName(), Guid());
        }

        internal static LogBookDate LogBookDate(int year, int month, int day)
        {
            return Pieceworks.Domain.projectLogBook.LogBookDate.Create(new DateOnly(year, month, day));
        }

        internal static ProjectLogBookWeek ProjectLogBookWeek(int year, int week)
        {
            return Pieceworks.Domain.projectLogBook.ProjectLogBookWeek.Create(LogBookYear.Create(year), LogBookWeek.Create(week));
        }

        internal static ProjectLogBookDay ProjectLogBookDay(int year, int month, int day, int hours, int minutes)
        {
            return Pieceworks.Domain.projectLogBook.ProjectLogBookDay.Create(Any.LogBookDate(year, month, day),
                LogBookTime.Create(LogBookHours.Create(hours), LogBookMinutes.Create(minutes)));
        }

        internal static ProjectLogBookId ProjectLogBookId()
        {
            return Pieceworks.Domain.projectLogBook.ProjectLogBookId.Create(Instance<Guid>());
        }

        internal static ProjectFolderLock ProjectFolderLock()
        {
            return Pieceworks.Domain.projectFolderRoot.ProjectFolderLock.Unlocked();
        }

        internal static ProjectFolderExtraWork ProjectFolderExtraWork()
        {
            return Pieceworks.Domain.projectFolderRoot.ProjectFolderExtraWork.Normal();
        }

        internal static ProjectFolder ProjectFolder(FolderRateAndSupplement folderRateAndSupplement)
        {
            var projectId = ProjectFolderId();
            var name = ProjectFolderName();
            var description = ProjectFolderDescription();
            var createdBy = ProjectFolderCreatedBy();
            var folderLock = ProjectFolderLock();
            var extraWork = ProjectFolderExtraWork();
            return Pieceworks.Domain.projectFolderRoot.ProjectFolder.Create(projectId, name, description, createdBy, folderRateAndSupplement,
                folderLock, extraWork);
        }

        internal static ProjectFolder ProjectFolder()
        {
            return ProjectFolder(FolderRateAndSupplement.Create(GetDefaultBaseRateAndSupplement()));
        }

        internal static ProjectFolder UnlockedFolder()
        {
            var projectId = ProjectFolderId();
            var name = ProjectFolderName();
            var description = ProjectFolderDescription();
            var createdBy = ProjectFolderCreatedBy();
            var folderRateAndSupplement = FolderRateAndSupplement.Create(GetDefaultBaseRateAndSupplement());
            var folderLock = Pieceworks.Domain.projectFolderRoot.ProjectFolderLock.Unlocked();
            var extraWork = ProjectFolderExtraWork();
            return Pieceworks.Domain.projectFolderRoot.ProjectFolder.Create(projectId, name, description, createdBy, folderRateAndSupplement,
                folderLock, extraWork);
        }

        internal static ProjectFolder LockedFolder()
        {
            var projectId = ProjectFolderId();
            var name = ProjectFolderName();
            var description = ProjectFolderDescription();
            var createdBy = ProjectFolderCreatedBy();
            var folderRateAndSupplement = FolderRateAndSupplement.Create(GetDefaultBaseRateAndSupplement());
            var folderLock = Pieceworks.Domain.projectFolderRoot.ProjectFolderLock.Locked();
            var extraWork = ProjectFolderExtraWork();
            return Pieceworks.Domain.projectFolderRoot.ProjectFolder.Create(projectId, name, description, createdBy, folderRateAndSupplement,
                folderLock, extraWork);
        }

        internal static WorkItem WorkItem()
        {
            var workItemId = WorkItemId();
            var materialId = CatalogMaterialId();
            var workItemDate = WorkItemDate();
            var workItemUser = WorkItemUser();
            var workItemText = WorkItemText();
            var workItemEanNumber = WorkItemEanNumber();
            var workItemMountingCode = WorkItemMountingCode();
            var workItemOperationCode = WorkItemDuration();
            var workItemAmount = WorkItemAmount();
            var workItemUnit = WorkItemUnit();
            var supplementOperations = new List<SupplementOperation>();
            var supplements = new List<Supplement>();

            return Pieceworks.Domain.FolderWork.WorkItem.Create(workItemId, materialId, workItemDate, workItemUser, workItemText,
                workItemEanNumber, workItemMountingCode, workItemOperationCode, workItemAmount, workItemUnit, supplementOperations, supplements);
        }

        internal static WorkItemUnit WorkItemUnit()
        {
            return Pieceworks.Domain.FolderWork.WorkItemUnit.Meter;
        }

        internal static WorkItemId WorkItemId()
        {
            return Pieceworks.Domain.FolderWork.WorkItemId.Create(Instance<Guid>());
        }

        internal static WorkItemDate WorkItemDate()
        {
            return Pieceworks.Domain.FolderWork.WorkItemDate.Create(DateOnly.FromDayNumber(30));
        }

        internal static WorkItemUser WorkItemUser()
        {
            return Pieceworks.Domain.FolderWork.WorkItemUser.Create(Instance<Guid>(), Instance<string>());
        }

        internal static WorkItemText WorkItemText()
        {
            return Pieceworks.Domain.FolderWork.WorkItemText.Create(Instance<string>());
        }

        internal static WorkItemEanNumber WorkItemEanNumber()
        {
            return Pieceworks.Domain.FolderWork.WorkItemEanNumber.Create("0000000000123");
        }

        internal static WorkItemMountingCode WorkItemMountingCode()
        {
            return Pieceworks.Domain.FolderWork.WorkItemMountingCode.FromCode(3);
        }

        internal static WorkItemDuration WorkItemDuration()
        {
            return Pieceworks.Domain.FolderWork.WorkItemDuration.Create(3254);
        }

        internal static WorkItemAmount WorkItemAmount()
        {
            return Pieceworks.Domain.FolderWork.WorkItemAmount.Empty();
        }

        internal static ProjectId ProjectId()
        {
            return Pieceworks.Domain.project.ProjectId.Create(Instance<Guid>());
        }

        public static ProjectFolderId ProjectFolderId()
        {
            return Pieceworks.Domain.projectFolderRoot.ProjectFolderId.Create(Instance<Guid>());
        }

        internal static ProjectUser ProjectUser()
        {
            return Pieceworks.Domain.projectUser.ProjectUser.Create(ProjectUserId.Create(Guid()));
        }

        internal static ProjectCompensationList ProjectCompensationList()
        {
            return Pieceworks.Domain.projectCompensation.ProjectCompensationList.Create(ProjectId(), ProjectCompensationListId());
        }

        internal static ProjectCompensation ProjectCompensation()
        {
            return Pieceworks.Domain.projectCompensation.ProjectCompensation.Create(ProjectCompensationId(), ProjectCompensationPayment(),
                ProjectCompensationPeriod(), new List<ProjectParticipantId> { ProjectParticipantId.Create(Instance<Guid>()) });
        }

        internal static ProjectCompensationPayment ProjectCompensationPayment()
        {
            return Pieceworks.Domain.projectCompensation.ProjectCompensationPayment.Create(Instance<decimal>());
        }

        internal static ProjectCompensationPeriod ProjectCompensationPeriod()
        {
            var random = new Random();
            var from = DateTimeOffset.FromUnixTimeSeconds(random.NextInt64(250000000000));
            var to = from.AddSeconds(random.NextInt64(3000000000));
            var fromDate = ProjectCompensationDate(from.Year, from.Month, from.Day);
            var toDate = ProjectCompensationDate(to.Year, to.Month, to.Day);
            return Pieceworks.Domain.projectCompensation.ProjectCompensationPeriod.Create(fromDate, toDate);
        }

        internal static ProjectCompensationListId ProjectCompensationListId()
        {
            return Pieceworks.Domain.projectCompensation.ProjectCompensationListId.Create(Instance<Guid>());
        }

        internal static ProjectCompensationId ProjectCompensationId()
        {
            return Pieceworks.Domain.projectCompensation.ProjectCompensationId.Create(Instance<Guid>());
        }

        internal static ProjectCompensationDate ProjectCompensationDate(int year, int month, int day)
        {
            return Pieceworks.Domain.projectCompensation.ProjectCompensationDate.Create(new DateOnly(year, month, day));
        }

        internal static ProjectCompensationDate ProjectCompensationStartDate()
        {
            return Pieceworks.Domain.projectCompensation.ProjectCompensationDate.Create(DateOnly.MinValue);
        }
        
        internal static ProjectCompensationDate ProjectCompensationEndDate()
        {
            return Pieceworks.Domain.projectCompensation.ProjectCompensationDate.Create(DateOnly.MaxValue);
        }

        internal static ProjectName ProjectName()
        {
            return Pieceworks.Domain.project.ProjectName.Create(
                $"Project_{RandomNumberGenerator.GetInt32(Int32.MaxValue)}");
        }

        internal static ProjectFolderName ProjectFolderName()
        {
            return Pieceworks.Domain.projectFolderRoot.ProjectFolderName.Create(
                $"Folder_{RandomNumberGenerator.GetInt32(Int32.MaxValue)}");
        }

        internal static ProjectFolderDescription ProjectFolderDescription()
        {
            return Pieceworks.Domain.projectFolderRoot.ProjectFolderDescription.Create(
                $"Folder_{RandomNumberGenerator.GetInt32(Int32.MaxValue)}");
        }

        internal static ProjectDescription ProjectDescription()
        {
            return Pieceworks.Domain.project.ProjectDescription.Create(Instance<string>());
        }

        internal static ProjectOwner ProjectOwner()
        {
            return Pieceworks.Domain.project.ProjectOwner.Create(Instance<string>(), Instance<Guid>());
        }

        internal static ProjectOwner ProjectOwner(Guid id)
        {
            return Pieceworks.Domain.project.ProjectOwner.Create(Instance<string>(), id);
        }

        internal static ProjectOwner ProjectOwner(string name)
        {
            return Pieceworks.Domain.project.ProjectOwner.Create(name, Guid());
        }

        internal static ProjectPieceworkType ProjectPieceworkType()
        {
            return Pieceworks.Domain.project.ProjectPieceworkType.TwelveTwo;
        }

        internal static ProjectOrderNumber ProjectOrderNumber()
        {
            return Pieceworks.Domain.project.ProjectOrderNumber.Create("1111111");
        }

        internal static ProjectPieceWorkNumber ProjectPieceWorkNumber()
        {
            return Pieceworks.Domain.project.ProjectPieceWorkNumber.Create("2222222");
        }

        internal static ProjectFolderCreatedBy ProjectFolderCreatedBy()
        {
            return Pieceworks.Domain.projectFolderRoot.ProjectFolderCreatedBy.Create(Instance<string>(), DateTimeOffset.Now);
        }

        internal static string String(int length)
        {
            string chars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return Enumerable.Range(0, length)
                .Select(x => chars.Substring(RandomNumberGenerator.GetInt32(chars.Length), 1))
                .Aggregate("", (a, b) => a + b);
        }

        public static ProjectDocumentId ProjectDocumentId()
        {
            return Pieceworks.Domain.projectDocument.ProjectDocumentId.Create(Instance<Guid>());
        }

        public static ProjectDocumentName ProjectDocumentName()
        {
            return Pieceworks.Domain.projectDocument.ProjectDocumentName.Create(String(10));
        }

        public static ProjectDocumentUploadedTimestamp ProjectDocumentUploadedTimestamp()
        {
            return Pieceworks.Domain.projectDocument.ProjectDocumentUploadedTimestamp
                .Create(Instance<DateTimeOffset>());
        }

        public static ProjectDocument ProjectDocument(IFileReference fileReference)
        {
            return Pieceworks.Domain.projectDocument.ProjectDocument.Create(ProjectId(), ProjectFolderId(),
                ProjectDocumentId(), ProjectDocumentName(), ProjectDocumentUploadedTimestamp(), fileReference);
        }

        public static LogBookName LogBookName()
        {
            return Pieceworks.Domain.projectLogBook.LogBookName.Create(String(10));
        }

        public static LogBookNote LogBookNote()
        {
            return Pieceworks.Domain.projectLogBook.LogBookNote.Create(String(10));
        }

        public static Project OwnedBy(this Project project, ProjectOwner owner)
        {
            var newProject = Pieceworks.Domain.project.Project.Create(project.ProjectId, project.ProjectName, project.ProjectNumber,
                project.ProjectPieceWorkNumber, project.ProjectOrderNumber, project.ProjectDescription, owner, project.ProjectPieceworkType,
                ProjectLumpSumPayment.Empty(), ProjectStartDate.Empty(), ProjectEndDate.Empty(), ProjectCreatedBy.Empty(),
                ProjectCreatedTime.Empty(), ProjectCompany.Empty());
            return newProject;
        }

        public static Project OwnedBy(this Project project, Guid ownerId)
        {
            return OwnedBy(project, Pieceworks.Domain.project.ProjectOwner.Create(System.String.Empty, ownerId));
        }

        public static Project OwnedBy(this Project project, Guid ownerId, string name)
        {
            return OwnedBy(project, Pieceworks.Domain.project.ProjectOwner.Create(name, ownerId));
        }

        public static Project WithParticipant(this Project project, Guid participantId)
        {
            project.AddProjectParticipant(ProjectParticipant(participantId));
            return project;
        }

        public static Project WithProjectManager(this Project project, Guid projectManagerId)
        {
            project.AddProjectManager(ProjectManager(projectManagerId));
            return project;
        }

        public static BaseRateAndSupplement BaseRateAndSupplement()
        {
            var personalTimeSupplementIntervals = new List<PersonalTimeSupplementInterval>()
            {
                PersonalTimeSupplementInterval.Create(PersonalTimeSupplement.Create(6), DateInterval.Always())
            };
            var baseRateIntervals = new List<BaseRateInterval>() { BaseRateInterval.Create(BaseRate.Create(214.74m), DateInterval.Always()) };
            return Pieceworks.Domain.BaseRateAndSupplement.Create(IndirectTimeSupplement.Create(64), SiteSpecificTimeSupplement.Create(2),
                personalTimeSupplementIntervals, baseRateIntervals, BaseRateRegulation.Create(0));
        }

        public static SupplementOperationId SupplementOperationId()
        {
            return Pieceworks.Domain.FolderWork.Supplements.SupplementOperationId.Create(Instance<Guid>());
        }

        public static CatalogSupplementOperationId CatalogSupplementOperationId()
        {
            return Pieceworks.Domain.FolderWork.Supplements.CatalogSupplementOperationId.Create(Instance<Guid>());
        }

        public static CatalogMaterialId CatalogMaterialId()
        {
            return Pieceworks.Domain.FolderWork.CatalogMaterialId.Create(Instance<Guid>());
        }

        public static SupplementOperationText SupplementOperationText()
        {
            return Pieceworks.Domain.FolderWork.Supplements.SupplementOperationText.Create(String(10));
        }

        public static ProjectFolderWork ProjectFolderWork()
        {
            return Pieceworks.Domain.FolderWork.ProjectFolderWork.Create(ProjectFolderWorkId(), ProjectId(), ProjectFolderId());
        }

        public static ProjectFolderWorkId ProjectFolderWorkId()
        {
            return Pieceworks.Domain.FolderWork.ProjectFolderWorkId.Create(Guid());
        }

        public static CatalogOperationId CatalogOperationId()
        {
            return Pieceworks.Domain.FolderWork.CatalogOperationId.Create(Guid());
        }

        public static WorkItemOperationNumber WorkItemOperationNumber()
        {
            return Pieceworks.Domain.FolderWork.WorkItemOperationNumber.Create(String(10));
        }

        public static IList<Guid> Guids()
        {
            return new[] { new Guid(), new Guid() }.ToList();
        }

        public static ProjectCatalogFavoriteListId ProjectCatalogFavoriteListId()
        {
            return Pieceworks.Domain.ProjectCatalogFavorite.ProjectCatalogFavoriteListId.Create(Guid());
        }

        public static ProjectCatalogFavoriteList ProjectCatalogFavoriteList()
        {
            return Pieceworks.Domain.ProjectCatalogFavorite.ProjectCatalogFavoriteList.Create(ProjectCatalogFavoriteListId(), ProjectId());
        }

        public static CatalogFavoriteId CatalogFavoriteId()
        {
            return Pieceworks.Domain.ProjectCatalogFavorite.CatalogFavoriteId.Create(Guid());
        }

        public static CatalogItemId CatalogItemId()
        {
            return Pieceworks.Domain.ProjectCatalogFavorite.CatalogItemId.Create(Guid());
        }

        public static CatalogItemNumber CatalogItemNumber()
        {
            return Pieceworks.Domain.ProjectCatalogFavorite.CatalogItemNumber.Create(String(13));
        }

        public static CatalogItemText CatalogItemText()
        {
            return Pieceworks.Domain.ProjectCatalogFavorite.CatalogItemText.Create(String(25));
        }

        public static CatalogItemUnit CatalogItemUnit()
        {
            return Pieceworks.Domain.ProjectCatalogFavorite.CatalogItemUnit.Create(String(10));
        }

        public static CatalogFavorite CatalogFavorite()
        {
            return Pieceworks.Domain.ProjectCatalogFavorite.CatalogFavorite.Create(CatalogFavoriteId(), CatalogItemType.Material, CatalogItemId(),
                CatalogItemNumber(), CatalogItemText(), CatalogItemUnit());
        }

        private const string ManagerName = "admin";
        private const string ManagerEmail = "admin@incharge.dk";
        private const string ManagerAdr = "Bestemmergad 123";
        private const string ManagerPhone = "12233445";

        public static ProjectManager ProjectManager()
        {
            return ProjectManager(Guid());
        }

        public static ProjectManager ProjectManager(Guid participantId)
        {
            return Pieceworks.Domain.project.ProjectManager.Create(participantId, ManagerName, ProjectEmail.Create(ManagerEmail), ManagerAdr,
                ManagerPhone);
        }

        public static ProjectManager ProjectManager(string name)
        {
            return Pieceworks.Domain.project.ProjectManager.Create(Guid(), name, ProjectEmail.Create(ManagerEmail), ManagerAdr, ManagerPhone);
        }

        public static ProjectManager ProjectManager(Guid participantId, string name)
        {
            return Pieceworks.Domain.project.ProjectManager.Create(participantId, name, ProjectEmail.Create(ManagerEmail), ManagerAdr, ManagerPhone);
        }

        public static ProjectManager ProjectManager(string name, string email, string adr, string phone)
        {
            return Pieceworks.Domain.project.ProjectManager.Create(Guid(), name, ProjectEmail.Create(email),
                adr, phone);
        }

        private const string ParticipantName = "electrician";
        private const string ParticipantEmail = "elsam@elsom.dk";
        private const string ParticipantAdr = "Testvej 42, 4200, Testrup";
        private const string ParticipantPhone = "23232424";

        public static ProjectParticipant ProjectParticipant()
        {
            return ProjectParticipant(Guid());
        }

        public static ProjectParticipant ProjectParticipant(Guid participantId)
        {
            return Pieceworks.Domain.project.ProjectParticipant.Create(ProjectParticipantId.Create(participantId),
                ProjectParticipantName.Create(ParticipantName), ProjectEmail.Create(ParticipantEmail),
                ProjectParticipantAddress.Create(ParticipantAdr), ProjectParticipantPhoneNumber.Create(ParticipantPhone));
        }

        public static ProjectParticipant ProjectParticipant(ProjectParticipantName name)
        {
            return Pieceworks.Domain.project.ProjectParticipant.Create(ProjectParticipantId.Create(Guid()), name,
                ProjectEmail.Create(ParticipantEmail),
                ProjectParticipantAddress.Create(ParticipantAdr), ProjectParticipantPhoneNumber.Create(ParticipantPhone));
        }

        public static ProjectParticipant ProjectParticipant(Guid participantId, ProjectParticipantName name)
        {
            return Pieceworks.Domain.project.ProjectParticipant.Create(ProjectParticipantId.Create(participantId), name,
                ProjectEmail.Create(ParticipantEmail),
                ProjectParticipantAddress.Create(ParticipantAdr), ProjectParticipantPhoneNumber.Create(ParticipantPhone));
        }

        public static ProjectParticipant ProjectParticipant(string name, string email, string adr, string phone)
        {
            return Pieceworks.Domain.project.ProjectParticipant.Create(ProjectParticipantId.Create(Guid()), ProjectParticipantName.Create(name),
                ProjectEmail.Create(email),
                ProjectParticipantAddress.Create(adr), ProjectParticipantPhoneNumber.Create(phone));
        }

        public static ProjectExtraWorkAgreement ProjectExtraWorkAgreementIncludingWorkTime()
        {
            return ProjectExtraWorkAgreement.Create(ProjectId(), ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName(), ProjectExtraWorkAgreementDescription(), ProjectExtraWorkAgreementType(),
                ProjectExtraWorkAgreementNumber(), Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementWorkTime.Create(
                    Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementHours.Create(20),
                    Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementMinutes.Create(20)));
        }

        public static ProjectExtraWorkAgreement ProjectExtraWorkAgreementIncludingPaymentDkr()
        {
            return ProjectExtraWorkAgreement.Create(ProjectId(), ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName(), ProjectExtraWorkAgreementDescription(), ProjectExtraWorkAgreementNumber(),
                Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementPaymentDkr.Create(20));
        }

        public static ProjectExtraWorkAgreement ProjectExtraWorkAgreementOther()
        {
            return ProjectExtraWorkAgreement.Create(ProjectId(), ProjectExtraWorkAgreementId(),
                ProjectExtraWorkAgreementName(), ProjectExtraWorkAgreementDescription(), ProjectExtraWorkAgreementNumber());
        }

        public static ProjectExtraWorkAgreementList ProjectExtraWorkAgreementList()
        {
            return Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementList.Create(ProjectId(), ProjectExtraWorkAgreementListId());
        }

        public static ProjectExtraWorkAgreementListId ProjectExtraWorkAgreementListId()
        {
            return Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementListId.Create(Guid());
        }

        public static ProjectExtraWorkAgreementTotalPaymentDkr ProjectExtraWorkAgreementTotalPaymentDkr()
        {
            return Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementTotalPaymentDkr.Create(10);
        }

        public static ProjectExtraWorkAgreementId ProjectExtraWorkAgreementId()
        {
            return Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementId.Create(Guid());
        }

        public static ProjectExtraWorkAgreementName ProjectExtraWorkAgreementName()
        {
            return Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementName.Create(String(10));
        }

        public static ProjectExtraWorkAgreementDescription ProjectExtraWorkAgreementDescription()
        {
            return Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementDescription.Create(String(15));
        }

        public static ProjectExtraWorkAgreementType ProjectExtraWorkAgreementType()
        {
            return Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementType.CustomerHours();
        }

        public static ProjectExtraWorkAgreementNumber ProjectExtraWorkAgreementNumber()
        {
            return Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementNumber.Create(String(10));
        }

        public static ProjectExtraWorkAgreementPaymentDkr ProjectExtraWorkAgreementPaymentDkr()
        {
            return Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementPaymentDkr.Create(10);
        }

        public static ProjectExtraWorkAgreementHours ProjectExtraWorkAgreementHours()
        {
            return Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementHours.Create(5);
        }

        public static ProjectExtraWorkAgreementMinutes ProjectExtraWorkAgreementMinutes()
        {
            return Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementMinutes.Create(10);
        }

        public static ProjectExtraWorkAgreementWorkTime ProjectExtraWorkAgreementWorkTime()
        {
            return Pieceworks.Domain.projectExtraWorkAgreement.ProjectExtraWorkAgreementWorkTime.Create(ProjectExtraWorkAgreementHours(),
                ProjectExtraWorkAgreementMinutes());
        }

        public static ProjectNumber ProjectNumber()
        {
            return Pieceworks.Domain.project.ProjectNumber.Create(Instance<long>());
        }

        public static ProjectCreatedBy ProjectCreator()
        {
            return ProjectCreatedBy.Create(String(7), Guid());
        }

        public static FolderSupplement FolderSupplementWithPercentage(decimal percentage)
        {
            return Pieceworks.Domain.projectFolderRoot.FolderSupplement.Create(SupplementId.Create(Guid()), CatalogSupplementId.Create(Guid()),
                SupplementNumber.Create(String(10)), SupplementText.Create(String(15)), SupplementPercentage.Create(percentage));
        }

        public static List<FolderSupplement> FolderSupplements()
        {
            var supplements = new List<FolderSupplement>
            {
                Pieceworks.Domain.projectFolderRoot.FolderSupplement.Create(SupplementId.Create(Guid()), CatalogSupplementId.Create(Guid()),
                    SupplementNumber.Create("L42"),
                    SupplementText.Create("For lavt"), SupplementPercentage.Create(42)),
                Pieceworks.Domain.projectFolderRoot.FolderSupplement.Create(SupplementId.Create(Guid()), CatalogSupplementId.Create(Guid()),
                    SupplementNumber.Create("H42"),
                    SupplementText.Create("For højt"), SupplementPercentage.Create(142))
            };
            return supplements;
        }

        public static string Email()
        {
            return "test@testerson.dk";
        }
        
        public static ProjectSpecificOperationList ProjectSpecificOperationList()
        {
            return Pieceworks.Domain.projectSpecificOperation.ProjectSpecificOperationList.Create(ProjectId(), ProjectSpecificOperationListId());
        }

        public static ProjectSpecificOperationListId ProjectSpecificOperationListId()
        {
            return Pieceworks.Domain.projectSpecificOperation.ProjectSpecificOperationListId.Create(Guid());
        }

        public static ProjectSpecificOperationId ProjectSpecificOperationId()
        {
            return Pieceworks.Domain.projectSpecificOperation.ProjectSpecificOperationId.Create(Guid());
        }

        public static ProjectSpecificOperation ProjectSpecificOperation()
        {
            return Pieceworks.Domain.projectSpecificOperation.ProjectSpecificOperation.Create(ProjectSpecificOperationId(),
                ProjectSpecificOperationExtraWorkAgreementNumber.Create("12345"), ProjectSpecificOperationName.Create("12345"),
                ProjectSpecificOperationDescription.Create("12345"), ProjectSpecificOperationTime.Create(420000),
                ProjectSpecificOperationTime.Empty());
        }
        
        public static ProjectSpecificOperation ProjectSpecificOperation(string extraWorkAgreementNumber, string name, string description, decimal operationTimeMs, decimal workingTimeMs)
        {
            return Pieceworks.Domain.projectSpecificOperation.ProjectSpecificOperation.Create(ProjectSpecificOperationId(),
                ProjectSpecificOperationExtraWorkAgreementNumber.Create(extraWorkAgreementNumber), ProjectSpecificOperationName.Create(name),
                ProjectSpecificOperationDescription.Create(description), ProjectSpecificOperationTime.Create(operationTimeMs),
                ProjectSpecificOperationTime.Create(workingTimeMs));
        }

        internal static Invitation Invitation()
        {
            var invitationId = InvitationId();
            var projectId = ProjectId();
            var invitationEmail = InvitationEmail();
            var invitationRandomValue = InvitationRandomValue();
            var invitationExpiration = InvitationExpiration();
            var invitationRetries = InvitationRetries();
            return Pieceworks.Domain.InvitationFlow.Invitation.Create(invitationId, projectId, invitationEmail, 
                invitationRandomValue, invitationExpiration, invitationRetries);
        }

        internal static InvitationId InvitationId()
        {
            return Pieceworks.Domain.InvitationFlow.InvitationId.Create(Instance<Guid>());
        }

        internal static InvitationEmail InvitationEmail()
        {
            return Pieceworks.Domain.InvitationFlow.InvitationEmail.Create(Instance<string>());
        }

        internal static InvitationRandomValue InvitationRandomValue()
        {
            return Pieceworks.Domain.InvitationFlow.InvitationRandomValue.Create(Instance<string>());
        }

        internal static InvitationExpiration InvitationExpiration()
        {
            return Pieceworks.Domain.InvitationFlow.InvitationExpiration.Create(DateTime.Now);
        }

        internal static InvitationRetries InvitationRetries()
        {
            return Pieceworks.Domain.InvitationFlow.InvitationRetries.Create(Instance<int>());
        }
    }
}
