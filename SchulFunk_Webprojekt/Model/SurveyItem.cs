namespace SchulFunk_Webprojekt.Model
{
    public class SurveyModel
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string CreatorName { get; set; } = string.Empty;
        public int CreatedUserID { get; set; }

        public int GroupId { get; set; }

        public DateOnly CreatedAt { get; set; }

        public DateOnly OnlineUntil { get; set; }

        public List<string> Classes { get; set; } = new();

        public List<int> UserIDs { get; set; } = new();

        public List<QuestionModel> Questions { get; set; } = new();
    }
}