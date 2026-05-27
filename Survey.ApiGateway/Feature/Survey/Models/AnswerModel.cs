namespace Survey.ApiGateway.Feature.Survey.Models
{
    public class AnswerModel
    {
        public int Id { get; set; }
        public string Answers { get; set; } = string.Empty;
        public int NumberOfSelectedAnswer { get; set; }
        public int QuestionModelId { get; set; }
    }
}
