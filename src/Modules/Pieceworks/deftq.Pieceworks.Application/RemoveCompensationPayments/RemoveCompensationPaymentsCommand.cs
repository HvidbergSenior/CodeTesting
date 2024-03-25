using Baseline;
using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectCompensation;

namespace deftq.Pieceworks.Application.RemoveCompensationPayments
{
    public sealed class RemoveCompensationPaymentsCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public IList<ProjectCompensationId> CompensationPaymentIds { get; }

        private RemoveCompensationPaymentsCommand(ProjectId projectId, IList<ProjectCompensationId> compensationPaymentIds)
        {
            ProjectId = projectId;
            CompensationPaymentIds = compensationPaymentIds;
        }

        public static RemoveCompensationPaymentsCommand Create(Guid projectId, IList<Guid> compensationPaymentIds)
        {
            return new RemoveCompensationPaymentsCommand(ProjectId.Create(projectId),
                compensationPaymentIds.Select(ProjectCompensationId.Create).ToList());
        }
    }

    internal class RemoveCompensationPaymentsCommandHandler : ICommandHandler<RemoveCompensationPaymentsCommand, ICommandResponse>
    {
        private readonly IProjectCompensationListRepository _projectCompensationListRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public RemoveCompensationPaymentsCommandHandler(IProjectCompensationListRepository projectCompensationListRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            _projectCompensationListRepository = projectCompensationListRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<ICommandResponse> Handle(RemoveCompensationPaymentsCommand request, CancellationToken cancellationToken)
        {
            var projectCompensationPayments = await _projectCompensationListRepository.GetByProjectId(request.ProjectId.Value);
            projectCompensationPayments.RemoveCompensations(request.CompensationPaymentIds);

            await _projectCompensationListRepository.Update(projectCompensationPayments, cancellationToken);
            await _projectCompensationListRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            
            return EmptyCommandResponse.Default;
        }
    }

    internal class RemoveCompensationPaymentsCommandAuthorizer : IAuthorizer<RemoveCompensationPaymentsCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;

        public RemoveCompensationPaymentsCommandAuthorizer(IProjectRepository projectRepository, IUnitOfWork unitOfWork,
            IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(RemoveCompensationPaymentsCommand command, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellation);

            if (project.IsOwner(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
