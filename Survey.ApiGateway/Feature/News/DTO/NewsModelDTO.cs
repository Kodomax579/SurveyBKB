using Survey.ApiGateway.Feature.User.Models;
using Survey.ApiGateway.Models.DTO;

namespace Survey.ApiGateway.Feature.News.DTO
{
    public class NewsModelDTO
    {
        public int Id { get; set; }
        public string Titel { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public string PreviewText { get; set; } = string.Empty;
        public string MainText { get; set; } = string.Empty;
        public string ImageLink { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int NumberOfMembers { get; set; }
        public UserDTO User { get; set; } = new();
    }
}
