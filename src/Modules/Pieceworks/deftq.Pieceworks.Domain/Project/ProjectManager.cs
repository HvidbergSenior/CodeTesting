using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectManager : Entity
    {
        public string Name { get; private set; }
        public ProjectEmail Email { get; private set; }
        public string? Address { get; private set; }
        public string? Phone { get; private set; }

        private ProjectManager()
        {
            Id = Guid.Empty;
            Name = String.Empty;
            Email = ProjectEmail.Empty();
        }

        private ProjectManager(Guid id, string name, ProjectEmail email, string? address, string? phone)
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
            Phone = phone;
        }

        public static ProjectManager Create(Guid id, string name, ProjectEmail email, string? address, string? phone)
        {
            return new ProjectManager(id, name, email, address, phone);
        }

        public static ProjectManager Empty()
        {
            return new ProjectManager(Guid.Empty, String.Empty, ProjectEmail.Empty(), null, null);
        }
    }
}
