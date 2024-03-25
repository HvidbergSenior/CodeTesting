using deftq.BuildingBlocks.Application.Generators;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    internal class ProjectCreatedEventHandler : IDomainEventListener<ProjectCreatedDomainEvent>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly IIdGenerator<Guid> _idGenerator;

        public ProjectCreatedEventHandler(IProjectFolderRootRepository projectFolderRootRepository, IBaseRateAndSupplementRepository baseRateAndSupplementRepository, IIdGenerator<Guid> idGenerator)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
            _idGenerator = idGenerator;
        }

        public async Task Handle(ProjectCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var baseRateAndSupplement = await _baseRateAndSupplementRepository.Get(cancellationToken);
            var folderRateAndSupplement = FolderRateAndSupplement.OverwriteAll(baseRateAndSupplement);
            var projectFolderRoot = ProjectFolderRoot.Create(notification.ProjectId, notification.ProjectName, ProjectFolderRootId.Create(_idGenerator.Generate()), folderRateAndSupplement);

            await _projectFolderRootRepository.Add(projectFolderRoot, cancellationToken);
            await _projectFolderRootRepository.SaveChanges(cancellationToken);
        }
    }
}