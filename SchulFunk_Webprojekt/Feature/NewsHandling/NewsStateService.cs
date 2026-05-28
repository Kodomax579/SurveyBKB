using SchulFunk_Webprojekt.Feature.NewsHandling.Model;

namespace SchulFunk_Webprojekt.Feature.NewsHandling
{
    public class NewsStateService
    {
        private List<NewsItem> newsItems = new();

        public event Action OnChange;

        public List<NewsItem> GetNewsItems() 
        { 
            return newsItems; 
        }

        public void SetNews(List<NewsItem?> newsItems)
        {
            if (newsItems == null)
            {
                return;
            }
            this.newsItems = newsItems;
            NotifyStateChanged();
        }

        public bool UpdateNewsItem(NewsItem updatedNews)
        {
            var i = newsItems.FindIndex(n  => n.Id == updatedNews.Id);

            if(i == -1)
            {
                return false;
            }

            newsItems[i] = updatedNews;
            NotifyStateChanged();
            return true;
        }

        public bool DeleteNewsItem(int id)
        {
            var news = newsItems.FirstOrDefault(n => n.Id == id);

            if (news == null)
            {
                return false;
            }

            newsItems.Remove(news);
            NotifyStateChanged();
            return true;
        }

        public void AddNewsItem(NewsItem newsItem)
        {
            newsItems.Add(newsItem);
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
