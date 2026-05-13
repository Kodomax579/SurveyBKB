using Survey.ApiGateway.Models.DTO;

namespace Survey.ApiGateway.Models
{
    public class SurveyModel
    {
        public string Title { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public UserDTO User { get; set; } = new();
        public DateOnly CreatedAt { get; set; }
        public DateOnly OnlineUntil{ get; set; }
        public List<string> Classes { get; set; } = new();
        public List<int> UserIDs { get; set; } = new();
        public List<QuestionModel> Questions { get; set; } = new();
    }
}
