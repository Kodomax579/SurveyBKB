using Microsoft.EntityFrameworkCore;
using Survey.ApiGateway.Database;
using Survey.ApiGateway.Feature.Survey.DTO;
using Survey.ApiGateway.Feature.Survey.Models;
using Survey.ApiGateway.Models.DTO;

namespace Survey.ApiGateway.Feature.Survey
{
    public class SurveyService(SurveyDbContext surveyDbContext)
    {
        public async Task<SurveyDTO?> CreateSurvey(SurveyModel survey)
        {
            try
            {
                await surveyDbContext.Surveys.AddAsync(survey);
                await surveyDbContext.SaveChangesAsync();

                return await GetSurveyById(survey.Id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<SurveyDTO>> GetAllSurveys()
        {
            var surveys = await surveyDbContext.Surveys
                .Where(s => s.OnlineUntil >= DateOnly.FromDateTime(DateTime.UtcNow))
                .Include(s => s.User)
                    .ThenInclude(u => u.Class) 
                .Include(s => s.Questions)
                    .ThenInclude(q => q.Options)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return surveys.Select(ConvertToDto).ToList();
        }

        public async Task<SurveyDTO?> GetSurveyById(int id)
        {
            var survey = await surveyDbContext.Surveys
                .Include(s => s.User)
                    .ThenInclude(u => u.Class) 
                .Include(s => s.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (survey == null)
            {
                return null;
            }

            return ConvertToDto(survey);
        }

        public async Task<SurveyDTO?> UpdateSurvey(int id, SurveyModel surveyUpdate)
        {
            var existingSurvey = await surveyDbContext.Surveys
                .Include(s => s.User)
                    .ThenInclude(u => u.Class)
                .Include(s => s.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(s => s.Id == id);

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
                return ConvertToDto(existingSurvey);
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

        private SurveyDTO ConvertToDto(SurveyModel survey)
        {
            return new SurveyDTO()
            {
                Id = survey.Id,
                Title = survey.Title,
                GroupId = survey.GroupId,
                User = new UserDTO()
                {
                    Id = survey.User.Id,
                    Name = survey.User.Name,
                    Email = survey.User.Email,
                    Group = survey.User.Group,
                    Lastname = survey.User.Lastname,
                    Class = new User.DTO.ClassDTO()
                    {
                        Classname = survey.User.Class.ClassName
                    }
                },
                CreatedAt = survey.CreatedAt,
                OnlineUntil = survey.OnlineUntil,
                Classes = survey.Classes,
                UserIDs = survey.UserIDs,
                Questions = survey.Questions.Select(q => new QuestionDTO()
                {
                    Id = q.Id,
                    Question = q.Question,
                    SurveyModelId = q.SurveyModelId,
                    Options = q.Options.Select(o => new AnswerDTO()
                    {
                        Id = o.Id,
                        Answers = o.Answers,
                        NumberOfSelectedAnswer = o.NumberOfSelectedAnswer
                    }).ToList()
                }).ToList()
            };
        }
    }
}