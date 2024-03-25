using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectCompensation;

namespace deftq.Pieceworks.Application.RegisterProjectCompensation
{
    public sealed class RegisterProjectCompensationCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public ProjectCompensationId ProjectCompensationId { get; set; }
        public IList<ProjectParticipantId> ProjectParticipantIds { get; set; }
        public ProjectCompensationPayment ProjectCompensationPayment { get; set; }
        public ProjectCompensationPeriod ProjectCompensationPeriod { get; set; }

        private RegisterProjectCompensationCommand(ProjectId projectId, ProjectCompensationId projectCompensationId,
            IList<ProjectParticipantId> projectParticipantIds, ProjectCompensationPayment projectCompensationPayment,
            ProjectCompensationPeriod projectCompensationPeriod)
        {
            ProjectId = projectId;
            ProjectCompensationId = projectCompensationId;
            ProjectParticipantIds = projectParticipantIds;
            ProjectCompensationPayment = projectCompensationPayment;
            ProjectCompensationPeriod = projectCompensationPeriod;
        }

        public static RegisterProjectCompensationCommand Create(Guid projectId, Guid projectCompensationId, IList<Guid> compensationParticipantIds, decimal projectCompensationPayment,
            ProjectCompensationDate startDate, ProjectCompensationDate endDate)
        {
            var projectParticipantIds = compensationParticipantIds.Select(guid => ProjectParticipantId.Create(guid)).ToList();
            return new RegisterProjectCompensationCommand(ProjectId.Create(projectId), ProjectCompensationId.Create(projectCompensationId),
                projectParticipantIds, ProjectCompensationPayment.Create(projectCompensationPayment),
                ProjectCompensationPeriod.Create(startDate, endDate));
        }
    }

    internal class RegisterProjectCompensationCommandHandler : ICommandHandler<RegisterProjectCompensationCommand, ICommandResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectCompensationListRepository _projectCompensationListRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterProjectCompensationCommandHandler(IProjectRepository projectRepository, IProjectCompensationListRepository projectCompensationListRepository, IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _projectCompensationListRepository = projectCompensationListRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(RegisterProjectCompensationCommand command, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellationToken);
            
            var compensationList = await _projectCompensationListRepository.GetByProjectId(command.ProjectId.Value);

            foreach (var participantId in command.ProjectParticipantIds)
            {
                if (!project.ProjectParticipants.Any(x => x.ParticipantId == participantId) && participantId.Value != project.ProjectOwner.Id)
                {
                    throw new InvalidOperationException("only project participants and project owner allowed in the list");
                }
            }

            var compensation = ProjectCompensation.Create(command.ProjectCompensationId, command.ProjectCompensationPayment,
                command.ProjectCompensationPeriod, command.ProjectParticipantIds);
            compensationList.AddCompensation(compensation);
                
            await _projectCompensationListRepository.Update(compensationList, cancellationToken);
            await _projectCompensationListRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class RegisterProjectCompensationCommandAuthorizer : IAuthorizer<RegisterProjectCompensationCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public RegisterProjectCompensationCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(RegisterProjectCompensationCommand command, CancellationToken cancellation)
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
