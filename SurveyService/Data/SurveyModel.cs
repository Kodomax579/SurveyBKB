namespace SurveyService.Data
{
    public class SurveyModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public string CreatedEmail { get; set; } = string.Empty;
        public DateOnly CreatedAt { get; set; }
        public DateOnly OnlineUntil{ get; set; }
        public List<string> Classes { get; set; } = new();
        public List<int> UserIDs { get; set; } = new();
        public List<QuestionModel> Questions { get; set; } = new();
    }
}
