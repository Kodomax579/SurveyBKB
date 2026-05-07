using Microsoft.EntityFrameworkCore;
using SurveyService.Data;
using SurveyService.Database;

namespace SurveyService.Feature
{
    public class SurveyService (SurveyContext surveyDbContext)
    {
        public async Task<List<SurveyModel>> GetAllSurveysAsync()
        {
            return await surveyDbContext.Survey
                .Include(s => s.Questions)
                    .ThenInclude(q => q.Options)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> CreateSurveyAsync(SurveyModel entity)
        {
            await surveyDbContext.Survey.AddAsync(entity);
            var changes = await surveyDbContext.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> DeleteSurveyAsync(int id)
        {
            var survey = await surveyDbContext.Survey.FindAsync(id);
            if (survey == null)
            {
                return false;
            }

            surveyDbContext.Survey.Remove(survey);
            var changes = await surveyDbContext.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> UpdateAnswerSelectionAsync(int userId, int answerId)
        {
            var survey = await surveyDbContext.Survey
                .Include(s => s.Questions)
                    .ThenInclude(q => q.Options)
                    .FirstOrDefaultAsync(q => q.Questions.Any(p => p.Options.Any(p => p.Id == answerId)));

            if (survey == null)
            {
                return false;
            }
            if (survey.UserIDs.Contains(userId))
            {
                return false;
            }

            // 4. Antwort suchen und Counter erhöhen
            var answer = survey.Questions
                .SelectMany(q => q.Options)
                .FirstOrDefault(o => o.Id == answerId);
            
            if (answer == null)
            {
                return false;
            }
            answer.NumberOfSelectedAnswer++;
            survey.UserIDs.Add(userId);

            var changes = await surveyDbContext.SaveChangesAsync();
            return changes > 0;
        }
    }
}
