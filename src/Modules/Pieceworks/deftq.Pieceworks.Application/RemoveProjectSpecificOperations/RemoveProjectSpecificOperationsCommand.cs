using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectSpecificOperation;

namespace deftq.Pieceworks.Application.RemoveProjectSpecificOperations
{
    public sealed class RemoveProjectSpecificOperationsCommand : ICommand<ICommandResponse>
    {
        public ProjectId ProjectId { get; }
        public IList<ProjectSpecificOperationId> ProjectSpecificOperationIds { get; }

        private RemoveProjectSpecificOperationsCommand(ProjectId projectId, IList<ProjectSpecificOperationId> projectSpecificOperationIds)
        {
            ProjectId = projectId;
            ProjectSpecificOperationIds = projectSpecificOperationIds;
        }

        public static RemoveProjectSpecificOperationsCommand Create(Guid projectId, IList<Guid> projectSpecificOperationIds)
        {
            return new RemoveProjectSpecificOperationsCommand(ProjectId.Create(projectId), projectSpecificOperationIds.Select(ProjectSpecificOperationId.Create).ToList());
        }
    }
    
    internal class RemoveProjectSpecificOperationsCommandHandler : ICommandHandler<RemoveProjectSpecificOperationsCommand, ICommandResponse>
    {
        private readonly IProjectSpecificOperationListRepository _projectSpecificOperationListRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveProjectSpecificOperationsCommandHandler(IProjectSpecificOperationListRepository projectSpecificOperationListRepository, IUnitOfWork unitOfWork)
        {
            _projectSpecificOperationListRepository = projectSpecificOperationListRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ICommandResponse> Handle(RemoveProjectSpecificOperationsCommand request, CancellationToken cancellationToken)
        {
            var list = await _projectSpecificOperationListRepository.GetByProjectId(request.ProjectId.Value, cancellationToken);
            list.RemoveProjectSpecificOperations(request.ProjectSpecificOperationIds);

            await _projectSpecificOperationListRepository.Update(list, cancellationToken);
            await _projectSpecificOperationListRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            
            return EmptyCommandResponse.Default;
        }
    }

    internal class RemoveProjectSpecificOperationsCommandAuthorizer : IAuthorizer<RemoveProjectSpecificOperationsCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public RemoveProjectSpecificOperationsCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }
        
        public async Task<AuthorizationResult> Authorize(RemoveProjectSpecificOperationsCommand command, CancellationToken cancellation)
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
