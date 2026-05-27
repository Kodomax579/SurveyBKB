using Survey.ApiGateway.Feature.Survey.Models;
using Survey.ApiGateway.Feature.User.Models;
using Survey.ApiGateway.Models.DTO;

namespace Survey.ApiGateway.Feature.Survey.DTO
{
    public class SurveyDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public UserDTO User { get; set; } = new();
        public DateOnly CreatedAt { get; set; }
        public DateOnly OnlineUntil { get; set; }
        public List<string> Classes { get; set; } = new();
        public List<int> UserIDs { get; set; } = new();
        public List<QuestionDTO> Questions { get; set; } = new();
    }
}
