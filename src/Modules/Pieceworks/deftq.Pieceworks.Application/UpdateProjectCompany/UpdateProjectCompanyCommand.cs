using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.DataAccess;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.project.Company;

namespace deftq.Pieceworks.Application.UpdateProjectCompany
{
    public sealed class UpdateProjectCompanyCommand : ICommand<ICommandResponse>
    {
        internal ProjectId ProjectId { get; }
        public ProjectCompany ProjectCompany { get; private set; }

        public UpdateProjectCompanyCommand()
        {
            ProjectId = ProjectId.Empty();
            ProjectCompany = ProjectCompany.Empty();
        }

        private UpdateProjectCompanyCommand(ProjectId projectId, ProjectCompany projectCompany)
        {
            ProjectId = projectId;
            ProjectCompany = projectCompany;
        }

        public static UpdateProjectCompanyCommand Create(Guid projectId, string companyName, string companyAddress, string CvrNo, string pNo)
        {
            var name = ProjectCompanyName.Create(companyName);
            var address = ProjectAddress.Create(companyAddress);
            var cvr = ProjectCompanyCvrNo.Create(CvrNo);
            var p = ProjectCompanyPNo.Create(pNo);
            var company = ProjectCompany.Create(name, address, cvr, p);
            return new UpdateProjectCompanyCommand(ProjectId.Create(projectId), company);
        }
    }

    internal class UpdateProjectCompanyCommandHandler : ICommandHandler<UpdateProjectCompanyCommand, ICommandResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProjectCompanyCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(UpdateProjectCompanyCommand command, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellationToken);
            
            project.UpdateProjectCompany(command.ProjectCompany);
            
            await _projectRepository.Update(project, cancellationToken);
            await _projectRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class UpdateProjectCompanyCommandAuthorizer : IAuthorizer<UpdateProjectCompanyCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public UpdateProjectCompanyCommandAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(UpdateProjectCompanyCommand command, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(command.ProjectId.Value, cancellationToken);
            if (project.IsOwner(_executionContext.UserId) || project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
