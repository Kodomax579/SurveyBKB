using Microsoft.EntityFrameworkCore;
using NewsService.Data;

namespace NewsService.Database
{
    public class NewsContext : DbContext
    {
        public NewsContext(DbContextOptions<NewsContext> options) : base(options)
        { }

        public DbSet<NewsModel> News { get; set; }
    }
}
