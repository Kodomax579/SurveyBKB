using System.Text.Json.Serialization;

namespace SchulFunk_Webprojekt.Model
{
    public class NewsItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("titel")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("tag")]
        public string Tag { get; set; } = string.Empty;

        [JsonPropertyName("previewText")]
        public string PreviewText { get; set; } = string.Empty;

        [JsonPropertyName("mainText")]
        public string MainText { get; set; } = string.Empty;

        [JsonPropertyName("imageLink")]
        public string ImageLink { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("expiredDate")]
        public DateTime ExpiredDate { get; set; }

        [JsonPropertyName("numberOfMembers")]
        public int NumberOfMembers { get; set; }

        // Hier ist der entscheidende Teil für deinen Fehler:
        [JsonPropertyName("user")]
        public UserModel? UserModel { get; set; }
    }
}
