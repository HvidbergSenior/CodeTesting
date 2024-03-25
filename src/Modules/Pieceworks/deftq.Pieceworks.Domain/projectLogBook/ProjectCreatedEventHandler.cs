using deftq.BuildingBlocks.Application.Generators;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    internal class ProjectCreatedEventHandler : IDomainEventListener<ProjectCreatedDomainEvent>
    {
        private readonly IProjectLogBookRepository _projectLogBookRepository;
        private readonly IIdGenerator<Guid> _idGenerator;

        public ProjectCreatedEventHandler(IProjectLogBookRepository projectLogBookRepository, IIdGenerator<Guid> idGenerator)
        {
            _projectLogBookRepository = projectLogBookRepository;
            _idGenerator = idGenerator;
        }

        public async Task Handle(ProjectCreatedDomainEvent notification, CancellationToken cancellation)
        {
            var projectLogBook = ProjectLogBook.Create(notification.ProjectId, ProjectLogBookId.Create(_idGenerator.Generate()));

            await _projectLogBookRepository.Add(projectLogBook, cancellation);
            await _projectLogBookRepository.SaveChanges(cancellation);
        }
    }
}
