using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;

namespace deftq.Pieceworks.Application.GetProjectFolderRoot
{
    public sealed class GetProjectFolderRootQuery : IQuery<GetProjectFolderRootQueryResponse>
    {
        public ProjectId ProjectId { get; }

        private GetProjectFolderRootQuery(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static GetProjectFolderRootQuery Create(ProjectId projectId)
        {
            return new GetProjectFolderRootQuery(projectId);
        }
    }

    internal class GetProjectFolderRootQueryHandler : IQueryHandler<GetProjectFolderRootQuery, GetProjectFolderRootQueryResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly ISystemTime _systemTime;

        public GetProjectFolderRootQueryHandler(IProjectFolderRootRepository projectFolderRootRepository,
            IBaseRateAndSupplementRepository baseRateAndSupplementRepository, ISystemTime systemTime)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
            _systemTime = systemTime;
        }

        public async Task<GetProjectFolderRootQueryResponse> Handle(GetProjectFolderRootQuery query,
            CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(query.ProjectId.Value, cancellationToken);
            var baseRateAndSupplement = await _baseRateAndSupplementRepository.Get(cancellationToken);
            return MapResponse(projectFolderRoot, baseRateAndSupplement);
        }

        private GetProjectFolderRootQueryResponse MapResponse(ProjectFolderRoot projectFolderRoot, BaseRateAndSupplement baseRateAndSupplement)
        {
            var folderRoot = MapFolderResponse(projectFolderRoot.RootFolder, baseRateAndSupplement);
            return new GetProjectFolderRootQueryResponse(projectFolderRoot.ProjectId.Value, folderRoot);
        }

        private ProjectFolderResponse MapFolderResponse(ProjectFolder projectFolder, BaseRateAndSupplement baseRateAndSupplement)
        {
            var folderLock = projectFolder.FolderLock.IsLocked()
                ? ProjectFolderResponse.ProjectFolderLock.Locked
                : ProjectFolderResponse.ProjectFolderLock.Unlocked;

            var extraWork = projectFolder.IsExtraWork()
                ? ProjectFolderResponse.ExtraWork.ExtraWork
                : ProjectFolderResponse.ExtraWork.NormalWork;

            var folderBaseRateAndSupplement = GetFolderEffectiveRateAndSupplements(projectFolder, baseRateAndSupplement);
            var supplements = MapSupplementResponse(projectFolder);

            var result = new ProjectFolderResponse(projectFolder.ProjectFolderId.Value, projectFolder.Name.Value,
                projectFolder.Description.Value, new List<ProjectFolderResponse>(),
                new List<DocumentReferenceResponse>(), projectFolder.CreatedBy.Name, projectFolder.CreatedBy.Timestamp, folderLock, extraWork,
                folderBaseRateAndSupplement, supplements);

            foreach (var subFolder in projectFolder.SubFolders)
            {
                result.AddFolder(MapFolderResponse(subFolder, baseRateAndSupplement));
            }

            foreach (var document in projectFolder.Documents)
            {
                result.AddDocument(MapDocumentResponse(document));
            }

            return result;
        }

        private BaseRateAndSupplementsResponse GetFolderEffectiveRateAndSupplements(ProjectFolder projectFolder,
            BaseRateAndSupplement baseRateAndSupplement)
        {
            var folderRateAndSupplements = projectFolder.FolderRateAndSupplement;
            var indirectTimeSupplementPercentage = new BaseRateAndSupplementsValueResponse(
                MapInheritStatus(folderRateAndSupplements.IndirectTimeSupplement.InheritStatus),
                folderRateAndSupplements.IndirectTimeSupplement.Value);
            var siteSpecificTimeSupplementPercentage = new BaseRateAndSupplementsValueResponse(
                MapInheritStatus(folderRateAndSupplements.SiteSpecificTimeSupplement.InheritStatus),
                folderRateAndSupplements.SiteSpecificTimeSupplement.Value);
            var baseRateRegulationPercentage = new BaseRateAndSupplementsValueResponse(
                MapInheritStatus(folderRateAndSupplements.BaseRateRegulation.InheritStatus), folderRateAndSupplements.BaseRateRegulation.Value);
            var baseRatePerMinDkr = baseRateAndSupplement.GetBaseRateInterval(_systemTime.Today()).BaseRate.Value / 60m;
            var personalTimeSupplementPercentage =
                baseRateAndSupplement.GetPersonalTimeSupplementInterval(_systemTime.Today()).PersonalTimeSupplement.Value;

            var workItemCalculator =
                new WorkItemCalculator(new BaseRateAndSupplementProxy(baseRateAndSupplement, projectFolder));
            var combinedSupplementPercentage = workItemCalculator.CalculateCombinedSupplementPercentage(_systemTime.Today()).Evaluate().Value * 100;

            var folderBaseRateAndSupplement = new BaseRateAndSupplementsResponse(indirectTimeSupplementPercentage,
                siteSpecificTimeSupplementPercentage, baseRateRegulationPercentage, combinedSupplementPercentage, baseRatePerMinDkr,
                personalTimeSupplementPercentage);
            return folderBaseRateAndSupplement;
        }

        private static BaseRateAndSupplementsValueStatus MapInheritStatus(FolderValueInheritStatus inheritStatus)
        {
            if (inheritStatus.IsOverwritten())
            {
                return BaseRateAndSupplementsValueStatus.Overwrite;
            }

            return BaseRateAndSupplementsValueStatus.Inherit;
        }

        private static DocumentReferenceResponse MapDocumentResponse(DocumentReference document)
        {
            return new DocumentReferenceResponse(document.ProjectDocumentId.Value, document.ProjectDocumentName.Value,
                document.UploadedTimestamp.Value);
        }

        private static IList<FolderSupplementResponse> MapSupplementResponse(ProjectFolder folder)
        {
            var supplements = new List<FolderSupplementResponse>();
            foreach (var supplement in folder.FolderSupplements)
            {
                supplements.Add(new FolderSupplementResponse(supplement.CatalogSupplementId.Value, supplement.SupplementNumber.Value,
                    supplement.SupplementText.Value));
            }

            return supplements;
        }
    }

    internal class GetProjectFolderRootQueryAuthorizer : IAuthorizer<GetProjectFolderRootQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetProjectFolderRootQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetProjectFolderRootQuery query, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellation);
            if (project.IsOwner(_executionContext.UserId) || project.IsParticipant(_executionContext.UserId) || project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
