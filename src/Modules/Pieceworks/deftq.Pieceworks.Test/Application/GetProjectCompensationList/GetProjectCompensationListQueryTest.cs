using deftq.Pieceworks.Application.GetProjectCompensation;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectCompensation;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectCompensationList
{
    public class GetProjectCompensationListQueryTest
    {
        private readonly ProjectCompensationListInMemoryRepository _projectCompensationListInMemoryRepository;
        private readonly ProjectInMemoryRepository _projectInMemoryRepository;
        private readonly ProjectLogBookInMemoryRepository _projectLogBookRepository;

        public GetProjectCompensationListQueryTest()
        {
            _projectCompensationListInMemoryRepository = new ProjectCompensationListInMemoryRepository();
            _projectInMemoryRepository = new ProjectInMemoryRepository();
            _projectLogBookRepository = new ProjectLogBookInMemoryRepository();
        }

        [Fact]
        public async Task GivenSingleCompensation_WhenGettingCompensationList_ShouldReturnSingleCompensation()
        {
            // Project
            var project = Any.Project();
            await _projectInMemoryRepository.Add(project);

            // Log book
            var logbook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            await _projectLogBookRepository.Add(logbook);
            
            // Register single project compensation
            var compensations = ProjectCompensationList.Create(project.ProjectId, Any.ProjectCompensationListId());
            var compensation = ProjectCompensation.Create(Any.ProjectCompensationId(), Any.ProjectCompensationPayment(),
                Any.ProjectCompensationPeriod(), new List<ProjectParticipantId> { ProjectParticipantId.Create(project.ProjectOwner.Id) });
            compensations.AddCompensation(compensation);
            await _projectCompensationListInMemoryRepository.Add(compensations);

            // Get compensation list
            var handler = new GetProjectCompensationListQueryHandler(_projectInMemoryRepository, _projectLogBookRepository,
                _projectCompensationListInMemoryRepository);
            var compensationQuery = GetProjectCompensationListQuery.Create(project.ProjectId.Value);
            var compensationResponse = await handler.Handle(compensationQuery, CancellationToken.None);

            // Assert single compensation in list
            Assert.Single(compensationResponse.Compensations);
            Assert.Equal(compensation.ProjectCompensationId.Id, compensationResponse.Compensations[0].ProjectCompensationId);
        }

        [Fact]
        public async Task GivenCompensationWithClosedHours_WhenGettingCompensation_AmountAndHoursAreReturned()
        {
            // Project
            var project = Any.Project();
            await _projectInMemoryRepository.Add(project);

            // Register hours in log book
            var startDate = new DateOnly(2023, 7, 3);
            var endDate = new DateOnly(2023, 7, 4);
            var logbook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            var days = new List<ProjectLogBookDay>
            {
                ProjectLogBookDay.Create(LogBookDate.Create(startDate), LogBookTime.Create(LogBookHours.Create(7), LogBookMinutes.Create(30))),
                ProjectLogBookDay.Create(LogBookDate.Create(startDate.AddDays(1)),
                    LogBookTime.Create(LogBookHours.Create(6), LogBookMinutes.Create(15))),
                ProjectLogBookDay.Create(LogBookDate.Create(startDate.AddDays(2)),
                    LogBookTime.Create(LogBookHours.Create(1), LogBookMinutes.Create(45)))
            };
            var logBookUser = ProjectLogBookUser.Create(project.ProjectOwner.Id);
            logbook.RegisterWeek(logBookUser, LogBookYear.Create(2023), LogBookWeek.Create(27), LogBookNote.Empty(), days);
            logbook.CloseWeek(logBookUser, LogBookYear.Create(2023), LogBookWeek.Create(27));
            await _projectLogBookRepository.Add(logbook);

            // Register project compensation
            var compensations = ProjectCompensationList.Create(project.ProjectId, Any.ProjectCompensationListId());
            var period = ProjectCompensationPeriod.Create(ProjectCompensationDate.Create(startDate), ProjectCompensationDate.Create(endDate));
            var compensation = ProjectCompensation.Create(Any.ProjectCompensationId(), ProjectCompensationPayment.Create(20),
                period, new List<ProjectParticipantId> { ProjectParticipantId.Create(project.ProjectOwner.Id) });
            compensations.AddCompensation(compensation);
            await _projectCompensationListInMemoryRepository.Add(compensations);

            // Get compensation list
            var handler = new GetProjectCompensationListQueryHandler(_projectInMemoryRepository, _projectLogBookRepository,
                _projectCompensationListInMemoryRepository);
            var compensationQuery = GetProjectCompensationListQuery.Create(project.ProjectId.Value);
            var compensationResponse = await handler.Handle(compensationQuery, CancellationToken.None);

            // Assert
            Assert.Single(compensationResponse.Compensations);
            Assert.Single(compensationResponse.Compensations[0].CompensationParticipant);
            Assert.Equal(13.75m, compensationResponse.Compensations[0].CompensationParticipant[0].ClosedHoursInPeriod);
            Assert.Equal(275.0m, compensationResponse.Compensations[0].CompensationParticipant[0].CompensationAmountDkr);
        }
    }
}
