using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;

namespace deftq.Pieceworks.Application.GetFavoriteList
{
    public sealed class GetProjectFavoriteListQuery : IQuery<GetProjectFavoriteListQueryResponse>
    {
        public ProjectId ProjectId { get; }

        private GetProjectFavoriteListQuery(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static GetProjectFavoriteListQuery Create(Guid projectId)
        {
            return new GetProjectFavoriteListQuery(ProjectId.Create(projectId));
        }
    }

    internal class GetProjectFavoriteListQueryHandler : IQueryHandler<GetProjectFavoriteListQuery, GetProjectFavoriteListQueryResponse>
    {
        private readonly IProjectCatalogFavoriteListRepository _projectCatalogFavoriteListRepository;

        public GetProjectFavoriteListQueryHandler(IProjectCatalogFavoriteListRepository projectCatalogFavoriteListRepository)
        {
            _projectCatalogFavoriteListRepository = projectCatalogFavoriteListRepository;
        }

        public async Task<GetProjectFavoriteListQueryResponse> Handle(GetProjectFavoriteListQuery query, CancellationToken cancellationToken)
        {
            var projectCatalogFavoriteList = await _projectCatalogFavoriteListRepository.GetByProjectId(query.ProjectId.Value);

            return MapResponse(projectCatalogFavoriteList);
        }

        private GetProjectFavoriteListQueryResponse MapResponse(ProjectCatalogFavoriteList projectCatalogFavoriteList)
        {
            var result = projectCatalogFavoriteList.Favorites.Select(favorite => new FavoritesResponse(favorite.CatalogFavoriteId.Value,
                favorite.CatalogItemId.Value, favorite.CatalogItemText.Value, favorite.CatalogItemNumber.Value, favorite.CatalogItemUnit.Value,
                MapCatalogType(favorite.CatalogItemType))).ToList();
            return new GetProjectFavoriteListQueryResponse(result);
        }

        private CatalogItemType MapCatalogType(Domain.ProjectCatalogFavorite.CatalogItemType catalogItemType)
        {
            return catalogItemType == Domain.ProjectCatalogFavorite.CatalogItemType.Material ? CatalogItemType.Material : CatalogItemType.Operation;
        }
    }

    internal class GetProjectFavoriteListQueryAuthorizer : IAuthorizer<GetProjectFavoriteListQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetProjectFavoriteListQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetProjectFavoriteListQuery query, CancellationToken cancellation)
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
