using System.Text.Json.Serialization;

namespace SchulFunk_Webprojekt.Model
{
    public class QuestionModel
    {
        [JsonPropertyName("question")]
        public string Question { get; set; } = string.Empty;

        [JsonPropertyName("options")]
        public List<AnswerModel> Option { get; set; } = new();
    }
}
