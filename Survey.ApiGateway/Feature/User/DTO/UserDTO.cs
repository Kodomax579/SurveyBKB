using Survey.ApiGateway.Feature.User.DTO;
using Survey.ApiGateway.Feature.User.Models;

namespace Survey.ApiGateway.Models.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ClassDTO Class { get; set; } = new();
        public Groups Group { get; set; }
    }
}
