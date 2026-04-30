using Microsoft.EntityFrameworkCore;
using UserService.Data;

namespace UserService.Database
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<ClassModel> Classes { get; set; }

        }
}
