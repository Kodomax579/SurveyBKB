using Microsoft.EntityFrameworkCore;
using SurveyService.Data;

namespace SurveyService.Database
{
    public class SurveyContext : DbContext
    {
        public SurveyContext(DbContextOptions<SurveyContext> options) : base (options)
        {}
        public DbSet<AnswerModel> Answers { get; set; }
        public DbSet<QuestionModel> Question { get; set; }
        public DbSet<SurveyModel> Survey { get; set; }
    }
}
