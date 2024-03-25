namespace deftq.Pieceworks.Application.GetProjectUsers
{
    public enum ProjectUserRole {Owner, Participant, Manager }
    public class GetProjectUsersQueryResponse
    {
        public GetProjectUsersQueryResponse(IList<ProjectUserResponse> users)
        {
            Users = users;
        }

        public IList<ProjectUserResponse> Users { get; private set; }
    }

    public class ProjectUserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public ProjectUserRole Role { get; private set; }
        public string Email { get; private set; }
        public string? Phone { get; private set; }
        public string? Address { get; private set; }

        public ProjectUserResponse(Guid id, string name, ProjectUserRole role, string email, string? phone, string? address)
        {
            Id = id;
            Name = name;
            Role = role;
            Email = email;
            Phone = phone;
            Address = address;
        }
    }
}
