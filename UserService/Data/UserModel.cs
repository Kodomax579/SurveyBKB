namespace UserService.Data
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public ClassModel Class { get; set; } = new ClassModel();
        public int GroupId { get; set; }
    }
}
