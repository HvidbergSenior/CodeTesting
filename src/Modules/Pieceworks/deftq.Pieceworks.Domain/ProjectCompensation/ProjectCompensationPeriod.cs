using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectCompensation
{
    public sealed class ProjectCompensationPeriod : ValueObject
    {
        public ProjectCompensationDate StartDate { get; private set; }
        public ProjectCompensationDate EndDate { get; private set; }

        private ProjectCompensationPeriod()
        {
            StartDate = ProjectCompensationDate.Empty();
            EndDate = ProjectCompensationDate.Empty();
        }

        private ProjectCompensationPeriod(ProjectCompensationDate startDate, ProjectCompensationDate endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
        
        public static ProjectCompensationPeriod Create(ProjectCompensationDate startDate, ProjectCompensationDate endDate)
        {
            if (startDate.Value < endDate.Value)
            {
                return new ProjectCompensationPeriod(startDate, endDate);
            }
            
            throw new ArgumentException("Start date can not be later than end date", nameof(startDate));
        }

        public static ProjectCompensationPeriod Empty()
        {
            return new ProjectCompensationPeriod(ProjectCompensationDate.Empty(), ProjectCompensationDate.Empty());
        }
        
        public bool IsDateIncluded(DateOnly dateOnly)
        {
            return StartDate.Value <= dateOnly && EndDate.Value >= dateOnly;
        }
    }
}
