using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;

namespace deftq.Pieceworks.Application.RegisterProjectCatalogFavorite
{
    public sealed class RegisterProjectCatalogFavoriteCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public CatalogFavoriteId CatalogFavoriteId { get; set; }
        public CatalogItemType CatalogItemType { get; set; }
        public CatalogItemId CatalogItemId { get; set; }
        public CatalogItemNumber CatalogItemNumber { get; set; }
        public CatalogItemText CatalogItemText { get; set; }
        public CatalogItemUnit CatalogItemUnit { get; set; }

        private RegisterProjectCatalogFavoriteCommand(ProjectId projectId, CatalogFavoriteId catalogFavoriteId, CatalogItemType catalogItemType,
            CatalogItemId catalogItemId, CatalogItemNumber catalogItemNumber, CatalogItemText catalogItemText, CatalogItemUnit catalogItemUnit)
        {
            ProjectId = projectId;
            CatalogFavoriteId = catalogFavoriteId;
            CatalogItemType = catalogItemType;
            CatalogItemId = catalogItemId;
            CatalogItemNumber = catalogItemNumber;
            CatalogItemText = catalogItemText;
            CatalogItemUnit = catalogItemUnit;
        }

        public static RegisterProjectCatalogFavoriteCommand Create(Guid projectId, Guid catalogFavoriteId, CatalogItemType catalogItemType,
            Guid catalogItemId, string catalogItemNumber, string catalogItemText, string catalogItemUnit)
        {
            return new RegisterProjectCatalogFavoriteCommand(ProjectId.Create(projectId), CatalogFavoriteId.Create(catalogFavoriteId),
                catalogItemType, CatalogItemId.Create(catalogItemId), CatalogItemNumber.Create(catalogItemNumber),
                CatalogItemText.Create(catalogItemText), CatalogItemUnit.Create(catalogItemUnit));
        }
    }

    internal class RegisterProjectCatalogFavoriteCommandHandler : ICommandHandler<RegisterProjectCatalogFavoriteCommand, ICommandResponse>
    {
        private readonly IProjectCatalogFavoriteListRepository _projectCatalogFavoriteListRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterProjectCatalogFavoriteCommandHandler(IProjectCatalogFavoriteListRepository projectCatalogFavoriteListRepository, IUnitOfWork unitOfWork)
        {
            _projectCatalogFavoriteListRepository = projectCatalogFavoriteListRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(RegisterProjectCatalogFavoriteCommand command, CancellationToken cancellationToken)
        {
            var favoriteList = await _projectCatalogFavoriteListRepository.GetByProjectId(command.ProjectId.Value);

            var favorite = CatalogFavorite.Create(command.CatalogFavoriteId, command.CatalogItemType, command.CatalogItemId,
                command.CatalogItemNumber, command.CatalogItemText, command.CatalogItemUnit);
            favoriteList.AddFavorite(favorite);

            await _projectCatalogFavoriteListRepository.Update(favoriteList, cancellationToken);
            await _projectCatalogFavoriteListRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            
            return EmptyCommandResponse.Default;
        }
    }

    internal class RegisterProjectCatalogFavoriteCommandAuthorizer : IAuthorizer<RegisterProjectCatalogFavoriteCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public RegisterProjectCatalogFavoriteCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(RegisterProjectCatalogFavoriteCommand command, CancellationToken cancellation)
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
