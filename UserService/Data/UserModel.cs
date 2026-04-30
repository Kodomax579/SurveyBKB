namespace UserService.Data
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Hier kannst du DB-spezifische Dinge tun
        public ClassModel Class { get; set; } = new ClassModel();
        public string Username { get; set; } = string.Empty;
        public int GroupId { get; set; }
    }
}
