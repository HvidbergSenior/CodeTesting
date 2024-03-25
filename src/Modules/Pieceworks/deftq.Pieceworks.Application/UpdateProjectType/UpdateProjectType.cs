using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Application.UpdateProjectType
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
    }
}
