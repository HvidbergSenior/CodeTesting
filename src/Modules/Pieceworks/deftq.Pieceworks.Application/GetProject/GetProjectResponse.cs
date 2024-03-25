using deftq.Pieceworks.Application.CreateProject;

namespace deftq.Pieceworks.Application.GetProject
{
    public class GetProjectResponse
    {
        public Guid Id { get; }
        public string Title { get; }
        public long ProjectNumber { get; }
        public string PieceWorkNumber { get; }
        public string OrderNumber { get; }
        public string Description { get; }

        public PieceworkType PieceworkType { get; }
        public decimal? LumpSumPaymentDkr { get; }

        public DateOnly? StartDate { get; }
        public DateOnly? EndDate { get; }

        public DateTimeOffset ProjectCreatedTime { get; }

        public string CompanyName { get; }
        public string CompanyAddress { get; }
        public string CompanyCvrNo { get; }
        public string CompanyPNo { get; }

        public IList<ProjectParticipant> Participants { get; private set; }
        public ProjectRole CurrentUserRole { get; private set; }

        public GetProjectResponse(Guid id, string title, long projectNumber, string pieceWorkNumber, string orderNumber, string description, PieceworkType pieceworkType,
            decimal? lumpSumPaymentDkr, IList<ProjectParticipant> participants, ProjectRole currentUserRole, DateOnly? startDate, DateOnly? endDate,
            DateTimeOffset projectCreatedTime, string companyName, string companyAddress, string companyCvrNo, string companyPNo)
        {
            Id = id;
            Title = title;
            ProjectNumber = projectNumber;
            Description = description;
            PieceworkType = pieceworkType;
            LumpSumPaymentDkr = lumpSumPaymentDkr;
            Participants = participants;
            CurrentUserRole = currentUserRole;
            StartDate = startDate;
            EndDate = endDate;
            PieceWorkNumber = pieceWorkNumber;
            OrderNumber = orderNumber;
            ProjectCreatedTime = projectCreatedTime;
            CompanyName = companyName;
            CompanyAddress = companyAddress;
            CompanyCvrNo = companyCvrNo;
            CompanyPNo = companyPNo;
        }
    }

    public class ProjectParticipant
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }

        public ProjectParticipant(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }

    public enum ProjectRole
    {
        ProjectManager,
        ProjectOwner,
        ProjectParticipant,
        Undefined
    }
}
