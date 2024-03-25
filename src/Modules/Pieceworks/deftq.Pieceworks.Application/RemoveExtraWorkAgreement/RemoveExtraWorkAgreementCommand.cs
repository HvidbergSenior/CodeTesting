using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;

namespace deftq.Pieceworks.Application.RemoveExtraWorkAgreement
{
    public sealed class RemoveExtraWorkAgreementCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public IList<ProjectExtraWorkAgreementId> ExtraWorkAgreementIds { get; }

        private RemoveExtraWorkAgreementCommand(ProjectId projectId, IList<ProjectExtraWorkAgreementId> extraWorkAgreementIds)
        {
            ProjectId = projectId;
            ExtraWorkAgreementIds = extraWorkAgreementIds;
        }

        public static RemoveExtraWorkAgreementCommand Create(Guid projectId, IList<Guid> extraWorkAgreementIds)
        {
            return new RemoveExtraWorkAgreementCommand(ProjectId.Create(projectId),
                extraWorkAgreementIds.Select(ProjectExtraWorkAgreementId.Create).ToList());
        }
    }

    internal class RemoveExtraWorkAgreementCommandHandler : ICommandHandler<RemoveExtraWorkAgreementCommand, ICommandResponse>
    {
        private readonly IProjectExtraWorkAgreementListRepository _projectExtraWorkAgreementListRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public RemoveExtraWorkAgreementCommandHandler(IProjectExtraWorkAgreementListRepository projectExtraWorkAgreementListRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectExtraWorkAgreementListRepository = projectExtraWorkAgreementListRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<ICommandResponse> Handle(RemoveExtraWorkAgreementCommand request, CancellationToken cancellationToken)
        {
            var projectExtraWorkAgreements =
                await _projectExtraWorkAgreementListRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);
            projectExtraWorkAgreements.RemoveExtraWorkAgreements(request.ExtraWorkAgreementIds);

            await _projectExtraWorkAgreementListRepository.Update(projectExtraWorkAgreements, cancellationToken);
            await _projectExtraWorkAgreementListRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            
            return EmptyCommandResponse.Default;
        }
    }

    internal class RemoveExtraWorkAgreementCommandAuthorizer : IAuthorizer<RemoveExtraWorkAgreementCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public RemoveExtraWorkAgreementCommandAuthorizer(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(RemoveExtraWorkAgreementCommand command, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellationToken);

            if (project.IsOwner(_executionContext.UserId) || (project.IsProjectManager(_executionContext.UserId)))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
