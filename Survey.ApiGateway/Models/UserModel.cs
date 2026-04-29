using Survey.ApiGateway.Models.Enum;

namespace Survey.ApiGateway.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Klasse { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public Groups Group{ get; set; }
    }
}
