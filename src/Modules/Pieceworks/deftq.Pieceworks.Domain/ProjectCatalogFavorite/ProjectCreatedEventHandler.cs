using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.ProjectCatalogFavorite
{
    public class ProjectCreatedEventHandler : IDomainEventListener<ProjectCreatedDomainEvent>
    {
        private readonly IProjectCatalogFavoriteListRepository _projectCatalogFavoriteListRepository;

        public ProjectCreatedEventHandler(IProjectCatalogFavoriteListRepository projectCatalogFavoriteListRepository)
        {
            _projectCatalogFavoriteListRepository = projectCatalogFavoriteListRepository;
        }

        public async Task Handle(ProjectCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var projectCatalogFavoriteList = ProjectCatalogFavoriteList.Create(ProjectCatalogFavoriteListId.Create(Guid.NewGuid()), notification.ProjectId);
            await _projectCatalogFavoriteListRepository.Add(projectCatalogFavoriteList, cancellationToken);
            await _projectCatalogFavoriteListRepository.SaveChanges(cancellationToken);
        }
    }
}
