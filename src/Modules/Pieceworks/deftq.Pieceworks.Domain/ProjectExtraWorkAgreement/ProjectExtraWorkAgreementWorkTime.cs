using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public sealed class ProjectExtraWorkAgreementWorkTime : ValueObject
    {
        public ProjectExtraWorkAgreementHours Hours { get; private set; }
        public ProjectExtraWorkAgreementMinutes Minutes { get; private set; }

        private ProjectExtraWorkAgreementWorkTime()
        {
            Hours = ProjectExtraWorkAgreementHours.Empty();
            Minutes = ProjectExtraWorkAgreementMinutes.Empty();
        }

        private ProjectExtraWorkAgreementWorkTime(ProjectExtraWorkAgreementHours hours, ProjectExtraWorkAgreementMinutes minutes)
        {
            Hours = hours;
            Minutes = minutes;
        }

        public static ProjectExtraWorkAgreementWorkTime Create(ProjectExtraWorkAgreementHours hours, ProjectExtraWorkAgreementMinutes minutes)
        {
            return new ProjectExtraWorkAgreementWorkTime(hours, minutes);
        }

        public static ProjectExtraWorkAgreementWorkTime Empty()
        {
            return new ProjectExtraWorkAgreementWorkTime(ProjectExtraWorkAgreementHours.Empty(), ProjectExtraWorkAgreementMinutes.Empty());
        }
    }
}
