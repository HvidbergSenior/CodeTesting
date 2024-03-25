using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectParticipant : Entity
    {
        public ProjectParticipantId ParticipantId { get; private set; }
        public ProjectParticipantName Name { get; private set; }
        public ProjectEmail Email { get; private set; }
        public ProjectParticipantAddress? Address { get; private set; }
        public ProjectParticipantPhoneNumber? PhoneNumber { get; private set; }

        private ProjectParticipant()
        {
            Id = Guid.Empty;
            ParticipantId = ProjectParticipantId.Empty();
            Name = ProjectParticipantName.Empty();
            Email = ProjectEmail.Empty();
        }

        private ProjectParticipant(ProjectParticipantId projectParticipantId, ProjectParticipantName name, ProjectEmail email, ProjectParticipantAddress? address, ProjectParticipantPhoneNumber? phoneNumber)
        {
            Id = projectParticipantId.Value;
            ParticipantId = projectParticipantId;
            Name = name;
            Email = email;
            Address = address;
            PhoneNumber = phoneNumber;
        }

        public static ProjectParticipant Create(ProjectParticipantId id, ProjectParticipantName name, ProjectEmail email, ProjectParticipantAddress? address, ProjectParticipantPhoneNumber? phoneNumber)
        {
            return new ProjectParticipant(id, name, email, address, phoneNumber);
        }

        public static ProjectParticipant Empty()
        {
            return new ProjectParticipant(ProjectParticipantId.Empty(), ProjectParticipantName.Empty(), ProjectEmail.Empty(), ProjectParticipantAddress.Empty(), ProjectParticipantPhoneNumber.Empty());
        }
    }
}
