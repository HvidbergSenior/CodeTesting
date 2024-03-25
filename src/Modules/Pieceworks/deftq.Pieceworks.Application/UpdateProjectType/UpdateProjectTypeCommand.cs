using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess; 
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Application.UpdateProjectType
{
    public sealed class UpdateProjectTypeCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public ProjectPieceworkType PieceworkType { get; }
        public ProjectStartDate StartDate { get; }
        public ProjectEndDate EndDate { get; }
        public ProjectLumpSumPayment ProjectLumpSumPayment { get; }

        private UpdateProjectTypeCommand(ProjectId projectId, ProjectPieceworkType pieceworkType, ProjectStartDate startDate, ProjectEndDate endDate, ProjectLumpSumPayment projectLumpSumPayment)
        {
            ProjectId = projectId;
            PieceworkType = pieceworkType;
            StartDate = startDate;
            EndDate = endDate;
            ProjectLumpSumPayment = projectLumpSumPayment;
        }

        public static UpdateProjectTypeCommand Create(Guid projectId, PieceworkType pieceworkType, ProjectStartDate startDate, ProjectEndDate endDate, ProjectLumpSumPayment projectLumpSumPayment)
        {
            return new UpdateProjectTypeCommand(ProjectId.Create(projectId), pieceworkType.ToDomain(), startDate, endDate, projectLumpSumPayment);
        }
    }
    
    internal class UpdateProjectTypeCommandHandler : ICommandHandler<UpdateProjectTypeCommand, ICommandResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProjectTypeCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(UpdateProjectTypeCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(request.ProjectId.Value, cancellationToken);
            project.UpdateProjectType(request.PieceworkType, request.ProjectLumpSumPayment);
            project.UpdateProjectDates(request.StartDate, request.EndDate);
            
            await _projectRepository.Update(project, cancellationToken);
            await _projectRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }
    
    internal class UpdateProjectTypeCommandAuthorizer : IAuthorizer<UpdateProjectTypeCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public UpdateProjectTypeCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }
        public async Task<AuthorizationResult> Authorize(UpdateProjectTypeCommand command, CancellationToken cancellation)
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
