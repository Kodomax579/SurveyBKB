using Contracts.Protos;

namespace Survey.ApiGateway.Models.DTO
{
    public class UserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ClassModel Class { get; set; } = new ClassModel();
        public UserGroup Group { get; set; }
    }
}
