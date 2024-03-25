using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Application.RegisterExtraWorkAgreement;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectExtraWorkAgreement;

namespace deftq.Pieceworks.Application.UpdateExtraWorkAgreement
{
    public sealed class UpdateExtraWorkAgreementCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public ProjectExtraWorkAgreementId ProjectExtraWorkAgreementId { get; }
        public ProjectExtraWorkAgreementNumber ProjectExtraWorkAgreementNumber { get; }
        public ProjectExtraWorkAgreementName ProjectExtraWorkAgreementName { get; }
        public ProjectExtraWorkAgreementDescription ProjectExtraWorkAgreementDescription { get; }
        public ProjectExtraWorkAgreementType ProjectExtraWorkAgreementType { get; }
        public ProjectExtraWorkAgreementPaymentDkr? ProjectExtraWorkAgreementPaymentDkr { get; }
        public ProjectExtraWorkAgreementHours? ProjectExtraWorkAgreementHours { get; }
        public ProjectExtraWorkAgreementMinutes? ProjectExtraWorkAgreementMinutes { get; }

        private UpdateExtraWorkAgreementCommand(ProjectId projectId, ProjectExtraWorkAgreementId projectExtraWorkAgreementId,
            ProjectExtraWorkAgreementNumber projectExtraWorkAgreementNumber,
            ProjectExtraWorkAgreementName projectExtraWorkAgreementName, ProjectExtraWorkAgreementDescription projectExtraWorkAgreementDescription,
            ProjectExtraWorkAgreementType projectExtraWorkAgreementType, ProjectExtraWorkAgreementPaymentDkr? projectExtraWorkAgreementPaymentDkr,
            ProjectExtraWorkAgreementHours? projectExtraWorkAgreementHours, ProjectExtraWorkAgreementMinutes? projectExtraWorkAgreementMinutes)
        {
            ProjectId = projectId;
            ProjectExtraWorkAgreementId = projectExtraWorkAgreementId;
            ProjectExtraWorkAgreementNumber = projectExtraWorkAgreementNumber;
            ProjectExtraWorkAgreementName = projectExtraWorkAgreementName;
            ProjectExtraWorkAgreementDescription = projectExtraWorkAgreementDescription;
            ProjectExtraWorkAgreementType = projectExtraWorkAgreementType;
            ProjectExtraWorkAgreementPaymentDkr = projectExtraWorkAgreementPaymentDkr;
            ProjectExtraWorkAgreementHours = projectExtraWorkAgreementHours;
            ProjectExtraWorkAgreementMinutes = projectExtraWorkAgreementMinutes;
        }

        public static UpdateExtraWorkAgreementCommand Create(Guid projectId, Guid projectExtraWorkAgreementId, string projectExtraWorkAgreementNumber,
            string projectExtraWorkAgreementName, string projectExtraWorkAgreementDescription, UpdateExtraWorkAgreementType updateExtraWorkAgreementType,
            decimal? projectExtraWorkAgreementPaymentDkr, int? projectExtraWorkAgreementHours, int? projectExtraWorkAgreementMinutes)
        {
            var extraWorkAgreementPaymentDkr = projectExtraWorkAgreementPaymentDkr is not null
                ? ProjectExtraWorkAgreementPaymentDkr.Create(projectExtraWorkAgreementPaymentDkr.Value)
                : null;
            
            var extraWorkAgreementHours = projectExtraWorkAgreementHours is not null
                ? ProjectExtraWorkAgreementHours.Create(projectExtraWorkAgreementHours.Value)
                : null;

            var extraWorkAgreementMinutes = projectExtraWorkAgreementMinutes is not null
                ? ProjectExtraWorkAgreementMinutes.Create(projectExtraWorkAgreementMinutes.Value)
                : null;

            return new UpdateExtraWorkAgreementCommand(ProjectId.Create(projectId), ProjectExtraWorkAgreementId.Create(projectExtraWorkAgreementId),
                ProjectExtraWorkAgreementNumber.Create(projectExtraWorkAgreementNumber),
                ProjectExtraWorkAgreementName.Create(projectExtraWorkAgreementName),
                ProjectExtraWorkAgreementDescription.Create(projectExtraWorkAgreementDescription), updateExtraWorkAgreementType.ToDomain(),
                extraWorkAgreementPaymentDkr, extraWorkAgreementHours, extraWorkAgreementMinutes);
        }
    }

    internal class UpdateExtraWorkAgreementCommandHandler : ICommandHandler<UpdateExtraWorkAgreementCommand, ICommandResponse>
    {
        private readonly IProjectExtraWorkAgreementListRepository _projectExtraWorkAgreementListRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public UpdateExtraWorkAgreementCommandHandler(IProjectExtraWorkAgreementListRepository projectExtraWorkAgreementListRepository,
            IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectExtraWorkAgreementListRepository = projectExtraWorkAgreementListRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<ICommandResponse> Handle(UpdateExtraWorkAgreementCommand request, CancellationToken cancellationToken)
        {
            var extraWorkAgreementList = await _projectExtraWorkAgreementListRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);

           extraWorkAgreementList.UpdateExtraWorkAgreement(request.ProjectId, request.ProjectExtraWorkAgreementId, request.ProjectExtraWorkAgreementNumber,
                request.ProjectExtraWorkAgreementName, request.ProjectExtraWorkAgreementDescription, request.ProjectExtraWorkAgreementType,
                request.ProjectExtraWorkAgreementPaymentDkr, request.ProjectExtraWorkAgreementHours, request.ProjectExtraWorkAgreementMinutes);

           await _projectExtraWorkAgreementListRepository.Update(extraWorkAgreementList, cancellationToken);
           await _projectExtraWorkAgreementListRepository.SaveChanges(cancellationToken);
           await _unitOfWork.Commit(cancellationToken);
           
            return EmptyCommandResponse.Default;
        }
    }

    internal class UpdateExtraWorkAgreementCommandAuthorizer : IAuthorizer<UpdateExtraWorkAgreementCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public UpdateExtraWorkAgreementCommandAuthorizer(IProjectRepository projectRepository, IUnitOfWork unitOfWork,
            IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(UpdateExtraWorkAgreementCommand command, CancellationToken cancellation)
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
