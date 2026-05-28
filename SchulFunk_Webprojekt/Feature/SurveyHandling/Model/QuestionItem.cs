using System.Text.Json.Serialization;

namespace SchulFunk_Webprojekt.Feature.SurveyHandling.Model
{
    public class QuestionModel
    {
        [JsonPropertyName("question")]
        public string Question { get; set; } = string.Empty;

        [JsonPropertyName("options")]
        public List<AnswerModel> AnswerModels { get; set; } = new();
    }
}
