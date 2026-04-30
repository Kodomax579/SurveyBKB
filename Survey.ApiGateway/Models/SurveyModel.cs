namespace Survey.ApiGateway.Models
{
    public class SurveyModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CreatedUserId { get; set; }
        public List<string> Classes { get; set; } = new();
        public List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
    }
}
