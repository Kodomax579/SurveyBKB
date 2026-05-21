using Microsoft.EntityFrameworkCore;
using Survey.ApiGateway.Feature.News.Models;
using Survey.ApiGateway.Feature.Survey.Models;
using Survey.ApiGateway.Feature.User.Models;

namespace Survey.ApiGateway.Database
{
    public class SurveyDbContext (DbContextOptions<SurveyDbContext> options): DbContext(options)
    {
        public DbSet<NewsModel> News { get; set; }
        public DbSet<SurveyModel> Surveys { get; set; }
        public DbSet<QuestionModel> Questions { get; set; }
        public DbSet<AnswerModel> Answers { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ClassModel> Classes{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SurveyModel>()
                .HasMany(s => s.Questions)
                .WithOne()
                .HasForeignKey(q => q.SurveyModelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuestionModel>()
                .HasMany(q => q.Options)
                .WithOne() 
                .HasForeignKey(a => a.QuestionModelId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
