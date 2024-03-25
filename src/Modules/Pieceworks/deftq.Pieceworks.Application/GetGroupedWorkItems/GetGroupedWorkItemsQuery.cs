using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.GetGroupedWorkItems
{
    public sealed class GetGroupedWorkItemsQuery : IQuery<GetGroupedWorkItemsQueryResponse>
    {
        private const uint MaxHitsLimit = 1000;
        public ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        public uint MaxHits { get; }


        private GetGroupedWorkItemsQuery(ProjectId projectId, ProjectFolderId projectFolderId, uint maxHits)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            MaxHits = Math.Min(MaxHitsLimit, maxHits);
        }

        public static GetGroupedWorkItemsQuery Create(Guid projectId, Guid folderId, uint maxHits)
        {
            return new GetGroupedWorkItemsQuery(ProjectId.Create(projectId), ProjectFolderId.Create(folderId), maxHits);
        }
    }

    internal class GetGroupedWorkItemsQueryHandler : IQueryHandler<GetGroupedWorkItemsQuery, GetGroupedWorkItemsQueryResponse>
    {
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IExecutionContext _executionContext;

        public GetGroupedWorkItemsQueryHandler(IProjectFolderRootRepository projectFolderRootRepository,
            IProjectFolderWorkRepository projectFolderWorkRepository, IExecutionContext executionContext)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _projectFolderWorkRepository = projectFolderWorkRepository;
            _executionContext = executionContext;
        }

        public async Task<GetGroupedWorkItemsQueryResponse> Handle(GetGroupedWorkItemsQuery query, CancellationToken cancellationToken)
        {
            // Fetch data
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(query.ProjectId.Value, cancellationToken);

            // Fetch data - list of ProjectFolderWork under folder
            var folderAndSubfolders = projectFolderRoot.GetFolderAndSubfolders(query.ProjectFolderId);
            var folderGuids = folderAndSubfolders.Select(folder => folder.ProjectFolderId.Value).ToList();
            var projectFolderWorks =
                await _projectFolderWorkRepository.GetByProjectAndFolderIds(query.ProjectId.Value, folderGuids, cancellationToken);

            // Compile grouped work items
            return CompileGroupedWorkItems(projectFolderRoot, query.ProjectFolderId, projectFolderWorks, query.MaxHits);
        }

        private GetGroupedWorkItemsQueryResponse CompileGroupedWorkItems(ProjectFolderRoot folderRoot, ProjectFolderId folderId,
            IList<ProjectFolderWork> folderWorkList, uint maxHits)
        {
            var workItemGrouped = new GroupedWorkItemsCalculator();
            var calculateGroupedWorkItems = workItemGrouped.CalculateGroupedWorkItems(folderRoot, folderId, folderWorkList);

            var groupedWorkItemsResponses = calculateGroupedWorkItems
                .Select(groupedWorkItem =>
                    new GroupedWorkItemsResponse(groupedWorkItem.Id, groupedWorkItem.Text, groupedWorkItem.Amount, groupedWorkItem.PaymentDkr))
                .OrderByDescending(a => a.Amount).Take((int)maxHits).ToList();

            return new GetGroupedWorkItemsQueryResponse(groupedWorkItemsResponses);
        }
    }

    internal class GetGroupedWorkItemsQueryAuthorizer : IAuthorizer<GetGroupedWorkItemsQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetGroupedWorkItemsQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetGroupedWorkItemsQuery query, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellation);
            if (project.IsOwner(_executionContext.UserId) || project.IsParticipant(_executionContext.UserId) ||
                project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
