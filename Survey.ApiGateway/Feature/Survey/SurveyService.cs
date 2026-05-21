using Microsoft.EntityFrameworkCore;
using Survey.ApiGateway.Database;
using Survey.ApiGateway.Feature.Survey.Models;

namespace Survey.ApiGateway.Feature.Survey
{
    public class SurveyService(SurveyDbContext surveyDbContext)
    {
        public async Task<SurveyModel?> CreateSurvey(SurveyModel survey)
        {
            try
            {
                await surveyDbContext.Surveys.AddAsync(survey);
                await surveyDbContext.SaveChangesAsync();
                return survey;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<SurveyModel>> GetAllSurveys()
        {
            return await surveyDbContext.Surveys
                .Where(s => s.OnlineUntil >= DateOnly.FromDateTime(DateTime.UtcNow))
                .Include(s => s.User)
                .Include(s => s.Questions)
                    .ThenInclude(q => q.Options)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<SurveyModel?> GetSurveyById(int id)
        {
            return await surveyDbContext.Surveys
                .Include(s => s.User)
                .Include(s => s.Questions)            
                    .ThenInclude(q => q.Options)      
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<SurveyModel?> UpdateSurvey(int id, SurveyModel surveyUpdate)
        {
            var existingSurvey = await surveyDbContext.Surveys.FindAsync(id);
            if (existingSurvey == null)
            {
                return null;
            }

            existingSurvey.Title = surveyUpdate.Title;
            existingSurvey.OnlineUntil = surveyUpdate.OnlineUntil;
            existingSurvey.Classes = surveyUpdate.Classes;

            try
            {
                await surveyDbContext.SaveChangesAsync();
                return existingSurvey;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task DeleteSurvey(int id)
        {
            var survey = await surveyDbContext.Surveys.FindAsync(id);
            if (survey != null)
            {
                surveyDbContext.Surveys.Remove(survey);
                await surveyDbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> IncrementAnswerCount(int answerId)
        {
            var answer = await surveyDbContext.Answers.FindAsync(answerId);

            if (answer == null)
            {
                return false;
            }

            answer.NumberOfSelectedAnswer++;

            try
            {
                await surveyDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
