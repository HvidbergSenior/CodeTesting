
namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectType
{
    public class UpdateProjectTypeRequest
    {
        public UpdateProjectPieceworkType PieceworkType { get; }
        public decimal? PieceWorkSum { get; }
        public DateOnly? StartDate { get; }
        public DateOnly? EndDate { get; }
        
        public enum UpdateProjectPieceworkType {
            TwelveOneA = 0 ,
            TwelveOneB = 1,
            TwelveOneC = 2,
            TwelveTwo = 3
        }
        
        public UpdateProjectTypeRequest(UpdateProjectPieceworkType pieceworkType, decimal? pieceWorkSum, DateOnly? startDate, DateOnly? endDate)
        {
            PieceworkType = pieceworkType;
            PieceWorkSum = pieceWorkSum;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
