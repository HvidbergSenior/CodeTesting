using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;

namespace deftq.Pieceworks.Application.GetLogBookAsSpreadSheet
{
    public sealed class GetLogBookAsSpreadSheetQuery : IQuery<GetLogBookAsSpreadSheetQueryResponse>
    {
        public ProjectId ProjectId { get; private set; }

        private GetLogBookAsSpreadSheetQuery()
        {
            ProjectId = ProjectId.Empty();
        }

        public GetLogBookAsSpreadSheetQuery(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static GetLogBookAsSpreadSheetQuery Create(Guid projectId)
        {
            return new GetLogBookAsSpreadSheetQuery(ProjectId.Create(projectId));
        }
    }

    internal class GetLogBookAsSpreadSheetQueryHandler : IQueryHandler<GetLogBookAsSpreadSheetQuery, GetLogBookAsSpreadSheetQueryResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectLogBookRepository _projectLogBookRepository;

        public GetLogBookAsSpreadSheetQueryHandler(IProjectRepository projectRepository, IProjectLogBookRepository projectLogBookRepository)
        {
            _projectRepository = projectRepository;
            _projectLogBookRepository = projectLogBookRepository;
        }

        public async Task<GetLogBookAsSpreadSheetQueryResponse> Handle(GetLogBookAsSpreadSheetQuery query, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellationToken);
            var logBook = await _projectLogBookRepository.GetByProjectId(project.ProjectId.Value, cancellationToken);

            using var openXmlExporter = new OpenXmlExporter();
            openXmlExporter.ExportLogBook(project, logBook);
            var export = openXmlExporter.GetExport();

            return new GetLogBookAsSpreadSheetQueryResponse(export.Data, export.FileName);
        }
        
        internal class GetLogBookAsSpreadSheetQueryAuthorizer : IAuthorizer<GetLogBookAsSpreadSheetQuery>
        {
            private readonly IProjectRepository _projectRepository;
            private readonly IExecutionContext _executionContext;

            public GetLogBookAsSpreadSheetQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
            {
                _projectRepository = projectRepository;
                _executionContext = executionContext;
            }

            public async Task<AuthorizationResult> Authorize(GetLogBookAsSpreadSheetQuery query, CancellationToken cancellation)
            {
                var project = await _projectRepository.GetById(query.ProjectId.Value, cancellation);
                if (project.IsOwner(_executionContext.UserId) ||
                    project.IsParticipant(_executionContext.UserId) ||
                    project.IsProjectManager(_executionContext.UserId))
                {
                    return AuthorizationResult.Succeed();
                }

                return AuthorizationResult.Fail();
            }
        }
    }
}
