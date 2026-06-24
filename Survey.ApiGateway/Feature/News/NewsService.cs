using Microsoft.EntityFrameworkCore;
using Survey.ApiGateway.Database;
using Survey.ApiGateway.Feature.News.DTO;
using Markdig;
using Survey.ApiGateway.Feature.News.Models;
using Survey.ApiGateway.Models.DTO; // Stelle sicher, dass die DTO-Namespaces passen
using System.Formats.Asn1;

namespace Survey.ApiGateway.Feature.News
{
    public class NewsService(SurveyDbContext surveyDbContext)
    {
        public async Task<NewsModelDTO?> CreateNews(NewsModel news)
        {
            try
            {
                var userExists = await surveyDbContext.Users
                    .Include(u => u.Class)
                    .FirstOrDefaultAsync(u => u.Id == news.User.Id);

                if (userExists == null) return null;

                news.User = userExists;

                await surveyDbContext.News.AddAsync(news);
                await surveyDbContext.SaveChangesAsync();

                return ConvertToDto(news);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<NewsModelDTO>> GetAllNews()
        {
            var newsList = await surveyDbContext.News
                .Include(n => n.User)
                    .ThenInclude(u => u.Class) 
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return newsList.Select(ConvertToDto).ToList();
        }

        public async Task<NewsModelDTO?> GetNewsById(int id)
        {
            var news = await surveyDbContext.News
                .Include(n => n.User)
                    .ThenInclude(u => u.Class)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (news == null) return null;

            return ConvertToDto(news);
        }

        public async Task<NewsModelDTO?> UpdateNews(int id, NewsModel newsUpdate)
        {
            var existingNews = await surveyDbContext.News
                .Include(n => n.User)
                    .ThenInclude(u => u.Class)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (existingNews == null)
            {
                return null;
            }

            // Deine Logik
            newsUpdate.NumberOfMembers++;

            existingNews.Titel = newsUpdate.Titel;
            existingNews.PreviewText = newsUpdate.PreviewText;
            existingNews.MainText = newsUpdate.MainText;
            existingNews.ExpiredDate = newsUpdate.ExpiredDate;
            existingNews.Tag = newsUpdate.Tag;
            existingNews.NumberOfMembers = newsUpdate.NumberOfMembers;
            existingNews.Prioritaet = newsUpdate.Prioritaet;
            try
            {
                await surveyDbContext.SaveChangesAsync();
                return ConvertToDto(existingNews);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteNews(int id)
        {
            var news = await surveyDbContext.News.FindAsync(id);

            if (news == null)
            {
                return false;
            }
            surveyDbContext.Remove(news);
            await surveyDbContext.SaveChangesAsync();
            return true;
        }

        private NewsModelDTO ConvertToDto(NewsModel news)
        {
            return new NewsModelDTO()
            {
                Id = news.Id,
                Titel = news.Titel,
                Tag = news.Tag,
                PreviewText = news.PreviewText,
                MainText = news.MainText,
                MainTextHtml = Markdown.ToHtml(news.MainText ?? string.Empty),
                ImageLink = news.ImageLink,
                CreatedAt = news.CreatedAt,
                ExpiredDate = news.ExpiredDate,
                NumberOfMembers = news.NumberOfMembers,
                Prioritaet = news.Prioritaet,

                User = new UserDTO()
                {
                    Id = news.User.Id,
                    Firstname = news.User.Firstname,
                    Email = news.User.Email,
                    Group = news.User.Group,
                    Lastname = news.User.Lastname,
                    ImageLink = news.User.ImageLink,
                    Class = new User.DTO.ClassDTO()
                    {
                        Classname = news.User.Class.ClassName
                    }
                }
            };
        }
    }
}