using Microsoft.EntityFrameworkCore;
using Survey.ApiGateway.Database;
using Survey.ApiGateway.Feature.News.Models;
using System.Formats.Asn1;

namespace Survey.ApiGateway.Feature.News
{
    public class NewsService(SurveyDbContext surveyDbContext)
    {
        public async Task<NewsModel?> CreateNews(NewsModel news)
        {
            try
            {
                var userExists = await surveyDbContext.Users.FirstOrDefaultAsync(u => u.Id == news.User.Id);
                news.User = userExists;

                await surveyDbContext.News.AddAsync(news);
                await surveyDbContext.SaveChangesAsync();
                return news;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<NewsModel>> GetAllNews()
        {
            return await surveyDbContext.News
                .Include(n => n.User)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<NewsModel?> GetNewsById(int id)
        {
            return await surveyDbContext.News
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<NewsModel?> UpdateNews(int id, NewsModel newsUpdate)
        {
            var existingNews = await surveyDbContext.News.FindAsync(id);

            if (existingNews == null)
            {
                return null;
            }

            newsUpdate.NumberOfMembers++;

            existingNews.Titel = newsUpdate.Titel;
            existingNews.PreviewText = newsUpdate.PreviewText;
            existingNews.MainText = newsUpdate.MainText;
            existingNews.ExpiredDate = newsUpdate.ExpiredDate;
            existingNews.Tag = newsUpdate.Tag;
            existingNews.NumberOfMembers = newsUpdate.NumberOfMembers;

            try
            {
                await surveyDbContext.SaveChangesAsync();
                return existingNews;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteNews(int id)
        {
            var news = await surveyDbContext.News.FindAsync(id);

            if(news == null)
            {
                return false;
            }
            surveyDbContext.Remove(news);
            await surveyDbContext.SaveChangesAsync();
            return true;
        }
    }
}
