using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.projectUser;

namespace deftq.Pieceworks.Domain.project
{
    internal class ProjectCreatedEventHandler : IDomainEventListener<ProjectCreatedDomainEvent>
    {
        private readonly IProjectUserRepository _projectUserRepository;

        public ProjectCreatedEventHandler(IProjectUserRepository projectUserRepository)
        {
            _projectUserRepository = projectUserRepository;
        }

        public async Task Handle(ProjectCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var projectUser = await _projectUserRepository.FindById(notification.ProjectOwner.Id, cancellationToken);
            if (projectUser == null)
            {
                projectUser = ProjectUser.Create(ProjectUserId.Create(notification.ProjectOwner.Id));
            }
            projectUser.RegisterOwnedProject(notification.ProjectId);

            await _projectUserRepository.Update(projectUser, cancellationToken);
        }
    }
}
