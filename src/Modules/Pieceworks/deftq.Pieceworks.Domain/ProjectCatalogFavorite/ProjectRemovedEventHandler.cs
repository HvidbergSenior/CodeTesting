using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;

namespace deftq.Pieceworks.Domain.projectCatalogFavorite
{
    public class ProjectRemovedEventHandler  : IDomainEventListener<ProjectRemovedDomainEvent>
    {
        private readonly IProjectCatalogFavoriteListRepository _projectCatalogFavoriteListRepository;
        private readonly IUnitOfWork _unitOfWork;
        
        public ProjectRemovedEventHandler(IProjectCatalogFavoriteListRepository projectCatalogFavoriteListRepository, IUnitOfWork unitOfWork)
        {
            _projectCatalogFavoriteListRepository = projectCatalogFavoriteListRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProjectRemovedDomainEvent notification, CancellationToken cancellationToken)
        {
            var projectFavoriteList = await _projectCatalogFavoriteListRepository.GetByProjectId(notification.ProjectId.Value);
            
            await _projectCatalogFavoriteListRepository.Delete(projectFavoriteList, cancellationToken);
            await _projectCatalogFavoriteListRepository.SaveChanges(cancellationToken);
        }
    }
}
