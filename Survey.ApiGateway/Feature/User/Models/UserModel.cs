namespace Survey.ApiGateway.Feature.User.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public ClassModel? Class { get; set; }
        public Groups Group{ get; set; }
    }
}
