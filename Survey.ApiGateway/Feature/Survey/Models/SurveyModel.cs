using Survey.ApiGateway.Feature.User.Models;
using Survey.ApiGateway.Models.DTO;

namespace Survey.ApiGateway.Feature.Survey.Models
{
    public class SurveyModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public UserModel User { get; set; } = new();
        public DateOnly CreatedAt { get; set; }
        public DateOnly OnlineUntil { get; set; }
        public List<string> Classes { get; set; } = new();
        public List<int> UserIDs { get; set; } = new();
        public List<QuestionModel> Questions { get; set; } = new();
    }
}
