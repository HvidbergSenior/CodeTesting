using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;

namespace deftq.Pieceworks.Application.GetCompensationPaymentParticipantsInPeriod
{
    public sealed class GetCompensationPaymentParticipantsInPeriodQuery : IQuery<GetCompensationPaymentResponse>
    {
        public ProjectId ProjectId { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public decimal Amount { get; set; }

        private GetCompensationPaymentParticipantsInPeriodQuery(ProjectId projectId, DateOnly startDate, DateOnly endDate, decimal amount)
        {
            ProjectId = projectId;
            StartDate = startDate;
            EndDate = endDate;
            Amount = amount;
        }

        public static GetCompensationPaymentParticipantsInPeriodQuery Create(ProjectId projectId, DateOnly periodStart, DateOnly periodEnd, decimal amount)
        {
            return new GetCompensationPaymentParticipantsInPeriodQuery(projectId, periodStart, periodEnd, amount);
        }
    }

    internal class
        GetCompensationPaymentParticipantsInPeriodQueryHandler : IQueryHandler<GetCompensationPaymentParticipantsInPeriodQuery,
            GetCompensationPaymentResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;
        private readonly IProjectLogBookRepository _projectLogBookRepository;
        
        public GetCompensationPaymentParticipantsInPeriodQueryHandler(IProjectRepository projectRepository, IExecutionContext executionContext,
            IProjectLogBookRepository projectLogBookRepository)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
            _projectLogBookRepository = projectLogBookRepository;
        }

        public async Task<GetCompensationPaymentResponse> Handle(GetCompensationPaymentParticipantsInPeriodQuery query,
            CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellationToken);
            var logbook = await _projectLogBookRepository.GetByProjectId(query.ProjectId.Value, cancellationToken);

            var calculator = new CompensationPaymentCalculator();
            var calculatedResult = calculator.CalculateUsersCompensationPayment(logbook.ProjectLogBookUsers, query.StartDate, query.EndDate, query.Amount);

            var response = MapUsersAndResult(project, calculatedResult);
            
            return new GetCompensationPaymentResponse(query.StartDate, query.EndDate,response);
        }

        private IEnumerable<GetCompensationPaymentParticipantResponse> MapUsersAndResult(Project project, CompensationPaymentResult calculatedResult)
        {
            var list = new List<GetCompensationPaymentParticipantResponse>();
            
            list.Add(MapParticipantAndResult(project.ProjectOwner.Id, project.ProjectOwner.Name, "", calculatedResult));
            
            foreach (var participant in project.ProjectParticipants)
            {
                list.Add(MapParticipantAndResult(participant.Id, participant.Name.Value, participant.Email.Value, calculatedResult));
            }

            return list;
        }

        private GetCompensationPaymentParticipantResponse MapParticipantAndResult(Guid userId, string name, string email,
            CompensationPaymentResult calculatedResult)
        {
            var result = calculatedResult.Participants.FirstOrDefault(u => u.ProjectParticipantId == userId);
            var hours = result is not null ? result.Hours.Value : 0;
            var payment = result is not null ? result.Payment.Value : 0;
            return new GetCompensationPaymentParticipantResponse(userId, name, email, hours, payment);
        }
    }
    
    public class GetCompensationPaymentParticipantsInPeriodQueryAuthorizer : IAuthorizer<GetCompensationPaymentParticipantsInPeriodQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetCompensationPaymentParticipantsInPeriodQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetCompensationPaymentParticipantsInPeriodQuery query,
            CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellationToken);
            if (project.IsOwner(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
