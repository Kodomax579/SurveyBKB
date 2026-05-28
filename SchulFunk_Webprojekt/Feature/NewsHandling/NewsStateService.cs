using SchulFunk_Webprojekt.Feature.NewsHandling.Model;
using SchulFunk_Webprojekt.SignalRHub;

namespace SchulFunk_Webprojekt.Feature.NewsHandling
{
    public class NewsStateService
    {
        private List<NewsItem> newsItems = new();

        public event Action OnChange;

        public NewsStateService(SignalRHub.SignalRHub signalRHub)
        {
            signalRHub.OnNewsItemDeleted += DeleteNewsItem;
            signalRHub.OnNewsItemsUpdated += UpdateNewsItem;
            signalRHub.OnNewsItemCreated += AddNewsItem;
        }

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

        public void UpdateNewsItem(NewsItem updatedNews)
        {
            var i = newsItems.FindIndex(n  => n.Id == updatedNews.Id);

            if(i == -1)
            {
                return ;
            }

            newsItems[i] = updatedNews;
            NotifyStateChanged();
        }

        public void DeleteNewsItem(int id)
        {
            var news = newsItems.FirstOrDefault(n => n.Id == id);

            if (news == null)
            {
                return;
            }

            newsItems.Remove(news);
            NotifyStateChanged();
        }

        public void AddNewsItem(NewsItem newsItem)
        {
            newsItems.Add(newsItem);
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
