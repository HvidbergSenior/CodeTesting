using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Domain.projectLogBook;

namespace deftq.Pieceworks.Application.GetProjectCompensation
{
    public sealed class GetProjectCompensationListQuery : IQuery<GetProjectCompensationListQueryResponse>
    {
        public ProjectId ProjectId { get; }

        private GetProjectCompensationListQuery(ProjectId projectId)
        {
            ProjectId = projectId;
        }

        public static GetProjectCompensationListQuery Create(Guid projectId)
        {
            return new GetProjectCompensationListQuery(ProjectId.Create(projectId));
        }
    }

    internal class GetProjectCompensationListQueryHandler : IQueryHandler<GetProjectCompensationListQuery, GetProjectCompensationListQueryResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectLogBookRepository _projectLogBookRepository;
        private readonly IProjectCompensationListRepository _projectCompensationListRepository;

        public GetProjectCompensationListQueryHandler(IProjectRepository projectRepository, IProjectLogBookRepository projectLogBookRepository,
            IProjectCompensationListRepository projectCompensationListRepository)
        {
            _projectRepository = projectRepository;
            _projectLogBookRepository = projectLogBookRepository;
            _projectCompensationListRepository = projectCompensationListRepository;
        }

        public async Task<GetProjectCompensationListQueryResponse> Handle(GetProjectCompensationListQuery query, CancellationToken cancellationToken)
        {
            var projectId = query.ProjectId.Value;
            var project = await _projectRepository.GetById(projectId, cancellationToken);
            var logBook = await _projectLogBookRepository.GetByProjectId(projectId, cancellationToken);
            var projectCompensationList = await _projectCompensationListRepository.GetByProjectId(projectId);

            return MapResponse(project, logBook, projectCompensationList);
        }

        private static GetProjectCompensationListQueryResponse MapResponse(Project project, ProjectLogBook logBook,
            ProjectCompensationList projectCompensationList)
        {
            var calculator = new CompensationPaymentCalculator();

            var result = projectCompensationList.Compensations.Select(compensation =>
            {
                var startDate = compensation.ProjectCompensationPeriod.StartDate.Value;
                var endDate = compensation.ProjectCompensationPeriod.EndDate.Value;
                var amount = compensation.ProjectCompensationPayment.Value;
                var compensationResult = calculator.CalculateUsersCompensationPayment(logBook.ProjectLogBookUsers, startDate, endDate, amount);

                var projectCompensationParticipants =
                    compensation.ProjectParticipantIds.Select(participantId =>
                    {
                        ProjectCompensationParticipant compensationParticipant;
                        var res = compensationResult.Participants.FirstOrDefault(p => p.ProjectParticipantId == participantId.Value);
                        var hours = res?.Hours.Value ?? 0;
                        var paymentDkr = res?.Payment.Value ?? 0;
                        if (project.IsParticipant(participantId.Value))
                        {
                            var participant = project.GetParticipant(participantId.Value);
                            compensationParticipant =
                                new ProjectCompensationParticipant(participantId.Value, participant.Name.Value, participant.Email.Value, hours, paymentDkr);
                        }
                        else if (project.IsOwner(participantId.Value))
                        {
                            var owner = project.ProjectOwner;
                            compensationParticipant =
                                new ProjectCompensationParticipant(participantId.Value, owner.Name, string.Empty, hours, paymentDkr);
                        }
                        else
                        {
                            throw new InvalidOperationException(
                                $"Project participant with id {participantId.Value} not found");
                        }

                        return compensationParticipant;
                    }).ToList();
                return new CompensationResponse(compensation.ProjectCompensationId.Id, startDate, endDate, amount, projectCompensationParticipants);
            }).ToList();

            return new GetProjectCompensationListQueryResponse(result);
        }
    }

    internal class GetProjectCompensationListQueryAuthorizer : IAuthorizer<GetProjectCompensationListQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetProjectCompensationListQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetProjectCompensationListQuery query, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellation);

            if (project.IsOwner(_executionContext.UserId) || project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
