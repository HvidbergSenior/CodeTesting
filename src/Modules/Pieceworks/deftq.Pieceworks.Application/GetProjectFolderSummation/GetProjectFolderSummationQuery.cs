using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Application.GetProjectFolderSummation
{
    public sealed class GetProjectFolderSummationQuery : IQuery<GetProjectFolderSummationQueryResponse>
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }

        private GetProjectFolderSummationQuery(ProjectId projectId, ProjectFolderId projectFolderId)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
        }

        public static GetProjectFolderSummationQuery Create(ProjectId projectId, ProjectFolderId projectFolderId)
        {
            return new GetProjectFolderSummationQuery(projectId, projectFolderId);
        }
    }

    internal class GetProjectFolderSummationQueryHandler : IQueryHandler<GetProjectFolderSummationQuery, GetProjectFolderSummationQueryResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;

        public GetProjectFolderSummationQueryHandler(IProjectFolderRootRepository projectFolderRootRepository,
            IProjectFolderWorkRepository projectFolderWorkRepository)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _projectFolderWorkRepository = projectFolderWorkRepository;
        }

        public async Task<GetProjectFolderSummationQueryResponse> Handle(GetProjectFolderSummationQuery query, CancellationToken cancellationToken)
        {
            var projectId = query.ProjectId.Value;
            var folderId = query.ProjectFolderId.Value;
            
            var folderRoot = await _projectFolderRootRepository.GetByProjectId(projectId, cancellationToken);
            var folder = folderRoot.GetFolder(query.ProjectFolderId);

            var allFoldersInSubTree = folderRoot.GetFolderAndSubfolders(folder.ProjectFolderId);
            var allFoldersInSubTreeIds = allFoldersInSubTree.Select(f => f.ProjectFolderId.Value).ToList();
            var allFolderWorks = await _projectFolderWorkRepository.GetByProjectAndFolderIds(projectId, allFoldersInSubTreeIds, cancellationToken);
            
            var folderCalculator = new FolderCalculator();
            var folderResult = folderCalculator.CalculateTotalOperationTime(folderRoot, folder.ProjectFolderId, allFolderWorks);

            var totalWorkTimeMilliseconds = folderResult.TotalWorkTimeExpression.Evaluate().Value;
            var totalPaymentDkr = folderResult.TotalPaymentExpression.Evaluate().Value;
            var totalExtraWorkTimeMilliseconds = folderResult.TotalExtraWorkTimeExpression.Evaluate().Value;
            var totalExtraPaymentDkr = folderResult.TotalExtraWorkPaymentExpression.Evaluate().Value;
            
            return await Task.FromResult(new GetProjectFolderSummationQueryResponse(totalWorkTimeMilliseconds, totalPaymentDkr, totalExtraWorkTimeMilliseconds, totalExtraPaymentDkr));
        }
    }

    internal class GetProjectFolderSummationQueryAuthorizer : IAuthorizer<GetProjectFolderSummationQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetProjectFolderSummationQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetProjectFolderSummationQuery query, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellation);
            if (project.IsOwner(_executionContext.UserId) ||
                project.IsParticipant(_executionContext.UserId) ||
                project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
