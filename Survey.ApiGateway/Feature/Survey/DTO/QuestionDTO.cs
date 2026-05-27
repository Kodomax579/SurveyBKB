using Survey.ApiGateway.Feature.Survey.Models;

namespace Survey.ApiGateway.Feature.Survey.DTO
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public List<AnswerDTO> Options { get; set; } = new();
        public int SurveyModelId { get; set; }
    }
}
