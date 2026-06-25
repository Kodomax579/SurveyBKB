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
                // Ensure the user exists in the database and attach the tracked entity
                var userExists = await surveyDbContext.Users
                    .Include(u => u.Class)
                    .FirstOrDefaultAsync(u => u.Id == survey.User.Id);

                if (userExists == null)
                {
                    // If the provided user does not exist, fail gracefully
                    return null;
                }

                survey.User = userExists;

                // Ensure CreatedAt is set
                if (survey.CreatedAt == default)
                {
                    survey.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                }

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
        public async Task<bool> EndSurveyEarly(int id)
        {
            // Hol dir die Umfrage aus der Datenbank (passe ggf. den Entitätsnamen an)
            var survey = await surveyDbContext.Surveys.FindAsync(id);

            if (survey == null)
            {
                return false;
            }

            // Setze das Enddatum auf gestern, damit sie abgelaufen ist
            survey.OnlineUntil = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));

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

        public async Task<bool> IncrementAnswerCounts(List<int> answerIds, int userId)
        {
            if (answerIds == null || !answerIds.Any())
            {
                return false;
            }

            // 1. Alle benötigten Daten laden
            // Wir holen alle Antworten, die in der Liste sind
            var answers = await surveyDbContext.Answers
                .Where(a => answerIds.Contains(a.Id))
                .ToListAsync();

            if (answers.Count != answerIds.Distinct().Count())
            {
                // Falls eine oder mehrere IDs nicht existieren
                return false;
            }

            var firstAnswer = answers.First();
            var question = await surveyDbContext.Questions.FirstOrDefaultAsync(q => q.Id == firstAnswer.QuestionModelId);
            var survey = await surveyDbContext.Surveys.FirstOrDefaultAsync(s => s.Id == question.SurveyModelId);

            if (survey == null)
            {
                return false;
            }

            // 2. User-Check: Hat er bereits für DIESE Umfrage abgestimmt?
            survey.UserIDs ??= new List<int>();
            if (userId > 0 && survey.UserIDs.Contains(userId))
            {
                return false;
            }

            foreach (var answer in answers)
            {
                answer.NumberOfSelectedAnswer++;
            }

            if (userId > 0)
            {
                survey.UserIDs.Add(userId);
            }

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
                    Firstname = survey.User.Firstname,
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