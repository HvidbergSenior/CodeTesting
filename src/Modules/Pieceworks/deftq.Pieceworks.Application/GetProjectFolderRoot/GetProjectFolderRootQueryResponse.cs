namespace deftq.Pieceworks.Application.GetProjectFolderRoot
{
    public class GetProjectFolderRootQueryResponse
    {
        public Guid ProjectId { get; private set; }

        public ProjectFolderResponse RootFolder { get; private set; }

        private GetProjectFolderRootQueryResponse()
        {
            ProjectId = Guid.Empty;
            RootFolder = new ProjectFolderResponse(Guid.Empty, String.Empty, String.Empty,
                new List<ProjectFolderResponse>(), new List<DocumentReferenceResponse>(), String.Empty,
                DateTimeOffset.MinValue, ProjectFolderResponse.ProjectFolderLock.Locked, ProjectFolderResponse.ExtraWork.NormalWork,
                new BaseRateAndSupplementsResponse(), new List<FolderSupplementResponse>());
        }

        public GetProjectFolderRootQueryResponse(Guid projectId, ProjectFolderResponse rootFolder)
        {
            ProjectId = projectId;
            RootFolder = rootFolder;
        }
    }

    public class ProjectFolderResponse
    {
        public Guid ProjectFolderId { get; private set; }
        public string ProjectFolderName { get; private set; }
        public string ProjectFolderDescription { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTimeOffset CreatedTime { get; private set; }
        public IList<ProjectFolderResponse> SubFolders { get; private set; }
        public IList<DocumentReferenceResponse> Documents { get; private set; }

        public enum ProjectFolderLock { Locked, Unlocked }

        public ProjectFolderLock ProjectFolderLocked { get; private set; }

        public enum ExtraWork { ExtraWork, NormalWork }

        public ExtraWork FolderExtraWork { get; private set; }

        public BaseRateAndSupplementsResponse BaseRateAndSupplements { get; private set; }

        public IList<FolderSupplementResponse> FolderSupplements { get; set; }

        private ProjectFolderResponse()
        {
            ProjectFolderId = Guid.Empty;
            ProjectFolderName = "";
            ProjectFolderDescription = "";
            SubFolders = new List<ProjectFolderResponse>();
            CreatedBy = String.Empty;
            CreatedTime = new DateTimeOffset();
            Documents = new List<DocumentReferenceResponse>();
            ProjectFolderLocked = ProjectFolderLock.Locked;
            FolderExtraWork = ExtraWork.NormalWork;
            BaseRateAndSupplements = new BaseRateAndSupplementsResponse();
            FolderSupplements = new List<FolderSupplementResponse>();
        }

        public ProjectFolderResponse(Guid projectFolderId, string projectFolderName, string projectFolderDescription,
            IList<ProjectFolderResponse> subFolders, IList<DocumentReferenceResponse> documents, string createdBy, DateTimeOffset createdTime,
            ProjectFolderLock projectFolderLocked, ExtraWork folderExtraWork, BaseRateAndSupplementsResponse baseRateAndSupplements, IList<FolderSupplementResponse> folderSupplements)
        {
            ProjectFolderId = projectFolderId;
            ProjectFolderName = projectFolderName;
            ProjectFolderDescription = projectFolderDescription;
            SubFolders = subFolders;
            CreatedBy = createdBy;
            CreatedTime = createdTime;
            Documents = documents;
            ProjectFolderLocked = projectFolderLocked;
            FolderExtraWork = folderExtraWork;
            BaseRateAndSupplements = baseRateAndSupplements;
            FolderSupplements = folderSupplements;
        }

        public void AddFolder(ProjectFolderResponse projectFolderResponse)
        {
            SubFolders.Add(projectFolderResponse);
        }

        public void AddDocument(DocumentReferenceResponse document)
        {
            Documents.Add(document);
        }
    }

    public class DocumentReferenceResponse
    {
        public Guid DocumentId { get; private set; }
        public string Name { get; private set; }
        public DateTimeOffset UploadedTimestamp { get; private set; }

        private DocumentReferenceResponse()
        {
            DocumentId = Guid.NewGuid();
            Name = string.Empty;
            UploadedTimestamp = DateTimeOffset.MinValue;
        }

        public DocumentReferenceResponse(Guid documentId, string name, DateTimeOffset uploadedTimestamp)
        {
            DocumentId = documentId;
            Name = name;
            UploadedTimestamp = uploadedTimestamp;
        }
    }

    public class BaseRateAndSupplementsResponse
    {
        public BaseRateAndSupplementsValueResponse IndirectTimeSupplementPercentage { get; private set; }
        public BaseRateAndSupplementsValueResponse SiteSpecificTimeSupplementPercentage { get; private set; }
        public BaseRateAndSupplementsValueResponse BaseRateRegulationPercentage { get; private set; }
        public decimal CombinedSupplementPercentage { get; private set; }
        public decimal BaseRatePerMinDkr { get; private set; }
        public decimal PersonalTimeSupplementPercentage { get; private set; }

        internal BaseRateAndSupplementsResponse()
        {
            IndirectTimeSupplementPercentage = new BaseRateAndSupplementsValueResponse(BaseRateAndSupplementsValueStatus.Overwrite, 0);
            SiteSpecificTimeSupplementPercentage = new BaseRateAndSupplementsValueResponse(BaseRateAndSupplementsValueStatus.Overwrite, 0);
            BaseRateRegulationPercentage = new BaseRateAndSupplementsValueResponse(BaseRateAndSupplementsValueStatus.Overwrite, 0);
            CombinedSupplementPercentage = 0;
            BaseRatePerMinDkr = 0;
            PersonalTimeSupplementPercentage = 0;
        }

        public BaseRateAndSupplementsResponse(BaseRateAndSupplementsValueResponse indirectTimeSupplementPercentage,
            BaseRateAndSupplementsValueResponse siteSpecificTimeSupplementPercentage,
            BaseRateAndSupplementsValueResponse baseRateRegulationPercentage, decimal combinedSupplementPercentage, decimal baseRatePerMinDkr,
            decimal personalTimeSupplementPercentage)
        {
            IndirectTimeSupplementPercentage = indirectTimeSupplementPercentage;
            SiteSpecificTimeSupplementPercentage = siteSpecificTimeSupplementPercentage;
            BaseRateRegulationPercentage = baseRateRegulationPercentage;
            CombinedSupplementPercentage = combinedSupplementPercentage;
            BaseRatePerMinDkr = baseRatePerMinDkr;
            PersonalTimeSupplementPercentage = personalTimeSupplementPercentage;
        }
    }

    public class BaseRateAndSupplementsValueResponse
    {
        public BaseRateAndSupplementsValueStatus ValueStatus { get; private set; }
        public decimal Value { get; private set; }

        private BaseRateAndSupplementsValueResponse()
        {
            ValueStatus = BaseRateAndSupplementsValueStatus.Overwrite;
            Value = 0;
        }

        public BaseRateAndSupplementsValueResponse(BaseRateAndSupplementsValueStatus valueStatus, Decimal value)
        {
            ValueStatus = valueStatus;
            Value = value;
        }
    }

    public enum BaseRateAndSupplementsValueStatus
    {
        Inherit,
        Overwrite
    }

    public class FolderSupplementResponse
    {
        public Guid SupplementId { get; private set; }
        public string SupplementNumber { get; private set; }
        public string SupplementText { get; private set; }

        private FolderSupplementResponse()
        {
            SupplementId = Guid.NewGuid();
            SupplementNumber = string.Empty;
            SupplementText = String.Empty;
        }

        public FolderSupplementResponse(Guid supplementId, string supplementNumber, string supplementText)
        {
            SupplementId = supplementId;
            SupplementNumber = supplementNumber;
            SupplementText = supplementText;
        }
    }
}
