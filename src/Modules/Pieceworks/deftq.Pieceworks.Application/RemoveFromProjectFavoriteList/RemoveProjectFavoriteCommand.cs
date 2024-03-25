using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;

namespace deftq.Pieceworks.Application.RemoveFromProjectFavorite
{
    public sealed class RemoveProjectFavoriteCommand : ICommand<ICommandResponse>
    {
        public ProjectId ProjectId { get; }
        public IList<CatalogFavoriteId> CatalogFavoriteIds { get; }

        private RemoveProjectFavoriteCommand(ProjectId projectId, IList<CatalogFavoriteId> catalogFavoriteIds)
        {
            ProjectId = projectId;
            CatalogFavoriteIds = catalogFavoriteIds;
        }

        public static RemoveProjectFavoriteCommand Create(Guid projectId, IList<Guid> catalogFavoriteIds)
        {
            return new RemoveProjectFavoriteCommand(ProjectId.Create(projectId), catalogFavoriteIds.Select(CatalogFavoriteId.Create).ToList());
        }
    }
    
    internal class RemoveFromProjectFavoriteListCommandHandler : ICommandHandler<RemoveProjectFavoriteCommand, ICommandResponse>
    {
        private readonly IProjectCatalogFavoriteListRepository _projectCatalogFavoriteListRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;
        
        public RemoveFromProjectFavoriteListCommandHandler(IProjectCatalogFavoriteListRepository projectCatalogFavoriteListRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectCatalogFavoriteListRepository = projectCatalogFavoriteListRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }
        public async Task<ICommandResponse> Handle(RemoveProjectFavoriteCommand request, CancellationToken cancellationToken)
        {
            var favoriteList = await _projectCatalogFavoriteListRepository.GetByProjectId(request.ProjectId.Value);
            favoriteList.RemoveFavorites(request.CatalogFavoriteIds);

            await _projectCatalogFavoriteListRepository.Update(favoriteList, cancellationToken);
            await _projectCatalogFavoriteListRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            
            return EmptyCommandResponse.Default;
        }
    }

    internal class RemoveProjectFavoriteCommandAuthorizer : IAuthorizer<RemoveProjectFavoriteCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public RemoveProjectFavoriteCommandAuthorizer(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }
        
        public async Task<AuthorizationResult> Authorize(RemoveProjectFavoriteCommand command, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellation);
            if (project.IsOwner(_executionContext.UserId) || project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}
