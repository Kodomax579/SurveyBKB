using Microsoft.EntityFrameworkCore;
using NewsService.Data;
using NewsService.Database;

namespace NewsService.Feature
{
    public class NewsService (NewsContext newsDbContext)
    {
        public async Task<List<NewsModel>> GetAllNewsAsync()
        {
            return await newsDbContext.News.ToListAsync();
        }

        public async Task<NewsModel?> GetNewsByIdAsync(int id)
        {
            return await newsDbContext.News.FindAsync(id);
        }

        public async Task<bool> CreateNewsAsync(NewsModel entity)
        {
            newsDbContext.News.Add(entity);
            var changes = await newsDbContext.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> UpdateNewsAsync(NewsModel entity)
        {
            var existingNews = await newsDbContext.News.FindAsync(entity.Id);
            if (existingNews == null) return false;

            existingNews.Titel = entity.Titel;
            existingNews.PreviewText = entity.PreviewText;
            existingNews.MainText = entity.MainText;
            existingNews.CreatedAt = entity.CreatedAt;

            var changes = await newsDbContext.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> DeleteNewsAsync(int id)
        {
            var existingNews = await newsDbContext.News.FindAsync(id);
            if (existingNews == null) return false;

            newsDbContext.News.Remove(existingNews);
            var changes = await newsDbContext.SaveChangesAsync();
            return changes > 0;
        }
    }
}
