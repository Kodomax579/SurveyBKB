using System.Text.Json.Serialization; // Wichtig für die Attribute

namespace SchulFunk_Webprojekt.Feature.SurveyHandling.Model
{
    public class SurveyModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("creatorName")]
        public string CreatorName { get; set; } = string.Empty;

        [JsonPropertyName("createdUserID")]
        public int CreatedUserID { get; set; }

        [JsonPropertyName("groupId")]
        public int GroupId { get; set; }

        [JsonPropertyName("createdAt")]
        public DateOnly CreatedAt { get; set; }

        [JsonPropertyName("onlineUntil")]
        public DateOnly OnlineUntil { get; set; }

        [JsonPropertyName("classes")]
        public List<string> Classes { get; set; } = new();

        [JsonPropertyName("userIDs")]
        public List<int> UserIDs { get; set; } = new();

        [JsonPropertyName("questions")]
        public List<QuestionModel> Questions { get; set; } = new();
    }
}