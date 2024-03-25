using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.project.Company;
using deftq.Pieceworks.Domain.projectCompensation;

namespace deftq.Pieceworks.Application.CreateProject
{
    public sealed class CreateProjectCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        internal ProjectName ProjectName { get; }
        internal ProjectDescription ProjectDescription { get; }
        internal ProjectPieceworkType ProjectPieceworkType { get; }
        internal ProjectLumpSumPayment ProjectLumpSum { get; }

        private CreateProjectCommand(ProjectId projectId, ProjectName projectName, ProjectDescription projectDescription,
            ProjectPieceworkType projectPieceworkType, ProjectLumpSumPayment projectLumpSum)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            ProjectDescription = projectDescription;
            ProjectPieceworkType = projectPieceworkType;
            ProjectLumpSum = projectLumpSum;
        }

        public static CreateProjectCommand Create(Guid projectId, string name, string description, PieceworkType pieceworkType,
            decimal projectLumpSumPaymentDkr)
        {
            return new CreateProjectCommand(ProjectId.Create(projectId), ProjectName.Create(name), ProjectDescription.Create(description),
                pieceworkType.ToDomain(), ProjectLumpSumPayment.Create(projectLumpSumPaymentDkr));
        }
    }

    internal class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand, ICommandResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecutionContext _executionContext;
        private readonly ISystemTime _systemTime;
        private readonly IProjectNumberGenerator _projectNumberGenerator;

        public CreateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext,
            ISystemTime systemTime, IProjectNumberGenerator projectNumberGenerator)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _executionContext = executionContext;
            _systemTime = systemTime;
            _projectNumberGenerator = projectNumberGenerator;
        }

        public async Task<ICommandResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var projectNumber = await _projectNumberGenerator.GetNextProjectNumber(cancellationToken);
            var projectCreator = ProjectCreatedBy.Create(_executionContext.UserName, _executionContext.UserId);
            var projectCreationTime = ProjectCreatedTime.Create(_systemTime.Now());
            var project = Project.Create(request.ProjectId, request.ProjectName, ProjectNumber.Create(projectNumber), ProjectPieceWorkNumber.Empty(),
                ProjectOrderNumber.Empty(), request.ProjectDescription, ProjectOwner.Create(_executionContext.UserName, _executionContext.UserId),
                request.ProjectPieceworkType, request.ProjectLumpSum, ProjectStartDate.Create(_systemTime.Today()), ProjectEndDate.Empty(),
                projectCreator, projectCreationTime, ProjectCompany.Empty() );

            await _projectRepository.Add(project, cancellationToken);
            await _projectRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }
}
