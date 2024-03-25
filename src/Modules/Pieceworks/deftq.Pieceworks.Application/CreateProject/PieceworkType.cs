using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Application.CreateProject
{
    public enum PieceworkType
    {
        TwelveOneA = 0 ,
        TwelveOneB = 1,
        TwelveOneC = 2,
        TwelveTwo = 3
    }

    public static class MapToDomain
    {
        public static ProjectPieceworkType ToDomain(this PieceworkType pieceworkType)
        {
            return pieceworkType switch
            {
                PieceworkType.TwelveTwo => ProjectPieceworkType.TwelveTwo,
                PieceworkType.TwelveOneA => ProjectPieceworkType.TwelveOneA,
                PieceworkType.TwelveOneB => ProjectPieceworkType.TwelveOneB,
                PieceworkType.TwelveOneC => ProjectPieceworkType.TwelveOneC,
                _ => throw new ArgumentOutOfRangeException(pieceworkType.GetType().Name),
            };
        }
        
        public static PieceworkType FromDomain(this ProjectPieceworkType projectPieceworkType)
        {
            return projectPieceworkType switch
            {
                ProjectPieceworkType.TwelveTwo => PieceworkType.TwelveTwo,
                ProjectPieceworkType.TwelveOneA => PieceworkType.TwelveOneA,
                ProjectPieceworkType.TwelveOneB => PieceworkType.TwelveOneB,
                ProjectPieceworkType.TwelveOneC => PieceworkType.TwelveOneC,
                _ => throw new ArgumentOutOfRangeException(projectPieceworkType.GetType().Name),
            };
        }

        public static string ToHumanReadable(this ProjectPieceworkType projectPieceworkType)
        {
            return projectPieceworkType switch
            {
                ProjectPieceworkType.TwelveTwo => "12.2",
                ProjectPieceworkType.TwelveOneA => "12.1A",
                ProjectPieceworkType.TwelveOneB => "12.1B",
                ProjectPieceworkType.TwelveOneC => "12.1C",
                _ => throw new ArgumentOutOfRangeException(projectPieceworkType.GetType().Name),
            };
        }

    }
}
