using deftq.Pieceworks.Application.CreateProject;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateProject
{
    public class CreateProjectRequest
    {
        public string Title { get; }
        public string? Description { get; }
        public PieceworkType PieceworkType { get; }
        public decimal? PieceworkSum { get; }
        
        public CreateProjectRequest(string title, string? description, PieceworkType pieceworkType, decimal? pieceworkSum)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException($"'{nameof(title)}' cannot be null or empty.", nameof(title));
            }
            Title = title;
            Description = description;
            PieceworkType = pieceworkType;
            PieceworkSum = pieceworkSum;
        }
    }
}
