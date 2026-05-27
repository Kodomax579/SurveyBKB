using System.Text.Json.Serialization;

namespace SchulFunk_Webprojekt.Feature.SurveyHandling.Model
{
    public class AnswerModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("answers")]
        public string Answers { get; set; } = string.Empty;

        [JsonPropertyName("numberOfSelectedAnswer")]
        public int NumberOfSelectedAnswer { get; set; }
    }
}