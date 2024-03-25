using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectSpecificOperation;

namespace deftq.Pieceworks.Application.RegisterProjectSpecificOperation
{
    public sealed class RegisterProjectSpecificOperationCommand : ICommand<ICommandResponse>
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectSpecificOperationId ProjectSpecificOperationId { get; private set; }
        public ProjectSpecificOperationExtraWorkAgreementNumber ProjectSpecificOperationExtraWorkAgreementNumber { get; private set; }
        public ProjectSpecificOperationName ProjectSpecificOperationName { get; private set; }
        public ProjectSpecificOperationDescription ProjectSpecificOperationDescription { get; private set; }
        public ProjectSpecificOperationTime OperationTime { get; private set; }
        public ProjectSpecificOperationTime WorkingTime { get; private set; }

        private RegisterProjectSpecificOperationCommand(ProjectId projectId, ProjectSpecificOperationId projectSpecificOperationId,
            ProjectSpecificOperationExtraWorkAgreementNumber projectSpecificOperationExtraWorkAgreementNumber,
            ProjectSpecificOperationName projectSpecificOperationName, ProjectSpecificOperationDescription projectSpecificOperationDescription,
            ProjectSpecificOperationTime operationTime, ProjectSpecificOperationTime workingTime)
        {
            ProjectId = projectId;
            ProjectSpecificOperationId = projectSpecificOperationId;
            ProjectSpecificOperationExtraWorkAgreementNumber = projectSpecificOperationExtraWorkAgreementNumber;
            ProjectSpecificOperationName = projectSpecificOperationName;
            ProjectSpecificOperationDescription = projectSpecificOperationDescription;
            OperationTime = operationTime;
            WorkingTime = workingTime;
        }

        public static RegisterProjectSpecificOperationCommand Create(Guid projectId, Guid projectSpecificOperationId, string extraWorkAgreementNumber,
            string name, string? description, decimal operationTimeMs, decimal workingTimeMs)
        {
            var workingTime = workingTimeMs > 0 ? ProjectSpecificOperationTime.Create(workingTimeMs) : ProjectSpecificOperationTime.Empty();
            var operationTime = operationTimeMs > 0 ? ProjectSpecificOperationTime.Create(operationTimeMs) : ProjectSpecificOperationTime.Empty();
            
            return new RegisterProjectSpecificOperationCommand(ProjectId.Create(projectId),
                ProjectSpecificOperationId.Create(projectSpecificOperationId),
                ProjectSpecificOperationExtraWorkAgreementNumber.Create(extraWorkAgreementNumber), ProjectSpecificOperationName.Create(name),
                ProjectSpecificOperationDescription.Create(description), operationTime,
                workingTime);
        }
    }

    internal class RegisterProjectSpecificOperationCommandHandler : ICommandHandler<RegisterProjectSpecificOperationCommand, ICommandResponse>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly IProjectSpecificOperationListRepository _projectSpecificOperationListRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemTime _systemTime;

        public RegisterProjectSpecificOperationCommandHandler(IProjectFolderRootRepository projectFolderRootRepository,
            IBaseRateAndSupplementRepository baseRateAndSupplementRepository,
            IProjectSpecificOperationListRepository projectSpecificOperationListRepository, IUnitOfWork unitOfWork, ISystemTime systemTime)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
            _projectSpecificOperationListRepository = projectSpecificOperationListRepository;
            _unitOfWork = unitOfWork;
            _systemTime = systemTime;
        }

        public async Task<ICommandResponse> Handle(RegisterProjectSpecificOperationCommand command, CancellationToken cancellationToken)
        {
            var projectSpecificOperationList =
                await _projectSpecificOperationListRepository.GetByProjectId(command.ProjectId.Value, cancellationToken);

            if (command.OperationTime.IsEmpty() && command.WorkingTime.IsEmpty())
            {
                throw new ArgumentException("Operation and working time is nothing", nameof(command));
            }

            var operation = ProjectSpecificOperation.Create(command.ProjectSpecificOperationId,
                command.ProjectSpecificOperationExtraWorkAgreementNumber, command.ProjectSpecificOperationName,
                command.ProjectSpecificOperationDescription, command.OperationTime, command.WorkingTime);

            if (command.OperationTime.IsEmpty())
            {
                var folderRoot = await _projectFolderRootRepository.GetByProjectId(command.ProjectId.Value, cancellationToken);
                var systemBaseRateAndSupplement = await _baseRateAndSupplementRepository.Get(cancellationToken);
                var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(systemBaseRateAndSupplement, folderRoot.RootFolder);
                var calc = new ProjectSpecificOperationCalculator(baseRateAndSupplementProxy, _systemTime);
                var operationTime = calc.CalculateOperationTime(command.WorkingTime);
                operation.UpdateOperationTime(operationTime);
            } else if (!command.OperationTime.IsEmpty() && !command.WorkingTime.IsEmpty())
            {
                operation.UpdateWorkingTime(ProjectSpecificOperationTime.Empty());
            }

            projectSpecificOperationList.AddProjectSpecificOperation(operation);

            await _projectSpecificOperationListRepository.Update(projectSpecificOperationList, cancellationToken);
            await _projectSpecificOperationListRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class RegisterProjectSpecificOperationCommandAuthorizer : IAuthorizer<RegisterProjectSpecificOperationCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public RegisterProjectSpecificOperationCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(RegisterProjectSpecificOperationCommand command, CancellationToken cancellation)
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
