using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectParticipant
{
    [SwaggerSchema(Required = new [] { "Name", "Role", "Email"})]
    public class RegisterProjectUserRequest
    {
        public string Name { get; private set; }
        public UserRole Role { get; private set; }
        public string Email { get; private set; }
        public string? Address { get; private set; }
        public string? Phone { get; private set; }

        public RegisterProjectUserRequest(string name, UserRole role, string email, string? address, string? phone)
        {
            Name = name;
            Role = role;
            Email = email;
            Address = address;
            Phone = phone;
        }
    }
    
    public enum UserRole
    {
        ProjectManager,
        ProjectParticipant,
    }
}
